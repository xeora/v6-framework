using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xeora.Web.Server.Service
{
    public class HttpServer
    {
        public static void Start()
        {
            int x = 0;
            while(x < 10)
            {
                System.Console.WriteLine("Connection done, IP Address: 192.168.1.1");

                System.Threading.Thread.Sleep(5000);

                x++;
            }
        }
    }
}
