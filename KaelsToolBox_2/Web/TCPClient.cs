using KaelsToolBox_2.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Web
{
    public class TCPClient
    {
        private Logger logger;

        //private List<string> LIST OF OBJECT TO SEND???

        public TCPClient(Logger? logger, string hostname, int port = 25565)
        {
            this.logger = logger ?? new Logger();

            new Thread(() =>
            {
                TcpClient self = new TcpClient(hostname, port);
                while (true)
                {
                    try
                    {
                        NetworkStream stream = self.GetStream();
                        Read(logger, stream);
                        // SEND STUFF

                    }
                    catch (Exception e)
                    {
                        logger.Log(e.ToString());
                    }
                }
            }).Start();
        }

        private static void Read(Logger? logger, NetworkStream stream)
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
