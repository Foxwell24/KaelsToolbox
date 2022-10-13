using KaelsToolbox.Events;
using KaelsToolbox.Networking;
using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            UDP_Server uDP_Server = new UDP_Server(11000);
            Thread.Sleep(100);
            uDP_Server.PacketRecived += UDP_Server_PacketRecived;

            Console.ReadKey();
        }

        private static void UDP_Server_PacketRecived(object sender, GenericEventArgs<string> e)
        {
            Console.WriteLine($"Receved Packet : {e.Data}");
        }
    }
}
