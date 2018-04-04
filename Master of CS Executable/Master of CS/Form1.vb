'***********************************************************************************************************
' You may find your character is not moving or occassionally move a bit
' It is because you have not implemented Listener subroutine
' Thus the program is always busy updating graphics due to Me.Invalidate() in Listener
' If you want to test if your build works before the completion of Listener, uncomment Thread.Sleep(100)
' Remember to uncomment Thread.Sleep(100) after Listener is implemented or you may experience packet loss
'***********************************************************************************************************

Imports System.Net
Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
Imports System.Net.NetworkInformation

Public Class Form1
    Public ReceivingThread As Thread
    Dim ListenPort As Integer
    Dim MyPlayer As PlayerClass
    Dim AllPlayers As New List(Of PlayerClass)
    Dim AllMsg As New List(Of String)
    Dim Playing As Boolean = False
    Dim msg As String
    Dim Mouse_x, Mouse_y As Integer
    Dim run_thread As Boolean = False
    Dim BroadcastIP As String
    Dim VpnIP As String = BroadcastIP '"198.18.0.3"
    Private MsgLock As New Object ' Prevent other threads accessing the list while operating on it
    Private PlayersLock As New Object ' Prevent other threads accessing the list while operating on it

    Private Sub Listener()
        ' Creates a UdpClient for reading incoming data.
        Dim RemoteIPEndPoint As New IPEndPoint(IPAddress.Any, 0)
        Dim receivingUdpClient As New UdpClient(ListenPort)
        Dim cmd() As String
        Dim exist As Boolean = True
        Dim Player As PlayerClass

        ' Loop forever in order to receive message from Internet
        Do While run_thread
            Try
                Dim buffer As [Byte]() = receivingUdpClient.Receive(RemoteIPEndPoint)
                Dim message As String = Encoding.ASCII.GetString(buffer)
                cmd = Split(message, " ")
                If cmd(0) = "A" Then
                    SyncLock PlayersLock
                        For Each Player In AllPlayers
                            If cmd(1) = Player.Name Then
                                exist = True
                                Exit For
                            Else
                                exist = False
                            End If
                        Next
                    End SyncLock
                End If
                If cmd(0) = "V" Then
                    SyncLock PlayersLock
                        For Each Player In AllPlayers
                            If cmd(1) = Player.Name Then
                                Player.x = cmd(2)
                                Player.y = cmd(3)
                            End If
                        Next
                    End SyncLock
                End If
                If (cmd(0) = "M") Then
                    SyncLock PlayersLock
                        For Each Player In AllPlayers
                            If cmd(1) = Player.Name Then
                                Dim index As Integer
                                Dim inMsg As String = cmd(2)
                                If (cmd.Length - 1) > 2 Then
                                    For index = 3 To cmd.Length - 1
                                        inMsg &= " " & cmd(index)
                                    Next
                                End If
                                SyncLock MsgLock
                                    AllMsg.Add(Player.Name & ": " & inMsg)
                                End SyncLock
                            End If
                        Next
                    End SyncLock
                End If
                If (cmd(0) = "C") Then
                    SyncLock PlayersLock
                        For Each Player In AllPlayers
                            If (cmd(1)) = Player.Name Then
                                Player.Health = cmd(2)
                            End If
                        Next
                    End SyncLock
                End If
                If (cmd(0)) = "D" Then

                    SyncLock PlayersLock
                        For Each Player In AllPlayers
                            If cmd(1) = Player.Name Then
                                AllMsg.Add(Player.Name & " has died!")
                                AllPlayers.Remove(Player)
                            End If
                        Next
                    End SyncLock
                End If
                If Not exist Then
                    Dim newPlayer As New PlayerClass(cmd(1), cmd(2))
                    newPlayer.x = cmd(3)
                    newPlayer.y = cmd(4)
                    SyncLock PlayersLock
                        AllPlayers.Add(newPlayer)
                    End SyncLock
                    exist = True
                End If


            Catch e As Exception
                Me.Invalidate()

            End Try
            '=============================================================
            ' Comment Thread.sleep(100) after you implemented Listener()
            '=============================================================
            'Thread.Sleep(100)
            Me.Invalidate()
        Loop

        receivingUdpClient.Close()
    End Sub

    Private Sub Broadcast_Move(ByVal player As PlayerClass)
        Dim Send_Message As String
        Dim udpClient As New UdpClient()
        Dim destIp As New IPEndPoint(IPAddress.Parse(BroadcastIP), ListenPort)
        Dim sendByte As Byte() = New Byte() {}
        Send_Message = "V " & MyPlayer.Name & " " & MyPlayer.x & " " & MyPlayer.y
        sendByte = Encoding.ASCII.GetBytes(Send_Message)
        udpClient.Send(sendByte, sendByte.Length, destIp)
        Me.Invalidate()
    End Sub
    Private Sub Broadcast_Msg()
        Dim Send_Message As String
        Dim udpClient As New UdpClient()
        Dim destIp As New IPEndPoint(IPAddress.Parse(BroadcastIP), ListenPort)
        Dim sendByte As Byte() = New Byte() {}
        Send_Message = "M " & MyPlayer.Name & " " & msg
        sendByte = Encoding.ASCII.GetBytes(Send_Message)
        udpClient.Send(sendByte, sendByte.Length, destIp)
        msg = ""
        Me.Invalidate()
    End Sub

    Private Sub Broadcast_Actions()
        Dim Send_Message As String
        Dim udpClient As New UdpClient()
        Dim destIp As New IPEndPoint(IPAddress.Parse(BroadcastIP), ListenPort)
        Dim sendByte As Byte() = New Byte() {}
        SyncLock PlayersLock
            For Each player In AllPlayers
                If Collided(MyPlayer, player) Then
                    player.Health -= 5
                    Send_Message = "C " & player.Name & " " & player.Health
                    sendByte = Encoding.ASCII.GetBytes(Send_Message)
                    udpClient.Send(sendByte, sendByte.Length, destIp)
                End If
            Next
        End SyncLock
        Me.Invalidate()
    End Sub
    Private Sub Broadcast_Alive()
        Dim Send_Message As String
        Dim udpClient As New UdpClient()
        Dim destIp As New IPEndPoint(IPAddress.Parse(BroadcastIP), ListenPort)
        Dim sendByte As Byte() = New Byte() {}
        If MyPlayer.Health > 0 Then
            Send_Message = "A " & MyPlayer.Name & " " & MyPlayer.Role & " " &
            MyPlayer.x & " " & MyPlayer.y
        Else
            Send_Message = "D " & MyPlayer.Name
        End If
        sendByte = Encoding.ASCII.GetBytes(Send_Message)
        udpClient.Send(sendByte, sendByte.Length, destIp)
        Me.Invalidate()
    End Sub
    '***********************************************************************************************************
    '                                    !! Do not change code below !!
    '***********************************************************************************************************
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        Dim Player As PlayerClass
        Dim g As Graphics = e.Graphics
        Dim drawFont As New Font("Arial", 10)
        Dim WhiteBrush As New SolidBrush(Color.White)
        Dim BlueBrush As New SolidBrush(Color.Blue)
        Dim GreenBrush As New SolidBrush(Color.Green)
        Dim RedBrush As New SolidBrush(Color.Red)
        Dim s As String
        Dim y As Integer
        Dim stringSize As New SizeF

        SyncLock PlayersLock
            For Each Player In AllPlayers
                g.DrawImage(Player.CharacterImage, Player.x, Player.y)
                s = Player.x & ":" & Player.y
                stringSize = e.Graphics.MeasureString(Player.Name, drawFont)
                g.DrawString(Player.Name, drawFont, WhiteBrush, Player.x - (stringSize.Width - 31) / 2, Player.y - stringSize.Height)
            Next
        End SyncLock
        y = 0
        SyncLock MsgLock
            For Each s In AllMsg
                g.DrawString(s, drawFont, BlueBrush, 0, y)
                y += 14
            Next
        End SyncLock
        If msg <> "" Then
            ' Measure string.
            stringSize = e.Graphics.MeasureString(msg & "  ", drawFont)

            ' Draw rectangle representing size of string.
            g.DrawRectangle(New Pen(Color.Green, 1), 0.0F, y, stringSize.Width, stringSize.Height)
            g.DrawString(msg, drawFont, BlueBrush, 0, y)
        End If
        stringSize = e.Graphics.MeasureString("Health", drawFont)
        g.FillRectangle(GreenBrush, 800, 10, 200, stringSize.Height)
        g.FillRectangle(RedBrush, 800, 10, MyPlayer.Health * 2, stringSize.Height)
        g.DrawString("Health", drawFont, WhiteBrush, 800 - stringSize.Width, 10)
    End Sub

    Function Collided(ByVal p1 As PlayerClass, ByVal p2 As PlayerClass) As Boolean
        If (p2.x > p1.x) And (p2.x < p1.x + 31) And (p2.y > p1.y) And (p2.y < p1.y + 41) Then
            Return True
        End If
        If (p2.x + 31 > p1.x) And (p2.x + 31 < p1.x + 31) And (p2.y > p1.y) And (p2.y < p1.y + 41) Then
            Return True
        End If
        If (p2.x + 31 > p1.x) And (p2.x + 31 < p1.x + 31) And (p2.y + 46 > p1.y) And (p2.y + 46 < p1.y + 41) Then
            Return True
        End If
        If (p2.x > p1.x) And (p2.x < p1.x + 31) And (p2.y + 46 > p1.y) And (p2.y + 46 < p1.y + 41) Then
            Return True
        End If
        Return False

    End Function

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Playing = False
        run_thread = False
        MyPlayer.Health = -1
        Broadcast_Alive()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        SyncLock MsgLock
            If AllMsg IsNot Nothing And AllMsg.Count <> 0 Then
                AllMsg.Remove(AllMsg.First)
                Me.Invalidate()
            End If
        End SyncLock
    End Sub

    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        Select Case e.Button
            Case MouseButtons.Left
                Mouse_x = e.X
                Mouse_y = e.Y
            Case MouseButtons.Right
                Broadcast_Actions()
        End Select
    End Sub

    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        e.Handled = True
        If (e.KeyChar = vbCr) Then
            Broadcast_Msg()
        ElseIf (e.KeyChar = vbBack) And msg <> "" Then
            msg = msg.Remove(msg.Length - 1)
        Else
            msg &= e.KeyChar
        End If
        Me.Invalidate()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If (Mouse_x <> -1) And (Mouse_y <> -1) And MyPlayer.Health > 0 Then
            If (Mouse_x < MyPlayer.x) Then
                MyPlayer.x -= 1
            End If
            If (Mouse_x > MyPlayer.x) Then
                MyPlayer.x += 1
            End If
            If (Mouse_y < MyPlayer.y) Then
                MyPlayer.y -= 1
            End If
            If (Mouse_y > MyPlayer.y) Then
                MyPlayer.y += 1
            End If
            If (MyPlayer.x > 989) Then
                MyPlayer.x = 989
            End If
            If MyPlayer.x < 0 Then
                MyPlayer.x = 0
            End If
            If (MyPlayer.y > 730) Then
                MyPlayer.y = 730
            End If
            If MyPlayer.y < 0 Then
                MyPlayer.y = 0
            End If
            Broadcast_Move(MyPlayer)
            Me.Invalidate()
        End If
    End Sub

    Function FindBroadcastIP() As String
        Dim IP As String
        Dim strIPNetMask() As String
        Dim strCurrentIP() As String
        Dim CurrentIP(3) As Byte
        Dim IPNetMask(3) As Byte
        Dim BoradcastIP(3) As Byte
        IP = String.Empty
        Dim strHostName As String = System.Net.Dns.GetHostName()
        Dim iphe As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)
        Dim mask As String

        For Each ipheal As System.Net.IPAddress In iphe.AddressList
            If ipheal.AddressFamily = System.Net.Sockets.AddressFamily.InterNetwork Then
                IP = ipheal.ToString()
                Exit For
            End If
        Next
        strCurrentIP = IP.Split(".")

        Dim Interfaces() As NetworkInterface = NetworkInterface.GetAllNetworkInterfaces()
        For Each adapter As NetworkInterface In Interfaces
            Dim UnicastIPInfoCol As UnicastIPAddressInformationCollection = adapter.GetIPProperties().UnicastAddresses
            For Each UnicatIPInfo As UnicastIPAddressInformation In UnicastIPInfoCol
                If UnicatIPInfo.Address.AddressFamily = AddressFamily.InterNetwork Then
                    If IP.Equals(UnicatIPInfo.Address.ToString()) Then
                        mask = UnicatIPInfo.IPv4Mask.ToString()
                    End If
                End If

            Next
        Next
        strIPNetMask = mask.Split(".")
        For i As Integer = 0 To 3
            CurrentIP(i) = Convert.ToInt32(strCurrentIP(i))
            IPNetMask(i) = Convert.ToInt32(strIPNetMask(i))
            BoradcastIP(i) = CurrentIP(i) Or (Not IPNetMask(i))
        Next
        Return BoradcastIP(0).ToString() + "." + BoradcastIP(1).ToString() + "." + BoradcastIP(2).ToString() + "." + BoradcastIP(3).ToString()
    End Function

    Private Sub ALive_Tick(sender As Object, e As EventArgs) Handles ALive.Tick
        Broadcast_Alive()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim CoverPage As New Form2

        ClientSize = New Size(1024, 768)
        DoubleBuffered = True
        CoverPage.ShowDialog()
        MyPlayer = New PlayerClass(CoverPage.RoleName, CoverPage.Role)
        ListenPort = CoverPage.Port
        AllPlayers.Add(MyPlayer)
        BroadcastIP = FindBroadcastIP()
        Broadcast_Move(MyPlayer)
        ReceivingThread = New Thread(AddressOf Listener)
        run_thread = True
        ReceivingThread.Start()
        Playing = True
        Timer1.Start()
        Timer2.Start()
        ALive.Start()
        Mouse_x = -1
        Mouse_y = -1
        Me.Text = "By Dr. Lin for demo purpose." & "Broadcast IP:" & BroadcastIP & ":" & ListenPort
    End Sub
End Class
