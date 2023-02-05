using KaelsToolBox_2.GameStuff;
using KaelsToolBox_2.Math;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Net;
using System.Text;

namespace Tester_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            BigNumber number = new();
            number.AddNumberAtPos(100);

            Console.WriteLine(number.Value);
            Console.ReadKey();
        }
    }
}