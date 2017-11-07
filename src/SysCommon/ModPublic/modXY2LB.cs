using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace SysCommon
{
    public static class modXY2LB
    {
        public const double Parak0 = 1.57048687472752E-07;
        public const double Parak1 = 5.05250559291393E-03;
        public const double Parak2 = 2.98473350966158E-05;
        public const double Parak3 = 2.41627215981336E-07;
        public const double Parak4 = 2.22241909461273E-09;
        public const double ParaE2 = 6.73950181947292E-03;
        public const double ParaC = 6399596.65198801;
        public static void ComputeXYGeo(double x, double y, out double B, out double L, double center)
        {

            double y1;
            double bf;

            y1 = y - 38500000;

            double e;

            e = Parak0 * x;

            double se;

            se = Math.Sin(e);
            bf = e + Math.Cos(e) * (Parak1 * se - Parak2 * Math.Pow(se, 3) + Parak3 * Math.Pow(se, 5) - Parak4 * Math.Pow(se, 7));
            double v;
            double t;
            double N;
            double nl;
            double vt;
            double yn;
            double t2;
            double g;
            g = 1;
            t = Math.Tan(bf);

            nl = ParaE2 * Math.Pow(Math.Cos(bf), 2);
            v = Math.Sqrt(1 + nl);
            N = ParaC / v;
            yn = y1 / N;
            vt = Math.Pow(v, 2) * t;
            t2 = Math.Pow(t, 2);
            B = bf - vt * Math.Pow(yn, 2) / 2 + (5 + 3 * t2 + nl - 9 * nl * t2) * vt * Math.Pow(yn, 4) / 24 - (61 + 90 * t2 + 45 * Math.Pow(t2, 2)) * vt * Math.Pow(yn, 6) / 720;

            B = TransArcToDegree(B);

            double cbf;

            cbf = 1 / Math.Cos(bf);
            L = cbf * yn - (1 + 2 * t2 + nl) * cbf * Math.Pow(yn, 3) / 6 + (5 + 28 * t2 + 24 * Math.Pow(t2, 2) + 6 * nl + 8 * nl * t2) * cbf * Math.Pow(yn, 5) / 120 + center;
            L = TransArcToDegree(L);
        }

        public static double TransArcToDegree(double arc)
        {
            double sec = arc * mdlPublic.RHO;
            sec = double.Parse(sec.ToString("####.000000"));
            double TransArcToDegree = sec;
            return TransArcToDegree;
        }

        public static double Power(double dblX, double dblY)
        {
            double Power = Math.Pow(dblX, dblY);
            return Power;
        }
    }
   
}
