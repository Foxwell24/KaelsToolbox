using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Tools
{
    public class Logger
    {
        private static object _lock = new object();

        private List<string> logs = new();
        public int frequency = 10;

        public Logger(int frequency = 10)
        {
            this.frequency = frequency;
            new Thread(() =>
            {
                while (true)
                {
                    if (logs.Count != 0)
                        lock (_lock)
                        {
                            Console.WriteLine(logs[0]);
                            logs.RemoveAt(0);
                        }
                    Thread.Sleep(frequency);
                }
            }).Start();
        }

        public void Log(object item)
        {
            lock (_lock) { logs.Add($"[{DateTime.Now.ToString("O")}]{item.ToString()}"); }
        }

    }
}
