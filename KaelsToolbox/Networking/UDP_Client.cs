using KaelsToolbox.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace KaelsToolbox.Networking
{
    public class UDP_Client
    {
        Socket socket;
        IPEndPoint endPoint;

        public event EventHandler<GenericEventArgs<string>> PacketReceved;

        public UDP_Client(IPAddress ipAdress, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            endPoint = new IPEndPoint(ipAdress, port);

            //WebClient webClient = new WebClient();
            //string publicIp = webClient.DownloadString("https://api.ipify.org");
            //Console.WriteLine($"My public IP Address is : {publicIp}");
        }

        public void Send(byte[] buffer)
        {
            socket.SendTo(buffer, endPoint);
        }
    }
}
