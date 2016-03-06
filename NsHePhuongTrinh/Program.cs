using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NsMaTran;

namespace NsHePhuongTrinh
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var he = new HePhuongTrinh(@"HePhuongTrinh\heBaDuongCheo.txt");
            Console.WriteLine(he.ToString());
            //Console.WriteLine("--------- Phương pháp Gauss ----------");
            //Console.WriteLine(he.Gauss().ToString());
            //Console.WriteLine("--------- Phương pháp Gauss-Jordan ---");
            //Console.WriteLine(he.GaussJordan().ToString());
            //Console.WriteLine("--------- Phương pháp lặp Jacobi -----");
            //Console.WriteLine(he.LapJacobi(0.02).ToString());
            Console.WriteLine(he.BaDuongCheo().ToString());
            Console.ReadKey();
        }
    }
}
