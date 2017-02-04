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

            System.Threading.Thread httpServerThread = new System.Threading.Thread(Service.HttpServer.Start);

            httpServerThread.Start();
            httpServerThread.Join();
        }
    }
}
