using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.IO;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;


namespace GeoDBATool
{
    /// <summary>
    /// 陈亚飞添加
    /// </summary>
    public partial class frmDataSubmit : DevComponents.DotNetBar.Office2007Form
    {
        Plugin.Application.IAppGISRef v_AppGIS;
        EnumOperateType v_OpeType;
        private clsDataThread m_DataThread;
        private System.Windows.Forms.Timer _timer;
        private IGeometry m_Geometry = null;

        public frmDataSubmit(EnumOperateType pType, Plugin.Application.IAppGISRef pAppGIS)
        {
            InitializeComponent();
            v_OpeType = pType;
            v_AppGIS = pAppGIS;
            InitForm();
        }
        private void InitForm()
        {
            object[] TagDBType = new object[] { "ESRI文件数据库(*.gdb)", "ArcSDE(For Oracle)", "ESRI个人数据库(*.mdb)" };//"GDB", "SDE", "PDB"
            comboBoxOrgType.Items.AddRange(TagDBType);//源数据
            comboBoxOrgType.SelectedIndex = 0;
            comboBoxOrgType.Enabled = false;
            comBoxType.Items.AddRange(TagDBType);//目标数据
            comBoxType.SelectedIndex = 0;
            cmbHistoType.Items.AddRange(TagDBType);//历史库
            cmbHistoType.SelectedIndex = 0;

            groupPanel3.Visible = false;

            TagDBType = new object[] { "ORACLE", "ACCESS", "SQL" };
            cmbFIDType.Items.AddRange(TagDBType);//FID记录表
            cmbFIDType.SelectedIndex = 0;
            #region 从xml中读取信息并且表现在界面上
            XmlNode ProNode = v_AppGIS.ProjectTree.SelectedNode.Tag as XmlNode;
            //图幅工作库信息
            XmlElement workDBElem = ProNode.SelectSingleNode(".//内容//图幅工作库//连接信息") as XmlElement;
            //cyf 20110628 modify:界面统一
            if (workDBElem.GetAttribute("类型").Trim() == "PDB")
            {
                comboBoxOrgType.Text = "ESRI个人数据库(*.mdb)";
            }
            else if (workDBElem.GetAttribute("类型").Trim() == "GDB")
            {
                comboBoxOrgType.Text = "ESRI文件数据库(*.gdb)";
            }
            else if (workDBElem.GetAttribute("类型").Trim() == "SDE")
            {
                comboBoxOrgType.Text = "ArcSDE(For Oracle)";
            }
            //comboBoxOrgType.Text = workDBElem.GetAttribute("类型").ToString().Trim();
            //end
            textBoxX1.Text = workDBElem.GetAttribute("数据库").ToString().Trim();
            //现势库信息
            XmlElement userDBElem = ProNode.SelectSingleNode(".//内容//现势库//连接信息") as XmlElement;
            //cyf 20110628 modify:界面统一
            if (userDBElem.GetAttribute("类型").Trim() == "PDB")
            {
                comBoxType.Text = "ESRI个人数据库(*.mdb)";
            }
            else if (userDBElem.GetAttribute("类型").Trim() == "GDB")
            {
                comBoxType.Text = "ESRI文件数据库(*.gdb)";
            }
            else if (userDBElem.GetAttribute("类型").Trim() == "SDE")
            {
                comBoxType.Text = "ArcSDE(For Oracle)";
            }
            //comBoxType.Text = userDBElem.GetAttribute("类型").Trim();
            //end
            txtServer.Text = userDBElem.GetAttribute("服务器").Trim();
            txtInstance.Text = userDBElem.GetAttribute("服务名").Trim();
            txtDB.Text = userDBElem.GetAttribute("数据库").Trim();
            txtUser.Text = userDBElem.GetAttribute("用户").Trim();
            txtPassword.Text = userDBElem.GetAttribute("密码").Trim();
            txtVersion.Text = userDBElem.GetAttribute("版本").Trim();
            //历史库信息
            XmlElement historyDBElem = ProNode.SelectSingleNode(".//内容//历史库//连接信息") as XmlElement;
            //cyf 20110628 modify:界面统一
            if (historyDBElem.GetAttribute("类型").Trim() == "PDB")
            {
                cmbHistoType.Text = "ESRI个人数据库(*.mdb)";
            }
            else if (historyDBElem.GetAttribute("类型").Trim() == "GDB")
            {
                cmbHistoType.Text = "ESRI文件数据库(*.gdb)";
            }
            else if (historyDBElem.GetAttribute("类型").Trim() == "SDE")
            {
                cmbHistoType.Text = "ArcSDE(For Oracle)";
            }
            //cmbHistoType.Text = historyDBElem.GetAttribute("类型").Trim();
            //end
            txtHistoServer.Text = historyDBElem.GetAttribute("服务器").Trim();
            txtHistoInstance.Text = historyDBElem.GetAttribute("服务名").Trim();
            txtHistoDB.Text = historyDBElem.GetAttribute("数据库").Trim();
            txtHistoUser.Text = historyDBElem.GetAttribute("用户").Trim();
            txtHistoPassword.Text = historyDBElem.GetAttribute("密码").Trim();
            txtHistoVersion.Text = historyDBElem.GetAttribute("版本").Trim();
            //FID记录表连接信息
            //XmlElement FIDDBElem = ProNode.SelectSingleNode(".//内容//FID记录表//连接信息") as XmlElement;
            //cmbFIDType.Text = FIDDBElem.GetAttribute("类型").Trim();
            //txtFIDServer.Text = FIDDBElem.GetAttribute("服务名").Trim();
            //txtFIDInstance.Text = FIDDBElem.GetAttribute("数据库").Trim();
            //txtFIDUser.Text = FIDDBElem.GetAttribute("用户").Trim();
            //txtFIDPassword.Text = FIDDBElem.GetAttribute("密码").Trim();
            #endregion
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110628 
            if (comBoxType.Text == "ESRI个人数据库(*.mdb)")
            {
                comBoxType.Tag = "PDB";
            }
            else if (comBoxType.Text == "ESRI文件数据库(*.gdb)")
            {
                comBoxType.Tag = "GDB";
            }
            else if (comBoxType.Text == "ArcSDE(For Oracle)")
            {
                comBoxType.Tag = "SDE";
            }
            //end
            if (comBoxType.Text != "ArcSDE(For Oracle)")
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

        private void cmbFIDType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFIDType.Text == "ACCESS")
            {
                btnAccess.Visible = true;
                txtFIDInstance.Size = new Size(txtFIDUser.Size.Width - btnAccess.Size.Width, txtFIDUser.Size.Height);
                txtFIDServer.Enabled = false;
                txtFIDUser.Enabled = false;
                txtFIDPassword.Enabled = false;
            }
            else if (cmbFIDType.Text == "ORACLE")
            {
                btnAccess.Visible = false;
                txtFIDInstance.Size = new Size(txtFIDUser.Size.Width, txtFIDUser.Size.Height);
                txtFIDServer.Enabled = false ;
                txtFIDUser.Enabled = true;
                txtFIDPassword.Enabled = true;
            }
            else
            {
                btnAccess.Visible = false;
                txtFIDInstance.Size = new Size(txtFIDUser.Size.Width, txtFIDUser.Size.Height);
                txtFIDServer.Enabled = true;
                txtFIDUser.Enabled = true;
                txtFIDPassword.Enabled = true;
            }
        }

        private void btnXML_Click(object sender, EventArgs e)
        {
            //OpenFileDialog OpenFile = new OpenFileDialog();
            //OpenFile.CheckFileExists = true;
            //OpenFile.CheckPathExists = true;
            //OpenFile.Title = "选择映射文件";
            //OpenFile.Filter = "映射文件(*.xml)|*.xml";
            //if (OpenFile.ShowDialog() == DialogResult.OK)
            //{
            //    txtXML.Text = OpenFile.FileName;
            //}
        }

        private void btnDB_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
            {
                case "ArcSDE(For Oracle)":

                    break;

                case "ESRI文件数据库(*.gdb)":
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

                case "ESRI个人数据库(*.mdb)":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择ESRI个人数据库";
                    OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDB.Text = OpenFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        private void btnAccess_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择MDB数据";
            OpenFile.Filter = "MDB数据(*.mdb)|*.mdb";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
               txtFIDInstance.Text = OpenFile.FileName;
            }
        }

        private void btnOrg_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
            if (pFolderBrowser.ShowDialog() == DialogResult.OK)
            {
                if (!pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB格式文件");
                    return;
                }
                this.textBoxX1.Text = pFolderBrowser.SelectedPath;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            SysCommon.Gis.SysGisDataSet pHistoSysGisDT = new SysCommon.Gis.SysGisDataSet();//历史库连接
            //判断源数据是否连接
            if (comboBoxOrgType.Text == "ESRI文件数据库(*.gdb)")
            {
                if (this.textBoxX1.Text == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置源数据连接");
                    return;
                }
            }

            else if (comBoxType.Text == "ArcSDE(For Oracle)")
            {
                if (txtServer.Text==""||txtInstance.Text==""||txtDB.Text==""||txtVersion.Text==""|| txtUser.Text == "" || txtPassword.Text == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置目标数据连接!");
                    return;
                }
            }
            else
            {
                if (txtDB.Text == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置目标数据连接!");
                    return;
                }
            }

            string fileXml = "";
            if (v_OpeType == EnumOperateType.Submit)
            {
                //图幅数据提交
                fileXml = ModData.DBTufuInputXml;
            }
            if (!File.Exists(fileXml))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "对照关系文件不存在!\n"+fileXml);
                return;
            }

            //判断历史库是否连接成功
            bool isConSucc = true;
            if (cmbHistoType.Text == "ArcSDE(For Oracle)")
            {
                if (txtHistoServer.Text.Trim() == "" || txtHistoUser.Text.Trim() == "" || txtHistoPassword.Text.Trim() == "")
                {
                    isConSucc = false;
                }
                else
                {
                    pHistoSysGisDT.SetWorkspace(txtHistoServer.Text.Trim(), txtHistoInstance.Text.Trim(), txtHistoDB.Text.Trim(), txtHistoUser.Text.Trim(), txtHistoPassword.Text.Trim(), txtHistoVersion.Text.Trim(), out eError);
                    if (eError != null)
                    {
                        isConSucc = false;
                    }
                }
            }
            else if (cmbHistoType.Text.Trim() == "ESRI文件数据库(*.gdb)")
            {
                if (txtHistoDB.Text.Trim() == "")
                {
                    isConSucc = false;
                }
                else
                {
                    pHistoSysGisDT.SetWorkspace(txtHistoDB.Text.Trim(), SysCommon.enumWSType.GDB, out eError);
                    if (eError != null)
                    {
                        isConSucc = false;
                    }
                }

            }
            else
            {
                if (txtHistoDB.Text.Trim() == "")
                {
                    isConSucc = false;
                }
                else
                {
                    pHistoSysGisDT.SetWorkspace(txtHistoDB.Text.Trim(), SysCommon.enumWSType.PDB, out eError);
                    if (eError != null)
                    {
                        isConSucc = false;
                    }
                }
            }
            if (!isConSucc)
            {
                //若历史库连接不成功
                SysCommon.Error.frmInformation InfoDial = new SysCommon.Error.frmInformation("是", "否", "是否将数据置为历史？");
                if (InfoDial.ShowDialog() == DialogResult.OK)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "历史库连接失败,请重新选择历史库！");
                    return;
                }
            }
#region FID记录表是否连接成功
            //SysCommon.DataBase.SysDataBase pSysDB = new SysCommon.DataBase.SysDataBase();
            //string connectionStr = "";
            //switch (cmbFIDType.Text.ToUpper())
            //{
            //    case "ORACLE":
            //        connectionStr = "Data Source=" + txtFIDInstance.Text + ";Persist Security Info=True;User ID=" + txtFIDUser.Text + ";Password=" + txtFIDPassword.Text + ";Unicode=True";
            //        pSysDB.SetDbConnection(connectionStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            //        break;
            //    case "ACCESS":
            //        connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +txtFIDInstance.Text + ";Mode=Share Deny None;Persist Security Info=False";
            //        pSysDB.SetDbConnection(connectionStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            //        break;
            //    case "SQL":
            //        connectionStr = "Data Source=" + txtFIDInstance.Text + ";Initial Catalog=" + txtFIDServer.Text + ";User ID=" + txtFIDUser.Text + ";Password=" + txtFIDPassword.Text;
            //        pSysDB.SetDbConnection(connectionStr, SysCommon.enumDBConType.SQL, SysCommon.enumDBType.SQLSERVER, out eError);
            //        break;
            //}
            //if (eError != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "FID记录表库连接失败!");
            //    return;
            //}
#endregion

            //获得图幅总范围
            XmlNode ProNode =v_AppGIS.ProjectTree.SelectedNode.Tag as XmlNode;
            XmlElement workDBElem = ProNode.SelectSingleNode(".//内容//图幅工作库//范围信息") as XmlElement;
            string rangeStr = workDBElem.GetAttribute("范围").Trim();
            byte[] xmlByte = Convert.FromBase64String(rangeStr);
            object pGeo = new PolygonClass();
            if (XmlDeSerializer(xmlByte, pGeo) == false)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "几何范围解析出错！");
                return;
            }
            m_Geometry = pGeo as IGeometry;
            if (m_Geometry == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "图幅范围为空！");
                return;
            }

            string pDateTimeStr = DateTime.Now.ToString("u");

            Plugin.Application.IAppFormRef pAppFormRef =v_AppGIS as Plugin.Application.IAppFormRef;
            pAppFormRef.OperatorTips = "获取数据信息...";

            //根据窗体设置形成数据操作xml内容
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<数据移植></数据移植>");
            XmlNode sourceNode = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "源数据连接");

            XmlElement dataElement = xmlDoc.CreateElement("源数据");
            dataElement.SetAttribute("类型", comboBoxOrgType.Tag.ToString().Trim());
            if (comboBoxOrgType.Text.Trim() == "ESRI文件数据库(*.gdb)")
            {
                dataElement.SetAttribute("服务器", "");
                dataElement.SetAttribute("服务名", "");
                dataElement.SetAttribute("数据库", this.textBoxX1.Text);
                dataElement.SetAttribute("用户", "");
                dataElement.SetAttribute("密码", "");
                dataElement.SetAttribute("版本", "");
            } 
            sourceNode.AppendChild((XmlNode)dataElement);

            XmlElement objElementTemp = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "目标数据连接") as XmlElement;
            objElementTemp.SetAttribute("类型", comBoxType.Tag.ToString().Trim());
            objElementTemp.SetAttribute("服务器", txtServer.Text.Trim());
            objElementTemp.SetAttribute("服务名", txtInstance.Text.Trim());
            objElementTemp.SetAttribute("数据库", txtDB.Text.Trim());
            objElementTemp.SetAttribute("用户", txtUser.Text.Trim());
            objElementTemp.SetAttribute("密码", txtPassword.Text.Trim());
            objElementTemp.SetAttribute("版本", txtVersion.Text.Trim());

            XmlElement RuleElement = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "规则") as XmlElement;
            RuleElement.SetAttribute("路径", fileXml);
           
            pAppFormRef.OperatorTips = "初始化数据处理树图...";
            //初始化数据处理树图
            InitialDBTree newInitialDBTree = new InitialDBTree();
            newInitialDBTree.OnCreateDataTree(v_AppGIS.DataTree, xmlDoc);
            if ((bool)v_AppGIS.DataTree.Tag == false) return;

            //进行数据移植
            this.Hide();
            pAppFormRef.OperatorTips = "进行图幅数据提交...";
            esriSpatialRelEnum RelEnum = esriSpatialRelEnum.esriSpatialRelIntersects;
			if (m_DataThread == null)
            {
            m_DataThread = new clsDataThread(v_AppGIS, m_Geometry, RelEnum, true, null, null, v_OpeType,pHistoSysGisDT,pDateTimeStr);
			}
            Thread aThread = new Thread(new ThreadStart(m_DataThread.DoBatchWorks));
            m_DataThread.CurrentThread = aThread;
			m_DataThread.UserName = txtHistoUser.Text;//历史库用户名  xisheng 2011.07.15
            m_DataThread.UserNameNow = txtUser.Text;//现实库用户名  xisheng 2011.07.15
            v_AppGIS.CurrentThread = aThread;
            aThread.Start();

            //利用计时器刷新mapcontrol
			if (_timer == null)
            {
            _timer = new System.Windows.Forms.Timer();
			}
            _timer.Interval = 800;
            _timer.Enabled = true;
            _timer.Tick += new EventHandler(Timer_Tick);
        }

        //利用计时器刷新mapcontrol
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (m_DataThread.CurrentThread.ThreadState == ThreadState.Stopped)
            {
                if (m_DataThread.Res == true)
                {
                    #region 将界面上的信息写入XML里面保存起来
                    //XmlDocument ProXMlDoc = new XmlDocument();
                    //ProXMlDoc.Load(ModData.v_projectXML);
                    XmlNode ProjectNode = v_AppGIS.ProjectTree.SelectedNode.Tag as XmlNode;// ProXMlDoc.SelectSingleNode(".//工程[@名称='" + v_AppGIS.ProjectTree.SelectedNode.Name + "']");
                    //将界面上的图幅工作库信息写入到XML中
                    XmlElement workDBElem = ProjectNode.SelectSingleNode(".//内容//图幅工作库//连接信息") as XmlElement;
                    workDBElem.SetAttribute("类型", comboBoxOrgType.Tag.ToString().Trim());  //cyf 20110628
                    workDBElem.SetAttribute("数据库", textBoxX1.Text.Trim());
                    //将界面上的现势库信息写入到XML中
                    XmlElement userDBElem = ProjectNode.SelectSingleNode(".//内容//现势库//连接信息") as XmlElement;
                    userDBElem.SetAttribute("类型", comBoxType.Tag.ToString().Trim()); //cyf 20110628 
                    userDBElem.SetAttribute("服务器", txtServer.Text.Trim());
                    userDBElem.SetAttribute("服务名", txtInstance.Text.ToString().Trim());
                    userDBElem.SetAttribute("数据库", txtDB.Text.Trim());
                    userDBElem.SetAttribute("用户", txtUser.Text.Trim());
                    userDBElem.SetAttribute("密码", txtPassword.Text.Trim());
                    userDBElem.SetAttribute("版本", txtVersion.Text.Trim());
                    //将界面上的历史库信息写入到XML中
                    XmlElement historyDBElem = ProjectNode.SelectSingleNode(".//内容//历史库//连接信息") as XmlElement;
                    historyDBElem.SetAttribute("类型", cmbHistoType.Tag.ToString().Trim());//cyf 20110628
                    historyDBElem.SetAttribute("服务器", txtHistoServer.Text.Trim());
                    historyDBElem.SetAttribute("服务名", txtHistoInstance.Text.ToString().Trim());
                    historyDBElem.SetAttribute("数据库", txtHistoDB.Text.Trim());
                    historyDBElem.SetAttribute("用户", txtHistoUser.Text.Trim());
                    historyDBElem.SetAttribute("密码", txtHistoPassword.Text.Trim());
                    historyDBElem.SetAttribute("版本", txtHistoVersion.Text.Trim());
                    //cyf 2011028
                    //将界面上的FID记录表库体信息写入到XML中 
                    //XmlElement FIDDBElem = ProjectNode.SelectSingleNode(".//内容//FID记录表//连接信息") as XmlElement;
                    //FIDDBElem.SetAttribute("类型", cmbFIDType.Text.ToString().Trim());
                    //FIDDBElem.SetAttribute("服务名", txtFIDServer.Text.ToString().Trim());
                    //FIDDBElem.SetAttribute("数据库", txtFIDInstance.Text.Trim());
                    //FIDDBElem.SetAttribute("用户", txtFIDUser.Text.Trim());
                    //FIDDBElem.SetAttribute("密码", txtFIDPassword.Text.Trim());
                    //将映射规则信息写入XML中
                    //XmlElement ruleElem = ProjectNode.SelectSingleNode(".//内容//数据操作规则//规则[@类型='图幅数据更新入库']") as XmlElement;
                    //ruleElem.SetAttribute("路径", ModData.DBTufuSubmitXml);
                    //ProjectNode.OwnerDocument.Save(ModData.v_projectXML);
                    //end
                    #endregion
                  
                    this.Close();
                }
                else
                {
                    this.Show();
                }

                m_DataThread = null;
                _timer.Enabled = false;
            }
        }

        /// <summary>
        /// 将xmlByte解析为obj
        /// </summary>
        /// <param name="xmlByte"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool XmlDeSerializer(byte[] xmlByte, object obj)
        {
            try
            {
                //判断字符串是否为空
                if (xmlByte != null)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    xmlStream.LoadFromBytes(ref xmlByte);
                    pStream.Load(xmlStream as ESRI.ArcGIS.esriSystem.IStream);

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 从坐标字符串得到范围Polygon
        /// </summary>
        /// <param name="strCoor">坐标字符串,格式为X@Y,以逗号分割</param>
        /// <returns></returns>
        public static IPolygon GetPolygonByCol(string strCoor)
        {
            try
            {
                object after = Type.Missing;
                object before = Type.Missing;
                IPolygon polygon = new PolygonClass();
                IPointCollection pPointCol = (IPointCollection)polygon;
                string[] strTemp = strCoor.Split(',');
                for (int index = 0; index < strTemp.Length; index++)
                {
                    string CoorLine = strTemp[index];
                    string[] coors = CoorLine.Split('@');

                    double X = Convert.ToDouble(coors[0]);
                    double Y = Convert.ToDouble(coors[1]);

                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(X, Y);
                    pPointCol.AddPoint(pPoint, ref before, ref after);
                }

                polygon = (IPolygon)pPointCol;
                polygon.Close();

                ITopologicalOperator pTopo = (ITopologicalOperator)polygon;
                pTopo.Simplify();

                return polygon;
            }
            catch(Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************

                return null;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            //OpenFileDialog OpenFile = new OpenFileDialog();
            //OpenFile.CheckFileExists = true;
            //OpenFile.CheckPathExists = true;
            //OpenFile.Title = "选择图形范围坐标txt";
            //OpenFile.Filter = "图形范围坐标文本(*.txt)|*.txt";
            //if (OpenFile.ShowDialog() == DialogResult.OK)
            //{
            //    this.textBoxX2.Text = OpenFile.FileName;
            //    StringBuilder sb = new StringBuilder();
            //    try
            //    {
            //        StreamReader sr = new StreamReader(OpenFile.FileName);
            //        while (sr.Peek() >= 0)
            //        {
            //            string[] strTemp = sr.ReadLine().Split(',');
            //            if (sb.Length != 0)
            //            {
            //                sb.Append(",");
            //            }
            //            sb.Append(strTemp[0] + "@" + strTemp[1]);
            //        }
            //    }
            //    catch
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "图形范围坐标txt格式不正确!\n文本每行为点坐标且以','分割");
            //        return;
            //    }

            //    if (sb.Length == 0) return;
            //    m_Geometry = ModDBOperator.GetPolygonByCol(sb.ToString()) as IGeometry;
            //}
        }

        private void cmbHistoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110628 
            if (cmbHistoType.Text == "ESRI个人数据库(*.mdb)")
            {
                cmbHistoType.Tag = "PDB";
            }
            else if (cmbHistoType.Text == "ESRI文件数据库(*.gdb)")
            {
                cmbHistoType.Tag = "GDB";
            }
            else if (cmbHistoType.Text == "ArcSDE(For Oracle)")
            {
                cmbHistoType.Tag = "SDE";
            }
            //end
            if (cmbHistoType.Text != "ArcSDE(For Oracle)")
            {
               btnHistoDB.Visible = true;
               txtHistoDB.Size = new Size(txtHistoServer.Size.Width -btnHistoDB.Size.Width,txtHistoDB.Size.Height);
               txtHistoServer.Enabled = false;
               txtHistoInstance.Enabled = false;
               txtHistoUser.Enabled = false;
               txtHistoPassword.Enabled = false;
               txtHistoVersion.Enabled = false;
            }
            else
            {
                btnHistoDB.Visible = false;
                txtHistoDB.Size = new Size(txtHistoServer.Size.Width, txtHistoDB.Size.Height);
                txtHistoServer.Enabled = true;
                txtHistoInstance.Enabled = true;
                txtHistoUser.Enabled = true;
                txtHistoPassword.Enabled = true;
                txtHistoVersion.Enabled = true;

            }
        }

        private void btnHistoDB_Click(object sender, EventArgs e)
        {
            switch (cmbHistoType.Text)
            {
                case "ArcSDE(For Oracle)":

                    break;

                case "ESRI文件数据库(*.gdb)":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (!pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB");
                            return;
                        }
                       txtHistoDB.Text = pFolderBrowser.SelectedPath;
                    }
                    break;

                case "ESRI个人数据库(*.mdb)":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择ESRI个人数据库";
                    OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                       txtHistoDB.Text = OpenFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        private void comboBoxOrgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110628 
            if (comboBoxOrgType.Text == "ESRI个人数据库(*.mdb)")
            {
                comboBoxOrgType.Tag = "PDB";
            }
            else if (comboBoxOrgType.Text == "ESRI文件数据库(*.gdb)")
            {
                comboBoxOrgType.Tag = "GDB";
            }
            else if (comboBoxOrgType.Text == "ArcSDE(For Oracle)")
            {
                comboBoxOrgType.Tag = "SDE";
            }
            //end
        }
    }
}