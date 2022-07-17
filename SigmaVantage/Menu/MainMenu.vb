Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq
Imports System.Windows.Forms.DataVisualization.Charting
Public Class MainMenu

    Public Shared client As New Net.Http.HttpClient()

    Public Selections As New Selections()
    Public Sub AVcall(APIfunction As String, company As String)
        Dim url = "https://www.alphavantage.co/query?function=" & APIfunction & "&symbol=" & company & "&apikey=" & My.Settings.APIkey
    End Sub

    Public Sub GraphLoad()

    End Sub
    'Private Async Sub Button1_Click(sender As Object, e As EventArgs)
    '    Dim url = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=MSFT&apikey="
    '    Dim json = Await client.GetStringAsync(url)
    '    Dim jss = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Object)(json)

    '    For Each Jproperty In jss("Time Series (Daily)")
    '        Dim thedate = Jproperty.Name.ToString
    '        TextBox1.Text &= thedate & Environment.NewLine

    '        For Each Vproperty In jss("Time Series (Daily)")(thedate)
    '            Dim thesubname = Mid(Vproperty.Name.ToString, 4)
    '            Dim thevalue = Vproperty.Value.ToString
    '            TextBox1.Text &= thesubname & ": " & thevalue & Environment.NewLine
    '        Next
    '        TextBox1.Text &= Environment.NewLine & Environment.NewLine
    '    Next
    'End Sub

    Private Sub MainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Selections.DrawAvatar(pbDBAvatar)

        Chart1.Series.Clear()
        Chart1.Titles.Add("Demo")
        'Create a new series and add data points to it.
        Dim s As New Series
        s.Name = "aline"
        'Change to a line graph.
        s.ChartType = SeriesChartType.Line
        s.Points.AddXY("1990", 27)
        s.Points.AddXY("1991", 15)
        s.Points.AddXY("1992", 17)
        'Add the series to the Chart1 control.
        Chart1.Series.Add(s)

    End Sub

    Private Async Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.KeyPress
        Dim url = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=" & ComboBox1.Text & "&apikey=" & My.Settings.APIkey
        Dim json = Await client.GetStringAsync(url)
        Dim jss = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Object)(json)

        For Each Jproperty In jss("bestMatches:")
            Dim number = Jproperty.Name.ToString
            For Each Vproperty In jss("bestMatches")(number)
                Dim symbol = Vproperty.Value1.ToString
                Dim name = Vproperty.Value2.ToString
                ComboBox1.Items.Add(name & ", " & symbol)
            Next

        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Login.Show()
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        InformationScreen.Show()
        Me.Close()
    End Sub
End Class