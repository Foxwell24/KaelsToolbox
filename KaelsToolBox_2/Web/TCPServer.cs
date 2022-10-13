using KaelsToolBox_2.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Web
{
    public class TCPServer
    {
        // need for all interactions with clients { lock (_lock) {..// do stuff with clients} }
        static readonly object _lock = new object();

        Logger logger;

        TcpListener listener;

        Dictionary<int, TcpClient> clients;

        public TCPServer(Logger? logger, int port = 25565)
        {
            this.logger = logger ?? new Logger();

            clients = new Dictionary<int, TcpClient>();
            listener = new TcpListener(IPAddress.Any, port);

            new Thread(() => ServerThread()).Start();
        }

        private void ServerThread()
        {
            listener.Start();

            int clientCounter = 0;
            while (true)
            {
                clientCounter++;
                TcpClient client = listener.AcceptTcpClient();

                lock (_lock)
                {
                    clients.Add(clientCounter, client);
                    new Thread(() => ClientThread(clientCounter)).Start();
                    logger.Log($"Client Connected ({clientCounter})");
                }
            }
        }

        private void ClientThread(int clientID)
        {
            bool connected = true;
            while (connected)
            {
                try
                {
                    NetworkStream stream = clients[clientID].GetStream();
                    Read(stream);
                }
                catch (Exception e)
                {
                    connected = false;
                    clients.Remove(clientID);
                    logger.Log(e.ToString());
                }
            }
        }

        private void Read(NetworkStream stream)
        {
            List<byte> completed = new();

            byte openBrace = Encoding.UTF8.GetBytes("{")[0];
            byte closeBrace = Encoding.UTF8.GetBytes("}")[0];
            int open = 0, closed = 0;
            while (open > closed && open != 0)
            {
                int receved = stream.ReadByte();
                if (receved == -1) break;

                if (receved == openBrace) open++; else if (receved == closeBrace) closed++;
                completed.Add((byte)receved);
            }
            string completedS = Encoding.UTF8.GetString(completed.ToArray());
            logger.Log(completedS);
        }
    }
}
