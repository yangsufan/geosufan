using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
//Zq 20110923  add
namespace GeoDataManagerFrame
{
    public partial class frmDJAttributeQuery : DevComponents.DotNetBar.Office2007Form
    {
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        private IFeatureClass m_FeatureClass;
        private string m_User;
        private AxMapControl m_MapControl;
        private string username;
        public frmDJAttributeQuery(IFeatureClass pFeatureClass,IWorkspace pWorkspace,string StrUser,AxMapControl pMapControl)
        {
            InitializeComponent();
            m_FeatureClass = pFeatureClass;
            DJAttributeQueryClass.m_Workspace = pWorkspace;
            m_User = StrUser + ".";
            m_MapControl = pMapControl;
            username = (pFeatureClass as IDataset).Name;
            if (username.Contains("."))
                username = username.Substring(0, username.LastIndexOf('.') + 1);
            //初始化tab页面
            if (SysCommon.ModField._DicFieldName.Count ==0)
            {
                SysCommon.ModField.InitNameDic(DJAttributeQueryClass.m_Workspace, SysCommon.ModField._DicFieldName, "属性对照表");
            }

            DJAttributeQueryClass.GetTable(dataGridVVQLR, username + "CZDJ_QLR");
            DJAttributeQueryClass.GetTable(dataGridVQSDC, username + "CZDJ_QSDC");
            DJAttributeQueryClass.GetTable(dataGridVQSLYZM, username + "CZDJ_QSLYZM");
            DJAttributeQueryClass.GetTable(dataGridVQSSP, username + "CZDJ_QSSP");
            DJAttributeQueryClass.GetTable(dataGridVSQDJ, username + "CZDJ_SQDJ");
            DJAttributeQueryClass.GetTable(dataGridVTXQLZM, username + "CZDJ_TXQLDJ");
            DJAttributeQueryClass.GetTable(dataGridVZCDJ, username + "CZDJ_ZCDJ");
        
        }
        private void dataGridVVQLR_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridVVQLR.RowCount == 0) { return; }
            try
            {
                labQuery.Text = dataGridVVQLR.Rows[e.RowIndex].Cells["CumNameVQLR"].Value.ToString() + "：";
                labQuery.Tag = dataGridVVQLR.Rows[e.RowIndex].Cells["CumEngVQLR"].Value.ToString();
                bttQuery.Tag = dataGridVVQLR.Tag;
            }
            catch { }
        }

        private void dataGridVQSLYZM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridVQSLYZM.RowCount == 0) { return; }
            try
            {
                labQuery.Text = dataGridVQSLYZM.Rows[e.RowIndex].Cells["CumNameQSLYZM"].Value.ToString() + "：";
                labQuery.Tag = dataGridVQSLYZM.Rows[e.RowIndex].Cells["CumEngQSLYZM"].Value.ToString();
                bttQuery.Tag = dataGridVQSLYZM.Tag;
            }
            catch { }
        }

        private void dataGridVSQDJ_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridVSQDJ.RowCount == 0) { return; }
            try
            {
                labQuery.Text = dataGridVSQDJ.Rows[e.RowIndex].Cells["CumNameSQDJ"].Value.ToString() + "：";
                labQuery.Tag = dataGridVSQDJ.Rows[e.RowIndex].Cells["CumEngSQDJ"].Value.ToString();
                bttQuery.Tag = dataGridVSQDJ.Tag;
            }
            catch { }
        }

        private void dataGridVQSDC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridVQSDC.RowCount == 0) { return; }
            try
            {
                labQuery.Text = dataGridVQSDC.Rows[e.RowIndex].Cells["CumNameQSDC"].Value.ToString() + "：";
                labQuery.Tag = dataGridVQSDC.Rows[e.RowIndex].Cells["CumEngQSDC"].Value.ToString();
                bttQuery.Tag = dataGridVQSDC.Tag;
            }
            catch { }
        }

        private void dataGridVQSSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridVQSSP.RowCount == 0) { return; }
            try
            {
                labQuery.Text = dataGridVQSSP.Rows[e.RowIndex].Cells["CumNameQSSP"].Value.ToString() + "：";
                labQuery.Tag = dataGridVQSSP.Rows[e.RowIndex].Cells["CumEngQSSP"].Value.ToString();
                bttQuery.Tag = dataGridVQSSP.Tag;
            }
            catch { }
        }

        private void dataGridVZCDJ_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridVZCDJ.RowCount == 0) { return; }
            try
            {
                labQuery.Text = dataGridVZCDJ.Rows[e.RowIndex].Cells["CumNameZCDJ"].Value.ToString() + "：";
                labQuery.Tag = dataGridVZCDJ.Rows[e.RowIndex].Cells["CumEngZCDJ"].Value.ToString();
                bttQuery.Tag = dataGridVZCDJ.Tag;
            }
            catch { }
        }

        private void dataGridVTXQLZM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridVTXQLZM.RowCount == 0) { return; }
            try
            {
                labQuery.Text = dataGridVTXQLZM.Rows[e.RowIndex].Cells["CumNameTXQLZM"].Value.ToString() + "：";
                labQuery.Tag = dataGridVTXQLZM.Rows[e.RowIndex].Cells["CumEngTXQLZM"].Value.ToString();
                bttQuery.Tag = dataGridVTXQLZM.Tag;
            }
            catch { }
        }
       

        private void bttQuery_Click(object sender, EventArgs e)
        {
           
            if (labQuery.Text == "属性名称：") { MessageBox.Show("请在属性表中选择查询的字段！","提示！"); return; }
            if (txtKeys.Text == "") { MessageBox.Show("请输入关键字信息！","提示！"); return; }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("根据" + labQuery.Text + "字段查找与" + txtKeys.Text + "值相关的宗地信息");
            }
            DJAttributeQueryClass.Query(labQuery, bttQuery.Tag.ToString(), txtKeys.Text, m_FeatureClass, dataGridVRe);
        }

        private void dataGridVRe_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string strDJH = dataGridVRe.Rows[e.RowIndex].Cells["CumDJH"].Value.ToString();
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("查看地籍号为：" + strDJH + "的宗地详细信息");
                }
                if (strDJH == "")
                {
                    MessageBox.Show("地籍号信息缺失！", "提示！");
                    return;
                }
                if (SysCommon.ModField._DicFieldName == null)
                {
                    SysCommon.ModField.InitNameDic(DJAttributeQueryClass.m_Workspace, SysCommon.ModField._DicFieldName, "属性对照表");
                }
                DJAttributeQueryClass.QueryResult(dataGridVVQLR, username + "CZDJ_QLR", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVQSDC, username + "CZDJ_QSDC", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVQSLYZM, username + "CZDJ_QSLYZM", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVQSSP, username + "CZDJ_QSSP", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVSQDJ, username + "CZDJ_SQDJ", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVTXQLZM, username + "CZDJ_TXQLDJ", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVZCDJ, username + "CZDJ_ZCDJ", strDJH);
                IFeature pFeature = DJAttributeQueryClass.QueryFeature(m_FeatureClass, strDJH);
                if (pFeature == null)
                {
                    MessageBox.Show("未找到该地籍号图形信息！", "提示！");
                    return;
                }
                //先刷新，后闪烁问题
                ///ZQ 20111020 定位范围扩大1.5倍
                IEnvelope pExtent = pFeature.Extent;
                SysCommon.ModPublicFun.ResizeEnvelope(pExtent, 1.5);
                m_MapControl.Extent = pExtent;
                m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
                m_MapControl.ActiveView.ScreenDisplay.UpdateWindow();
                m_MapControl.FlashShape(pFeature.ShapeCopy, 3, 200, null);
            }
            catch { }

        }

        private void bttVisble_Click(object sender, EventArgs e)
        {
            frmCheckItem pfrmCheckItem = null;
            try
            {
                switch (tabConTable.SelectedTab.Name.ToString())
                {
                    case "tabQLR":
                        pfrmCheckItem = new frmCheckItem(dataGridVVQLR);
                        break;
                    case "tabQSLYZM":
                        pfrmCheckItem = new frmCheckItem(dataGridVQSLYZM);
                        break;
                    case "tabSQDJ":
                        pfrmCheckItem = new frmCheckItem(dataGridVSQDJ);
                        break;
                    case "tabQSDC":
                        pfrmCheckItem = new frmCheckItem(dataGridVQSDC);
                        break;
                    case "tabQSSP":
                        pfrmCheckItem = new frmCheckItem(dataGridVQSSP);
                        break;
                    case "tabZCDJ":
                        pfrmCheckItem = new frmCheckItem(dataGridVZCDJ);
                        break;
                    case "tabTXQLZM":
                        pfrmCheckItem = new frmCheckItem(dataGridVTXQLZM);
                        break;
                }
                pfrmCheckItem.ShowDialog();
            }
            catch { }
        }
        private void dataGridVRe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string strDJH = dataGridVRe.Rows[e.RowIndex].Cells["CumDJH"].Value.ToString();
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("查看地籍号为：" + strDJH + "的宗地详细信息");
                }
                if (strDJH == "")
                {
                    MessageBox.Show("地籍号信息缺失！", "提示！");
                    return;
                }
                if (SysCommon.ModField._DicFieldName == null)
                {
                    SysCommon.ModField.InitNameDic(DJAttributeQueryClass.m_Workspace, SysCommon.ModField._DicFieldName, "属性对照表");
                }
                DJAttributeQueryClass.QueryResult(dataGridVVQLR, username + "CZDJ_QLR", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVQSDC, username + "CZDJ_QSDC", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVQSLYZM, username + "CZDJ_QSLYZM", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVQSSP, username + "CZDJ_QSSP", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVSQDJ, username + "CZDJ_SQDJ", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVTXQLZM, username + "CZDJ_TXQLDJ", strDJH);
                DJAttributeQueryClass.QueryResult(dataGridVZCDJ, username + "CZDJ_ZCDJ", strDJH);
                IFeature pFeature = DJAttributeQueryClass.QueryFeature(m_FeatureClass, strDJH);
                if (pFeature == null)
                {
                    MessageBox.Show("未找到该地籍号图形信息！", "提示！");
                    return;
                }
            }
            catch { }
        }
       

       
      

      

     
    }
}
