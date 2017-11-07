using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeoCustomExport
{
    /// <summary>
    /// 提供添加或者修改列表中的图层名或者字段名 xisheng 20120511
    /// </summary>
    public partial class FrmEditLabel : DevComponents.DotNetBar.Office2007Form
    {
        public FrmEditLabel()
        {
            InitializeComponent();
        }

        public FrmEditLabel(bool IsLayer, DevComponents.DotNetBar.Controls.ListViewEx list)
        {
              InitializeComponent();
            m_IsLayer = IsLayer;
            m_List = list;
            isEdit = false;
            InInitializeForm();
        }

        public FrmEditLabel(bool IsLayer,string strSource,string strTarget)
        {
              InitializeComponent();
              m_IsLayer = IsLayer;
            isEdit = true;
            txt_Source.Text = strSource;
            txt_Target.Text = strTarget;
            InInitializeForm();
        }
        private bool m_IsLayer;//是的话就是图层，否的话就是字段
        private DevComponents.DotNetBar.Controls.ListViewEx m_List;//图层列表或者字段列表
        private bool isEdit;//是编辑还是添加
        public string strTarget
        {
            get { return txt_Target.Text; }
        }

        private List<string> m_ListField;//记录删除的字段方便添加
        public List<string> ListField
        {
            set { m_ListField = value; }
        }

        //返回listItem
        public object objItem
        {
            get {
                if (m_IsLayer)
                {
                    return m_List.Items[cb_Source.SelectedIndex];
                }
                else
                {
                    return cb_Source.Text as object;
                }
            }
        }

        /// <summary>
        /// 初始化窗体的名称以及控件的名称。
        /// </summary>
        /// <param name="isLayer"></param>
        private void InInitializeForm()
        {
            string name = "对应关系";
            if (m_IsLayer)
            {
                labelSoure.Text = "源图层";
                labelTarget.Text = "目标图层";
                name = "图层" + name;
            }
            else
            {
                labelSoure.Text = "源字段";
                labelTarget.Text = "目标字段";
                name = "字段" + name;
            }
            if (isEdit)
            {
                txt_Source.Visible = true;
                cb_Source.Visible = false;
                name = "修改" + name;
            }
            else
            {
                txt_Source.Visible = false;
                cb_Source.Visible = true;
                name = "添加" + name;
            }
            this.Text = name;

        }

        private void FrmEditLabel_Load(object sender, EventArgs e)
        {
            if (!isEdit)
            {
                if (m_IsLayer)
                {
                    for (int i = 0; i < m_List.Items.Count; i++)
                    {
                        if (!cb_Source.Items.Contains(m_List.Items[i]))
                        {
                            cb_Source.Items.Add(m_List.Items[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < m_List.Items.Count; i++)
                    {
                        if (!cb_Source.Items.Contains(m_List.Items[i]))
                        {
                            cb_Source.Items.Add(m_List.Items[i]);
                        }
                    }
                }
                if (cb_Source.Items.Count > 0)
                {
                    cb_Source.SelectedIndex = 0;
                }
            }
        }

        private void cb_Source_SelectedIndexChanged(object sender, EventArgs e)
        {
           txt_Target.Text= cb_Source.Text;
        }

        /// <summary>
        /// 点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
