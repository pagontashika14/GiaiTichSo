using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace NsDaThuc
{
    class DaThuc
    {
        #region Constructor
        public DaThuc()
        {
            daThuc = new List<double>();
        }
        public DaThuc(int n)
        {
            daThuc = new List<double>();
            for (int i = 0; i < n + 1; i++)
            {
                daThuc.Add(0);
            }
        }
        public DaThuc(string file)
        {
            var txt = new ReadTxt(file);
            var temp = txt.TachPhanTuSo()[0].ToList();
            temp.Reverse();
            daThuc = temp;
        }
        public double this[int index]
        {
            get
            {
                if (index > N)
                {
                    return 0;
                }
                else
                {
                    return daThuc[index];
                }
            }
            set
            {
                if (index > N)
                {
                    if (value != 0)
                    {
                        int soLuongAddThem = index - N;
                        for (int i = 0; i < soLuongAddThem; i++)
                        {
                            daThuc.Add(0);
                        }
                        daThuc[index] = value;
                    }
                }
                else
                {
                    daThuc[index] = value;
                }
            }
        }
        #endregion
        #region Member
        List<double> daThuc;
        public int N
        {
            get
            {
                tinhLaiCap();
                int n = daThuc.Count;
                return n - 1;
            }
        }
        #endregion
        #region Public
        public DaThuc Copy()
        {
            var B = new DaThuc(N);
            for (int i = 0; i < N + 1; i++)
            {
                B[i] = this[i];
            }
            return B;
        }
        public override string ToString()
        {
            string s = string.Empty;
            if (N == -1)
            {
                s = "0";
            }
            else
            {
                if (N == 0)
                {
                    s += this[N].ToString();
                }
                else
                {
                    s += string.Format("{0}.x^{1} ", this[N], N);
                }
                for (int i = N - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        if (this[i] == 0)
                        {
                            continue;
                        }
                        string dau = this[i] > 0 ? "+" : "-";
                        s += string.Format("{0} {1}", dau, Math.Abs(this[i]));
                        break;
                    }
                    if (this[i] == 0)
                    {
                        continue;
                    }
                    else if (this[i] > 0)
                    {
                        s += string.Format("+ {0}.x^{1} ", this[i], i);
                    }
                    else
                    {
                        s += string.Format("- {0}.x^{1} ", -this[i], i);
                    }
                }
            }
            return s;
        }
        public static DaThuc operator +(DaThuc A, DaThuc B)
        {
            return A.Cong(B);
        }
        public static DaThuc operator -(DaThuc A)
        {
            return A.DoiDau();
        }
        public static DaThuc operator -(DaThuc A, DaThuc B)
        {
            return A.Tru(B);
        }
        public static DaThuc operator *(DaThuc A, double d)
        {
            return PhepTinhVoiSo(A, (ref double ketQua) =>
             {
                 ketQua = ketQua * d;
             });
        }
        public static DaThuc operator /(DaThuc A, double d)
        {
            if (d == 0)
            {
                throw new Exception("Không thể chia cho 0");
            }
            return PhepTinhVoiSo(A, (ref double ketQua) =>
             {
                 ketQua = ketQua / d;
             });
        }
        public DaThuc Cong(DaThuc B)
        {
            DaThuc lon = null;
            DaThuc be = null;
            kiemTraCapDo(this, B, ref lon, ref be);
            DaThuc C = lon.Copy();
            for (int i = 0; i < be.N + 1; i++)
            {
                C[i] += B[i];
            }
            return C;
        }
        public DaThuc DoiDau()
        {
            var C = new DaThuc(N);
            for (int i = 0; i < N + 1; i++)
            {
                C[i] = -this[i];
            }
            return C;
        }
        public DaThuc Tru(DaThuc B)
        {
            DaThuc lon = null;
            DaThuc be = null;
            kiemTraCapDo(this, B, ref lon, ref be);
            DaThuc C;
            if (lon == this)
            {
                C = this.Copy();
                C = C.Cong(B.Copy().DoiDau());
            }
            else
            {
                C = B.Copy().DoiDau();
                C = C.Cong(this);
            }
            return C;
        }
        public void Horner(double c, ref DaThuc KetQua, ref double SoDu)
        {
            if (N < 2)
            {
                throw new Exception("Bậc của đa thức nhỏ hơn 2");
            }
            KetQua = new DaThuc(N - 1);
            KetQua[N - 1] = this[N];
            for (int i = KetQua.N - 1; i >= 0; i--)
            {
                KetQua[i] = c * KetQua[i + 1] + this[i + 1];
            }
            SoDu = c * KetQua[0] + this[0];
        }
        public DaThuc ChiaChoDaThucBac1(DaThuc B, ref double soDu)
        {
            if (B.N > 1)
            {
                throw new Exception("Bậc của đa thức chia lớn hơn 1");
            }
            if (B[1] == 0)
            {
                return this / B[0];
            }
            var biChia = this.Copy();
            if (B[1] != 1)
            {
                biChia = biChia / B[1];
                B = B / B[1];
            }
            DaThuc ketQua = null;
            biChia.Horner(-B[0], ref ketQua, ref soDu);
            return ketQua;
        }
        public DaThuc NhanVoiDaThucBac1(DaThuc B)
        {
            if (B.N > 1)
            {
                throw new Exception("Bậc của đa thức nhân lớn hơn 1");
            }
            if (B[1] == 0)
            {
                return this * B[0];
            }
            var goc = this.Copy();
            if (B[1] != 1)
            {
                goc = goc * B[1];
                B = B / B[1];
            }
            var ketQua = new DaThuc();
            ketQua[0] = B[0] * goc[0];
            for (int i = 1; i < goc.N + 1; i++)
            {
                ketQua[i] = goc[i - 1] + B[0] * goc[i];
            }
            ketQua[goc.N + 1] = goc[goc.N];
            return ketQua;
        }
        public DaThuc DaoHam()
        {
            var ketQua = new DaThuc();
            for (int i = 0; i < N; i++)
            {
                ketQua[i] = this[i + 1] * (i + 1);
            }
            return ketQua;
        }
        #endregion
        #region Private
        void kiemTraCapDo(DaThuc A, DaThuc B, ref DaThuc lon, ref DaThuc be)
        {
            if (A.N > B.N)
            {
                lon = A;
                be = B;
            }
            else
            {
                lon = B;
                be = A;
            }
        }
        void tinhLaiCap()
        {
            int i = daThuc.Count - 1;
            if (i >= 0)
            {
                while (daThuc[i] == 0)
                {
                    daThuc.RemoveAt(i);
                    i--;
                    if (i < 0) break;
                }
            }

        }
        delegate void phepTinhVoiSo(ref double ketQua);
        static DaThuc PhepTinhVoiSo(DaThuc A, phepTinhVoiSo phepTinh)
        {
            var B = new DaThuc(A.N);
            for (int i = 0; i < A.N + 1; i++)
            {
                double temp = B[i];
                phepTinh(ref temp);
                B[i] = temp;
            }
            return B;
        }
        #endregion
    }
}
