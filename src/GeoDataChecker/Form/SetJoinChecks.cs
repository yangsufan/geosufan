using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace GeoDataChecker
{
    /// <summary>
    /// 功能：设置接边检查每个层的属性检查
    /// 编写人：王冰
    /// </summary>
    public partial class SetJoinChecks : DevComponents.DotNetBar.Office2007Form
    {
        private Plugin.Application.IAppGISRef _AppHk;
        SysCommon.Gis.SysGisDataSet pGisDT = null;

        public SetJoinChecks(Plugin.Application.IAppGISRef AppHk)
        {
            InitializeComponent();

            _AppHk = AppHk;

            comBoxType.Items.AddRange(new object[] { "PDB", "GDB", "SDE" });
            comBoxType.SelectedIndex = 0;
        }
        private void btn_cancle_Click(object sender, EventArgs e)
        {
            int count = TreeLayer.Nodes.Count;
            for (int n = 0; n < count; n++)
            {
                TreeLayer.Nodes[n].Name = "";
            }
            this.Close();//关闭窗体
        }

        private void SetJoinChecks_Load(object sender, EventArgs e)
        {
            //BindTree();
        }

        /// <summary>
        /// 判断是管理员还是作业员 如果是管理员就返回1，如果是作业员就返回0
        /// </summary>
        /// <returns></returns>
        private int PurviewState()
        {
            XmlDocument doc = _AppHk.ProjectTree.Tag as XmlDocument;
            XmlElement Elment = doc.SelectSingleNode("//登陆") as XmlElement;
            string state = Elment.GetAttribute("是否是管理人员");
            if (state == "是") return 1;
            else return 0;
        }
        /// <summary>
        /// 容差只能输入数字和点，如果出错就显示出来
        /// </summary>
        private bool ShowException()
        {
            bool state = true;
            Regex reg = new Regex(@"^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$|^-?[1-9]\d*$");//符点数
            bool SearchBool = reg.IsMatch(txt_search.Text);//搜索里的数据
            bool AreaBool = reg.IsMatch(txt_area.Text);//范围里的数据
            if (!SearchBool || txt_search.Text == "")
            {
                errorProvider1.SetError(txt_search, "只能是数字或小数且不为空！");
                state = false;
            }
            if (!AreaBool || txt_area.Text =="")
            {
                errorProvider2.SetError(txt_area, "只能是数字或小数且不为空！");
                state = false;
            }
            return state;
        }
        /// <summary>
        /// 初始化设置界面上的所有操作图层，将更新库体里的层邦定到设置界面 并初始属性显示列表
        /// </summary>
        /// <returns></returns>
        private void BindTree()
        {
            //JoinCheck JoinChecks = new JoinCheck();
            ////获取要接边检查的数据图层
            //List<ILayer> listLayers = JoinChecks.GetCheckLayers(_AppHk.MapControl.Map, SetCheckState.CheckDataBaseName);
            //if (listLayers == null) return;
            //List<ILayer> listCheckLays = new List<ILayer>();
            //foreach (ILayer pTempLay in listLayers)
            //{
            //    IFeatureLayer pFeatLay = pTempLay as IFeatureLayer;
            //    if (pFeatLay == null) continue;
            //    if (pFeatLay.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation) continue;
            //    //接边针对的是线和面，所以我们操作时只需对线和面层进行即可
            //    if (!(pFeatLay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline || pFeatLay.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)) continue;

            //    listCheckLays.Add(pTempLay);
            //}
            //if (listCheckLays.Count == 0) return;

            //BindDataGrid(listCheckLays[0]);
            //foreach (ILayer layer in listCheckLays)
            //{
            //    DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            //    node.Text = layer.Name;//树的节点名称
            //    node.Tag = layer;//将层放在节点的TAG上，用来后面的操作
            //    node.CheckBoxVisible = true;
            //    TreeLayer.Nodes.Add(node);
            //}
        }

        /// <summary>
        /// 邦定DATAGRID值
        /// </summary>
        /// <param name="layer"></param>
        private void BindDataGrid(ILayer layer)
        {

            IFeatureLayer f_layer = layer as IFeatureLayer;
            IFeatureClass F_class = f_layer.FeatureClass;
            IFields Fields = F_class.Fields;//得到要素类当中的所有列集合
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("字段名", typeof(string));
            for (int n = 0; n < Fields.FieldCount; n++)
            {
                IField field = Fields.get_Field(n);
                if (field.Editable && field.Type != esriFieldType.esriFieldTypeGeometry && field.Type != esriFieldType.esriFieldTypeOID)
                {
                    System.Data.DataRow row = table.NewRow();
                    row[0] = Fields.get_Field(n).Name;//将列名赋值过去
                    table.Rows.Add(row);
                }
            }
            dataGridViewX1.DataSource = table;
            dataGridViewX1.Tag = 0;
            dataGridViewX1.Columns[0].Width = 30;
            dataGridViewX1.Columns[0].ReadOnly = true;
            dataGridViewX1.Columns[1].Width = 218;
            dataGridViewX1.Columns[1].ReadOnly = true;
        }
        /// <summary>
        /// 点击树节点时，改变DATAGRID里的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeLayer_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            //选择节点时，将上一次选择过的属性值赋值给它对应的节点
            if (e.Node.Index >= 0)
            {
                TreeLayer.Nodes[Convert.ToInt32(dataGridViewX1.Tag)].Name = "";
                for (int d = 0; d < dataGridViewX1.Rows.Count; d++)
                {
                    if (Convert.ToBoolean(dataGridViewX1.Rows[d].Cells[0].Value) == true)
                    {
                        string temp = dataGridViewX1.Rows[d].Cells[1].Value.ToString();
                        TreeLayer.Nodes[Convert.ToInt32(dataGridViewX1.Tag)].Name += temp + " ";
                    }

                }
            }
            //选择节点时，判别曾经是否有选择过
            if (e.Node.Name != "")
            {
                #region 如果以前选择过，就让它对应的属性成选择的状态

                ReadNode(e.Node);
                if (!e.Node.Checked)
                {
                    e.Node.Name = "";
                    return;
                }
                string Name = e.Node.Name.Trim();
                char[] sp = { ' ' };
                string[] StrName = Name.Split(sp);
                for (int c = 0; c < dataGridViewX1.Rows.Count; c++)
                {
                    for (int N = 0; N < StrName.Length; N++)
                    {
                        if (StrName[N] == dataGridViewX1.Rows[c].Cells[1].Value.ToString())
                        {
                            dataGridViewX1.Rows[c].Cells[0].Value = true;
                            break;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 没有选择过，就根据节点上的层名读它对应的所有属性
                if (e.Node.Checked)
                {
                    dataGridViewX1.Columns[0].ReadOnly = false;

                }
                else
                {
                    dataGridViewX1.Columns[0].ReadOnly = true;
                }
                ReadNode(e.Node);
                #endregion
            }


        }
        /// <summary>
        /// 根据树上的节点，读取相关图层里的属性邦定到显示控件当中
        /// </summary>
        /// <param name="node"></param>
        private void ReadNode(DevComponents.AdvTree.Node node)
        {
            if (dataGridViewX1.DataSource != null)
                dataGridViewX1.DataSource = null;
            ILayer layer = node.Tag as ILayer;//当前节点上的TAG
            IFeatureLayer f_layer = layer as IFeatureLayer;
            IFeatureClass F_class = f_layer.FeatureClass;
            IFields Fields = F_class.Fields;//得到要素类当中的所有列集合
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("字段名", typeof(string));
            for (int n = 0; n < Fields.FieldCount; n++)
            {
                IField field = Fields.get_Field(n);
                if (field.Editable && field.Type != esriFieldType.esriFieldTypeGeometry && field.Type != esriFieldType.esriFieldTypeOID)
                {
                    System.Data.DataRow row = table.NewRow();
                    row[0] = Fields.get_Field(n).Name;//将列名赋值过去
                    table.Rows.Add(row);
                }
            }
            dataGridViewX1.Tag = node.Index;
            dataGridViewX1.DataSource = table;
            dataGridViewX1.Columns[0].Width = 30;
            dataGridViewX1.Columns[1].Width = 218;
            dataGridViewX1.Columns[1].ReadOnly = true;
        }
        /// <summary>
        /// 关闭窗体时，清除操作过的选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetJoinChecks_FormClosing(object sender, FormClosingEventArgs e)
        {
            int count = TreeLayer.Nodes.Count;
            for (int n = 0; n < count; n++)
            {
                TreeLayer.Nodes[n].Name = "";
            }
        }
        /// <summary>
        /// 单击开始检查时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_enter_Click(object sender, EventArgs e)
        {
            Exception eError=null;

            if(txtRange.Text=="")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择接边范围数据库！");
                return;
            }
            SysCommon.Gis.SysGisDataSet pRangeGisDB = new SysCommon.Gis.SysGisDataSet();
            pRangeGisDB.SetWorkspace(txtRange.Text.ToString().Trim(), SysCommon.enumWSType.PDB, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "接边范围数据库连接失败！");
                return;
            }

            if (ShowException())
            {
                #region 错误日志连接信息
                string logPath = txtLog.Text;
                if (logPath.Trim() == string.Empty)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择日志输出路径!");
                    return;
                }
                if (File.Exists(logPath))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据文件已存在!\n" + logPath);
                }

                //生成日志信息EXCEL格式
                SysCommon.DataBase.SysDataBase pSysLog = new SysCommon.DataBase.SysDataBase();
                pSysLog.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + logPath + "; Extended Properties=Excel 8.0;", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "日志信息表连接失败！");
                    return;
                }
                string strCreateTableSQL = @" CREATE TABLE ";
                strCreateTableSQL += @" 错误日志 ";
                strCreateTableSQL += @" ( ";
                strCreateTableSQL += @" 检查功能名 VARCHAR, ";
                strCreateTableSQL += @" 错误类型 VARCHAR, ";
                strCreateTableSQL += @" 错误描述 VARCHAR, ";
                strCreateTableSQL += @" 数据图层1 VARCHAR, ";
                strCreateTableSQL += @" 数据OID1 VARCHAR, ";
                strCreateTableSQL += @" 数据图层2 VARCHAR, ";
                strCreateTableSQL += @" 数据OID2 VARCHAR, ";
                strCreateTableSQL += @" 定位点X VARCHAR , ";
                strCreateTableSQL += @" 定位点Y VARCHAR , ";
                strCreateTableSQL += @" 检查时间 VARCHAR ,";
                strCreateTableSQL += @" 数据文件路径 VARCHAR ";
                strCreateTableSQL += @" ) ";

                pSysLog.UpdateTable(strCreateTableSQL, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "插入表头出错！");
                    return;
                }

                #endregion

                double AreaValue = Convert.ToDouble(txt_area.Text);//范围容差
                double SearchValue = Convert.ToDouble(txt_search.Text);//搜索容差
                DataCheckClass JoinChecks = new DataCheckClass(_AppHk);
                //将日志表连接信息和表名保存起来
                JoinChecks.ErrDbCon = pSysLog.DbConn;
                JoinChecks.ErrTableName = "错误日志";
                JoinChecks.AREAValue = AreaValue;
                JoinChecks.SEARCHValue = SearchValue;

                SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TopologyCheckClass.GeoDataCheckParaPath;
                pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "GIS数据检查配置表连接失败！");
                    return;
                }
                //获得接边范围的图层名和字段名
                DataTable mTable = GetParaValueTable(pSysTable, 39, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询GIS数据检察配置表失败！");
                    return;
                }
                if (mTable.Rows.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未进行接边检查参数的配置！");
                    return;
                }
                string rangeLayerName = mTable.Rows[0]["图层"].ToString().Trim();
                string rangeFieldName = mTable.Rows[0]["字段项"].ToString().Trim();
                if (rangeFieldName == "" || rangeLayerName == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "接边检查参数配置不完整！");
                    return;
                }

                //设置界面上初始显示的属性表
                TreeLayer.Nodes[Convert.ToInt32(dataGridViewX1.Tag)].Name = "";
                for (int d = 0; d < dataGridViewX1.Rows.Count; d++)
                {
                    if (Convert.ToBoolean(dataGridViewX1.Rows[d].Cells[0].Value) == true)
                    {
                        string temp = dataGridViewX1.Rows[d].Cells[1].Value.ToString();
                        TreeLayer.Nodes[Convert.ToInt32(dataGridViewX1.Tag)].Name += temp + " ";
                    }

                }

                if (!JoinChecks.Initialize_Tree(pRangeGisDB, rangeLayerName, rangeFieldName, _AppHk.DataTree, TreeLayer, _AppHk, out eError))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "初始化树图失败！");
                    return;
                }
                //int Pur = PurviewState();//验证是管理员还是作业员

                ///说明：作业员以图幅去进行接边处理，管理员以任务区范围去接边处理
                //if (Pur == 1)
                //{
                    ////管理员
                    ////初始化数据处理树图
                    //if (!JoinChecks.Initialize_Tree("任务区范围", "CASE_NAME", _AppHk.DataTree, TreeLayer, _AppHk))
                    //{
                    //    return;
                    //}
                //}
                //else if(Pur == 0)
                //{
                //    //作业员
                //    //初始化数据处理树图
                //    if (!JoinChecks.Initialize_Tree("图幅范围", "MAP_ID", _AppHk.DataTree, TreeLayer, _AppHk))
                //    {
                //        return;
                //    }
                //}
                this.Close();
                //数据接边检查 
                //用线程做速度会很慢主要体现在要素空间查询时
                //System.Threading.ParameterizedThreadStart parstart = new System.Threading.ParameterizedThreadStart(JoinChecks.DoJoinCheck);
                //Thread aThread = new Thread(parstart);
                //_AppHk.CurrentThread = aThread;
                //aThread.Priority = ThreadPriority.Highest;
                //aThread.Start(_AppHk as object);, Pur
                int Pur = 0;        //以图幅进行接边
                JoinChecks.DoJoinCheck(_AppHk as object,Pur);
            }
        }

        private void btnDB_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (!pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB");
                            return;
                        }
                        txtDB.Text = pFolderBrowser.SelectedPath;
                    }
                    break;

                case "PDB":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择PDB数据";
                    OpenFile.Filter = "PDB数据(*.mdb)|*.mdb";
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDB.Text = OpenFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        private void btnCon_Click(object sender, EventArgs e)
        {
            Exception eError = null;

            //连接数据库
            pGisDT = new SysCommon.Gis.SysGisDataSet();
            if (comBoxType.Text.Trim() == "SDE")
            {
                pGisDT.SetWorkspace(txtServer.Text, txtInstance.Text, txtDB.Text, txtUser.Text, txtPassword.Text, txtVersion.Text, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }
            else if (comBoxType.Text.Trim() == "GDB")
            {
                pGisDT.SetWorkspace(txtDB.Text.Trim(), SysCommon.enumWSType.GDB, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }
            else
            {
                pGisDT.SetWorkspace(txtDB.Text.Trim(), SysCommon.enumWSType.PDB, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！");
                    return;
                }
            }

            //获得数据库中的所有要素类
            List<IDataset> LstDT = pGisDT.GetAllFeatureClass();
            //要进行检查的要素类
            List<ILayer> LstLayer = new List<ILayer>();
            for(int i=0;i<LstDT.Count;i++)
            {
                IFeatureClass pFeaCls = LstDT[i] as IFeatureClass;
                if(pFeaCls.FeatureType==esriFeatureType.esriFTAnnotation ) continue;
                if(pFeaCls.ShapeType==esriGeometryType.esriGeometryPolyline||pFeaCls.ShapeType==esriGeometryType.esriGeometryPolygon )
                {
                    //对线要素和面要素进行接边检查
                    IFeatureLayer pFeaLayer = new FeatureLayerClass();
                    pFeaLayer.FeatureClass = pFeaCls;
                    ILayer pLayer = pFeaLayer as ILayer;
                    if (!LstLayer.Contains(pLayer))
                    {
                        LstLayer.Add(pLayer);
                    }
                }
            }
            if (LstLayer.Count == 0)
            {
                return;
            }

            //初始化接边检查界面
            BindDataGrid(LstLayer[0]);
            for (int j = 0; j < LstLayer.Count; j++)
            {
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Text =((LstLayer[j] as IFeatureLayer).FeatureClass as IDataset).Name  ;//树的节点名称
                node.Tag = LstLayer[j];//将层放在节点的TAG上，用来后面的操作
                node.CheckBoxVisible = true;
                TreeLayer.Nodes.Add(node);
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.OverwritePrompt = false;
            saveFile.Title = "保存为EXCEL格式";
            saveFile.Filter = "EXCEL格式(*.xls)|*.xls";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                txtLog.Text = saveFile.FileName;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "选择任务范围";
            openFile.Filter = "PDB数据(*.mdb)|*.mdb";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtRange.Text = openFile.FileName;
            }
        }

           /// <summary>
        /// 查找参数值表格
        /// </summary>
        /// <param name="pFeaDataset"></param>
        /// <param name="pSysTable"></param>
        /// <param name="checkParaID">参数ID，唯一标识检查类型</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private DataTable  GetParaValueTable(SysCommon.DataBase.SysTable pSysTable,int checkParaID,out Exception eError)
        {
            eError = null;
            DataTable mTable = null;

            string selStr = "select * from GeoCheckPara where 参数ID=" + checkParaID;
            DataTable pTable = pSysTable.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查询表格错误，表名为：GeoCheckPara，参数ID为:" + checkParaID);
                return null ;
            }

            if (pTable == null || pTable.Rows.Count == 0)
            {
                eError = new Exception("找不到记录，参数ID为:" + checkParaID);
                return null ;
            }
            string ParaType = pTable.Rows[0]["参数类型"].ToString().Trim();            //参数类型
            if (ParaType == "GeoCheckParaValue")
            {
                int ParaValue=int.Parse(pTable.Rows[0]["参数值"].ToString().Trim());   //参数值，用来标识检查类型
                string str = "select * from GeoCheckParaValue where 检查类型=" + ParaValue;
                mTable = pSysTable.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    eError = new Exception("查询表格错误，表名为：GeoCheckParaValue，检查类型为:" + ParaValue);
                    return null ;
                }
            }
            return mTable;
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comBoxType.Text != "SDE")
            {
                btnDB.Visible = true;
                txtDB.Size = new Size(txtServer.Size.Width - btnDB.Size.Width, txtDB.Size.Height);
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtVersion.Enabled = false;
            }
            else
            {
                btnDB.Visible = false;
                txtDB.Size = new Size(txtServer.Size.Width, txtDB.Size.Height);
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                txtVersion.Enabled = true;

            }
        }
    }
}