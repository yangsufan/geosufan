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
        private bool m_bShowSum;　　　   //标志是否显示总和

        private double m_SegLength;　　　  // 活动段的长度
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
                 if (m_Units == esriUnits.esriKilometers)
                     m_Area = m_Area /1000000;
              
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
            string sUnit = "";
            sUnit = this.GetUnitsName(m_Units);
            ////////////////////判断坐标单位是否一致////////////////////////
            double dSum = SumLen;
            double bCurrentlen = segLen;
            double dlen = len;
            if (this.m_Displayunit != esriUnits.esriUnknownUnits)
            {
                if (this.m_Displayunit != this.m_Units)
                {
                    dSum = UnitConvert(dSum);
                    bCurrentlen = UnitConvert(bCurrentlen);
                    dlen = UnitConvert(dlen);
                    //Area = UnitConvert(Area);
                    //SumArea = UnitConvert(SumArea);
                }
            }
            ////////////////////////////////////////////////////////////////
            if (IsLine==true )
            {
                sResult = "线段测量结果：\n当前弧段长度：" + bCurrentlen + sUnit
                    + "\n当前线段长度：" + dlen + sUnit;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总长度：" + dSum + sUnit;
            }
            else 
            {
                sResult = "多边形测量结果：\n当前弧段长度：" + bCurrentlen + sUnit
                            + "\n当前多边形周长：" + dlen + sUnit
                            + "\n当前多边形面积：" + Area  + "平方" + sUnit;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总周长：" + dSum + sUnit
                                + "\n总面积：" + SumArea + "平方" + sUnit;
            }
            this.lblResult.Text = sResult;  

        }
        //显示结果到窗体上yjl20110816 add
        public void ShowResult()
        {

            this.lblResult.Text = "和X正轴夹角：" + ang.ToString("f2") + "°";

        }
        //yjl20110816 modify.1线2面3角度
        public void ShowResult(int meaType)
        {
            string sResult = "";
            string sUnit = "";
            sUnit = this.GetUnitsName(m_Units);
            ////////////////////判断坐标单位是否一致////////////////////////
            double dSum = this.dblSumLength;
            double bCurrentlen = this.dblSegLength;
            double dlen = this.dblLength;
            if (this.m_Displayunit != esriUnits.esriUnknownUnits)
            {
                if (this.m_Displayunit != this.m_Units)
                {
                    dSum = UnitConvert(dSum);
                    bCurrentlen = UnitConvert(bCurrentlen);
                    dlen = UnitConvert(dlen);

                    //增加面积变化 xisheng 20111117**************
                    if (dblSumArea > 0)
                    {
                        if (m_Units == esriUnits.esriKilometers)
                        {
                            dblSumArea = dblSumArea / 1000000;
                            m_Area = m_Area / 1000000;
                        }
                        else if (m_Units == esriUnits.esriMeters)
                        {
                            dblSumArea = dblSumArea * 1000000;
                            m_Area = m_Area * 1000000;
                        }
                    }
                    //增加面积变化 xisheng 20111117***********end
                }
            }
            ////////////////////////////////////////////////////////////////

            if (meaType == 1)
            {
                sResult = "线段测量结果：\n当前弧段长度：" + bCurrentlen + sUnit
                    + "\n当前线段长度：" + dlen + sUnit;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总长度：" + dSum + sUnit;
            }
            else if (meaType == 2)
            {
                sResult = "多边形测量结果：\n当前弧段长度：" + bCurrentlen + sUnit
                            + "\n当前多边形周长：" + dlen + sUnit
                            + "\n当前多边形面积：" + this.m_Area + "平方" + sUnit;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总周长：" + dSum + sUnit
                                + "\n总面积：" + this.dblSumArea + "平方" + sUnit;
            }
            else if (meaType==3)
            {
                sResult = "和X正轴夹角：" + ang.ToString("f2") + "°";
            }
            this.lblResult.Text = sResult;
        }


        private void toolMeasureLine_Click(object sender, EventArgs e)
        {
            this.m_bIsFeatureMeasure = false;
            this.m_CurMeasureType = 0;
            toolMeasureLine.Checked = true;
            toolMeasureArea.Checked = false;
            toolMeasureFeature.Checked = false;  
        }

        private void toolMeasureArea_Click(object sender, EventArgs e)
        {
            this.m_bIsFeatureMeasure = false;
            this.m_CurMeasureType = 1;
            toolMeasureLine.Checked = false;
            toolMeasureArea.Checked = true ;
            toolMeasureFeature.Checked = false;
            toolMeasureAngle.Checked = false;//yjl20110816
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
            //调整单位时当前结果跟着变化 xisheng 20111117************
            if (m_Units == esriUnits.esriKilometers)
            {
                this.m_Units = esriUnits.esriMeters;
                if (toolMeasureLine.Checked) ShowResult(1);
                else if (toolMeasureArea.Checked) ShowResult(2);
            }
            //调整单位时当前结果跟着变化 xisheng 20111117********end
            toolKM.Checked = false; toolM.Checked = true;
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
            //调整单位时当前结果跟着变化 xisheng 20111117************
            if (m_Units == esriUnits.esriMeters)
            {
                this.m_Units = esriUnits.esriKilometers;

                if (toolMeasureLine.Checked) ShowResult(1);
                else if (toolMeasureArea.Checked) ShowResult(2);
    
            }
            //调整单位时当前结果跟着变化 xisheng 20111117*********end
            toolKM.Checked = true; toolM.Checked = false;
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
        }
    }
}