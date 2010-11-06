open System
open System.IO
open System.Net

let getHtml url printStrategy =
    async { let request =  WebRequest.Create(Uri url)
            use! response = request.AsyncGetResponse()
            use stream = response.GetResponseStream()
            use reader = new StreamReader(stream)
            let contents = reader.ReadToEnd()
            do printStrategy url contents
            return contents }
 
let sites = ["http://www.bing.com";
             "http://www.google.com";
             "http://www.yahoo.com";
             "http://msdn.microsoft.com/en-us/fsharp/default.aspx"]

let printStrategy url (contents:string) =  
    printfn "%s - HTML Length %d" url contents.Length

let sitesHtml = Async.Parallel [for site in sites -> getHtml site printStrategy]
                |> Async.RunSynchronously

do printfn "\r\nTotal Character length = %d\r\nPress any key to continue" sitesHtml.Length

do Console.ReadLine() |> ignore
