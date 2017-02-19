using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xeora.Web.Server
{
    class Console
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("---         Xeora Micro Web Server          ---");
            System.Console.WriteLine("---         v1.0 2017                       ---");
            System.Console.WriteLine("-----------------------------------------------");

            System.Net.IPAddress ipAddress = System.Net.IPAddress.Any;
            short port = 80;
            int timeout = 30, maxClient = 50;
            foreach(string arg in args)
            {
                if (arg.IndexOf("/bind:") == 0)
                {
                    string argV = arg.Substring(6);

                    if (!System.Net.IPAddress.TryParse(argV, out ipAddress))
                    {
                        System.Console.WriteLine("Bind end point is looking wrong!");
                        break;
                    }
                }

                if (arg.IndexOf("/port:") == 0)
                {
                    string argV = arg.Substring(6);

                    short.TryParse(argV, out port);
                }

                if (arg.IndexOf("/timeout:") == 0)
                {
                    string argV = arg.Substring(9);

                    int.TryParse(argV, out timeout);
                }

                if (arg.IndexOf("/maxclient:") == 0)
                {
                    string argV = arg.Substring(11);

                    int.TryParse(argV, out maxClient);
                }
            }

            Service.HttpServer httpServer = new Service.HttpServer(ipAddress, port, timeout, maxClient);
            httpServer.start();
        }
    }
}
