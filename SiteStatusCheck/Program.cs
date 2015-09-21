using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SiteStatusCheck
{
    class Program
    {
        private static string input = string.Empty;

        static void Main(string[] args)
        {

            //ThreadPool.QueueUserWorkItem(ProcessInput);
            Thread th = new Thread(new ThreadStart(ProcessInput));
            th.Start();

            while (input != "-q")
            {
                Stopwatch stopwatch = new Stopwatch();
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                using (Stream data = client.OpenRead("http://www.myassetmanager.net/api/Alive/1"))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        stopwatch.Start();
                        string response = reader.ReadToEnd();
                        stopwatch.Stop();
                        TimeSpan ts = stopwatch.Elapsed;

                        string responseStr = string.Format("{0}, {1}, {2}\r\n", response, ts.ToString(), DateTime.Now);

                        using (StreamWriter sw = File.AppendText("site-response-times.txt"))
                        {
                            sw.Write(responseStr);
                            sw.Flush();
                        }

                        Console.WriteLine(string.Format("{0}        {1}", responseStr, "-q to exit"));
                        data.Close();
                        reader.Close();
                    }
                }
                Thread.Sleep(300000);
            }
        }

        private static void ProcessInput()
        {
            while(input != "-q")
            {
                input = Console.ReadLine();
                Thread.Sleep(500);
            }
        }
    }
}
