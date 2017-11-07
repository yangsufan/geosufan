using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using SysCommon;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geoprocessing;

namespace GeoDBIntegration
{
    /// <summary>
    /// 栅格数据建库　　陈亚飞添加　
    /// </summary>
    public partial class FrmCreateRasterDB : DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace pworkSpace = null;
        DevComponents.AdvTree.Node m_ProjectNode = null;  //当前数据库工程节点
        string m_sDBType = "";                              //数据库类型：高程数据库、影像数据库
        List<string> listroot = new List<string>();

        public FrmCreateRasterDB(DevComponents.AdvTree.Node ProjectNode,string sDBType)
        {
            InitializeComponent();

            #region 初始化界面信息

            // m_Hook = pHook;
            cmbRasterType.Items.Clear();
            cmbRasterSpaRef.Items.Clear();
            cmbGeoSpaRef.Items.Clear();
            comBoxType.Items.Clear();
            cmbRasterPixeType.Items.Clear();
            //cyf 20110609 add:设置栅格数据管理类型
            cmbManageType.Items.Clear();
            cmbManageType.Items.AddRange(new object[] {  "非管理型","管理型" });
            cmbManageType.SelectedIndex = 0;
            //end
            //cmbRasterType.Items.AddRange(new object[] { "DOM", "DEM" });//"DLG",
            cmbRasterType.Items.AddRange(new object[] { "栅格编目","栅格数据集" });
            cmbRasterType.SelectedIndex = 0;
            //cyf 20110630 modify;
            object[] TagDBType = new object[] { "ESRI文件数据库(*.gdb)", "ArcSDE(For Oracle)", "ESRI个人数据库(*.mdb)" };//"GDB", "SDE", "PDB"
            //end
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;
            object[] spaRef = new object[] { "西安高斯117度(3度带)", "西安高斯120度(3度带)", "西安高斯123度(3度带)"};
            cmbRasterSpaRef.Items.AddRange(spaRef);
            //cmbRasterSpaRef.SelectedIndex = 0;
            cmbGeoSpaRef.Items.AddRange(spaRef);
            //cmbGeoSpaRef.SelectedIndex = 0;

            cmbResampleType.Items.Clear();
            cmbCompression.Items.Clear();

            cmbResampleType.Items.AddRange(new object[] { "邻近法", "双线性内插法", "立方卷积法" });
            cmbResampleType.SelectedIndex = 0;

            cmbCompression.Items.AddRange(new object[] { "LZ77", "JPEG", "JPEG2000", "PackBits", "LZW" });
            cmbCompression.SelectedIndex = 0;

            cmbRasterPixeType.Items.AddRange(new object[] { "PT_UCHAR", "PT_UNKNOWN", "PT_U1", "PT_U2", "PT_U4", "PT_CHAR", 
                "PT_USHORT", "PT_SHORT", "PT_ULONG", "PT_LONG","PT_FLOAT","PT_DOUBLE","PT_COMPLEX","PT_DCOMPLEX" });
            cmbRasterPixeType.SelectedIndex = 0;

            tileH.Text = "128";
            tileW.Text = "128";
            txtBand.Text = "1";
            rbcatalog.Checked = true;
            #endregion


            //*********************************************************************************************************************//
            /////guozheng 2010-10-8   陈亚飞修改20101011
            /////根据输入的树图数据库工程节点，初始化界面的连接信息
            #region 将树节点信息填写在界面上
            try
            {
                if (ProjectNode == null) return;
                if(ProjectNode.Tag==null) return;
                if (sDBType == "") return;
                m_ProjectNode = ProjectNode;
                m_sDBType = sDBType;

                XmlElement ele = ProjectNode.Tag as XmlElement;

                string[] sConInfo=ele.GetAttribute("数据库连接信息").Split('|');
                string sDBFormatID = ele.GetAttribute("数据库平台ID");  //cyf 20110629 modify
                string sDBName= ele.GetAttribute("数据库工程名");
                if (sDBFormatID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
                {
                    comBoxType.SelectedIndex = 2;
                    this.txtServer.Text = "";
                    this.txtServer.Enabled = false;
                    this.txtInstance.Text = "";
                    this.txtInstance.Enabled = false;
                    this.txtDataBase.Text=sConInfo[2];
                    this.txtDataBase.Enabled = false;
                    this.txtUser.Text = "";
                    this.txtUser.Enabled = false;
                    this.txtPassWord.Text = "";
                    this.txtPassWord.Enabled = false;
                    this.txtVersion.Text = "";
                    this.txtVersion.Enabled = false;
                    //this.txtRasterName.Text = sDBName;
                    //this.txtRasterName.Enabled = false;
                }
                else if (sDBFormatID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
                {
                    comBoxType.SelectedIndex = 0;
                    this.txtServer.Text = "";
                    this.txtServer.Enabled = false;
                    this.txtInstance.Text = "";
                    this.txtInstance.Enabled = false;
                    this.txtDataBase.Text = sConInfo[2];
                    this.txtDataBase.Enabled = false;
                    this.txtUser.Text = "";
                    this.txtUser.Enabled = false;
                    this.txtPassWord.Text = "";
                    this.txtPassWord.Enabled = false;
                    this.txtVersion.Text = "";
                    this.txtVersion.Enabled = false;
                    //this.txtRasterName.Text = sDBName;
                    //this.txtRasterName.Enabled = false;
                }
                else if (sDBFormatID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                {
                    comBoxType.SelectedIndex = 1;
                    this.txtServer.Text = sConInfo[0];
                    this.txtInstance.Text = sConInfo[1];
                    this.txtDataBase.Text = sConInfo[2];
                    this.txtDataBase.Enabled = false;
                    this.txtUser.Text = sConInfo[3];
                    this.txtPassWord.Text = sConInfo[4];
                    this.txtVersion.Text = sConInfo[5];
                    //this.txtRasterName.Text = sDBName;
                    //this.txtRasterName.Enabled = false;
                }
                comBoxType.Enabled = false;
            }
            catch
            {
            }
            #endregion
            //***************************************************************************************************************************************//
        	if (comBoxType.SelectedIndex == 1)//若为SDE，则只能选择管理型 xisheng 07.19
            {
                cmbManageType.SelectedIndex = 1;
                cmbManageType.Enabled = false;
            }    
        }

        /// <summary>
        /// Get Raster Pixel Type
        /// </summary>
        /// <returns></returns>
        private rstPixelType GetPixelType()
        {
            rstPixelType pPixelType =rstPixelType.PT_UCHAR;
            switch(cmbRasterPixeType.Text.Trim())
            {     
                case "PT_UNKNOWN":
                    pPixelType = rstPixelType.PT_UNKNOWN;
                    break;
                case "PT_U1":
                    pPixelType = rstPixelType.PT_U1;
                    break;
                case "PT_U2":
                    pPixelType = rstPixelType.PT_U2;
                    break;
                case "PT_U4":
                    pPixelType = rstPixelType.PT_U4;
                    break;
                case "PT_UCHAR":
                    pPixelType = rstPixelType.PT_UCHAR;
                    break;
                case "PT_CHAR":
                    pPixelType = rstPixelType.PT_CHAR;
                    break;
                case "PT_USHORT":
                    pPixelType = rstPixelType.PT_USHORT;
                    break;
                case "PT_SHORT":
                    pPixelType = rstPixelType.PT_SHORT;
                    break;
                case "PT_ULONG":
                    pPixelType = rstPixelType.PT_ULONG;
                    break;
                case "PT_LONG":
                    pPixelType = rstPixelType.PT_LONG;
                    break;
                case "PT_FLOAT":
                    pPixelType = rstPixelType.PT_FLOAT;
                    break;
                case "PT_DOUBLE":
                    pPixelType = rstPixelType.PT_DOUBLE;
                    break;
                case "PT_COMPLEX":
                    pPixelType = rstPixelType.PT_COMPLEX;
                    break;
                case "PT_DCOMPLEX":
                    pPixelType = rstPixelType.PT_DCOMPLEX;
                    break;
                default:
                    break;
            }
            return pPixelType;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            Exception err = null;
            //cyf 20110607 modify
            if (ModuleData.TempWks == null)
            {
                MessageBox.Show("连接系统维护库失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
            if (pFeaWS == null)
            {
                MessageBox.Show("连接系统维护库失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //end
            #region 检查设置是否完备

            //是否选择建库的类型
            if (rbcatalog.Checked == false && rbdataset.Checked == false)
            {
                MessageBox.Show("请选择建立栅格库的类型", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //服务器连接设置检查

            if (comBoxType.SelectedIndex == 1)
            {
                if (txtUser.Text.Length == 0 || txtPassWord.Text.Length == 0)
                {
                    MessageBox.Show("请完整设置SDE服务器访问参数！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                if (txtDataBase.Text.Length == 0)
                {
                    MessageBox.Show("请完整设置本地数据库路径！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            //判断数据库是否已经存在
//cyf 20110625 delete:允许在一个数据库中创建多个图层
            //if (comBoxType.Text.Trim().ToUpper() == "PDB")
            //{
            //    if (File.Exists(txtDataBase.Text.Trim()))
            //    {
            //        MessageBox.Show("数据库'" + txtDataBase.Text.Trim().Substring(txtDataBase.Text.Trim().LastIndexOf('\\') + 1) + "'已经存在，请检查！");
            //        return;
            //    }
            //}
            //else if (comBoxType.Text.Trim().ToUpper() == "GDB")
            //{

            //    if (Directory.Exists(txtDataBase.Text.Trim()))
            //    {
            //        MessageBox.Show("数据库'" + txtDataBase.Text.Trim().Substring(txtDataBase.Text.Trim().LastIndexOf('\\') + 1) + "'已经存在，请检查！");
            //        return;
            //    }
                
            //}
//end
            if (rbdataset.Checked)//　cmbRasterType.Text.Trim() == "栅格数据集")
            {
                //栅格数据集设置检查

                if (cmbCompression.Text.Trim() == "")
                {
                    MessageBox.Show("请选择压缩类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cmbResampleType.Text.Trim() == "")
                {
                    MessageBox.Show("请选择重采样类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                try
                {
                    if (txtPyramid.Text.Trim() != "")
                    {
                        Convert.ToInt32(txtPyramid.Text.Trim());
                    }

                    Convert.ToInt32(tileH.Text.Trim());
                    Convert.ToInt32(tileW.Text.Trim());
                    Convert.ToInt32(txtBand.Text.Trim());
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("请填写有效的数字!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //labelXErr.Text = "请填写有效的数字！";
                    return;
                }
            }
            /* xisheng 0914 不提示
            //else if (rbcatalog.Checked)//cyf 20110629 add
            //{
            //    if (lvRootPath.CheckedItems.Count == 0)
            //    {
            //        MessageBox.Show("请选择栅格编目存储的ftp的路径!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //}
            end */

            if (txtRasterName.Text.Trim() == "")
            {
                MessageBox.Show("请设置栅格编目名称或栅格数据集名称！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //======================================================================
            //chenyafei  20100107  add:   增加对上个编目，名称的控制和保护
            string pRasterName = "";   //栅格数据集或者栅格编目名称
            pRasterName = txtRasterName.Text.Trim();
            if (pRasterName.ToUpper() == "RASTER")
            {
                MessageBox.Show("改名称是保留字段，不能用此命名", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                string firstLetter = pRasterName.Substring(0, 1);
                try
                {
                    Convert.ToInt16(firstLetter);
                    MessageBox.Show("栅格编目或栅格数据集名称不能数字开头", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                catch { }
            }
            //======================================================================================

            #endregion


            #region 设置数据库连接

            //SysCommon.Gis.SysGisDataSet pSysDT = new SysCommon.Gis.SysGisDataSet();
            if (this.comBoxType.SelectedIndex == 2)    //PDB库体
            {
                //pSysDT.SetWorkspace(txtDataBase.Text.Trim(),SysCommon.enumWSType.PDB,out err);
                SetDestinationProp("PDB", txtDataBase.Text, "", "", "", "");
            }
            else if (this.comBoxType.SelectedIndex == 1)    //SDE库体
            {
                //pSysDT.SetWorkspace(txtServer.Text, txtInstance.Text, "", txtUser.Text, txtPassWord.Text, txtVersion.Text, out err);
                SetDestinationProp("SDE", txtServer.Text, txtInstance.Text, txtUser.Text, txtPassWord.Text, txtVersion.Text);
            }
            else if (this.comBoxType.SelectedIndex == 0)   //GDB库体
            {
                //pSysDT.SetWorkspace(txtDataBase.Text.Trim(), SysCommon.enumWSType.GDB, out err);
                SetDestinationProp("GDB", txtDataBase.Text, "", "", "", "");
            }
            if (err != null)
            {
                MessageBox.Show("连接数据库出错！");
                return;
            }
            //pworkSpace=pSysDT.WorkSpace;
            #endregion

            //创建库体
            //ISpatialReference pGeoSpaRef = GetSpatialRef(cmbGeoSpaRef.Text.Trim());
            //ISpatialReference pRasterSpaRef = GetSpatialRef(cmbRasterSpaRef.Text.Trim());

            //几何空间参考

            ISpatialReference pGeoSpaRef = GetSpatialRef(txtGeoSpati.Text.Trim(), out err);
            if (err != null)
            {
                return;
            }
            //栅格空间参考

            ISpatialReference pRasterSpaRef = GetSpatialRef2(txtRasterSpati.Text.Trim(), out err);
            if (err != null)
            {
                return;
            }

            rstPixelType pPixelType = GetPixelType();
            //栅格数据工作空间
            IRasterWorkspaceEx pRasterWSEx = pworkSpace as IRasterWorkspaceEx;
            if (pRasterWSEx == null)
            {
                MessageBox.Show("数据库连接出错！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (rbcatalog.Checked)
            {
                
                //首先判断栅格目录是否存在
                try
                {
                    IRasterCatalog tempRasterCatalog = pRasterWSEx.OpenRasterCatalog(txtRasterName.Text.Trim());
                    if (tempRasterCatalog != null)
                    {
                        MessageBox.Show("栅格数据'" + txtRasterName.Text.Trim() + "'已经存在，请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (System.Exception ex)
                {

                }
                //创建栅格目录
                bool beManaged = true;   //栅格管理型
                if (cmbManageType.Text == "非管理型")
                {
                    beManaged = false;
                }
                CreateCatalog(pRasterWSEx, txtRasterName.Text.Trim(), "Raster", "Shape", pRasterSpaRef, pGeoSpaRef, "",beManaged, out err);
                //若为栅格目录类型，还需要创建历史栅格目录
                //cyf 20110619  add:添加创建历史RasterCatalog
                try
                {
                    IRasterCatalog tempRasterCatalog = pRasterWSEx.OpenRasterCatalog(txtRasterName.Text.Trim()+"_GOH");
                    if (tempRasterCatalog != null)
                    {
                        MessageBox.Show("栅格数据'" + txtRasterName.Text.Trim() + "_GOH" + "'已经存在，请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (System.Exception ex)
                {

                }
                //创建栅格目录
                CreateCatalog(pRasterWSEx, txtRasterName.Text.Trim()+"_GOH", "Raster", "Shape", pRasterSpaRef, pGeoSpaRef, "", beManaged, out err);
               //end
            }
            else if (rbdataset.Checked)
            {
                //其次判断栅格数据集是否存储
                try
                {
                    IRasterDataset tempRasterDataset = pRasterWSEx.OpenRasterDataset(txtRasterName.Text.Trim());
                    if (tempRasterDataset != null)
                    {
                        MessageBox.Show("栅格数据'" + txtRasterName.Text.Trim() + "'已经存在，请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (System.Exception ex)
                {

                }
                //创建栅格数据集

                CreateRasterDataset(pRasterWSEx, txtRasterName.Text.Trim(), Convert.ToInt32(txtBand.Text.Trim()), pPixelType, pRasterSpaRef, pGeoSpaRef, null, null, "", out err);
            }
            else
            {
                MessageBox.Show("请选择栅格数据库的类型！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (err != null)
            {
                MessageBox.Show(err.Message);
                //labelXErr.Text = err.Message;
                return;
            }

            #region 将参数信息写入数据库     陈亚飞 20101011

            string dbtypeStr = "";                  //栅格数据类型
           
            string pResampleStr="";                 //重采样
             string pCompressionStr="";              //压缩类型
            string pPyramidStr = "";                //金字塔 
            string pTileHStr="";                     //瓦片高度
            string pTileWStr="";                    //瓦片宽度
            string pBandStr = "";                     //波段
            string pDTName = "";                    //数据集名称
            
            //给变量赋值
            if (rbcatalog.Checked)
            {
                dbtypeStr = "栅格编目";
            }
            else if (rbdataset.Checked)
            {
                dbtypeStr = "栅格数据集";
            }
            pResampleStr=cmbResampleType.Text.Trim();
            pCompressionStr=cmbCompression.Text.Trim();
            pPyramidStr= txtPyramid.Text.Trim();
            pTileHStr= tileH.Text.Trim();
            pTileWStr=tileW.Text.Trim();
            pBandStr =txtBand.Text.Trim();
            pDTName = txtRasterName.Text.Trim();

            //将信息写入数据库
            DevComponents.AdvTree.Node pCurNode = m_ProjectNode;  //获得树图上选择的工程节点
            string pProjectID = pCurNode.DataKey.ToString();                         //数据库工程ID

            //连接数据库
            DevComponents.AdvTree.Node rootNode = m_ProjectNode.Parent.Parent;
            if (rootNode == null) return;
            if (rootNode.Tag == null) return;  
            //SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();

            //string pConnStr = rootNode.Tag.ToString();   //系统维护库连接信息
            //pSysTable.SetDbConnection(pConnStr, enumDBConType.ORACLE, enumDBType.ORACLE, out err);
            //if (err != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
            //    return;
            //}

            //插入参数信息
            string paraStr = dbtypeStr + "|" + pResampleStr + "|" + pCompressionStr + "|" + pPyramidStr + "|" + pTileHStr + "|" + pTileWStr + "|" + pBandStr;
            string updateStr = "update DATABASEMD set DBPARA='" + paraStr + "' where ID="+pProjectID;
            //cyf 20110607 modify
            try { ModuleData.TempWks.ExecuteSQL(updateStr); } 
            catch
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新系统维护库失败！");
                return;
            }
            //end
            //pSysTable.UpdateTable(updateStr, out err);
            //if(err!=null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新系统维护库失败！");
            //    return;
            //}
            //插入数据集名称
            //string selStr = "select * from DATABASEMD where ID=" + pProjectID;
            //DataTable pDT = pSysTable.GetSQLTable(selStr, out err);
            //if (err != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询系统维护库失败！");
            //    return;
            //}
            //if (pDT == null || pDT.Rows.Count == 0) return;
            //string pConnInfoFromDB = pDT.Rows[0]["CONNECTIONINFO"].ToString();
            //cyf 20110607 modify
            IQueryDef pQueryDef = pFeaWS.CreateQueryDef();
            pQueryDef.Tables = "DATABASEMD";
            pQueryDef.SubFields = "CONNECTIONINFO";
            pQueryDef.WhereClause = " ID=" + pProjectID;
            ICursor pCursor = pQueryDef.Evaluate();
           if (pCursor == null)
           {
               SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询表格‘DATABASEMD’失败！");
               return;
           }
           IRow pRow = pCursor.NextRow();
           string pConnInfoFromDB = "";   //栅格库连接信息字段值
           if (pRow != null)
           {
               pConnInfoFromDB = pRow.get_Value(0).ToString();
           }
            //释放游标
           System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            //cyf  20110625 modify:多rc或者rd创建是，将多个图层名保存起来，图层名之间以","隔开
           string pConnInfo=pConnInfoFromDB.Substring(0, (pConnInfoFromDB.LastIndexOf('|')+1)).Trim();  //连接信息，不包含图层名
           string RcName = pConnInfoFromDB.Substring(pConnInfoFromDB.LastIndexOf('|') + 1).Trim();      //图层名称
           string pNewConnInfoFromDB = "";  //更新后的连接信息字段值
           if (RcName.Length == 0)
           {
               //说明是第一个图层名
               pNewConnInfoFromDB = pConnInfo + pDTName;
           }
           else
           {
               //说明已经存在创建过的图层
               pNewConnInfoFromDB = pConnInfoFromDB + "," + pDTName;
           }
            //end
           updateStr = "update DATABASEMD set CONNECTIONINFO='" + pNewConnInfoFromDB + "' where ID=" + pProjectID;
            ModuleData.TempWks.ExecuteSQL(updateStr);
            //pSysTable.UpdateTable(updateStr, out err);
            //if (err != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新系统维护库失败！");
            //    return;
            //}

            //end
            #endregion
            //cyf 20110629 add:将栅格编码对应的ftp根目录信息写入数据库中进行存储
            if (rbcatalog.Checked)
            {
                pFeaWS=ModuleData.TempWks as IFeatureWorkspace;
                if(pFeaWS==null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","连接系统维护库失败!");
                    return;
                }
                //遍历根目录，将跟目录与图层进行关联
                for (int i = 0; i < lvRootPath.CheckedItems.Count; i++)
                {
                    string pRootPath = lvRootPath.CheckedItems[i].Text.Trim();      //根目录
                    long pOID = ModDBOperate.GetMaxID(pFeaWS, "RasterCatalogLayerInfo", "OBJECTID", out err);
                    if (err != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新数据库失败！\n原因：" + err.Message);
                        return;
                    }
                    string insertStr = "insert into RasterCatalogLayerInfo(OBJECTID,ProjectID,RasterLayerName,RootPath) values(" + pOID + ","+pProjectID+",'" + this.txtRasterName.Text.Trim() + "','" + pRootPath + "')";
                    //更新数据库
                    try { ModuleData.TempWks.ExecuteSQL(insertStr); }
                    catch { }
                }
            }
            //end

            #region 刷新栅格数据库xml的参数设置   陈亚飞添加 20101011
            //cyf 20110625 delete :不需要更新xml信息
            /*
            //根据不同的数据库类型读取不同的xml文件
            string pXmlPath = "";
            if (m_sDBType == enumInterDBType.高程数据库.ToString())
            {
                //cyf 20110609  modify
                pXmlPath = ModuleData.v_feaProjectXML;//.v_DEMProjectXml;
                //end
            }
            else if (m_sDBType == enumInterDBType.影像数据库.ToString())
            {
                //cyf 20110609 modify
                pXmlPath = ModuleData.v_feaProjectXML;// v_ImageProjectXml;
                //end
            }
            

            if (!File.Exists(pXmlPath)) return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pXmlPath);
            //工程节点
            System.Xml.XmlNode Projectnode = xmlDoc.SelectSingleNode("工程管理/工程[@编号='" + pProjectID + "']");
            if (Projectnode == null) return;
            System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;

            //内容节点
            XmlNode ConNode = ProjectNodeElement.FirstChild;
            //遍历所有内容子节点，设置栅格数据库参数信息
            foreach (XmlNode dbNode in ConNode.ChildNodes)
            {
                XmlElement DbTypeEle = dbNode as XmlElement;
                string sVisible = DbTypeEle.GetAttribute("是否显示");
                if (sVisible == bool.FalseString.ToLower()) continue;

                //设置数据库节点的“存储类型”属性
                DbTypeEle.SetAttribute("存储类型", dbtypeStr);

                //库体节点
                XmlElement ProjectUserDSEle = DbTypeEle.FirstChild.FirstChild as XmlElement;
                //设置库体名称属性
                ProjectUserDSEle.SetAttribute("名称", pDTName);

                //设置栅格数据参数
                System.Xml.XmlElement rasterParaEle = DbTypeEle.SelectSingleNode(".//栅格参数设置") as System.Xml.XmlElement;
                rasterParaEle.SetAttribute("重采样类型", pResampleStr);
                rasterParaEle.SetAttribute("压缩类型", pCompressionStr);
                rasterParaEle.SetAttribute("金字塔", pPyramidStr);
                rasterParaEle.SetAttribute("瓦片高度", pTileHStr);
                rasterParaEle.SetAttribute("瓦片宽度", pTileWStr);
                rasterParaEle.SetAttribute("波段", pBandStr);

                break;
            }
            xmlDoc.Save(pXmlPath);
            */
            //end
            #endregion


            MessageBox.Show("创建成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
	    this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmbRasterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRasterType.Text == "栅格编目")
            {
                //txtBand.Enabled = false;
                groupBox2.Enabled = false;
            }
            else if (cmbRasterType.Text == "栅格数据集")
            {
                //txtBand.Enabled = true;
                groupBox2.Enabled = true;
            }
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        DirectoryInfo dir = new DirectoryInfo(pFolderBrowser.SelectedPath);
                        string name = dir.Name;
                        if (dir.Parent == null)
                        {
                            name = dir.Name.Substring(0, dir.Name.Length - 2);
                        }
                        txtDataBase.Text = dir.FullName + "\\" + name + ".gdb";
                    }
                    break;

                case "PDB":
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Title = "保存为PDB数据";
                    saveFile.Filter = "PDB数据(*.mdb)|*.mdb";
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = saveFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }
       
        /// <summary>
        /// 设置连接属性        /// </summary>
        /// <param name="Type">数据库类型</param>
        /// <param name="IPoPath">数据库访问路径或服务器IP</param>
        /// <param name="Intance">sde服务实例</param>
        /// <param name="User">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="Version">sde版本</param>
        /// <returns></returns>
        public bool SetDestinationProp(string Type, string IPoPath, string Intance, string User, string PassWord, string Version)
        {
            IWorkspace TempWorkSpace = null;                                 //工作空间
            IWorkspaceFactory pWorkspaceFactory = null;                      //工作空间工厂

            try
            {
                //初始化工作空间工厂                
                if (Type == "PDB")
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                }
                else if (Type == "GDB")
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                }
                else if (Type == "SDE")
                {
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();
                }
                ///如果创建的是本地库体，则首先判断库体是否存在
                ///如果库体存在，则先删除原有库体                //cyf 20110625 delete
                //if (File.Exists(IPoPath))
                //{
                //    File.Delete(IPoPath);
                //}
                //end

                if (Type == "SDE")  //如果是SDE则设置sde工作空间连接信息
                {
                    IPropertySet propertySet = new PropertySetClass();
                    propertySet.SetProperty("SERVER", IPoPath);
                    propertySet.SetProperty("INSTANCE", Intance);
                    //propertySet.SetProperty("DATABASE", ""); 
                    propertySet.SetProperty("USER", User);
                    propertySet.SetProperty("PASSWORD", PassWord);
                    propertySet.SetProperty("VERSION", Version);
                    TempWorkSpace = pWorkspaceFactory.Open(propertySet, 0);
                }
                else  //如果不是sde则创建工作空间
                {
                    FileInfo finfo = new FileInfo(IPoPath);
                    string outputDBPath = finfo.DirectoryName;
                    string outputDBName = finfo.Name;
                    if (outputDBName.EndsWith(".gdb"))
                    {
                        outputDBName = outputDBName.Substring(0, outputDBName.Length - 4);
                    }

                    //cyf 20110625 add:打开已有的工作空间
                    try { TempWorkSpace = pWorkspaceFactory.OpenFromFile(IPoPath, 0); }
                    catch { }
                    //end
                    if (TempWorkSpace == null)
                    {
                        IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
                        ESRI.ArcGIS.esriSystem.IName pName = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
                        TempWorkSpace = (IWorkspace)pName.Open();
                    }
                }

                //判断获取工作空间是否成功
                if (TempWorkSpace != null)
                {
                    pworkSpace = TempWorkSpace;                //工作空间赋值
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServer.Text = "";
            txtInstance.Text = "";
            txtDataBase.Text = "";
            txtUser.Text = "";
            txtPassWord.Text = "";
            if (comBoxType.Text != "SDE")
            {
                btnServer.Visible = true;
                txtDataBase.Size = new Size(txtServer.Size.Width - btnServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;

                cmbManageType.Enabled = true;//可选管理型
            }
            else
            {
                btnServer.Visible = false;
                txtDataBase.Size = new Size(txtServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassWord.Enabled = true;
                txtVersion.Enabled = true;

                cmbManageType.SelectedIndex = 1;//sde只能设为管理型 xisheng 07.19;
                cmbManageType.Enabled = false;
            }
        }


        /// <summary>
        /// 在Geodatabase中创建栅格数据编目        /// </summary>
        /// <param name="pRasterWSEx">目标Geodatabase工作区</param>
        /// <param name="pCatalogName">栅格编目的名称</param>
        /// <param name="pRasterFielsName">栅格列的名称</param>
        /// <param name="pShapeFieldName">几何要素列名称(Shape)</param>
        /// <param name="pRasterSpatialRef">几何要素列空间参考</param>
        /// <param name="pGeoSpatialRef">栅格列空间参考</param>
        /// <param name="pKeyword"> 栅格编目表的字段</param>
        /// <param name="eError">ArcSDE 适用, 表示configuration keyword</param>
        /// <returns></returns>
        private IRasterCatalog CreateCatalog(IRasterWorkspaceEx pRasterWSEx, string pCatalogName, string pRasterFielsName, string pShapeFieldName, ISpatialReference pRasterSpatialRef, ISpatialReference pGeoSpatialRef, string pKeyword, bool ismanaged, out Exception eError)
        {
            eError = null;
            IRasterCatalog pRasterCat = null;
            try
            {
                #region 创建字段
                IFields pFields = new FieldsClass();
                IFieldsEdit pFeildsEdit = pFields as IFieldsEdit;
                IField pField = null;

                pField = CreateCommonField("Name", esriFieldType.esriFieldTypeString);
                if (pField == null)
                {
                    eError = new Exception("创建'name'字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);

                IField2 pField2 = CreateRasterField(pRasterFielsName, pRasterSpatialRef, ismanaged);
                if (pField == null)
                {
                    eError = new Exception("创建栅格字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField2);
                pField = CreateShapeField(pShapeFieldName, pGeoSpatialRef);
                if (pField == null)
                {
                    eError = new Exception("创建几何字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);
                pField = CreateOIDField("OBJECTID");
                if (pField == null)
                {
                    eError = new Exception("创建OID字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);
                pField = null;
                if (pKeyword.Trim() == "")
                {
                    pKeyword = "defaults";
                }
                pFields = pFeildsEdit as IFields;

                //创建用户自定义字段                
                #endregion

                pRasterCat = pRasterWSEx.CreateRasterCatalog(pCatalogName, pFields, pShapeFieldName, pRasterFielsName, pKeyword);

                return pRasterCat;

            }
            catch (System.Exception ex)
            {
                eError = new Exception("创建栅格编目出错！\n" + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 创建栅格字段
        /// </summary>
        /// <param name="pRasterFielsName">栅格字段名</param>
        /// <param name="pSpatialRes">栅格空间参考</param>
        /// <param name="eError"></param>
        /// <returns>返回字段</returns>
        private IField2 CreateRasterField(string pRasterFielsName, ISpatialReference pSpatialRes, bool isManaged)
        {
            IField2 pField = new FieldClass();
            IFieldEdit2 pFieldEdit = pField as IFieldEdit2;
            pFieldEdit.Name_2 = pRasterFielsName;
            pFieldEdit.AliasName_2 = pRasterFielsName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeRaster;

            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "this is Raster catalog";
            if (pSpatialRes == null)
            {
                //如果空间参考为空，则设置为UnknownCoordinateSystemClass
                pSpatialRes = new UnknownCoordinateSystemClass();
            }
            //only for PGDB
            pRasterDef.IsManaged = isManaged;
            pRasterDef.SpatialReference = pSpatialRes;
            pFieldEdit.RasterDef = pRasterDef;
            pField = pFieldEdit as IField2;

            return pField;
        }

        /// <summary>
        /// 创建shape字段
        /// </summary>
        /// <param name="pShapeFielsName">shape字段名</param>
        /// <param name="pSpatialRes">空间参考</param>
        /// <returns>返回字段</returns>
        private IField CreateShapeField(string pShapeFielsName, ISpatialReference pSpatialRes)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = pShapeFielsName;
            pFieldEdit.AliasName_2 = pShapeFielsName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            IGeometryDef pGeoDef = new GeometryDefClass();
            pGeoDef = CreateGeoDef(pSpatialRes);
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pField = pFieldEdit as IField;
            return pField;
        }

        /// <summary>
        /// 创建OID字段
        /// </summary>
        /// <param name="pOIDFieldName">OID字段名</param>
        /// <returns></returns>
        private IField CreateOIDField(string pOIDFieldName)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = pOIDFieldName;
            pFieldEdit.AliasName_2 = pOIDFieldName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            pField = pFieldEdit as IField;
            return pField;
        }

        /// <summary>
        /// 创建用户自定义字段        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        private IField CreateCommonField(string fieldName, esriFieldType fieldType)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = fieldName;
            pFieldEdit.AliasName_2 = fieldName;
            pFieldEdit.Type_2 = fieldType;
            pFieldEdit.Length_2 = 50;
            pField = pFieldEdit as IField;
            return pField;
        }

       
        /// <summary>
        /// 创建几何空间参考        /// </summary>
        /// <param name="LoadPath">空间参考文件</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public ISpatialReference GetSpatialRef(string LoadPath, out Exception eError)
        {
            eError = null;
            try
            {
                ISpatialReference pSR = null;
                ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass();

                if (!File.Exists(LoadPath))
                {
                    //eError = new Exception("空间参考文件不存在！");
                    return null;
                }
                pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(LoadPath);

                ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                IControlPrecision2 pSpatialPrecision = (IControlPrecision2)pSR;

                pSRR.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon
                pSRR.SetDefaultXYResolution();
                pSRT.SetDefaultXYTolerance();
                return pSR;
            }
            catch(Exception ex)
            {
                eError = ex;
                return null;
            }
        }

        /// <summary>
        /// 创建栅格空间参考        /// </summary>
        /// <param name="LoadPath">空间参考文件</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public ISpatialReference GetSpatialRef2(string LoadPath, out Exception eError)
        {
            eError = null;
            try
            {
                ISpatialReference pSR = null;
                ISpatialReferenceFactory pSpatialRefFac = new SpatialReferenceEnvironmentClass();

                if (!File.Exists(LoadPath))
                {
                    //eError = new Exception("空间参考文件不存在！");
                    return null;
                }
                pSR = pSpatialRefFac.CreateESRISpatialReferenceFromPRJFile(LoadPath);

                //ISpatialReferenceResolution pSRR = pSR as ISpatialReferenceResolution;
                //ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)pSR;
                //IControlPrecision2 pSpatialPrecision = (IControlPrecision2)pSR;

                //pSRR.ConstructFromHorizon();//Defines the XY resolution and domain extent of this spatial reference based on the extent of its horizon
                //pSRR.SetDefaultXYResolution();
                //pSRT.SetDefaultXYTolerance();
                return pSR;
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }



        /// <summary>
        /// 在Geodatabase中创建栅格数据集
        /// </summary>
        /// <param name="pRasterWsEx">目标Geodatabase工作区</param>
        /// <param name="pDsName">栅格数据集名称</param>
        /// <param name="iNBannd">波段数</param>
        /// <param name="iPixeType">像素类型</param>
        /// <param name="pSpaRef">空间参考</param>
        /// <param name="pRasterStoreRef">存储栅格参数定义</param>
        /// <param name="pRasterDef">栅格空间参考定义</param>
        /// <param name="pKeyword"></param>
        /// <param name="eex"></param>
        /// <returns></returns>
        private IRasterDataset CreateRasterDataset(IRasterWorkspaceEx pRasterWsEx, string pDsName, int iNBannd, rstPixelType iPixeType, ISpatialReference pRasterSpaRef,ISpatialReference pGeoSpaRef, IRasterStorageDef pRasterStoreRef, IRasterDef pRasterDef, string pKeyword, out Exception eex)
        {
            eex = null;
            IRasterDataset pRasterDs = null;
            try
            {
                IGeometryDef pGeoDef = null;
                if (pRasterDef == null)
                {
                    pRasterDef = CreateRasterDef(pRasterSpaRef);
                }
                if (pRasterStoreRef == null)
                {
                    pRasterStoreRef = CreaterRasterStoreDef();
                }
                pGeoDef = CreateGeoDef(pGeoSpaRef);     
                if (pKeyword.Trim() == "")
                {
                    pKeyword = "DEFAULTS";
                }
                pRasterDs = pRasterWsEx.CreateRasterDataset(pDsName,iNBannd,iPixeType, pRasterStoreRef, pKeyword, pRasterDef, pGeoDef);
            }
            catch(Exception ex)
            {
                eex =new Exception("创建栅格数据集出错！\n"+ex.Message) ;
            }
            return pRasterDs;
        }

        /// <summary>
        /// 设置空间参考定义        /// </summary>
        /// <param name="pSR"></param>
        /// <returns></returns>
        private IRasterDef CreateRasterDef(ISpatialReference pSR)
        {
            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "rasterDataset";
            if(pSR==null)
            {
                pSR = new UnknownCoordinateSystemClass();
            }
            pRasterDef.SpatialReference = pSR;
            return pRasterDef;
        }
        
        /// <summary>
        /// 设置栅格存储的参数        /// </summary>
        /// <returns></returns>
        private IRasterStorageDef CreaterRasterStoreDef()
        {
            IRasterStorageDef pRasterStorageDef = new RasterStorageDefClass();
            pRasterStorageDef.CompressionType = GetCompression();
            pRasterStorageDef.PyramidResampleType = GetResampleTpe();
            if (txtPyramid.Text.Trim() != "")
            {
                pRasterStorageDef.PyramidLevel = Convert.ToInt32(txtPyramid.Text.Trim());
            }
            pRasterStorageDef.TileHeight = Convert.ToInt32(tileH.Text.Trim());
            pRasterStorageDef.TileWidth = Convert.ToInt32(tileW.Text.Trim());
            return pRasterStorageDef;
        }

        /// <summary>
        /// 设置几何空间参考定义        /// </summary>
        /// <param name="pSpatialRes"></param>
        /// <returns></returns>
        private IGeometryDef CreateGeoDef(ISpatialReference pSpatialRes)
        {
            IGeometryDef pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;

            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            pGeoDefEdit.AvgNumPoints_2 = 4;
            pGeoDefEdit.GridCount_2 = 1;
            pGeoDefEdit.set_GridSize(0, 1000);

            if (pSpatialRes == null)
            {
                pSpatialRes = new UnknownCoordinateSystemClass();
            }
            pGeoDefEdit.SpatialReference_2 = pSpatialRes;
            pGeoDef = pGeoDefEdit as IGeometryDef;
            return pGeoDef;
        }

        /// <summary>
        /// 获得重采样类型        /// </summary>
        /// <returns></returns>
        private rstResamplingTypes GetResampleTpe()
        {
            rstResamplingTypes resampleType = rstResamplingTypes.RSP_NearestNeighbor;
            switch (cmbResampleType.Text.Trim())
            {
                case "邻近法":
                    resampleType = rstResamplingTypes.RSP_NearestNeighbor;
                    break;
                case "双线性内插法":
                    resampleType = rstResamplingTypes.RSP_BilinearInterpolation;
                    break;
                case "立方卷积法":
                    resampleType = rstResamplingTypes.RSP_CubicConvolution;
                    break;
            }
            return resampleType;
        }

        /// <summary>
        /// 获得压缩类型
        /// </summary>
        /// <returns></returns>
        private esriRasterCompressionType GetCompression()
        {
            esriRasterCompressionType compressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
            switch (cmbCompression.Text.Trim())
            {
                case "LZ77":
                    compressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
                    break;
                case "JPEG":
                    compressionType = esriRasterCompressionType.esriRasterCompressionJPEG;
                    break;
                case "JPEG2000":
                    compressionType = esriRasterCompressionType.esriRasterCompressionJPEG2000;
                    break;
                case "PackBits":
                    compressionType = esriRasterCompressionType.esriRasterCompressionPackBits;
                    break;
                case "LZW":
                    compressionType = esriRasterCompressionType.esriRasterCompressionLZW;
                    break;
            }
            return compressionType;
        }


        /// <summary>
        /// 创建栅格数据集，本地
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private IRasterDataset CreateFileRasterDataset(string directoryName, string fileName)
        {
            // This function creates a new img file in the given workspace
            // and then assigns pixel values
            try
            {
                IRasterDataset rasterDataset = null;
                IPoint originPoint = new PointClass();
                originPoint.PutCoords(0, 0);

                // Create the dataset
                IRasterWorkspace2 rasterWorkspace2 = null;
                rasterWorkspace2 = CreateRasterWorkspace(directoryName);

                rasterDataset = rasterWorkspace2.CreateRasterDataset(fileName, "IMAGINE Image", originPoint, 200, 100, 1, 1, 1, rstPixelType.PT_UCHAR, new UnknownCoordinateSystemClass(), true);

                IRawPixels rawPixels = null;
                IPixelBlock3 pixelBlock3 = null;
                IPnt pixelBlockOrigin = null;
                IPnt pixelBlockSize = null;
                IRasterBandCollection rasterBandCollection;
                IRasterProps rasterProps;



                // QI for IRawPixels and IRasterProps
                rasterBandCollection = (IRasterBandCollection)rasterDataset;
                rawPixels = (IRawPixels)rasterBandCollection.Item(0);
                rasterProps = (IRasterProps)rawPixels;



                // Create pixelblock
                pixelBlockOrigin = new DblPntClass();
                pixelBlockOrigin.SetCoords(0, 0);

                pixelBlockSize = new DblPntClass();
                pixelBlockSize.SetCoords(rasterProps.Width, rasterProps.Height);

                pixelBlock3 = (IPixelBlock3)rawPixels.CreatePixelBlock(pixelBlockSize);



                // Read pixelblock
                rawPixels.Read(pixelBlockOrigin, (IPixelBlock)pixelBlock3);

                // Get pixeldata array
                System.Object[,] pixelData;
                pixelData = (System.Object[,])pixelBlock3.get_PixelDataByRef(0);

                // Loop through all the pixels and assign value
                for (int i = 0; i < rasterProps.Width; i++)
                    for (int j = 0; j < rasterProps.Height; j++)
                        pixelData[i, j] = (i * j) % 255;



                // Write the pixeldata back
                System.Object cachePointer;

                cachePointer = rawPixels.AcquireCache();

                rawPixels.Write(pixelBlockOrigin, (IPixelBlock)pixelBlock3);

                rawPixels.ReturnCache(cachePointer);

                // Return raster dataset
                return rasterDataset;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// 在geodatabase中创建栅格工作空间        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        private IRasterWorkspace2 CreateRasterWorkspace(string pathName)
        {
            // Create RasterWorkspace
            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();

            return workspaceFactory.OpenFromFile(pathName, 0) as IRasterWorkspace2;
        }

        private void btnRuleFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择库体配置文件";
            OpenFile.Filter = "库体配置文件(*.mdb)|*.mdb|库体配置文件(*.gosch)|*.gosch";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                textRuleFilePath.Text = OpenFile.FileName;
                btnRuleFile.Tooltip = OpenFile.FileName;
            }
        }

        private void rbdataset_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdataset.Checked)
            {
                //栅格数据集
                groupBox2.Enabled = true;
                //cyf 20110609 add
                cmbManageType.Enabled = false;
                btnGeoSpati.Enabled = false;
                txtGeoSpati.Enabled = false;
                labelX2.Text = "栅格数据集空间参考";
                //end
                //cyf 20110629
                lvRootPath.Items.Clear();
                lvRootPath.Enabled = false; 
            }
        }

        private void rbcatalog_CheckedChanged(object sender, EventArgs e)
        {
            Exception outError = null;    //异常
            if (rbcatalog.Checked)
            {
                //栅格编目
                groupBox2.Enabled = false;
                //cyf 20110609 add
                cmbManageType.Enabled = true;
                btnGeoSpati.Enabled = true;
                txtGeoSpati.Enabled = true;
                labelX2.Text = "栅格列空间参考";
                //end
                //cyf 20110629
                lvRootPath.Enabled = true;
                lvRootPath.Items.Clear();
				if(comBoxType.SelectedIndex==1)//若为SDE，则只能选择管理型 xisheng 07.19
                {
                    cmbManageType.SelectedIndex = 1;
                    cmbManageType.Enabled = false;
                }
                //读取数据库FTP目录信息，加载在界面上
                if (ModuleData.TempWks == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                    return;
                }
                IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
                if (pFeaWS == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                    return;
                }
               ////deleted by xisheng 0914 不读取路径
                //ICursor pCursor = ModDBOperate.GetCursor(pFeaWS, "RasterFilePathInfo", "RasterFilePath", "", out outError);
                //if (outError != null || pCursor == null)
                //{
                //    return;
                //}
                //IRow pRow = pCursor.NextRow();
                //while (pRow != null)
                //{
                //    //this.txtRootPath.Text = pRow.get_Value(0).ToString().Trim();
                //    lvRootPath.Items.Add(pRow.get_Value(0).ToString().Trim());
                //    pRow = pCursor.NextRow();
                //}
                ////释放游标
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                ////end
            }
        }

        private void btnGeoSpati_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择空间参考";
            OpenFile.Filter = "空间参考文件(*.prj)|*.prj";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtGeoSpati.Text = OpenFile.FileName;
                btnGeoSpati.Tooltip = OpenFile.FileName;
            }
        }

        private void btnRasterSpati_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择空间参考";
            OpenFile.Filter = "空间参考文件(*.prj)|*.prj";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtRasterSpati.Text = OpenFile.FileName;
                btnRasterSpati.Tooltip = OpenFile.FileName;
            }
        }


        /// <summary>
        /// 获得空间参考        /// </summary>
        /// <param name="spatialStr">空间参考参数</param>
        /// <returns></returns>
        private ISpatialReference GetSpatialRef(string spatialStr)
        {
            ISpatialReference pSpaRef = null;
            ISpatialReferenceFactory2 pSpatRefFac = new SpatialReferenceEnvironmentClass();
            switch (spatialStr)
            {
                case "西安高斯117度(3度带)":
                    pSpaRef = pSpatRefFac.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_117E) as ISpatialReference;
                    break;
                case "西安高斯120度(3度带)":
                    pSpaRef = pSpatRefFac.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_120E) as ISpatialReference;
                    break;
                case "西安高斯123度(3度带)":
                    pSpaRef = pSpatRefFac.CreateProjectedCoordinateSystem((int)esriSRProjCS4Type.esriSRProjCS_Xian1980_3_Degree_GK_CM_123E) as ISpatialReference;
                    break;

            }
            return pSpaRef;
        }

        private void btnAddList_Click(object sender, EventArgs e)
        {
            if (pListViewDT.Items.Count != 0)
            {
                //清空历史数据
                pListViewDT.Items.Clear();
            }

            //添加数据
            IEnumDatasetName pEnumRasterName = null;
            IDatasetName pDT = null;
            ListViewItem pListViewItem = null;
            //添加栅格编目图层

            pEnumRasterName = pworkSpace.get_DatasetNames(esriDatasetType.esriDTRasterCatalog);
            if (pEnumRasterName == null) return;
            pDT = pEnumRasterName.Next();
            while (pDT != null)
            {
                //将查到的结果，添加在列表中                pListViewItem = new ListViewItem();
                pListViewItem.Name = pDT.Name;
                pListViewItem.Text = pDT.Name;
                pListViewItem.Tag = pDT;
                pListViewDT.Items.Add(pListViewItem);
                pDT = pEnumRasterName.Next();
            }
        }

        //added by xisheng 20110914 增加目录
        private void btn_CatalogSearch_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
            string dir ="";
            if (pFolderBrowser.ShowDialog() == DialogResult.OK)
            {
                dir = pFolderBrowser.SelectedPath;
                if (!listroot.Contains(dir))
                {
                    lvRootPath.Items.Add(dir);
                    lvRootPath.Items[lvRootPath.Items.Count - 1].Checked = true;
                    listroot.Add(dir);
                }
                else
                {
                    MessageBox.Show("已存在相同目录，不再添加", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //added by xisheng 20110914 增加目录
        private void btn_CatalogDel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvRootPath.Items)
            {
                if (item.Checked)
                {
                    lvRootPath.Items.Remove(item);
                    listroot.Remove(item.Text);
                }
            }
        }

    }
}