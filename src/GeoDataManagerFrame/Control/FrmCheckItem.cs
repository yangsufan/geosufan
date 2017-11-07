using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
//属性显示控制    张琪    20110924
namespace GeoDataManagerFrame
{
    public partial class frmCheckItem : DevComponents.DotNetBar.Office2007Form
    {
        DevComponents.DotNetBar.Controls.DataGridViewX m_DataGridView;
        public frmCheckItem(DevComponents.DotNetBar.Controls.DataGridViewX pDataGridView)
        {
            InitializeComponent();
            m_DataGridView = pDataGridView;
            Initialization();
        }
        public void Initialization()
        {
            if (m_DataGridView.RowCount == 0) { return; }
            for (int i = 0; i < m_DataGridView.Rows.Count-1;i++ )
            {
                checkedListBoxItem.Items.Add(m_DataGridView.Rows[i].Cells[0].Value.ToString(),m_DataGridView.Rows[i].Visible);
            }
        }

        private void btnAllselection_Click(object sender, EventArgs e)// 选中checkedListBox中的所有项
        {
            for (int i = 0; i < checkedListBoxItem.Items.Count;i++ )
            {
                checkedListBoxItem.SetItemChecked(i,true);
            }
        }

        private void btnReturnselection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxItem.Items.Count; i++)
            {
                if (checkedListBoxItem.GetItemChecked(i))
                {
                    checkedListBoxItem.SetItemChecked(i, false);
                }
                else
                {
                    checkedListBoxItem.SetItemChecked(i, true);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void btnDefine_Click(object sender, EventArgs e)//获取全部选中的值
        {
            if (m_DataGridView.RowCount == 0 || checkedListBoxItem.Items.Count == 0) { this.Close(); }
            for (int i = 0; i < m_DataGridView.RowCount-1; i++)
            {
                m_DataGridView.Rows[i].Visible = SetVisble(m_DataGridView.Rows[i].Cells[0].Value.ToString());
            }
                this.Close();
        }
        /// <summary>
        /// 根据用户的设置属性显示项
        /// </summary>
        /// <param name="strRowValue"></param>
        /// <returns></returns>
        private bool SetVisble(string strRowValue)
        {
            bool Visble = true;
            for (int i = 0; i < checkedListBoxItem.Items.Count; i++)
            {
                if (checkedListBoxItem.Items[i].ToString() == strRowValue)
                {
                    Visble = checkedListBoxItem.GetItemChecked(i);
                        return Visble;
                }
            }
           return Visble;
        }
    }
}
