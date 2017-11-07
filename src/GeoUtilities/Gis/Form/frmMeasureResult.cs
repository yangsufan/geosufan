using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using SysCommon.Error;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace GeoUtilities
{
    public partial class frmMeasureResult : DevComponents.DotNetBar.Office2007Form
    {
        //字段
        private esriUnits m_Units=esriUnits.esriMeters ;        //当前数值的单位
        private enumMyUnits m_MyUnits = enumMyUnits.Meters;     //added by chulili 自定义单位
        public bool m_bShowSum;　　　   //标志是否显示总和

        private double m_SegLength;　　　  // 活动段的长度
        private enum enumMyUnits
        {
            Meters,
            KiloMeters,
            Mu,         //亩
            Hectares    //公顷
        }
        public double dblSegLength
        {
            set
            {
                if (value < 0)
                    m_SegLength = System.Math.Abs(value);
                else
                    m_SegLength = value;
            }
            get
            {
                return m_SegLength;
            }
        }

        private double m_Length;　　　　　 //线段总长度
        public double dblLength
        {
            set
            {
                if (value < 0)
                    m_Length = System.Math.Abs(value);
                else
                    m_Length = value;
            }
            get
            {
                return m_Length;
            }
        }

        private double m_Area;            //多边形的面积
        public double dblArea
        {
            set
            {
                 if (value < 0)
                   m_Area = System.Math.Abs(value);
                else
                    m_Area = value;
                 //if (m_Units == esriUnits.esriKilometers)
                 //    m_Area = m_Area /1000000;
              
            }
            get
            {
                return m_Area;
            }
        }
        private double m_SumLength;       //总长度
        public double dblSumLength
        {
            set
            {
                if (value < 0)
                    m_SumLength = System.Math.Abs(value);
                else
                    m_SumLength = value;
            }
            get
            {
                return m_SumLength;
            }
        }
        private double m_SumArea=0;         //总面积
        public double dblSumArea
        {
            set
            {
                //if (m_Units == esriUnits.esriKilometers)
                //    value = value / 1000000;
                if (value < 0)
                    m_SumArea = System.Math.Abs(value);
                else
                    m_SumArea = value;
            }
            get
            {
                return m_SumArea;
            }
        }
        public   short   m_CurMeasureType;  //标志当前量算的对象类型:0为线；1为面;2为角度
        public bool m_bSnapToFeature;  //标志是否捕捉要素节点
        public bool m_bIsFeatureMeasure;  //标志是否对要素进行测量

        /////guozhen 2011-5-8 added 实际地图坐标
        private esriUnits m_Displayunit = esriUnits.esriUnknownUnits;
        ControlsMapMeasureToolDefClass pTool;

        public double ang = 0;//yjl20110816 add 角度测量的角度

        //构造函数
        public frmMeasureResult( esriUnits in_DisplayUnit,ControlsMapMeasureToolDefClass inTool)
        {
            InitializeComponent();
            m_CurMeasureType = 0;
            m_bIsFeatureMeasure = false;
            toolMeasureArea.Checked = false;
            toolMeasureFeature.Checked = false;
            toolMeasureLine.Checked = true; 

            m_bShowSum = false;
            m_bSnapToFeature = false;
            toolShowSum.Checked = false;
            toolSnapToFeature.Checked = false;
            toolM.Checked = true;//选中M
            toolKM.Checked = false;
           // toolM.ReadOnly = true;
            //toolSquareM.ReadOnly = true;
            /////
            this.m_Displayunit = in_DisplayUnit;
            if (in_DisplayUnit == esriUnits.esriUnknownUnits)//地图单位
            {
                m_Displayunit = esriUnits.esriMeters;
            }
            pTool = inTool;
        }
        private string GetMyLenUnitsName(enumMyUnits pUnit)
        {
            
            switch (pUnit)
            { 
                case enumMyUnits.Meters:
                    return "米";
                case enumMyUnits.KiloMeters:
                    return "千米";
                case enumMyUnits.Mu:
                    return "米";
                case enumMyUnits.Hectares:
                    return "米";
                default :
                    return "平方米";
                    break;

            }
        }
        private string GetMyAreaUnitsName(enumMyUnits pUnit)
        {

            switch (pUnit)
            {
                case enumMyUnits.Meters:
                    return "平方米";
                case enumMyUnits.KiloMeters:
                    return "平方千米";
                case enumMyUnits.Mu:
                    return "亩";
                case enumMyUnits.Hectares:
                    return "公顷";
                default:
                    return "平方米";
                    break;

            }
        }
        //获得单位
        private string GetUnitsName(esriUnits nUnit)
        {
            switch ((int)nUnit)
            {
                case 1:
                    return "英寸";
                case 3:
                    return "英尺";
                case 7:
                    return "毫米";
                case 8:
                    return "厘米";
                case 9:
                    return "米";
                case 10:
                    return "千米";
                case 12:
                    return "分米";
                default:
                    return "未知";
            }
        }

        //显示结果到窗体上
        public  void ShowResult(double segLen,double len,double  SumLen,double Area,double SumArea,bool IsLine)
        {
            string sResult = "";
            string sUnit_len = "";
            string sUnit_Area = "";
            sUnit_len = this.GetMyLenUnitsName(m_MyUnits);
            sUnit_Area = this.GetMyAreaUnitsName(m_MyUnits);
            ////////////////////判断坐标单位是否一致////////////////////////
            double dSum = SumLen;
            double bCurrentlen = segLen;
            double dlen = len;

            enumMyUnits pToUnit = m_MyUnits;
            double show_curCirclelen = dlen;
            double show_curlen = bCurrentlen;
            double show_sumlen = dSum;
            double show_curarea = Area;
            double show_sumarea = SumArea;
            if (pToUnit == enumMyUnits.Meters)
            {

            }
            else if (pToUnit == enumMyUnits.KiloMeters)
            {
                show_curCirclelen = show_curCirclelen / 1000;
                show_curlen = show_curlen / 1000;
                show_sumlen = show_sumlen / 1000;
                show_curarea = show_curarea / 1000000;
                show_sumarea = show_sumarea / 1000000;
            }
            else if (pToUnit == enumMyUnits.Mu)
            {
                show_curarea = show_curarea * 3 / 2000;
                show_sumarea = show_sumarea * 3 / 2000;
            }
            else if (pToUnit == enumMyUnits.Hectares)
            {
                show_curarea = show_curarea / 10000;
                show_sumarea = show_sumarea / 10000;
            }
           
            //原有代码
            //if (this.m_Displayunit != esriUnits.esriUnknownUnits)
            //{
            //    if (this.m_Displayunit != this.m_Units)
            //    {
            //        dSum = UnitConvert(dSum);
            //        bCurrentlen = UnitConvert(bCurrentlen);
            //        dlen = UnitConvert(dlen);
            //        //Area = UnitConvert(Area);
            //        //SumArea = UnitConvert(SumArea);
            //    }
            //}
            ////////////////////////////////////////////////////////////////
            //xisheng 20111118 end************************************end

            if (IsLine==true )
            {
                sResult = "线段测量结果：\n当前弧段长度：" +Math.Round( show_curCirclelen,3) + sUnit_len
                    + "\n当前线段长度：" + Math.Round(show_curlen,3) + sUnit_len;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总长度：" + Math.Round(show_sumlen,3) + sUnit_len;
            }
            else 
            {
                sResult = "多边形测量结果：\n当前弧段长度：" + Math.Round(show_curlen,3) + sUnit_len
                            + "\n当前多边形周长：" + Math.Round(show_curCirclelen,3) + sUnit_len
                            + "\n当前多边形面积：" + Math.Round(show_curarea,3)   + sUnit_Area ;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总周长：" + Math.Round(show_sumlen,3) + sUnit_len
                                + "\n总面积：" + Math.Round(show_sumarea, 3) + sUnit_Area;
            }
            this.lblResult.Text = sResult;  

        }
        //显示结果到窗体上yjl20110816 add
        public void ShowResult()
        {

            this.lblResult.Text = "和X正轴夹角：" + ang.ToString("f2") + "°";

        }
        ////yjl20110816 modify.1线2面3角度
        //public voidShowResult(int meaType)
        //{
        //    string sResult = "";
        //    string sUnit = "";
        //    sUnit = this.GetUnitsName(m_Units);
        //    ////////////////////判断坐标单位是否一致////////////////////////
        //    double dSum = this.dblSumLength;
        //    double bCurrentlen = this.dblSegLength;
        //    double dlen = this.dblLength;
            
        //    //如果是千米 就转换将测出的结果转换成千米 xisheng 20111118
        //    if (m_Units == esriUnits.esriKilometers)
        //    {
        //        dSum = dSum / 1000;
        //        bCurrentlen = bCurrentlen / 1000;
        //        dlen = dlen / 1000;
        //    }

        //    //原有代码
        //    //if (this.m_Displayunit != esriUnits.esriUnknownUnits)
        //    //{
        //    //    if (this.m_Displayunit != this.m_Units)
        //    //    {
        //    //        dSum = UnitConvert(dSum);
        //    //        bCurrentlen = UnitConvert(bCurrentlen);
        //    //        dlen = UnitConvert(dlen);
        //    //    }
        //    //}
        //    ////////////////////////////////////////////////////////////////
        //    //xisheng 20111118 end************************************

        //    if (meaType == 1)
        //    {
        //        sResult = "线段测量结果：\n当前弧段长度：" + bCurrentlen + sUnit
        //            + "\n当前线段长度：" + dlen + sUnit;
        //        if (this.m_bShowSum == true)
        //            sResult += "\n\n总长度：" + dSum + sUnit;
        //    }
        //    else if (meaType == 2)
        //    {
        //        sResult = "多边形测量结果：\n当前弧段长度：" + bCurrentlen + sUnit
        //                    + "\n当前多边形周长：" + dlen + sUnit
        //                    + "\n当前多边形面积：" + this.dblArea + "平方" + sUnit;
        //        if (this.m_bShowSum == true)
        //            sResult += "\n\n总周长：" + dSum + sUnit
        //                        + "\n总面积：" + this.dblSumArea + "平方" + sUnit;
        //    }
        //    else if (meaType==3)
        //    {
        //        sResult = "和X正轴夹角：" + ang.ToString("f2") + "°";
        //    }

        //    //重新画了之后把之前的面积置于0 xisheng 20111118
        //    sumarealast = 0;
        //    arealast = 0; 
        //    //**********************************************
        //    this.lblResult.Text = sResult;
        //}

        //xisheng 20111118 add.1线2面3角度 KTM是千米改变到米**********************
        double sumarealast = 0;
        double arealast = 0;
        public void ShowResult(int meaType,bool start)
        {
            string sResult = "";
            string sUnit_len = "";
            string sUnit_Area = "";
            sUnit_len = this.GetMyLenUnitsName(m_MyUnits);
            sUnit_Area = this.GetMyAreaUnitsName(m_MyUnits);
            enumMyUnits pToUnit = m_MyUnits;
            ////////////////////判断坐标单位是否一致////////////////////////
            double dSum = this.dblSumLength;
            double bCurrentlen = this.dblSegLength;
            double dlen = this.dblLength;
            
            //xs针对单位变化的时候来调整数据的 xisheng 2011117
            double area = dblArea;
            double sumarea = dblSumArea;

            double show_curCirclelen = dlen;
            double show_curlen = bCurrentlen;
            double show_sumlen = dSum;
            double show_curarea = area;
            double show_sumarea = sumarea;
            if (pToUnit == enumMyUnits.Meters)
            {
                if (arealast != 0)
                {
                    area = arealast;
                    sumarea = sumarealast;
                }
                else
                {
                    arealast = area ;
                    sumarealast = sumarea;
                    //dblSumArea = dblSumArea * 1000000;
                }
            }
            else if (pToUnit == enumMyUnits.KiloMeters)
            {
                if (arealast != 0)
                {
                    area = arealast;
                    sumarea = sumarealast;
                }
                else
                {
                    arealast = area;
                    sumarealast = sumarea ;
                    //dblSumArea = dblSumArea / 1000000;
                }
                show_curCirclelen = show_curCirclelen/1000;
                show_curlen = show_curlen / 1000;
                show_sumlen = show_sumlen / 1000;
                show_curarea = show_curarea / 1000000;
                show_sumarea = show_sumarea / 1000000;
            }
            else if (pToUnit == enumMyUnits.Mu)
            {
                if (arealast != 0)
                {
                    area=arealast ;
                    sumarea=sumarealast ;
                }
                else
                {
                    arealast = area;
                    sumarealast = sumarea;
                    //dblSumArea = dblSumArea / 1000000;
                }
                show_curarea = show_curarea*3/2000;
                show_sumarea = show_sumarea * 3/2000;
            }
            else if (pToUnit == enumMyUnits.Hectares)
            {
                if (arealast != 0)
                {
                    area = arealast;
                    sumarea = sumarealast;
                }
                else
                {
                    arealast = area;
                    sumarealast = sumarea;
                    //dblSumArea = dblSumArea / 1000000;
                }
                show_curarea = show_curarea/10000;
                show_sumarea = show_sumarea/10000;
            }

            //xs***************************************************end

            if (meaType == 1)
            {
                sResult = "线段测量结果：\n当前弧段长度：" + Math.Round(show_curCirclelen,3) + sUnit_len
                    + "\n当前线段长度：" + Math.Round(show_curlen,3) + sUnit_len;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总长度：" + Math.Round(show_sumlen,3) + sUnit_len;
            }
            else if (meaType == 2)
            {
                sResult = "多边形测量结果：\n当前弧段长度：" + Math.Round(show_curCirclelen,3) + sUnit_len
                            + "\n当前多边形周长：" + Math.Round(show_curlen,3) + sUnit_len
                            + "\n当前多边形面积：" + Math.Round(show_curarea,3)  + sUnit_Area ;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总周长：" + Math.Round(show_sumlen,3) + sUnit_len
                                + "\n总面积：" + Math.Round(show_sumarea, 3) + sUnit_Area;
            }
            else if (meaType == 3)
            {
                sResult = "和X正轴夹角：" + ang.ToString("f2") + "°";
            }
            if (start)
            {
                //重新画了之后把之前的面积置于0 xisheng 20111118
                sumarealast = 0;
                arealast = 0; 
            }
            this.lblResult.Text = sResult;
        }
        //public void ShowResult(int meaType,bool KTM)
        //{
        //    string sResult = "";
        //    string sUnit = "";
        //    sUnit = this.GetUnitsName(m_Units);
        //    ////////////////////判断坐标单位是否一致////////////////////////
        //    double dSum = this.dblSumLength;
        //    double bCurrentlen = this.dblSegLength;
        //    double dlen = this.dblLength;
           
        //    //xs针对单位变化的时候来调整数据的 xisheng 2011117
        //    double area = dblArea;
        //    double sumarea = dblSumArea;
        //    if (KTM)
        //    {
        //        if (arealast != 0)
        //        {
        //            arealast = area = arealast * 1000000;
        //            sumarealast = sumarea = sumarealast * 1000000;
        //        }
        //        else
        //        {
        //            arealast = area = area * 1000000;
        //            sumarealast = sumarea = sumarea * 1000000;
        //            dblSumArea = dblSumArea * 1000000;
        //        }
        //    }
        //    else
        //    {
        //        if (arealast != 0)
        //        {
        //            arealast = area = arealast / 1000000;
        //            sumarealast = sumarea = sumarealast / 1000000;
        //        }
        //        else
        //        {
        //            arealast = area = area / 1000000;
        //            sumarealast = sumarea = sumarea / 1000000;
        //            dblSumArea = dblSumArea / 1000000;
        //        }
        //        dSum = dSum / 1000;
        //        bCurrentlen = bCurrentlen / 1000;
        //        dlen = dlen / 1000;
        //    }
        //    //xs***************************************************end

        //    if (meaType == 1)
        //    {
        //        sResult = "线段测量结果：\n当前弧段长度：" + bCurrentlen + sUnit
        //            + "\n当前线段长度：" + dlen + sUnit;
        //        if (this.m_bShowSum == true)
        //            sResult += "\n\n总长度：" + dSum + sUnit;
        //    }
        //    else if (meaType == 2)
        //    {
        //        sResult = "多边形测量结果：\n当前弧段长度：" + bCurrentlen + sUnit
        //                    + "\n当前多边形周长：" + dlen + sUnit
        //                    + "\n当前多边形面积：" + area + "平方" + sUnit;
        //        if (this.m_bShowSum == true)
        //            sResult += "\n\n总周长：" + dSum + sUnit
        //                        + "\n总面积：" + sumarea + "平方" + sUnit;
        //    }
        //    else if (meaType == 3)
        //    {
        //        sResult = "和X正轴夹角：" + ang.ToString("f2") + "°";
        //    }
        //    this.lblResult.Text = sResult;
        //}
        //**************************************************************************end

        private void toolMeasureLine_Click(object sender, EventArgs e)
        {
            this.m_bIsFeatureMeasure = false;
            this.m_CurMeasureType = 0;
            toolMeasureLine.Checked = true;
            toolMeasureArea.Checked = false;
            toolMeasureFeature.Checked = false;
            toolMu.Enabled = false;
            toolHectares.Enabled = false;
            dblSumLength = 0;
            dblArea = 0;
            dblLength = 0;
            dblSegLength = 0;
            dblSumArea = 0;
        }

        private void toolMeasureArea_Click(object sender, EventArgs e)
        {
            this.m_bIsFeatureMeasure = false;
            this.m_CurMeasureType = 1;
            toolMeasureLine.Checked = false;
            toolMeasureArea.Checked = true ;
            toolMeasureFeature.Checked = false;
            toolMeasureAngle.Checked = false;//yjl20110816
            toolMu.Enabled = true;
            toolHectares.Enabled = true;
            dblSumLength = 0;
            dblArea = 0;
            dblLength = 0;
            dblSegLength = 0;
            dblSumArea = 0;
        }

        private void toolMeasureFeature_Click(object sender, EventArgs e)
        {
            this.m_bIsFeatureMeasure = true;
            toolMeasureLine.Checked = false;
            toolMeasureArea.Checked = false ;
            toolMeasureFeature.Checked = true;
            toolMeasureAngle.Checked = false;//yjl20110816
        }

        private void toolSnapToFeature_Click(object sender, EventArgs e)
        {
            if (this.m_bSnapToFeature == false)
            {
                if (pTool.CacheCount > 2000)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "要素缓存数量超出容限。请缩小量算视图的范围或将不需要的图层关闭。");
                    return;
                }
                this.m_bSnapToFeature = true;
            }
            else
                this.m_bSnapToFeature = false;
            toolSnapToFeature.Checked = m_bSnapToFeature;
        }

        private void toolShowSum_Click(object sender, EventArgs e)
        {
            if (this.m_bShowSum == false)
                this.m_bShowSum = true;
            else
                this.m_bShowSum = false;
            toolShowSum.Checked = m_bShowSum; 
        }

        private void toolClear_Click(object sender, EventArgs e)
        {
            this .lblResult.Text =""; 
            this.m_SumArea = 0;
            this.m_SumLength = 0;

			this.dblSegLength = 0;
            this.dblLength = 0;
        }

        //设置测量面积按钮是否可用
        public void IsCanMeasureArea(bool bCan)
        {
            toolMeasureArea.Enabled = bCan;
        }

        #region 单位设置


        private void toolM_Click(object sender, EventArgs e)
        {
           ////变化单位时重新显示数据 xisheng 20111118
           // if(m_Units==esriUnits.esriKilometers)
           // {
           //     this.m_Units = esriUnits.esriMeters;
           //     if (lblResult.Text.Trim() != "")
           //     {
           //         if (toolMeasureArea.Checked)
           //             ShowResult(2, true);
           //         else if (toolMeasureLine.Checked)
           //             ShowResult(1, true);
           //     }
           // }
           // //***********************************end
           // toolKM.Checked = false; toolM.Checked = true;
            //变化单位时重新显示数据 xisheng 20111118
            this.m_MyUnits = enumMyUnits.Meters ;
            if (lblResult.Text.Trim() != "")
            {
                if (toolMeasureArea.Checked)
                    ShowResult(2,false);
                else if (toolMeasureLine.Checked)
                    ShowResult(1, false);
            }
            toolKM.Checked = false;
            toolMu.Checked = false;
            toolHectares.Checked = false;
            toolM.Checked = true;
        }

        

        //private void toolCM_Click(object sender, EventArgs e)
        //{
        //    this.m_Units = esriUnits.esriCentimeters; 
        //}
        

        //private void toolMM_Click(object sender, EventArgs e)
        //{
        //    this.m_Units = esriUnits.esriMillimeters;
        //}

        #endregion

        /// <summary>
        /// guozheng 2011-5-8 added 将量算坐标单位转换为实际要求的单位
        /// </summary>
        /// <param name="in_Len">输入需要转换的数值</param>
        /// <returns>输出：转换后的数值，内部异常返回-1并记录异常日志</returns>
        private double UnitConvert(double in_Len)
        {
            IUnitConverter pUnitConvert = new UnitConverterClass();
            try
            {
                double Res = pUnitConvert.ConvertUnits(in_Len, this.m_Displayunit, this.m_Units);
                return Res;
            }
            catch (Exception eError)
            {
                if (SysCommon.Log.Module.SysLog == null) SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                return -1;
            }
        }

        private void toolKM_Click_1(object sender, EventArgs e)
        { 
            ////变化单位时重新显示数据 xisheng 20111118
            //if (m_Units == esriUnits.esriMeters)
            //{
            //    this.m_Units = esriUnits.esriKilometers;
            //    if (lblResult.Text.Trim() != "")
            //    {
            //        if (toolMeasureArea.Checked)
            //            ShowResult(2, false);
            //        else if (toolMeasureLine.Checked)
            //            ShowResult(1, false);
            //    }
            //}
            ////***************************************end
            //toolKM.Checked = true; toolM.Checked = false;
            //变化单位时重新显示数据 xisheng 20111118
            this.m_MyUnits = enumMyUnits.KiloMeters ;
            if (lblResult.Text.Trim() != "")
            {
                if (toolMeasureArea.Checked)
                    ShowResult(2, false);
                else if (toolMeasureLine.Checked)
                    ShowResult(1, false);
            }
            toolKM.Checked = true;
            toolMu.Checked = false;
            toolHectares.Checked = false;
            toolM.Checked = false;
           
        }

        private void frmMeasureResult_Load(object sender, EventArgs e)
        {
           
        }
        //yjl20110816 add
        private void toolMeasureAngle_Click(object sender, EventArgs e)
        {
            this.m_bIsFeatureMeasure = false;
            this.m_CurMeasureType = 2;
            toolMeasureLine.Checked = false;
            toolMeasureArea.Checked = false;
            toolMeasureFeature.Checked = false;
            toolMeasureAngle.Checked = true;
            toolMu.Enabled = false;
            toolHectares.Enabled = false;
        }

        private void frmMeasureResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.lblResult.Text = "";
            this.m_SumArea = 0;
            this.m_SumLength = 0;

            this.dblSegLength = 0;
            this.dblLength = 0;
            this.Dispose(true);
        }

        private void toolMu_Click(object sender, EventArgs e)
        {
            //变化单位时重新显示数据 xisheng 20111118
            this.m_MyUnits  = enumMyUnits.Mu;
            if (lblResult.Text.Trim() != "")
            {
                if (toolMeasureArea.Checked)
                    ShowResult(2, false);
                else if (toolMeasureLine.Checked)
                    ShowResult(1, false);
            }
            toolKM.Checked = false;
            toolMu.Checked = true;
            toolHectares.Checked = false;
            toolM.Checked = false;
        }

        private void toolHectares_Click(object sender, EventArgs e)
        {
            //变化单位时重新显示数据 xisheng 20111118
            this.m_MyUnits = enumMyUnits.Hectares;
            if (lblResult.Text.Trim() != "")
            {
                if (toolMeasureArea.Checked)
                    ShowResult(2, false);
                else if (toolMeasureLine.Checked)
                    ShowResult(1, false);
            }
            toolKM.Checked = false;
            toolMu.Checked = false;
            toolHectares.Checked = true;
            toolM.Checked = false;
        }

    }
}