using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

namespace GeoHistory.Control
{
    public partial class UCHistoryMap : UserControl
    {
        public AxMapControl AxMapCtrlHis
        {
            get { return this.axMapControl1; }
        }
        public UCHistoryMap()
        {
            InitializeComponent();
            axMapControl1.Tag = label1;
        }
        private void MapMain_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {

            switch (e.keyCode)
            {
                case 113://f2
                    //frmInputPassword frm = new frmInputPassword("Password", "请输入密码：");
                    //frm.ShowDialog();
                    //if (frm.bOK)
                    SaveMxd(axMapControl1.Object);
                    break;
                default:
                    break;
            }
        }

        public void SaveMxd(object obj)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            saveDialog.Filter = "mxd files (*.mxd)|*.mxd|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.RestoreDirectory = true;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IMapDocument mapDoc = new MapDocumentClass();
                    mapDoc.New(saveDialog.FileName);
                    mapDoc.ReplaceContents(obj as IMxdContents);
                    mapDoc.Save(true, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存文件时出错，无法保存MXD！\n\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

    }
}
