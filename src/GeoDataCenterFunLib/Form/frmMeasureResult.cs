using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataCenterFunLib
{
    public partial class frmMeasureResult : DevComponents.DotNetBar.Office2007Form
    {
        //字段
        private esriUnits m_Units=esriUnits.esriMeters  ;        //当前数值的单位
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
        private double m_SumArea;         //总面积
        public double dblSumArea
        {
            set
            {
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
        public   short   m_CurMeasureType;  //标志当前量算的对象类型:0为线；1为面
        public bool m_bSnapToFeature;  //标志是否捕捉要素节点
        public bool m_bIsFeatureMeasure;  //标志是否对要素进行测量

        //构造函数
        public frmMeasureResult()
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
                    return "公里";
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

            if (IsLine==true )
            {
                sResult = "线段测量结果：\n当前弧段长度：" + segLen.ToString("0.000") + sUnit
                    + "\n当前线段长度：" + len.ToString("0.000") + sUnit;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总长度：" + SumLen.ToString("0.000") + sUnit;
            }
            else 
            {
                sResult = "多边形测量结果：\n当前弧段长度：" + segLen.ToString("0.000") + sUnit
                            + "\n当前多边形周长：" + len.ToString("0.000") +sUnit
                            + "\n当前多边形面积：" + Area.ToString("0.000") + "平方" + sUnit;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总周长：" + SumLen.ToString("0.000") + sUnit
                                + "\n总面积：" + SumArea.ToString("0.000") + "平方" + sUnit;
            }
            this.lblResult.Text = sResult;  

        }

        public void ShowResult(bool IsLine)
        {
            string sResult = "";
            string sUnit = "";
            sUnit = this.GetUnitsName(m_Units);

            if (IsLine==true )
            {
                sResult = "线段测量结果：\n当前弧段长度：" + this.dblSegLength.ToString("0.000") + sUnit
                    + "\n当前线段长度：" + this.dblLength.ToString("0.000") + sUnit;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总长度：" + this.dblSumLength.ToString("0.000") + sUnit;
            }
            else 
            {
                sResult = "多边形测量结果：\n当前弧段长度：" + this.dblSegLength.ToString("0.000") + sUnit
                            + "\n当前多边形周长：" + this.dblLength.ToString("0.000") + sUnit
                            + "\n当前多边形面积：" + this.dblArea.ToString("0.000") + "平方" + sUnit;
                if (this.m_bShowSum == true)
                    sResult += "\n\n总周长：" + this.dblSumLength.ToString("0.000") + sUnit
                                + "\n总面积：" + this.dblSumArea.ToString("0.000") + "平方" + sUnit;
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
        }

        private void toolMeasureFeature_Click(object sender, EventArgs e)
        {
            this.m_bIsFeatureMeasure = true;
            toolMeasureLine.Checked = false;
            toolMeasureArea.Checked = false ;
            toolMeasureFeature.Checked = true;  
        }

        private void toolSnapToFeature_Click(object sender, EventArgs e)
        {
            if (this.m_bSnapToFeature == false)            
                this.m_bSnapToFeature = true; 
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
        }

        //设置测量面积按钮是否可用
        public void IsCanMeasureArea(bool bCan)
        {
            toolMeasureArea.Enabled = bCan;
        }

        #region 单位设置

        private void toolKM_Click(object sender, EventArgs e)
        {
            this.m_Units = esriUnits.esriKilometers;
            toolKM.Checked = true;
            toolM.Checked = false;
            toolCM.Checked = false;
            toolMM.Checked = false;
        }

        private void toolM_Click(object sender, EventArgs e)
        {
            this.m_Units = esriUnits.esriMeters;
            toolKM.Checked = false;
            toolM.Checked = true;
            toolCM.Checked = false;
            toolMM.Checked = false;
        }        

        private void toolCM_Click(object sender, EventArgs e)
        {
            this.m_Units = esriUnits.esriCentimeters;
            toolKM.Checked = false;
            toolM.Checked = false;
            toolCM.Checked = true;
            toolMM.Checked = false;
        }
        

        private void toolMM_Click(object sender, EventArgs e)
        {
            this.m_Units = esriUnits.esriMillimeters;
            toolKM.Checked = false;
            toolM.Checked = false;
            toolCM.Checked = false;
            toolMM.Checked = true;
        }

        #endregion
    }
}