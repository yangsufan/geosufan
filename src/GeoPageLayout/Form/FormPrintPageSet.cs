
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using System;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;

namespace GeoPageLayout
{
    public partial class FormPrintPageSet : DevComponents.DotNetBar.Office2007Form
    {
        private AxPageLayoutControl axPageLayoutControl = null;
        double mywidth = 0;
        double myheight = 0;//加载时的初始值，点取消时还原为此值
        private short orient = 0;
        public FormPrintPageSet(AxPageLayoutControl axPagelayoutCtl)
        {
            axPageLayoutControl = axPagelayoutCtl;
            InitializeComponent();
        }

        private void FormPrintPageSet_Load(object sender, EventArgs e)
        {
            
            axPageLayoutControl.Page.QuerySize(out mywidth,out myheight);
            orient = axPageLayoutControl.Page.Orientation;

            if (axPageLayoutControl.Page.Orientation == 1)
                radioButton3.Checked = true;
            else
                radioButton4.Checked = true;

            
            this.comboBox3.Items.Add("Letter - 8.5in x 11in.");
            this.comboBox3.Items.Add("Legal - 8.5in x 14in.");
            this.comboBox3.Items.Add("Tabloid - 11in x 17in.");
            this.comboBox3.Items.Add("C - 17in x 22in.");
            this.comboBox3.Items.Add("D - 22in x 34in.");
            this.comboBox3.Items.Add("E - 34in x 44in.");
            this.comboBox3.Items.Add("A5 - 148mm x 210mm.");
            this.comboBox3.Items.Add("A4 - 210mm x 297mm.");
            this.comboBox3.Items.Add("A3 - 297mm x 420mm.");
            this.comboBox3.Items.Add("A2 - 420mm x 594mm.");
            this.comboBox3.Items.Add("A1 - 594mm x 841mm.");
            this.comboBox3.Items.Add("A0 - 841mm x 1189mm.");
            this.comboBox3.Items.Add("Custom Page Size.");
            this.txtW.Text = mywidth.ToString();
            this.txtH.Text = myheight.ToString();
        }

       
     
        private void EnableOrientation(bool b)
        {
            txtW.Enabled = b;
            txtW.Enabled = b;
            txtH.Enabled = b;
            txtH.Enabled = b;

        }

        private bool isNumeric(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);
            foreach (byte c in bytestr)
            {
                if (c < 48 || c > 57)
                {
                    return false;
                }
            }
            return true;
        }

        private bool isdouble(string str)
        {
            if (str.Contains("."))
            {
                int indexdot = str.IndexOf(".");
                string str1 = str.Substring(0, indexdot);
                string str2 = str.Substring(indexdot + 1);
                if (isNumeric(str1) && isNumeric(str2))
                    return true;
                else
                    return false;
            }
            else
                return isNumeric(str);
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton4.Checked == true)//纵向
            {
                axPageLayoutControl.Page.Orientation = 2;
                pageOrientChanged();
            }
            else
            {
                axPageLayoutControl.Page.Orientation = 1;
                pageOrientChanged();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked == true)//横向
            {
                axPageLayoutControl.Page.Orientation = 1;
                pageOrientChanged();
            }
            else
            {
                axPageLayoutControl.Page.Orientation = 2;
                pageOrientChanged();
            }
        }
        //当纸张方向改变，长和宽互换yjl20110801
        private void pageOrientChanged()
        {
            if (axPageLayoutControl.Page.Orientation != orient)
            {
                //string tmp = txtW.Text;
                double tmpW = 0, tmpH = 0;
                axPageLayoutControl.Page.QuerySize(out tmpW, out tmpH);
                txtW.Text = tmpW.ToString();
                txtH.Text = tmpH.ToString();
            }
        }
      
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != 12)
            {
                EnableOrientation(false);
                axPageLayoutControl.Page.FormID = (esriPageFormID)comboBox3.SelectedIndex;
            }
            else
            {
                EnableOrientation(true);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked == true)
            {
                groupBox4.Enabled = false;
            }
            else
            {
                groupBox4.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
      
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //using (Graphics g = panel1.CreateGraphics())
            //{
            //    g.DrawImage(imageList1.Images[0], panel1.ClientRectangle);
            //}

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            axPageLayoutControl.Page.Orientation = orient;
            axPageLayoutControl.Page.PutCustomSize(mywidth, myheight);
            
            this.Close();
        }

        private void txtW_Leave(object sender, EventArgs e)
        {
            if (txtW.Text != "" && txtH.Text != "")
            {
                if (isdouble(txtW.Text) && isdouble(txtH.Text))
                {

                    if (Convert.ToDouble(txtW.Text) > 0.1 && Convert.ToDouble(txtH.Text) > 0.1)
                    {
                        axPageLayoutControl.Page.PutCustomSize(Convert.ToDouble(txtW.Text.ToString()), Convert.ToDouble(txtH.Text.ToString()));
                    }
                }
            }
       

        }

        private void txtH_Leave(object sender, EventArgs e)
        {
            if (txtW.Text != "" && txtH.Text != "")
            {
                if (isdouble(txtW.Text) && isdouble(txtH.Text))
                {

                    if (Convert.ToDouble(txtW.Text) > 0.1 && Convert.ToDouble(txtH.Text) > 0.1)
                    {
                        axPageLayoutControl.Page.PutCustomSize(Convert.ToDouble(txtW.Text.ToString()), Convert.ToDouble(txtH.Text.ToString()));
                    }
                }
            }

        }

 

    }
}