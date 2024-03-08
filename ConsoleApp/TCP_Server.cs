using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using KaelsToolBox_2.Tools;
using Org.BouncyCastle.Utilities;

namespace ConsoleApp;

public class DataReceivedEventArgs : EventArgs
{
    public byte[] Data { get; set; }
    public TcpClient Client { get; set; }

    public DataReceivedEventArgs(TcpClient client, byte[] data)
    {
        Data = data;
        Client = client;
    }
}


public class TcpConnectionHandler
{
    public event EventHandler<DataReceivedEventArgs>? DataReceived;

    DataSaver<TcpClient> saver = new();

    private TcpListener listener;

    public TcpConnectionHandler(string ipAddress, int port)
    {
        IPAddress localAddr = IPAddress.Parse(ipAddress);
        listener = new TcpListener(localAddr, port);

        saver.Created += (_, client) => ClientThread(client);
        saver.Deleted += (_, client) => Console.WriteLine("Client Disconnected");
    }

    public void StartListening()
    {
        listener.Start();

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client Connected");

            saver += client;
        }
    }

    public void StopListening()
    {
        listener.Stop();
    }

    private Task ClientThread(TcpClient client)
    {
        while (true)
        {
            try
            {
                // Get the network stream from the client
                NetworkStream stream = client.GetStream();

                int i = GetPacketSize(stream);

                byte[] bytes = new byte[i];
                stream.Read(bytes, 4, i);

                DataReceived?.Invoke(this, new DataReceivedEventArgs(client, bytes));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                break;
            }
        }

        saver -= client;
        return Task.CompletedTask;
    }



    /// <summary>
    /// Gets the size of the packet from the provided NetworkStream
    /// </summary>
    /// <param name="stream">The NetworkStream to read from</param>
    /// <returns>The size of the packet as an integer</returns>
    static int GetPacketSize(NetworkStream stream)
    {
        // Read 4 bytes from the stream
        byte[] numBytesToRead = new byte[4];
        stream.Read(numBytesToRead, 0, 4);

        // If the system architecture is little endian, reverse the byte array
        if (BitConverter.IsLittleEndian)
            Array.Reverse(numBytesToRead);

        // Convert the byte array to an integer
        int i = BitConverter.ToInt32(numBytesToRead, 0);
        return i;
    }
}
