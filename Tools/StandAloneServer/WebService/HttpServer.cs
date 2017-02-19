using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Xeora.Web.Server.Service
{
    public class HttpServer
    {
        private IPAddress _ipAddress;
        private short _port;
        private int _timeout;
        private int _maxClient;
        private Thread _listenThread;

        public HttpServer(IPAddress ipAddress, short port, int timeout, int maxClient)
        {
            _ipAddress = ipAddress;
            _port = port;
            _timeout = timeout;
            _maxClient = maxClient;

            _listenThread = new Thread(startServerInternal);
        }

        public void start()
        {
            _listenThread.Start();
            _listenThread.Join();
        }

        private void startServerInternal()
        {
            Console.WriteLine("Trying to create server...");

            Socket listenSocket = null;

            try
            {
                listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listenSocket.ReceiveTimeout = _timeout * 1000;
                listenSocket.Bind(new IPEndPoint(_ipAddress, _port));
                listenSocket.Listen(_maxClient);

                Console.WriteLine(string.Format("Server has started listening at {0}:{1}...", _ipAddress.ToString(), _port));

                Socket connection = null;

                try
                {
                    while (true)
                    {
                        connection = listenSocket.Accept();

                        Console.WriteLine(string.Format("Connection came from {0} at {1}", connection.RemoteEndPoint.ToString(), DateTime.Now.ToUniversalTime()));

                        HttpConnection httpConnection = new HttpConnection(connection);
                        httpConnection.handle();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (listenSocket != null)
                    listenSocket.Close();
            }
        }
    }
}
