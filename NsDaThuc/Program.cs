using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NsDaThuc
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new DaThuc(@"DaThuc\a.txt");
            var b = new DaThuc(@"DaThuc\b.txt");
            Console.WriteLine((a).ToString());
            Console.WriteLine((b).ToString());
            Console.WriteLine("============================");
            Console.WriteLine((a+b).ToString());
            Console.ReadLine();
        }
    }
}
