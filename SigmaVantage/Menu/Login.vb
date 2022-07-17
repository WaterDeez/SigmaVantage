Imports EasyEncryption
Public Module UserVariables
    Public username As String
End Module
Public Class Login
    Public SQLControl As New SQLConnector()
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SignUp.Show()
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Config.Show()
    End Sub

    Private Sub PassTBox_Keypress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles PassTBox.KeyPress
        If e.KeyChar = Chr(13) Then
            If Not String.IsNullOrWhiteSpace(usernameTBox.Text) AndAlso Not String.IsNullOrWhiteSpace(PassTBox.Text) Then
                SQLControl.ExecQuery("SELECT Count(UserName) As UserCount From Users Where UserName='" & usernameTBox.Text & "' AND Password='" & SHA.ComputeSHA256Hash(PassTBox.Text) & "'")
                If SQLControl.DBDT.Rows(0).Item("UserCount") >= 1 Then
                    username = usernameTBox.Text
                    MainMenu.Show()
                    Me.Close()
                Else
                    MsgBox("Incorrect Login", MsgBoxStyle.Critical, "Sigma Vantage")
                End If
            Else
                MsgBox("Insufficient information", MsgBoxStyle.Critical, "Sigma Vantage")
            End If
        End If
    End Sub
End Class
