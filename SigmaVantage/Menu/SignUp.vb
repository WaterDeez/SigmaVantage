Imports EasyEncryption
Imports System.IO
Public Class SignUp
    Public SQL As New SQLConnector
    Public doEncrypt As Boolean

    Private Sub SignUp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OFDBrowse.InitialDirectory = Application.StartupPath
        If My.Settings.EncryptedPassword = True Then
            doEncrypt = True
        Else
            doEncrypt = False
        End If
    End Sub
    Private Sub UploadImage(Path As String)
        ' EXIT IF IMAGE NOT SELECTED
        If String.IsNullOrEmpty(Path) Then Exit Sub

        ' GET IMAGE DATA VIA MEMORY STREAM
        Dim img As Image = Image.FromFile(Path)
        Dim ms As New MemoryStream()
        img.Save(ms, img.RawFormat)
        Dim buffer As Byte() = ms.GetBuffer()

        ' ADD SQL PARAMETERS
        SQL.AddParam("@image", buffer)
        ' RUN INSERT COMMAND
        SQL.ExecQuery("UPDATE Users Set Avatar=@image WHERE UserName='" + txtUser.Text + "';")


        ' REPORT ERRORS
        If Not String.IsNullOrEmpty(Sql.Exception) Then MsgBox(Sql.Exception)
    End Sub
    Private Sub InsertUser()
        'Use EasyEncrption library to store password with SHA256bit encryption if required
        Dim password
        If doEncrypt = True Then
            password = SHA.ComputeSHA256Hash(txtPass.Text)
        Else
            password = txtPass.Text
        End If

        'Add parameters for insert to stop malicious injection
        Sql.AddParam("@user", txtUser.Text)
        SQL.AddParam("@password", password)
        'Insert into SQL Table
        SQL.ExecQuery("INSERT INTO Users (UserName,Password,Created) VALUES (@user,@password,GETDATE());")
        'Report & Abort on SQL Errors
        If Sql.HasException(True) Then Exit Sub
        UploadImage(txtImagePath.Text)
        MsgBox("User created succesfully!")
        'Clear Signup Textbox Fields
        txtPass.Clear()
        txtPass2.Clear()
        txtUser.Clear()
        'Disable Enter Button Again
        Enterbtn.Enabled = False
    End Sub
    Private Function userInUse() As Boolean
        'Flush existing data to stop data collisions
        If Sql.DBDT IsNot Nothing Then
            Sql.DBDT.Clear()
        End If
        'Query Users table for a matching user and password
        Sql.ExecQuery("SELECT Count(UserName) As UserCount From Users Where UserName='" & txtUser.Text & "'")
        'Check if there is a matching user
        If Sql.DBDT.Rows(0).Item("UserCount") >= 1 Then
            Sql.DBDT.Clear()
            Return True 'Make boolean true
        End If
        Sql.DBDT.Clear() 'Flush data again 
        Return False 'Make boolean false
    End Function
    Private Sub ofdBrowse_FileOk(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles OFDBrowse.FileOk
        Try
            pbPreview.Image = Image.FromFile(OFDBrowse.FileName)
            txtImagePath.Text = OFDBrowse.FileName
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Enterbtn.Click
        'make sure passwords are matching for data validity
        If txtPass.Text = txtPass2.Text Then
            'function to check whether the username is already used in the database
            If userInUse() = False Then
                'Function to add all of the entered user details into the database
                InsertUser()
            Else
                'Warning box explaining username is already in use
                MsgBox("Username is already in use")
            End If
        Else
            'Warning that passwords do not match
            MsgBox("Passwords do not match...")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
    Private Sub txtFields_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtUser.TextChanged, txtPass.TextChanged, txtPass2.TextChanged
        'Check if all of the text fields are NOT blank
        If Not String.IsNullOrWhiteSpace(txtUser.Text) AndAlso Not String.IsNullOrWhiteSpace(txtPass.Text) AndAlso Not String.IsNullOrWhiteSpace(txtPass2.Text) Then
            'Enable the enter button
            Enterbtn.Enabled = True
        Else
            Enterbtn.Enabled = False
        End If
    End Sub


    Private Sub cmdBrowse_Click(sender As Object, e As EventArgs) Handles cmdBrowse.Click
        OFDBrowse.ShowDialog()
    End Sub

End Class