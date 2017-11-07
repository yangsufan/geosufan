using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace SysCommon
{
    public  class mdlPublic
    { 
        public const double RHO = 206264.8062471;
        public const double ParaE1 =6.69438499958795E-03;
        public const double PI = 3.14159265358979;
        public const double ROU = 206264.8062471;
        public const double a = 6378140;
        public const double Arf = 1 / 298.257;
        public const double ef = 6.69438499958795E-03;
        public const double RadB = 6356755.29;

        public  struct LLAreaPro                //'地球椭球面积计算参数
        {
            public double m_Globea; 
            public double m_Glober;
            public double  m_Globeb;
            public double  m_A;
            public double m_B;
            public double m_C;
            public double  m_D;
            public double m_E;
            public double m_PI;
            public double m_dblB;
            public double m_dblL;
            public double m_conste;
        }
        public static void SetLLAreaPrama( ref LLAreaPro pLLAreaPro, double fMapScale)
        {
            //bool SetLLAreaPrama = true;
            try
            {
                pLLAreaPro.m_Globea = a;
                pLLAreaPro.m_Glober = Arf;
                pLLAreaPro.m_Globeb = RadB;
                pLLAreaPro.m_conste = ParaE1;
                pLLAreaPro.m_A = 1 + (3 / 6.0) * pLLAreaPro.m_conste + (30 / 80.0) * Math.Pow(pLLAreaPro.m_conste, 2) + (35 / 112.0) * Math.Pow(pLLAreaPro.m_conste, 2) + (630 / 2304.0) * Math.Pow(pLLAreaPro.m_conste, 4);
                pLLAreaPro.m_B = (1 / 6.0) * pLLAreaPro.m_conste + (15 / 80.0) * Math.Pow(pLLAreaPro.m_conste, 2) + (21 / 112.0) * Math.Pow(pLLAreaPro.m_conste, 3) + (420 / 2304.0) * Math.Pow(pLLAreaPro.m_conste, 4);
                pLLAreaPro.m_C = (3 / 80.0) * Math.Pow(pLLAreaPro.m_conste, 2) + (7 / 112.0) * Math.Pow(pLLAreaPro.m_conste, 3) + (180 / 2304.0) * Math.Pow(pLLAreaPro.m_conste, 4);
                pLLAreaPro.m_D = (1 / 112.0) * Math.Pow(pLLAreaPro.m_conste, 3) + (45 / 2304.0) * Math.Pow(pLLAreaPro.m_conste, 4);
                pLLAreaPro.m_E = (5 / 2304.0) * Math.Pow(pLLAreaPro.m_conste, 4);
                pLLAreaPro.m_PI = PI;

                pLLAreaPro.m_dblB = 2.5 * 3600 / RHO;
                pLLAreaPro.m_dblL = 3.75 * 3600 / RHO;
                //return SetLLAreaPrama; return SetLLAreaPrama = false;
            }
            catch { }
  
        }
    }
}
