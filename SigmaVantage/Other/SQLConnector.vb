Imports System.Data.SqlClient
Public Class SQLConnector

    'database connection definitions from storred settings
    Private DBCmd As SqlCommand
    Private DBCon As New SqlConnection(My.Settings.SQLConnnection)
    'database data definitions
    Public DBDA As SqlDataAdapter
    Public DBDT As DataTable

    'query Data
    Public Params As New List(Of SqlParameter)
    Public RecordCount As Integer
    Public Exception As String

    'bug fixing subroutine
    Public Sub New()
    End Sub

    'public execute query subroutine
    Public Sub ExecQuery(Query As String)
        ' RESET QUERY STATS
        RecordCount = 0
        Exception = ""
        Try
            DBCon.Open()
            'construct sql command
            DBCmd = New SqlCommand(Query, DBCon)
            'load parameters into the sql command
            Params.ForEach(Sub(p) DBCmd.Parameters.Add(p))
            'clear query parameter list
            Params.Clear()
            'execute query command and fill local vb.net dataset
            DBDT = New DataTable
            DBDA = New SqlDataAdapter(DBCmd)
            RecordCount = DBDA.Fill(DBDT)
        Catch ex As Exception
            'catch any sql errors and display them to the user to debug
            Exception = "ExecQuery Error: " & vbNewLine & ex.Message
        Finally
            'close sql connection
            If DBCon.State = ConnectionState.Open Then DBCon.Close()
        End Try
    End Sub

    'add parameters to list function
    Public Sub AddParam(Name As String, Value As Object)
        Dim NewParam As New SqlParameter(Name, Value)
        Params.Add(NewParam)
    End Sub

    'error checking subroutine
    Public Function HasException(Optional Report As Boolean = False) As Boolean
        If String.IsNullOrEmpty(Exception) Then Return False
        If Report = True Then MsgBox(Exception, MsgBoxStyle.Critical, "Exception:")
        Return True
    End Function

End Class

