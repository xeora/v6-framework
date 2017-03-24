using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Xeora.Web.Server.Service
{
    public class HttpConnection
    {
        private Socket _connection;
        private Thread _handlerThread;

        public HttpConnection(Socket connection)
        {
            _connection = connection;
        }

        public void handle()
        {
            _handlerThread = new Thread(handleInternal);
            _handlerThread.Start();
        }

        private void handleInternal()
        {
            Console.WriteLine(string.Format("Connection from {0} has been accepted at {1}", _connection.RemoteEndPoint.ToString(), DateTime.Now.ToUniversalTime()));

            MemoryStream mS = null;
            try
            {
                int contentLength = 0;

                if (_connection.Poll(-1, SelectMode.SelectRead))
                {
                    byte[] inBytes = new byte[_connection.Available];
                    int readCount = _connection.Receive(inBytes);

                    string rawRequestData = new string(Encoding.UTF8.GetChars(inBytes, 0, readCount));

                    StringReader sR = new StringReader(rawRequestData);

                    while (sR.Peek() > -1)
                    {
                        string line = sR.ReadLine();

                        if (line.IndexOf("Content-Length", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            int.TryParse(line.Split(':')[1], out contentLength);

                            contentLength -= inBytes.Length;

                            break;
                        }
                    }
                }

                if (contentLength > 0)
                {
                    mS = new MemoryStream();

                    while (contentLength > 0)
                    {
                        byte[] inBytes = new byte[65535];
                        int rBC = inBytes.Length;
                        if (_connection.Available < inBytes.Length)
                            rBC = _connection.Available;

                        int readCount = _connection.Receive(inBytes, 0, rBC, SocketFlags.None);

                        if (readCount > 0)
                        {
                            mS.Write(inBytes, 0, readCount);

                            contentLength -= readCount;
                        }
                    }

                    mS.Seek(0, SeekOrigin.Begin);
                    
                    int rC = 0; byte[] outBytes = new byte[8192];
                    do
                    {
                        rC = mS.Read(outBytes, 0, outBytes.Length);

                        if (rC > 0)
                            Console.Write(Encoding.UTF8.GetChars(outBytes, 0, rC));
                    } while (rC > 0);
                }

                string header = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset = utf - 8\r\nConnection: close\r\n\r\n";

                _connection.SendTimeout = _connection.ReceiveTimeout;
                _connection.Send(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _connection.Close();

                if (mS != null)
                {
                    mS.Close();
                    GC.SuppressFinalize(mS);
                }
            }
        }
    }
}
