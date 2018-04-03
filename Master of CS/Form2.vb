'***********************************************************************************************************
'Name: Josh Poston
'Program: Master of CS
'Created: 11/14/2017
'Last Modified: 12/14/17
'Class: CSCI 332
'Grade Received: A
'***********************************************************************************************************

Public Class Form2
    Public Role As Integer = -1
    Public Port As Integer
    Public RoleName As String
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Role = 0
        PictureBox9.Image = My.Resources.r1
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Role = 1
        PictureBox9.Image = My.Resources.r2
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Role = 2
        PictureBox9.Image = My.Resources.r3
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Role = 3
        PictureBox9.Image = My.Resources.r4
    End Sub

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click
        Role = 4
        PictureBox9.Image = My.Resources.r5
    End Sub

    Private Sub PictureBox6_Click(sender As Object, e As EventArgs) Handles PictureBox6.Click
        Role = 5
        PictureBox9.Image = My.Resources.r6
    End Sub

    Private Sub PictureBox7_Click(sender As Object, e As EventArgs) Handles PictureBox7.Click
        Role = 6
        PictureBox9.Image = My.Resources.r7
    End Sub

    Private Sub PictureBox8_Click(sender As Object, e As EventArgs) Handles PictureBox8.Click
        Role = 7
        PictureBox9.Image = My.Resources.r8
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim complete As Boolean = True
        RoleName = TextBox1.Text
        Label3.Text = ""
        If String.Equals(RoleName, "") Then
            Label3.Text = "Enter your name!!"
            complete = False
        End If
        Try
            Port = TextBox2.Text
        Catch ex As Exception
            complete = False
            TextBox2.Text = ""
            Label3.Text += " Enter a port number between 1001 and 65535!!"
        End Try
        If Port <= 1000 Or Port > 65535 Then
            complete = False
            Label3.Text += " Enter a port number between 1001 and 65535!!"
            TextBox2.Text = ""
        End If
        If Role = -1 Then
            complete = False
            Label3.Text += " Choose your character!!"
        End If
        If complete Then
            Close()
        End If
    End Sub

End Class