using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataChecker
{
    public partial class frmCheckDataBase : DevComponents.DotNetBar.Office2007Form
    {
        System.Collections.ArrayList list = new System.Collections.ArrayList();
        public frmCheckDataBase(System.Collections.ArrayList List)
        {
            list = List;
            InitializeComponent();
        }

        private void frmCheckDataBase_Load(object sender, EventArgs e)
        {
            SetCheckState.CheckDataBaseName = "";
            int count = list.Count;
            if (count == 0) return;
            for (int n = 0; n < count; n++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = list[n].ToString();
                LST_Check.Items.Add(item);
            }
        }

        private void btn_cancle_Click(object sender, EventArgs e)
        {

            this.Close();
            return;
        }

        private void LST_Check_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            LST_Check.Items[e.Index].Checked = true;
        }

        private void btn_check_Click(object sender, EventArgs e)
        {
            string name = "";
            int count = LST_Check.Items.Count;
            for (int n = 0; n < count; n++)
            {
                if (LST_Check.Items[n].Selected)
                {
                    name =LST_Check.Items[n].Text;
                    SetCheckState.CheckDataBaseName = name;
                    break;
                }
            }
            if (name == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","请选择一个库体进行检查！");
                return;
            }
            this.Close();
            
        }
    }
}