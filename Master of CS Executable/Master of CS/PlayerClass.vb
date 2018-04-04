Imports System.Net

Public Class PlayerClass
    Public Name As String
    Public Role As Integer
    Public x, y As Integer
    Public Health As Integer
    Public CharacterImage As Bitmap '!!Do not send this object!!

    Public Sub New(ByVal Nm As String, ByVal rl As Integer)
        Name = Nm
        Role = rl
        Dim myAsm As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Health = 100
        Randomize()
        x = CInt(Int((989 * Rnd()) + 1))
        y = CInt(Int((515 * Rnd()) + 1))
        Select Case Role
            Case 0
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r1.png"))
            Case 1
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r2.png"))
            Case 2
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r3.png"))
            Case 3
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r4.png"))
            Case 4
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r5.png"))
            Case 5
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r6.png"))
            Case 6
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r7.png"))
            Case 7
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r8.png"))
        End Select
    End Sub

    Public Sub New(ByVal Nm As String, ByVal rl As Integer, ByVal ux As Integer, ByVal uy As Integer, ByVal h As Integer)
        Name = Nm
        Role = rl
        x = ux
        y = uy
        Health = h
        Dim myAsm As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Select Case Role
            Case 0
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r1.png"))
            Case 1
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r2.png"))
            Case 2
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r3.png"))
            Case 3
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r4.png"))
            Case 4
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r5.png"))
            Case 5
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r6.png"))
            Case 6
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r7.png"))
            Case 7
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r8.png"))
        End Select
    End Sub
    Public Sub New(ByVal old_player As PlayerClass)
        Name = old_player.Name
        Role = old_player.Role
        x = old_player.x
        y = old_player.y
        Health = old_player.Health
        Dim myAsm As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Select Case Role
            Case 0
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r1.png"))
            Case 1
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r2.png"))
            Case 2
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r3.png"))
            Case 3
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r4.png"))
            Case 4
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r5.png"))
            Case 5
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r6.png"))
            Case 6
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r7.png"))
            Case 7
                CharacterImage = New Bitmap(myAsm.GetManifestResourceStream(Me.GetType, "r8.png"))
        End Select
    End Sub

End Class
