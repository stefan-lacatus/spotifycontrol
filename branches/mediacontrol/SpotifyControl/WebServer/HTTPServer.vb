' based on this project http://www.planetsourcecode.com/vb/scripts/ShowCode.asp?txtCodeId=1885&lngWId=10

Imports System
Imports System.IO
Imports System.Net.Sockets
Imports System.Text
Imports Microsoft.Win32

'This is the base server class that all other server clasess use (Port 1000 Default)
Public Class BaseServer

    Public Port As Integer = 1000
    Public clients As New Hashtable()
    Public listener As TcpListener
    Public listenerThread As Threading.Thread
    Public Event StatusUpdate(ByVal sStatus As String)
    Public Event DataReceived(ByVal sender As UserConnection, ByVal Data As String)
    Public Event ClientConnected(ByVal sender As UserConnection)
    Public Event ClientDisConnected(ByVal sender As UserConnection)

    Public Sub StartServer()
        listenerThread = New Threading.Thread(AddressOf DoListen)
        listenerThread.Start()
        RaiseEvent StatusUpdate("Server started")
    End Sub

    Public Sub StopServer()
        On Error Resume Next
        listener.Stop()
        Dim cUserConnection As UserConnection
        For Each cUserConnection In clients
            CloseClient(cUserConnection)
            cUserConnection.client.Close()
            clients.Remove(cUserConnection)
            OnClientConnect(cUserConnection)
        Next
        RaiseEvent StatusUpdate("Server stopped")
    End Sub

    Public Sub CloseClient(ByVal cUserConnection As UserConnection)
        On Error Resume Next
        Dim NWStream As NetworkStream = cUserConnection.client.GetStream
        NWStream.Close()
        cUserConnection.client.Close()
        clients.Remove(cUserConnection)
        OnClientConnect(cUserConnection)
    End Sub

    Private Sub DoListen()
        Try
            ' Listen for new connections.
            listener = New TcpListener(System.Net.IPAddress.Any, Port)
            listener.Start()
            Do
                Dim client As New UserConnection(listener.AcceptTcpClient)
                clients.Add(client, client)
                OnClientConnect(client)
                AddHandler client.DataReceived, AddressOf OnDataReceived
                AddHandler client.ClientClose, AddressOf OnClientDisconnect
                RaiseEvent StatusUpdate("New connection found: waiting for log-in")
            Loop Until False
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub OnDataReceived(ByVal sender As UserConnection, ByVal data As String)
        If Len(data) > 0 Then
            RaiseEvent DataReceived(sender, data)
        End If
    End Sub

    Public Sub OnClientConnect(ByVal sender As UserConnection)
        RaiseEvent ClientConnected(sender)
    End Sub

    Public Sub OnClientDisconnect(ByVal sender As UserConnection)
        On Error Resume Next
        sender.client.Close()
        clients.Remove(sender)
        RaiseEvent ClientDisConnected(sender)
        sender = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        StopServer()
        clients = Nothing
        listener = Nothing
        MyBase.Finalize()
    End Sub

End Class

'The UserConnection class encapsulates the functionality of a TcpClient connection with streaming for a single user.
Public Class UserConnection
    Const READ_BUFFER_SIZE As Integer = 5120 '1024
    Public client As TcpClient
    Public sExtraData As String
    Public sExtraValues As ValueType
    Public readBuffer(READ_BUFFER_SIZE) As Byte
    Public Event DataReceived(ByVal sender As UserConnection, ByVal Data As String)
    Public Event ClientClose(ByVal sender As UserConnection)

    ' Overload the New operator to set up a read thread.
    Public Sub New(ByVal client As TcpClient)
        Me.client = client

        ' This starts the asynchronous read thread.  The data will be saved into
        Me.client.GetStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, AddressOf StreamReceiver, Nothing)
    End Sub

    Public Sub CloseClientConenction()
        Dim NWStream As NetworkStream = client.GetStream
        NWStream.Flush()
        client.Close()
        RaiseEvent ClientClose(Me)
    End Sub

    ' This subroutine uses a StreamWriter to send a message to the user.
    Public Sub SendData(ByVal Data As String)
        ' Synclock ensure that no other threads try to use the stream at the same time.
        SyncLock client.GetStream
            Using writer As New IO.StreamWriter(client.GetStream)
                writer.Write(Data)
                ' Make sure all data is sent now.
                writer.Flush()
            End Using
        End SyncLock
    End Sub

    Public Sub SendFile(ByVal sFilename As String)

        Dim NWStream As NetworkStream = client.GetStream
        Dim bytesToSend(client.SendBufferSize) As Byte
        Dim FI As New FileInfo(sFilename)
        Using FileSTR As New FileStream(sFilename, FileMode.Open, FileAccess.Read)
            Dim FileReader As New BinaryReader(FileSTR)
            Dim numBytesRead As Integer
            Dim Ipos As Integer
            Do Until Ipos >= FI.Length
                numBytesRead = FileSTR.Read(bytesToSend, 0, bytesToSend.Length)
                NWStream.Write(bytesToSend, 0, numBytesRead)
                Ipos = Ipos + numBytesRead
                NWStream.Flush()
            Loop
            NWStream.Flush()
            FileSTR.Close()
            FileReader.Close()
        End Using
    End Sub

    ' This is the callback function for TcpClient.GetStream.Begin. It begins an 
    ' asynchronous read from a stream.
    Private Sub StreamReceiver(ByVal ar As IAsyncResult)
        Dim BytesRead As Integer
        Dim strMessage As String
        Try
            ' Ensure that no other threads try to use the stream at the same time.
            SyncLock client.GetStream
                ' Finish asynchronous read into readBuffer and get number of bytes read.
                BytesRead = client.GetStream.EndRead(ar)
            End SyncLock

            If BytesRead < 1 Then
                RaiseEvent ClientClose(Me)
                Exit Sub
            End If

            ' Convert the byte array the message was saved into
            strMessage = Encoding.ASCII.GetString(readBuffer, 0, BytesRead)

            RaiseEvent DataReceived(Me, strMessage)

            ' Ensure that no other threads try to use the stream at the same time.
            SyncLock client.GetStream
                ' Start a new asynchronous read into readBuffer.

                client.GetStream.BeginRead(readBuffer, 0, READ_BUFFER_SIZE, AddressOf StreamReceiver, Nothing)
            End SyncLock

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        RaiseEvent ClientClose(Me)
        MyBase.Finalize()
    End Sub


End Class

'HTTP Server sends back files requested by the browser (Port 80 Default)
'Note this only supports the GET command at this moment
Public Class HTTPServer

    Public Enum ResponseCodes
        HTTP_OK = 200
        HTTP_NOTFOUND = 404
        HTTP_SERVERERROR = 500
    End Enum

    Public Structure HTTPConnection
        Dim Request_Method As String
        Dim Request_Accept As String
        Dim Request_AcceptLanguage As String
        Dim Request_Encoding As String
        Dim Request_HTTPVersion As String
        Dim Request_Useragent As String
        Dim Request_Host As String
        Dim Request_Cookie As String
        Dim Request_Referer As String
        Dim Request_ConnectionType As String
        Dim Request_Filename As String
        Dim Request_LocalFilename As String
        Dim Request_LocalIsDir As Boolean
        Dim Response_Number As ResponseCodes
        Dim Response_Text As String
        Dim Response_ContentLength As Integer
        Dim Response_ContentType As String
        Dim Response_ServerType As String
        Dim Response_Handled As Boolean 'flag this as true if you are handling it yourself
    End Structure


    Public RootDirectory As String
    Public StartFile As String

    Public WithEvents cServer As New BaseServer()
    Public Event StatusUpdate(ByVal sStatus As String)
    Public Event DataReceived(ByVal sender As UserConnection, ByVal Data As String)
    Public Event ON_HTTP_GET(ByVal sender As UserConnection, ByVal Data As HTTPConnection)
    Public Event ClientConnected(ByVal sender As UserConnection)
    Public Event ClientDisConnected(ByVal sender As UserConnection)
    Public HandledRequestList() As String
    Private Sub cServer_ClientConnected(ByVal sender As UserConnection) Handles cServer.ClientConnected
        RaiseEvent ClientConnected(sender)
    End Sub

    Private Sub cServer_ClientDisConnected(ByVal sender As UserConnection) Handles cServer.ClientDisConnected
        RaiseEvent ClientDisConnected(sender)
    End Sub

    Private Sub cServer_DataReceived(ByVal sender As UserConnection, ByVal Data As String) Handles cServer.DataReceived
        'get the request and send the data back to them
        Dim TConnection As HTTPConnection
        Dim sData As String = Data
        'sender.SendData(Data)
        Debug.Print(Data)
        ParseHTTPHeader(sData, TConnection)
        RaiseEvent ON_HTTP_GET(sender, TConnection)
        Dim ok As Integer = 0
        For Each value As String In HandledRequestList
            If value = TConnection.Request_Filename Then ok = 1
        Next
        If ok = 0 Then
            ServeHTTPData(sender, TConnection)
        End If
        ' RaiseEvent DataReceived(sender, Data)
    End Sub

    Private Sub ServeHTTPData(ByVal cUserConnection As UserConnection, ByRef TConnection As HTTPConnection)
        If TConnection.Response_Handled = True Then Exit Sub
        Try
            Dim DI As New DirectoryInfo(TConnection.Request_LocalFilename)
            Dim FI As New FileInfo(TConnection.Request_LocalFilename)

            ' Debug.WriteLine(TConnection.Request_LocalFilename)

            'see if we are serving a file or a directory
            If TConnection.Request_LocalIsDir = True Then
                'Debug.WriteLine("Is Dir")
                If DI.Exists = False Then
                    'serve an error message
                    TConnection.Response_Number = ResponseCodes.HTTP_NOTFOUND
                    TConnection.Response_Text = "Error"
                    TConnection.Response_ContentType = "text/html"
                    SendHTTPResponse(cUserConnection, TConnection, Return404() & vbCrLf & TConnection.Request_Filename)
                    Exit Sub
                End If
            Else
                'Debug.WriteLine("Is File")
                If FI.Exists = False Then
                    'serve an error message
                    TConnection.Response_Number = ResponseCodes.HTTP_NOTFOUND
                    TConnection.Response_Text = "Error"
                    TConnection.Response_ContentType = "text/html"
                    SendHTTPResponse(cUserConnection, TConnection, Return404() & vbCrLf & TConnection.Request_Filename)
                    Exit Sub
                End If
            End If


            If TConnection.Request_LocalIsDir = True Then
                'serve a directory
                '1 Check to see if index.html exists in the directory if so serve it instead
                '2 get the directory information as a stream
                '3 Send it to the socket
                If New FileInfo(TConnection.Request_LocalFilename & StartFile).Exists = True Then
                    TConnection.Response_Number = ResponseCodes.HTTP_OK
                    TConnection.Response_Text = "OK"
                    TConnection.Request_LocalFilename = TConnection.Request_LocalFilename & "index.html"
                    TConnection.Response_ContentType = GetMimeType(TConnection.Request_LocalFilename)
                    SendHTTPFILE(cUserConnection, TConnection)
                Else
                    TConnection.Response_Number = ResponseCodes.HTTP_OK
                    TConnection.Response_Text = "OK"
                    TConnection.Response_ContentType = "text/html"
                    SendHTTPDIR(cUserConnection, TConnection)
                    Exit Sub
                End If

            Else
                'serve a file
                '1 get the file as a stream
                '2 Get the Content Type from the registry
                '3 Send it to the socket
                TConnection.Response_Number = ResponseCodes.HTTP_OK
                TConnection.Response_Text = "OK"
                TConnection.Response_ContentType = GetMimeType(TConnection.Request_LocalFilename)
                SendHTTPFILE(cUserConnection, TConnection)
                Exit Sub
            End If


        Catch ex As Exception
            Try
                'serve an error message
                TConnection.Response_Number = ResponseCodes.HTTP_SERVERERROR
                TConnection.Response_Text = "Error"
                SendHTTPResponse(cUserConnection, TConnection, Return500(ex.Message))
                MsgBox(ex.Message)
            Finally
                cUserConnection.client.Close()
            End Try
        End Try
    End Sub

    Public Function GetMimeType(ByVal sFile As String) As String
        'look for the mime type info in the registry
        Dim FI As New FileInfo(sFile)
        Dim RegClasses As RegistryKey = Registry.ClassesRoot
        Dim FileTypeKey As RegistryKey = RegClasses.OpenSubKey(FI.Extension)
        Dim sVal As String
        If FileTypeKey Is Nothing Then
            Return "text/html"
            RegClasses.Close()
            FileTypeKey.Close()
            Exit Function
        Else
            sVal = FileTypeKey.GetValue("Content Type")
            If sVal = "" Then
                Return "text/html"
                RegClasses.Close()
                FileTypeKey.Close()
                Exit Function
            Else
                Return sVal
                RegClasses.Close()
                FileTypeKey.Close()
                Exit Function
            End If
        End If
    End Function

    Public Function Return404() As String
        Dim sTMP As String
        sTMP = sTMP & "<HTML><HEAD><TITLE>404 File not found</TITLE></HEAD><br>" & vbCrLf
        sTMP = sTMP & "<BODY BGCOLOR=" & Chr(34) & "#FFFFFF" & Chr(34) & " Text=" & Chr(34) & "#000000" & Chr(34) & " LINK=" & Chr(34) & "#0000FF" & Chr(34) & " VLINK=" & Chr(34) & "#000080" & Chr(34) & " ALINK=" & Chr(34) & "#008000" & Chr(34) & "><br>" & vbCrLf
        sTMP = sTMP & "<b>404</b> File not found<br>" & vbCrLf
        sTMP = sTMP & "</BODY><br>" & vbCrLf
        sTMP = sTMP & "</HTML><br>" & vbCrLf
        Return404 = sTMP
    End Function

    Public Function Return500(ByVal sMessage As String) As String
        Dim sTMP As String
        sTMP = sTMP & "<HTML><HEAD><TITLE>500 Internal Server Error</TITLE></HEAD><br>" & vbCrLf
        sTMP = sTMP & "<BODY BGCOLOR=" & Chr(34) & "#FFFFFF" & Chr(34) & " Text=" & Chr(34) & "#000000" & Chr(34) & " LINK=" & Chr(34) & "#0000FF" & Chr(34) & " VLINK=" & Chr(34) & "#000080" & Chr(34) & " ALINK=" & Chr(34) & "#008000" & Chr(34) & "><br>" & vbCrLf
        sTMP = sTMP & "<b>500</b> Sorry - Internal Server Error<br>" & sMessage & vbCrLf
        sTMP = sTMP & "</BODY><br>" & vbCrLf
        sTMP = sTMP & "</HTML><br>" & vbCrLf
        Return500 = sTMP
    End Function

    Public Sub SendHTTPDIR(ByVal cUserConnection As UserConnection, ByRef TConnection As HTTPConnection)
        'HTTP/1.1 200 OK
        'Server: Microsoft-IIS/5.0
        'Content-Location: http://127.0.0.1/index.html
        'Date: Wed, 10 Dec 2003 19:10:25 GMT
        'Content-Type: text/html
        'Accept-Ranges: bytes
        'Last-Modified: Mon, 22 Sep 2003 22:36:56 GMT
        'Content-Length: 1957
        Dim sHeader As String
        Dim sLine As String
        'Dim sParse As String
        Dim sPath As String
        Dim NWStream As NetworkStream = cUserConnection.client.GetStream
        Dim DI As New DirectoryInfo(TConnection.Request_LocalFilename)
        Dim DIRINFO As DirectoryInfo
        Dim FILEINFO As FileInfo

        If TConnection.Response_Handled = True Then Exit Sub
        sLine = sLine & "<html><head><title>Listing For: " & TConnection.Request_Filename & "</title></head><body>" & vbCrLf
        sLine = sLine & "<h4>Listing For: " & TConnection.Request_Filename & "</h4><br>" & vbCrLf
        sLine = sLine & "<BR><B>Directories</B> <font size=" & Chr(34) & "1" & Chr(34) & ">(Total Directories: " & DI.GetDirectories.GetUpperBound(0) + 1 & ")</font><HR>" & vbCrLf
        For Each DIRINFO In DI.GetDirectories
            sPath = Replace(LCase(DIRINFO.FullName), LCase(RootDirectory), "")
            sPath = Replace(sPath, "\", "/")
            sPath = sPath & "/"
            sLine = sLine & "<a href=" & Chr(34) & sPath & Chr(34) & ">" & DIRINFO.Name & "</a><br>" & vbCrLf
        Next
        sLine = sLine & "<BR><B>Files</B> <font size=" & Chr(34) & "1" & Chr(34) & ">(Total Files: " & DI.GetFiles.GetUpperBound(0) + 1 & ")</font><HR>" & vbCrLf
        For Each FILEINFO In DI.GetFiles
            sPath = Replace(LCase(FILEINFO.FullName), LCase(RootDirectory), "")
            sPath = Replace(sPath, "\", "/")
            sLine = sLine & "<a href=" & Chr(34) & sPath & Chr(34) & ">" & FILEINFO.Name & "</a> <font size=" & Chr(34) & "1" & Chr(34) & ">(" & FILEINFO.Length & ")</font><br>" & vbCrLf
        Next
        sLine = sLine & "</body></html>"
        sHeader = sHeader & TConnection.Request_HTTPVersion & " " & TConnection.Response_Number & " " & TConnection.Response_Text & vbCrLf
        sHeader = sHeader & "Server: " & TConnection.Response_ServerType & vbCrLf
        sHeader = sHeader & "Date: " & Now & vbCrLf 'Format(Now(), "Long Time")
        sHeader = sHeader & "Content-Type: " & TConnection.Response_ContentType & vbCrLf
        sHeader = sHeader & "Accept-Ranges: bytes" & vbCrLf
        sHeader = sHeader & "Content-Length: " & sLine.Length & vbCrLf
        sHeader = sHeader & vbCrLf
        sHeader = sHeader & sLine

        Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(sHeader)
        NWStream.Write(sendBytes, 0, sendBytes.Length)
        'close the connection
        If LCase(TConnection.Request_ConnectionType) <> "keep-alive" Then
            cUserConnection.client.Close()
        End If

    End Sub

    Public Sub SendHTTPFILE(ByVal cUserConnection As UserConnection, ByRef TConnection As HTTPConnection)
        'HTTP/1.1 200 OK
        'Server: Microsoft-IIS/5.0
        'Content-Location: http://127.0.0.1/index.html
        'Date: Wed, 10 Dec 2003 19:10:25 GMT
        'Content-Type: text/html
        'Accept-Ranges: bytes
        'Last-Modified: Mon, 22 Sep 2003 22:36:56 GMT
        'Content-Length: 1957
        Dim sHeader As String
        Dim NWStream As NetworkStream = cUserConnection.client.GetStream
        Dim FI As New FileInfo(TConnection.Request_LocalFilename)
        Dim IPOS As Integer = 0
        Dim SendBUFFSize As Integer = cUserConnection.client.SendBufferSize

        If TConnection.Response_Handled = True Then Exit Sub
        ' Debug.WriteLine(TConnection.Response_ContentType)
        sHeader = sHeader & TConnection.Request_HTTPVersion & " " & TConnection.Response_Number & " " & TConnection.Response_Text & vbCrLf
        sHeader = sHeader & "Server: " & TConnection.Response_ServerType & vbCrLf
        sHeader = sHeader & "Date: " & Now & vbCrLf 'Format(Now(), "Long Time")
        sHeader = sHeader & "Content-Type: " & TConnection.Response_ContentType & vbCrLf
        sHeader = sHeader & "Accept-Ranges: bytes" & vbCrLf
        sHeader = sHeader & "Content-Length: " & FI.Length & vbCrLf
        sHeader = sHeader & vbCrLf

        Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(sHeader)


        If sendBytes.Length > SendBUFFSize Then
            Do Until IPOS >= sendBytes.Length
                ' Debug.WriteLine(IPOS)
                NWStream.Write(sendBytes, IPOS, SendBUFFSize)
                IPOS = IPOS + SendBUFFSize
            Loop
        Else
            NWStream.Write(sendBytes, 0, sendBytes.Length)
        End If

        'NWStream.Write(sendBytes, 0, sendBytes.Length)
        cUserConnection.SendFile(TConnection.Request_LocalFilename)

        'close the connection
        If LCase(TConnection.Request_ConnectionType) <> "keep-alive" Then
            cUserConnection.client.Close()
        End If

    End Sub

    Public Sub SendHTTPResponse(ByVal cUserConnection As UserConnection, ByRef TConnection As HTTPConnection, ByVal sReponse As String)
        'HTTP/1.1 200 OK
        'Server: Microsoft-IIS/5.0
        'Content-Location: http://127.0.0.1/index.html
        'Date: Wed, 10 Dec 2003 19:10:25 GMT
        'Content-Type: text/html
        'Accept-Ranges: bytes
        'Last-Modified: Mon, 22 Sep 2003 22:36:56 GMT
        'Content-Length: 1957
        Dim sHeader As String
        Dim NWStream As NetworkStream = cUserConnection.client.GetStream
        Dim IPOS As Integer = 0
        Dim SendBUFFSize As Integer = cUserConnection.client.SendBufferSize

        sHeader = sHeader & TConnection.Request_HTTPVersion & " " & TConnection.Response_Number & " " & TConnection.Response_Text & vbCrLf
        sHeader = sHeader & "Server: " & TConnection.Response_ServerType & vbCrLf
        sHeader = sHeader & "Date: " & Now & vbCrLf 'Format(Now(), "Long Time")
        sHeader = sHeader & "Content-Type: " & TConnection.Response_ContentType & vbCrLf
        sHeader = sHeader & "Accept-Ranges: bytes" & vbCrLf
        sHeader = sHeader & "Content-Length: " & sReponse.Length & vbCrLf
        sHeader = sHeader & vbCrLf

        sHeader = sHeader & sReponse

        If TConnection.Response_Handled = True Then Exit Sub
        Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes(sHeader)
        If sendBytes.Length > SendBUFFSize Then
            Do Until IPOS >= sendBytes.Length
                ' Debug.WriteLine(IPOS)
                NWStream.Write(sendBytes, IPOS, SendBUFFSize)
                IPOS = IPOS + SendBUFFSize
            Loop
        Else
            NWStream.Write(sendBytes, 0, sendBytes.Length)
        End If
        NWStream.Flush()
        'close the connection
        If LCase(TConnection.Request_ConnectionType) <> "keep-alive" Then
            cUserConnection.client.Close()
        End If
    End Sub

    Private Sub ParseHTTPHeader(ByVal sData As String, ByRef TConnection As HTTPConnection)
        'GET /test/a%20picture.jpg HTTP/1.1
        'Accept: image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-powerpoint, application/vnd.ms-excel, application/msword, application/x-shockwave-flash, */*
        'Accept-Language: en-us
        'Accept-Encoding: gzip, deflate
        'User-Agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705; .NET CLR 1.1.4322)
        'Host: 127.0.0.1:81
        'Connection: Keep-Alive
        'Cookie: IDHTTPSESSIONID=RxQbcqGwptvxbZY
        'Referer: http://www.voidrealms.com
        Dim sLine As String
        Dim Ipos As Integer
        Dim sDelim As String
        Dim sMethod() As String
        Dim sFilename As String
        Dim sLines() As String
        Dim I As Integer
        'MsgBox(sData)
        sLines = Split(sData, vbCrLf)


        For I = sLines.GetLowerBound(0) To sLines.GetUpperBound(0)
            sLine = Trim(sLines(I))
            'MsgBox(sLine)
            'Debug.WriteLine(sLine)

            'get the Host
            sDelim = "host: "
            Ipos = InStr(LCase(sLine), sDelim)
            If Ipos <> 0 Then
                TConnection.Request_Host = Trim(Mid(sLine, sDelim.Length, sLine.Length))
            End If

            'get the accept types
            sDelim = "accept: "
            Ipos = InStr(LCase(sLine), sDelim)
            If Ipos <> 0 Then
                TConnection.Request_Accept = Trim(Mid(sLine, sDelim.Length, sLine.Length))
            End If

            'get the accept language
            sDelim = "accept-language: "
            Ipos = InStr(LCase(sLine), sDelim)
            If Ipos <> 0 Then
                TConnection.Request_AcceptLanguage = Trim(Mid(sLine, sDelim.Length, sLine.Length))
            End If

            'get the accept encoding
            sDelim = "accept-encoding: "
            Ipos = InStr(LCase(sLine), sDelim)
            If Ipos <> 0 Then
                TConnection.Request_Encoding = Trim(Mid(sLine, sDelim.Length, sLine.Length))
            End If

            'get the user agent
            sDelim = "user-agent: "
            Ipos = InStr(LCase(sLine), sDelim)
            If Ipos <> 0 Then
                TConnection.Request_Useragent = Trim(Mid(sLine, sDelim.Length, sLine.Length))
            End If

            'get the Connection type
            sDelim = "connection: "
            Ipos = InStr(LCase(sLine), sDelim)
            If Ipos <> 0 Then
                TConnection.Request_ConnectionType = Trim(Mid(sLine, sDelim.Length, sLine.Length))
            End If

            'get the Cookie
            sDelim = "cookie: "
            Ipos = InStr(LCase(sLine), sDelim)
            If Ipos <> 0 Then
                TConnection.Request_Cookie = Trim(Mid(sLine, sDelim.Length, sLine.Length))
            End If

            'get the Referer
            sDelim = "referer: "
            Ipos = InStr(LCase(sLine), sDelim)
            If Ipos > 0 Then
                TConnection.Request_Referer = Trim(Mid(sLine, sDelim.Length, sLine.Length))
            End If

            'get the Method and Data
            sDelim = "GET "
            Ipos = InStr(sLine, sDelim)
            If Ipos <> 0 Then
                sMethod = Split(sLine, " ")

                TConnection.Request_Method = Trim(sMethod(0))
                TConnection.Request_Filename = Trim(Replace(sMethod(1), "%20", " ")) 'replace any encoding
                TConnection.Request_HTTPVersion = Trim(sMethod(2))
                If Mid(RootDirectory, Len(RootDirectory), Len(RootDirectory)) = "\" Then
                    RootDirectory = Mid(RootDirectory, 1, Len(RootDirectory) - 1)
                End If
                sFilename = RootDirectory & Replace(TConnection.Request_Filename, "/", "\")
                TConnection.Request_LocalFilename = sFilename
            End If

        Next I
        If Mid(TConnection.Request_LocalFilename, Len(TConnection.Request_LocalFilename), Len(TConnection.Request_LocalFilename)) = "\" Then
            TConnection.Request_LocalIsDir = True
        End If
        TConnection.Response_Number = ResponseCodes.HTTP_OK
        TConnection.Response_Text = "OK"
        TConnection.Response_ContentType = "text/html"
        TConnection.Response_ServerType = "Power-Sockets"
        TConnection.Response_Handled = False
    End Sub

    Private Sub cServer_StatusUpdate(ByVal sStatus As String) Handles cServer.StatusUpdate
        RaiseEvent StatusUpdate(sStatus)
    End Sub

    Public Sub StartServer()
        cServer.StartServer()
    End Sub

    Public Sub StopServer()
        cServer.StopServer()
    End Sub

    Public Sub New()
        cServer.Port = 80
    End Sub

    Protected Overrides Sub Finalize()
        On Error Resume Next
        cServer.StopServer()
        cServer = Nothing
        MyBase.Finalize()
    End Sub
End Class
