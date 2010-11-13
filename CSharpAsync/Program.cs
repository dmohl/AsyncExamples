using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace CSharpAsync
{
    class Program
    {
        static async Task<string> GetHtml(string url, Action<string, string> printStrategy)
        {
            var request = WebRequest.Create(new Uri(url));
            using (var response = await request.GetResponseAsync())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var contents = reader.ReadToEnd();
                        printStrategy.Invoke(url, contents);
                        return contents;
                    }
                }
            }        
        }

        static async Task ProcessSites(string[] sites, Action<string, string> printStrategy)
        {
            var sitesHtml = 
                await TaskEx.WhenAll(
                    sites.Select(site => GetHtml(site, printStrategy)));
            Console.WriteLine("\r\nProcess Complete\r\nPress any key to continue");
        }

        static void Main(string[] args)
        {
            var sites = new[] { "http://www.bing.com",
                            "http://www.google.com",
                            "http://www.yahoo.com",
                            "http://msdn.microsoft.com/en-us/fsharp/default.aspx" };

            Action<string, string> printStrategy =
                (url, contents) =>
                    Console.WriteLine("{0} - HTML Length {1}", url, contents.Length);
            
            ProcessSites(sites, printStrategy).Wait();

            Console.ReadLine();
        }
    }
}
