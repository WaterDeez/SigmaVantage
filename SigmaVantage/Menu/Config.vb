Imports System.Data.SqlClient
Public Class Config
    Dim tested As Boolean = False


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SQLConnect.Text = My.Settings.SQLConnnection Then
            SQLConnect.Text = ""
            SQLConnect.UseSystemPasswordChar = False
            Button1.Text = "Reset"
        Else
            SQLConnect.UseSystemPasswordChar = True
            SQLConnect.Text = My.Settings.SQLConnnection
            Button1.Text = "Clear"
        End If
    End Sub

    Private Sub Config_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SQLConnect.Text = My.Settings.SQLConnnection
        If My.Settings.EncryptedPassword = True Then
            CheckBox1.CheckState = CheckState.Checked
        Else
            CheckBox1.CheckState = CheckState.Unchecked
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ConnectionTry(SQLConnect.Text)
    End Sub

    Private Sub ConnectionTry(ConnectionString As String)
        Dim testConnection As New SqlConnection(ConnectionString)
        Try
            testConnection.Open()
        Catch ex As Exception
            MsgBox("Error: " & vbNewLine & ex.Message)
            tested = False
        Finally
            If testConnection.State = ConnectionState.Open Then
                testConnection.Close()
                MsgBox("SQL Test Connection Success", MsgBoxStyle.Information, "SQL Connection")
                tested = True
            End If
        End Try
    End Sub
    Private Sub txtFields_TextChanged(sender As System.Object, e As System.EventArgs) Handles SQLConnect.TextChanged
        tested = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If tested = False Then
            MsgBox("Test a successful connection first!", MsgBoxStyle.Critical, "SQL Connection")
        Else
            My.Settings.SQLConnnection = SQLConnect.Text
            MsgBox("SQL Connection String Updated", MsgBoxStyle.Information, "SQL Connection")
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = False Then
            My.Settings.EncryptedPassword = False
        Else
            My.Settings.EncryptedPassword = True
        End If
    End Sub
End Class