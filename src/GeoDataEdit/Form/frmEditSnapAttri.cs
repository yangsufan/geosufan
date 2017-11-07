using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;

namespace GeoDataEdit
{
    public partial class frmEditSnapAttri : DevComponents.DotNetBar.Office2007Form
    {
        private ClsEditorMain clsEditorMain;
        public frmEditSnapAttri (ClsEditorMain _clsEditorMain)
        {
            clsEditorMain = _clsEditorMain;
            InitializeComponent ();
        }


        private void frmEditSnapAttri_Load ( object sender , EventArgs e )
        {

            if ( clsEditorMain.SnapPoint == null )
            {
                clsEditorMain.SnapPoint = new SnapPointClass ();
            }

            //将配置显示在属性窗体中

            this.chb_SnapType_Intersect.Checked = clsEditorMain.SnapPoint.IsSnapIntersectPoint;
            this.chb_SnapType_Mid.Checked = clsEditorMain.SnapPoint.IsSnapMidPoint;
            this.chb_SnapType_Node.Checked = clsEditorMain.SnapPoint.IsSnapNodePoint;
            this.chb_SnapType_Port.Checked = clsEditorMain.SnapPoint.IsSnapPortPoint;
            this.chb_SnapType_Boundry.Checked = clsEditorMain.SnapPoint.IsSnapBoundryPoint;
            this.chb_SnapType_Center.Checked = clsEditorMain.SnapPoint.IsSnapCenterPoint;

            this.txt_CacheRadius.Text = clsEditorMain.SnapPoint.CachePixelRadius.ToString ();
            this.txt_SnapRadius.Text = clsEditorMain.SnapPoint.SnapPixelRadius.ToString ();
        }

        private void btnCannel_Click ( object sender , EventArgs e )
        {
            this.Close ();
        }

        private void btnOK_Click ( object sender , EventArgs e )
        {

            //将用户的配置应用到程序中
            if ( this.gb_SnapType.Enabled && clsEditorMain.SnapPoint != null )
            {
                if ( txt_CacheRadius.Text == "" )
                {
                    MessageBox.Show ( "请填写缓冲半径！" , "系统提示" );
                    return;
                }
                if ( txt_SnapRadius.Text == "" )
                {
                    MessageBox.Show ( "请填写捕捉半径" , "系统提示" );
                    return;
                }
                if ( !IsNumber ( txt_CacheRadius.Text ) )
                {
                    MessageBox.Show ( "缓冲半径必须为数字！" , "系统提示" );
                    return;
                }
                if ( !IsNumber ( txt_SnapRadius.Text ) )
                {
                    MessageBox.Show ( "捕捉半径必须为数字！" , "系统提示" );
                    return;
                }
                if ( chb_SnapType_Node.Checked )
                    clsEditorMain.SnapPoint.CustomSetting ( chb_SnapType_Port.Checked , chb_SnapType_Mid.Checked ,
                        chb_SnapType_Node.Checked , chb_SnapType_Intersect.Checked , false ,chb_SnapType_Boundry.Checked,
                        chb_SnapType_Center.Checked,Convert.ToDouble ( txt_SnapRadius.Text ) , Convert.ToDouble ( txt_CacheRadius.Text ) );
                else
                    clsEditorMain.SnapPoint.CustomSetting ( chb_SnapType_Port.Checked, chb_SnapType_Mid.Checked ,
                        chb_SnapType_Node.Checked , chb_SnapType_Intersect.Checked , false ,chb_SnapType_Boundry.Checked,
                        chb_SnapType_Center.Checked,Convert.ToDouble ( txt_SnapRadius.Text ) , Convert.ToDouble ( txt_CacheRadius.Text ) );

                clsEditorMain.SnapPoint.InitMap ( clsEditorMain.HookHelper.FocusMap );
                this.Close ();
            }
        }
        //判断一个字符串是否为数字型
        private bool IsNumber(string sValue)
        {
            int iCount = 0;
            bool IsNumber = false;

            if (sValue.StartsWith(".")) return IsNumber;

            string NumberArray = "0123456789.";

            List<char> NumberList = new List<char>(NumberArray.ToCharArray());
            foreach (char c in sValue)
            {
                if (NumberList.Contains(c))
                {
                    IsNumber = true;
                }
                else
                {
                    IsNumber = false;
                }
                if (c.Equals('.'))
                {
                    iCount = iCount + 1;

                    if (iCount > 1)
                    {
                        return false;
                    }
                }
            }
            return IsNumber;
        }


        private void frmEditSnapAttri_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Escape )
            {
                this.Close ();
            }
        }
    }
}