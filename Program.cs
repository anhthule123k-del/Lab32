using System;
using System.IO;
using System.Collections.Generic;

public class HinhHoc
{
    public double DienTich { get; set; }
    public double ChuVi { get; set; }

    public virtual void TinhDienTichChuVi()
    {
    }

    public override string ToString()
    {
        return string.Format(
            "S = {0:F2}; P = {1:F2}",
            DienTich,
            ChuVi);
    }
}

public class HinhChuNhat : HinhHoc
{
    private double mChieuDai;
    private double mChieuRong;

    public double ChieuDai
    {
        get { return mChieuDai; }
        set
        {
            if (value > 0)
                mChieuDai = value;
            else
                throw new Exception("Chieu dai phai > 0");
        }
    }

    public double ChieuRong
    {
        get { return mChieuRong; }
        set
        {
            if (value > 0)
                mChieuRong = value;
            else
                throw new Exception("Chieu rong phai > 0");
        }
    }

    public HinhChuNhat()
    {
        ChieuDai = 1;
        ChieuRong = 1;
    }

    public HinhChuNhat(double d, double r)
    {
        ChieuDai = d;
        ChieuRong = r;
    }

    public override void TinhDienTichChuVi()
    {
        DienTich = ChieuDai * ChieuRong;
        ChuVi = (ChieuDai + ChieuRong) * 2;
    }

    public override string ToString()
    {
        return string.Format(
            "L = {0}; W = {1}; S = {2:F2}; P = {3:F2}",
            ChieuDai,
            ChieuRong,
            DienTich,
            ChuVi);
    }
}

public class HinhTron : HinhHoc
{
    private double mBanKinh;

    public double BanKinh
    {
        get { return mBanKinh; }
        set
        {
            if (value > 0)
                mBanKinh = value;
            else
                throw new Exception("Ban kinh phai > 0");
        }
    }

    public HinhTron()
    {
        BanKinh = 1;
    }

    public HinhTron(double r)
    {
        BanKinh = r;
    }

    public override void TinhDienTichChuVi()
    {
        DienTich = Math.PI * BanKinh * BanKinh;
        ChuVi = 2 * Math.PI * BanKinh;
    }

    public override string ToString()
    {
        return string.Format(
            "R = {0}; S = {1:F2}; P = {2:F2}",
            BanKinh,
            DienTich,
            ChuVi);
    }
}

class Program
{
    static void Main()
    {
        List<HinhHoc> ds = new List<HinhHoc>();

        string[] lines = File.ReadAllLines("HinhHoc.txt");

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] arr = line.Split(
                new char[] { ' ', '\t' },
                StringSplitOptions.RemoveEmptyEntries);

            int loai = int.Parse(arr[0]);

            if (loai == 1)
            {
                double dai = double.Parse(arr[1]);
                double rong = double.Parse(arr[2]);

                HinhChuNhat hcn = new HinhChuNhat(dai, rong);
                hcn.TinhDienTichChuVi();

                ds.Add(hcn);
            }
            else if (loai == 2)
            {
                double r = double.Parse(arr[1]);

                HinhTron ht = new HinhTron(r);
                ht.TinhDienTichChuVi();

                ds.Add(ht);
            }
        }

        double tongDT = 0;
        double tongCV = 0;

        HinhChuNhat hcnMax = null;
        HinhTron htMin = null;

        foreach (HinhHoc h in ds)
        {
            tongDT += h.DienTich;
            tongCV += h.ChuVi;

            if (h is HinhChuNhat)
            {
                HinhChuNhat temp = (HinhChuNhat)h;

                if (hcnMax == null ||
                    temp.DienTich > hcnMax.DienTich)
                {
                    hcnMax = temp;
                }
            }

            if (h is HinhTron)
            {
                HinhTron temp = (HinhTron)h;

                if (htMin == null ||
                    temp.ChuVi < htMin.ChuVi)
                {
                    htMin = temp;
                }
            }
        }

        using (StreamWriter sw = new StreamWriter("output.txt"))
        {
            sw.WriteLine("So hinh: " + ds.Count);
            sw.WriteLine("Tong dien tich: " + tongDT.ToString("F2"));
            sw.WriteLine("Tong chu vi: " + tongCV.ToString("F2"));

            sw.WriteLine();

            if (hcnMax != null)
            {
                sw.WriteLine("Hinh chu nhat co dien tich lon nhat:");
                sw.WriteLine(hcnMax);
            }

            if (htMin != null)
            {
                sw.WriteLine("Hinh tron co chu vi nho nhat:");
                sw.WriteLine(htMin);
            }
        }

        Console.WriteLine("Da ghi ket qua vao output.txt");
    }
}