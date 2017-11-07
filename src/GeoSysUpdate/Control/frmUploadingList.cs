using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoSysUpdate
{
    public partial class frmUploadingList : Form
    {
        public frmUploadingList(Dictionary<string,string> pDic)
        {
            InitializeComponent();
            if (pDic == null) return;
            foreach(string strKey in pDic.Keys)
            {
                listlog.Items.Add(strKey+"      "+pDic[strKey].ToString());
            }
        }
    }
}
