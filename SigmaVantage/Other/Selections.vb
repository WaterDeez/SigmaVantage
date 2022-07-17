Imports System.IO
Public Class Selections
    Public SQL As New SQLConnector()

    Public Sub LoadStudents(ByRef cmbox As ComboBox)
        cmbox.Items.Clear()

        ' RUN QUERY
        SQL.ExecQuery("SELECT DISTINCT *, (Surname + ', ' + PreferredNames) AS StudentName FROM StudentDetails ORDER BY Surname;")

        If SQL.HasException(True) Then Exit Sub

        cmbox.DataSource = SQL.DBDT
        cmbox.DisplayMember = "StudentName"
        cmbox.ValueMember = "StudentID"
    End Sub
    Public Function FetchAvatar(User As String) As Byte()
        ' QUERY DATABASE FOR IMAGE BYTES
        SQL.AddParam("@user", User)
        SQL.ExecQuery("SELECT Avatar FROM Users WHERE UserName=@user")

        ' ABORT ON ERRORS OR NO IMAGE DATA FOUND
        If Not String.IsNullOrEmpty(SQL.Exception) OrElse SQL.RecordCount < 1 OrElse IsDBNull(SQL.DBDT.Rows(0).Item("Avatar")) Then Return Nothing

        ' FILL AND RETURN IMAGE DATA BUFFER
        Dim buffer As Byte() = SQL.DBDT.Rows(0).Item("Avatar")

        Return buffer
    End Function
    Public Sub DrawAvatar(PB As PictureBox)
        ' GET IMAGE DATA DATA
        Dim buffer As Byte() = FetchAvatar(username)

        ' STREAM IMAGE DATA TO PICTUREBOX
        If buffer IsNot Nothing Then
            Using ms As New MemoryStream(buffer, 0, buffer.Length)
                ms.Write(buffer, 0, buffer.Length)
                PB.Image = Image.FromStream(ms, True)
            End Using
        End If
    End Sub


End Class