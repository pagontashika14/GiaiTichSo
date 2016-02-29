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
            Console.WriteLine((a).ToString());
            Console.WriteLine("============================");
            DaThuc test = null;
            double soDu = 0;
            test = a.DaoHam();
            Console.WriteLine(test.ToString());
            //Console.WriteLine("So du: {0}", soDu);
            Console.ReadLine();
        }
    }
}
