using KaelsToolbox.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KaelsToolbox.Networking
{
    public class UDP_Server
    {
        // need for all interactions with clients { lock (_lock) {..// do stuff with clients} }
        static readonly object _lock = new object();

        UdpClient listener;
        IPEndPoint endPoint;
        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<GenericEventArgs<string>> PacketRecived;

        public UDP_Server(int port)
        {
            listener = new UdpClient(port);
            endPoint = new IPEndPoint(IPAddress.Any, port);

            StartListener();
        }

        private void StartListener()
        {
            try
            {
                while (true)
                {
                    byte[] bytes = listener.Receive(ref endPoint);
                    string data = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    //Send(data);
                    if (PacketRecived != null)
                    {
                        PacketRecived.Invoke("", new GenericEventArgs<string>(data));
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Caught Error : {e.ErrorCode}");
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
