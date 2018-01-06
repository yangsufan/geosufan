using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoSysSetting
{
    public partial class FormQueryConfig : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.DotNetBar.Controls.ComboBoxEx _CurCombox;
        public FormQueryConfig()
        {
            InitializeComponent();
        }

        private void FormQueryConfig_Load(object sender, EventArgs e)
        {

        }
    
        private void listQueryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listQueryType.SelectedItem == null)
                return;
            string strQueryType= listQueryType.SelectedItem.ToString();
            switch (strQueryType)
            {
                case "地名查询":
                    groupPanelDM.Visible = true;
                    groupPanelRoad.Visible = false;
                    break;
                case "道路查询":
                    groupPanelRoad.Visible = true;
                    groupPanelDM.Visible = false;
                    break;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void comboBoxDMLayer1_DropDown(object sender, EventArgs e)
        {
            groupPanelDM.Controls.Add(panelExTreeLayer );
            panelExTreeLayer.Location = new Point(comboBoxDMLayer1.Location.X, comboBoxDMLayer1.Location.Y);
            panelExTreeLayer.Visible = true;
            panelExTreeLayer.BringToFront();
            _CurCombox = comboBoxDMLayer1;
            
        }

        private void comboBoxDMLayer2_DropDown(object sender, EventArgs e)
        {
            groupPanelDM.Controls.Add(panelExTreeLayer);
            panelExTreeLayer.Location = new Point(comboBoxDMLayer2.Location.X, comboBoxDMLayer2.Location.Y);
            panelExTreeLayer.Visible = true;
            panelExTreeLayer.BringToFront();
            _CurCombox = comboBoxDMLayer2;
        }

        private void comboBoxRoadLayer_DropDown(object sender, EventArgs e)
        {
            this.groupPanelRoad.Controls.Add(panelExTreeLayer);
            panelExTreeLayer.Location = new Point(comboBoxRoadLayer.Location.X, comboBoxRoadLayer.Location.Y);
            panelExTreeLayer.Visible = true;
            panelExTreeLayer.BringToFront();
            _CurCombox = comboBoxRoadLayer;
        }

        private void advTreeLayer_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            panelExTreeLayer.Visible = false;
            DevComponents.AdvTree.Node pNode = e.Node;
            _CurCombox.Text = pNode.Text;
        }

        private void panelExTreeLayer_Leave(object sender, EventArgs e)
        {
            panelExTreeLayer.Visible = false;
            _CurCombox = null;
        }

        private void advTreeLayer_Leave(object sender, EventArgs e)
        {
            panelExTreeLayer.Visible = false;
            _CurCombox = null;
        }
    }
}
