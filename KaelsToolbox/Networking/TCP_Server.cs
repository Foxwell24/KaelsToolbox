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
    /// <summary>
    /// How to process a packet
    /// </summary>
    public enum PacketSendType
    {
        /// <summary>
        /// Sent to no one and just processed by the server
        /// </summary>
        Server,
        /// <summary>
        /// Sent to a singular client
        /// </summary>
        Invividual,
        /// <summary>
        /// Sent to all connected clients
        /// </summary>
        Everyone,
        /// <summary>
        /// sent to a list of clients
        /// </summary>
        Group
    }

    public class TCP_Server
    {
        // need for all interactions with clients { lock (_lock) {..// do stuff with clients} }
        static readonly object _lock = new object();

        TcpListener listener;

        Dictionary<int, TcpClient> clients;
        List<byte[]> serverCommands = new List<byte[]>();

        #region Events
        /// <summary>
        /// Triggerd whenever a client connects. Sender = clientID. Args = client.RemoteEndPoint
        /// </summary>
        public event EventHandler<GenericEventArgs<string>> ClientConnected;
        /// <summary>
        /// Triggerd whenever a client disconnects. Sender = clientID. Args = Reason or stacktrace
        /// </summary>
        public event EventHandler<GenericEventArgs<string>> ClientDisconnected;
        /// <summary>
        /// Triggerd whenever a Packet is receved from a client. Sender = clientID. Args = data.string
        /// </summary>
        public event EventHandler<GenericEventArgs<byte[]>> PacketReceved;
        /// <summary>
        /// Triggerd when eror happens without crashing. Sender = null. Args = stacktrace
        /// </summary>
        public event EventHandler<GenericEventArgs<string>> Eror;
        #endregion

        public TCP_Server(int port)
        {
            clients = new Dictionary<int, TcpClient>();
            listener = new TcpListener(IPAddress.Any, port);
            Thread serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        private void ServerThread()
        {
            int clientCount = 0;

            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ClientConnected.Invoke(clientCount, new GenericEventArgs<string>(client.Client.RemoteEndPoint.ToString()));
                lock (_lock)
                {
                    clients.Add(clientCount, client);
                    Thread clientThread = new Thread(() =>
                    {
                        ClientThread(client, clientCount);
                    });
                }
                clientCount++;
            }
        }

        /// <summary>
        /// InfoBytes:<para/>
        ///     Byte 1 = <see cref="PacketSendType"/>.<para/>
        ///     Byte 2 = How many clients will be listed in Data.<para/>
        ///     Byte 3,4,5,6 = Number of bytes in Data.<para/>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="clientID"></param>
        private void ClientThread(TcpClient client, int clientID)
        {
            string disconectReason = "";

            int byteInfoSize = 6;
            while (true)
            {
                try
                {
                    if (!client.Client.Connected)
                    {
                        disconectReason = "client.Connected == false";
                        break;
                    }

                    NetworkStream stream = client.GetStream();

                    // How many bytes to receve //
                    byte[] buffer_info = new byte[byteInfoSize];
                    int byteCount = stream.Read(buffer_info, 0, buffer_info.Length);
                    if (byteCount < 6)
                    {
                        disconectReason = $"byteCount == {byteCount}. needs to be 6";
                        break;
                    }

                    // Get sendType //
                    PacketSendType packetSendType;
                    try
                    {
                        packetSendType = (PacketSendType)buffer_info[0];
                    }
                    catch (Exception e)
                    {
                        packetSendType = PacketSendType.Server;
                        Eror.Invoke(null, new GenericEventArgs<string>(e.StackTrace));
                    }

                    // Get byte 2 //
                    int numClientsInData = buffer_info[1];
                    // Get byte 3,4,5,6 //
                    byte[] intConvert = new byte[] { buffer_info[2], buffer_info[3], buffer_info[4], buffer_info[5] };
                    // Convert byte[] into int //
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(intConvert);
                    int numBytesInData = BitConverter.ToInt32(intConvert, 0);


                    // receve said number of bytes //
                    byte[] buffer_data = new byte[numBytesInData];
                    byteCount = stream.Read(buffer_data, 0, buffer_data.Length);
                    if (byteCount == 0)
                    {
                        disconectReason = $"byteCount == {byteCount}";
                        break;
                    }
                    // Get promised client id's //
                    List<int> clients = new List<int>();
                    List<byte> newData = new List<byte>();
                    for (int i = 0; i < buffer_data.Length; i++)
                    {
                        if (i < numClientsInData)
                        {
                            clients.Add(buffer_data[i]);
                        }
                        else
                        {
                            newData.Add(buffer_data[i]);
                        }
                    }
                    buffer_data = newData.ToArray();


                    PacketReceved.Invoke(clientID, new GenericEventArgs<byte[]>(buffer_data));

                    SendPackets(packetSendType, clients, buffer_data);
                }
                catch (Exception e)
                {
                    disconectReason = 
                        $"something went wrong in the clientThread loop. Exception StackTrace : \n{e.StackTrace}";
                }
            }
            Disconect(client, clientID, disconectReason);
        }

        private void SendPackets(PacketSendType sendType, List<int> clientIDs, byte[] buffer_data)
        {
            switch (sendType)
            {
                case PacketSendType.Everyone:
                    {
                        lock (_lock)
                        {
                            foreach (var client in clients)
                            {
                                if (client.Key == clientIDs[0]) continue;
                                NetworkStream stream = client.Value.GetStream();
                                stream.Write(buffer_data, 0, buffer_data.Length);
                            }
                        }
                    }
                    break;
                case PacketSendType.Group:
                    {
                        lock (_lock)
                        {
                            foreach (var client in clients)
                            {
                                if (!clientIDs.Contains(client.Key)) continue;
                                NetworkStream stream = client.Value.GetStream();
                                stream.Write(buffer_data, 0, buffer_data.Length);
                            }
                        }
                    }
                    break;
                case PacketSendType.Server:
                    {
                        serverCommands.Add(buffer_data);
                    }
                    break;
                case PacketSendType.Invividual:
                    {
                        foreach (var client in clients)
                        {
                            if (client.Key != clientIDs[0]) continue;
                            NetworkStream stream = client.Value.GetStream();
                            stream.Write(buffer_data, 0, buffer_data.Length);
                        }
                    }
                    break;
            }
        }

        private void Disconect(TcpClient client, int clientID, string reason)
        {
            client.GetStream().Close();
            client.Close();
            clients.Remove(clientID);
            ClientDisconnected.Invoke(clientID, new GenericEventArgs<string>(reason));
        }
    }
}
