using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;

namespace ElementCommandTool
{
    public partial class UC_AnchorPoint : UserControl
    {
        public esriAnchorPointEnum AnchorPoint
        {
            get;
            set;
        }
        public UC_AnchorPoint()
        {
            InitializeComponent();
        }

        public void DirectAnchor(esriAnchorPointEnum inAnchorPoint)
        {
            btn1.Enabled = true;
            btn2.Enabled = true;
            btn3.Enabled = true;
            btn4.Enabled = true;
            btn5.Enabled = true;
            btn6.Enabled = true;
            btn7.Enabled = true;
            btn8.Enabled = true;
            btn9.Enabled = true;

            switch (inAnchorPoint)
            {
                case esriAnchorPointEnum.esriBottomLeftCorner:
                    btn7.Enabled = false;
                    break;
                case esriAnchorPointEnum.esriBottomMidPoint:
                    btn8.Enabled = false;
                    break;
                case esriAnchorPointEnum.esriBottomRightCorner:
                    btn9.Enabled = false;
                    break;
                case esriAnchorPointEnum.esriCenterPoint:
                    btn5.Enabled = false;
                    break;
                case esriAnchorPointEnum.esriLeftMidPoint:
                    btn4.Enabled = false;
                    break;
                case esriAnchorPointEnum.esriRightMidPoint:
                    btn6.Enabled = false;
                    break;
                case esriAnchorPointEnum.esriTopLeftCorner:
                    btn1.Enabled = false;
                    break;
                case esriAnchorPointEnum.esriTopMidPoint:
                    btn2.Enabled = false;
                    break;
                case esriAnchorPointEnum.esriTopRightCorner:
                    btn3.Enabled = false;
                    break;
                default:
                    btn7.Enabled = false;
                    break;


            }
        }
    }
}
