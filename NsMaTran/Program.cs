using System;
using System.Collections.Generic;
using System.Text;

namespace NsMaTran
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new MaTran(@"MaTran\a.txt");
            var b = new MaTran(@"MaTran\b.txt");
            Console.WriteLine((a * b).ToString());
            Console.ReadKey();
        }
    }
}
