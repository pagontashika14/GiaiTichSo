using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsFxBang0
{
    public class Equation
    {
        #region Delegate
        public delegate void TinhHam(ref double fx, double x);
        TinhHam function;
        #endregion
        #region Members
        double m_f;
        #endregion
        #region Public Method
        public Equation()
        {

        }
        public Equation(TinhHam func)
        {
            DinhNghiaHam(func);
        }
        public void DinhNghiaHam(TinhHam func)
        {
            function = func;
        }
        public double F(double x)
        {
            m_f = 0;
            function(ref m_f, x);
            return m_f;
        }
        /// <summary>
        /// (a,b) là khoảng ly nghiệm. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon">Sai số cho phép</param>
        /// <returns></returns>
        public bool PhuongPhapChiaDoi(double a, double b, double epsilon, ref double x)
        {
            try
            {
                kiemTraInput(a, b, epsilon); ;
                while (b - a > epsilon)
                {
                    double c = (a + b) / 2;
                    if (F(c) == epsilon) break;
                    if (F(a) * F(c) > 0) a = c; else b = c;
                }
                x = (a + b) / 2;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion
        #region Private Method
        bool kiemTraInput(double a, double b, double epsilon)
        {
            if (a >= b)
            {
                throw new Exception("Xem lại khoảng li nghiệm");
            }
            else if (epsilon <= 0)
            {
                throw new Exception("epsilon <= 0");
            }
            else if (epsilon >= b - a)
            {
                throw new Exception("epsilon quá lớn");
            }
            return true;
        }
        #endregion
    }
}
