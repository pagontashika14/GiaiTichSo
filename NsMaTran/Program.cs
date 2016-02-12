using System;
using System.Collections.Generic;
using System.Text;

namespace NsMaTran
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new MaTran("test.txt");
            a.MTGhep = new MaTran("test2.txt");
            var b = a.DuaVeMaTranBacThang();
            Console.WriteLine(b.ToString());
            Console.ReadKey();
        }
    }
}
