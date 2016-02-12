using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Function
    {
        public static int ChuSoTinTuong(double dk)
        {
            int i = 0;
            while (dk < 0.5 * Math.Pow(10, -i))
            {
                i++;
            }
            return i;
        }
    }
}
