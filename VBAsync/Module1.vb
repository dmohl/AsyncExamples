Imports System
Imports System.IO
Imports System.Net
Imports System.Threading.Tasks
Imports System.Linq

Module Module1

    Async Function GetHtml(ByVal url As String, ByVal printStrategy As Action(Of String, String)) As Task(Of String)
        Dim request = WebRequest.Create(New Uri(url))
        Using response = Await request.GetResponseAsync()
            Using stream = response.GetResponseStream()
                Using reader = New StreamReader(stream)
                    Dim contents = reader.ReadToEnd()
                    printStrategy.Invoke(url, contents)
                    Return contents
                End Using
            End Using
        End Using
    End Function

    Async Function ProcessSites(ByVal sites As String(), ByVal printStrategy As Action(Of String, String)) As Task
        Dim sitesHtml = _
            Await TaskEx.WhenAll(sites.Select(Function(site) GetHtml(site, printStrategy)))
        Console.WriteLine("{0}Process Complete{0}Press any key to continue", _
            Environment.NewLine)
    End Function

    Sub Main()
        Dim sites = New String() {"http://www.bing.com", _
                "http://www.google.com", _
                "http://www.yahoo.com", _
                "http://msdn.microsoft.com/en-us/fsharp/default.aspx"}

        Dim printStrategy As Action(Of String, String) = _
            Sub(url, contents) Console.WriteLine("{0} - HTML Length {1}", url, contents.Length)
        ProcessSites(sites, printStrategy).Wait()
        Console.ReadLine()
    End Sub

End Module
