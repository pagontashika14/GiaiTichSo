using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NsMaTran;
using NsDaThuc;

namespace NsDaThucNoiSuy
{
    class DaThucNoiSuy
    {
        public DaThucNoiSuy(string file)
        {
            bangSo = new MaTran(file);
        }
        #region Members
        MaTran bangSo;
        #endregion
        #region Public
        public override string ToString()
        {
            return bangSo.ToString();
        }
        public DaThuc Lagrange()
        {
            var length = bangSo.N;
            var D1y = new MaTran(1, length);
            for (int i = 0; i < length; i++)
            {
                D1y[0, i] = 1;
            }
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    D1y[0, i] *= bangSo[0, i] - bangSo[0, j];
                }
                D1y[0, i] = bangSo[1, i] / D1y[0, i];
            }
            var ketQua = new DaThuc();
            for (int i = 0; i < length; i++)
            {
                var temp = new DaThuc();
                temp[0] = 1;
                for (int j = 0; j < length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    var temp2 = new DaThuc();
                    temp2[1] = 1;
                    temp2[0] = -bangSo[0, j];
                    temp = temp.NhanVoiDaThucBac1(temp2);
                }
                temp = temp * D1y[0, i];
                ketQua = ketQua + temp;
            }
            return ketQua;
        }
        public DaThuc NewtonTongQuat()
        {
            var length = bangSo.N;
            var bangTiHieu = new MaTran(length, length);
            for (int i = 0; i < length; i++)
            {
                bangTiHieu[i, 0] = bangSo[1, i];
            }
            for (int i = 1; i < length; i++)
            {
                for (int j = 0; j < length - i; j++)
                {
                    var tuSo = bangTiHieu[j + 1, i - 1] - bangTiHieu[j, i - 1];
                    var mauSo = bangSo[0, j + i] - bangSo[0, j];
                    bangTiHieu[j, i] = tuSo / mauSo;
                }
            }
            var ketQua = new DaThuc();
            ketQua[0] = bangTiHieu[0, 0];
            for (int i = 0; i < length - 1; i++)
            {
                var temp = new DaThuc();
                temp[0] = 1;
                for (int j = 0; j < i+1; j++)
                {
                    var temp2 = new DaThuc();
                    temp2[1] = 1;
                    temp2[0] = -bangSo[0, j];
                    temp = temp.NhanVoiDaThucBac1(temp2);
                }
                temp = temp * bangTiHieu[0,i+1];
                ketQua = ketQua + temp;
            }
            return ketQua;
        }
        #endregion
    }
}
