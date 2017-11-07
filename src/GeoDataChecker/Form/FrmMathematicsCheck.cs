using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    /// <summary>
    /// 陈亚飞  添加
    /// </summary>
    public partial class FrmMathematicsCheck : DevComponents.DotNetBar.Office2007Form
    {
        Plugin.Application.IAppGISRef _AppHk;
        enumErrorType _ErrorType;
        SysCommon.Gis.SysGisDataSet pGisDT = null;

        public FrmMathematicsCheck(Plugin.Application.IAppGISRef hook,enumErrorType errorType)
        {
            InitializeComponent();
           
            _AppHk= hook;
            _ErrorType = errorType;

            IntialForm();
        }

        private void IntialForm()
        {
            lstRangeName.Items.Clear();
            lstRangeName.CheckBoxes = true;

            comBoxType.Items.AddRange(new object[] { "PDB", "GDB", "SDE" });
            comBoxType.SelectedIndex = 0;
            switch (_ErrorType)
            {
                case enumErrorType.数学基础正确性检查:
                    txtPrj.Enabled = true;
                    btnPrj.Enabled = true;
                    txtMax.Enabled = false;
                    txtMin.Enabled = false;
                    break;
                case enumErrorType.空值检查:
                    txtPrj.Enabled = false ;
                    btnPrj.Enabled = false;
                    txtMax.Enabled = false;
                    txtMin.Enabled = false;
                    break;
                case enumErrorType.线长度逻辑性检查:
                    txtPrj.Enabled = false;
                    btnPrj.Enabled = false;
                    txtMax.Enabled = true;
                    txtMin.Enabled = true ;
                    break;
                case enumErrorType.面面积逻辑性检查:
                    txtPrj.Enabled = false;
                    btnPrj.Enabled = false;
                    txtMax.Enabled = true;
                    txtMin.Enabled = true;
                    break;
                case enumErrorType.高程值检查 :
                    txtPrj.Enabled = false;
                    btnPrj.Enabled = false;
                    txtMax.Enabled = true;
                    txtMin.Enabled = true;
                    break;
                case enumErrorType.等高线高程值检查:
                    txtPrj.Enabled = false;
                    btnPrj.Enabled = false;
                    txtMax.Enabled = true ;
                    txtMin.Enabled = true ;
                    break;
                default:
                    txtPrj.Enabled = false;
                    btnPrj.Enabled = false;
                    txtMax.Enabled = false;
                    txtMin.Enabled = false;
                    break;
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

        private void buttonX2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstRangeName.Items.Count; i++)
            {
                lstRangeName.Items[i].Checked = true;
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            lstRangeName.Items.Clear();

            //for (int i = 0; i < lstRangeName.Items.Count; i++)
            //{
            //    lstRangeName.Items[i].Checked = false;
            //}
        }

        private void btnPrj_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.CheckFileExists = true;
            OpenFile.CheckPathExists = true;
            OpenFile.Title = "选择空间参考文件";
            OpenFile.Filter = "空间参考文件(*.prj)|*.prj";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtPrj.Text = OpenFile.FileName;
            }
        }

        private void btnCon_Click(object sender, EventArgs e)
        {
            Exception eError = null;

            //连接数据库
            pGisDT = new SysCommon.Gis.SysGisDataSet();
            if (comBoxType.Text.Trim() == "SDE")
            {
                pGisDT.SetWorkspace(txtServer.Text,txtInstance.Text ,txtDB.Text ,txtUser.Text ,txtPassword.Text ,txtVersion.Text ,out eError);
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

            //获得数据库中的数据集
            List<string> feaDatasetNameLst = pGisDT.GetAllFeatureDatasetNames();
            for (int i = 0; i < feaDatasetNameLst.Count; i++)
            {
                ListViewItem aItem=lstRangeName.Items.Add(new ListViewItem(new string[]{feaDatasetNameLst[i]}));
                aItem.ToolTipText = feaDatasetNameLst[i];
            }
            for (int j = 0; j < lstRangeName.Items.Count; j++)
            {
                lstRangeName.Items[j].Checked = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;

           
                if (lstRangeName.CheckedItems.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择数据集！");
                    return;
                }

                if (txtPrj.Enabled == true)
                {
                    //判断是否输入空间参考文件
                    if (txtPrj.Text == "" || !File.Exists(txtPrj.Text.Trim()))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择标准的空间参考文件!");
                        return;
                    }
                }
                if (txtMin.Enabled == true && txtMax.Enabled == true)
                {
                    //判断文本框输入是否合理，是否字符串，最小值是否小于最大值
                    if (txtMin.Text == "" && txtMax.Text == "")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请填写最大值或最小值的范围！");
                        return;
                    } try
                    {
                        if (txtMin.Text != "" && txtMax.Text != "")
                        {
                            if (Convert.ToDouble(txtMin.Text.Trim()) > Convert.ToDouble(txtMax.Text.Trim()))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "最小值不能大于最大值！");
                                return;
                            }
                        }

                    }
                    catch (Exception eex)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "'最大值'或'最小值'应为数字，请输入有效的数字！");
                        return;
                    }
                }
                try
                {
                //错误日志连接信息
                string logPath = txtLog.Text;
                if (logPath.Trim() == string.Empty)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择日志输出路径!");
                    return;
                }
                if (File.Exists(logPath))
                {
                    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "数据文件\n'" + logPath + "'\n已经存在,是否替换?"))
                    {
                        File.Delete(logPath);
                    }
                    else 
                    {
                        return;
                    }
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据文件已存在!\n" );
                }
                //生成日志信息EXCEL格式
                SysCommon.DataBase.SysDataBase pSysLog = new SysCommon.DataBase.SysDataBase();
                pSysLog.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + logPath + "; Extended Properties=Excel 8.0;", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError );
                if (eError!= null)
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
                    pSysLog.CloseDbConnection();
                    return;
                }
                //将日志表连接信息和表名保存起来
                //TopologyCheckClass.ErrDbCon = pSysLog.DbConn;
                //TopologyCheckClass.ErrTableName = "错误日志";

                DataCheckClass dataCheckCls = new DataCheckClass(_AppHk);
                //将日志表连接信息和表名保存起来
                dataCheckCls.ErrDbCon = pSysLog.DbConn;
                dataCheckCls.ErrTableName = "错误日志";

                SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TopologyCheckClass.GeoDataCheckParaPath;
                pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                if (eError != null)
                {
                   SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","GIS数据检查配置表连接失败！");
                    return;
                }
                //分类代码字段名
                string codeName = GetCodeName(pSysTable, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }

                #region 遍历数据集，进行数据检查
                bool b = false;
                for (int i = 0; i < lstRangeName.CheckedItems.Count; i++)
                {
                    IFeatureDataset pFeaDataset = pGisDT.GetFeatureDataset(lstRangeName.CheckedItems[i].Text.Trim(), out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败,数据集名称为:" + lstRangeName.Items[i].Text.Trim());
                        continue;
                    }
                    DataTable mTable = null;
                    switch (_ErrorType)
                    {
                        case enumErrorType.数学基础正确性检查:
                            ISpatialReferenceFactory spatialRefFac = new SpatialReferenceEnvironmentClass();
                            ISpatialReference standardSpatialRef = spatialRefFac.CreateESRISpatialReferenceFromPRJFile(txtPrj.Text.Trim());
                            dataCheckCls.MathematicsCheck(pFeaDataset, standardSpatialRef, out eError);
                            if (eError != null)
                            {
                                //Enum.Parse(typeof(enumErrorType), _ErrorType.GetHashCode().ToString()).ToString()
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数学基础性正确检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.空值检查:
                            //空值检查参数ID为2
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 2, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            //执行空值检查
                            IsNullCheck(dataCheckCls, pFeaDataset, mTable, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "空值检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线长度逻辑性检查:
                            //线长度逻辑性检查，参数ID为3
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 3, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            //执行线长度逻辑性检查
                            LineLogicCheck(dataCheckCls, pFeaDataset, mTable, codeName, txtMin.Text.Trim(), txtMax.Text.Trim(), out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线长度逻辑性检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.面面积逻辑性检查:
                            //面面积逻辑性检查，参数ID为4
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 4, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            //执行面面积逻辑性检查
                            AreaCheck(dataCheckCls, pFeaDataset, mTable, codeName, txtMin.Text.Trim(), txtMax.Text.Trim(), out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面面积逻辑性检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.高程值检查:
                            //高程值检查，参数ID为5
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 5, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            //执行高程值检查
                            ElevValueCheck(dataCheckCls, pFeaDataset, mTable, txtMin.Text.Trim(), txtMax.Text.Trim(), out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "异常高程值检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.等高线高程值检查:
                            //等高线高程值检查
                            //等高线图层名，参数ID为20
                            string pFeaClsName = GetParaValue(pSysTable, 20, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            //等高线高程字段名,参数ID23
                            string pFieldName = GetParaValue(pSysTable, 23, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }

                            //等高线高程间距参数值,参数ID为21
                            string paraValue = GetParaValue(pSysTable, 21, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            double intevalValue = Convert.ToDouble(paraValue);
                            //执行等高线检查
                            dataCheckCls.contourIntevalCheck(pFeaDataset, pFeaClsName, pFieldName, txtMin.Text.Trim(), txtMax.Text.Trim(), intevalValue, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "等高线高程值检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.控制点注记一致性检查:
                            //控制点注记检查，参数ID为29
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 29, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            //控制点注记检查搜索半径,参数ID为32
                            string paraValue1 = GetParaValue(pSysTable, 32, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            double serchRadiu1 = Convert.ToDouble(paraValue1);
                            //控制点注记检查精度控制，参数ID为35
                            paraValue1 = GetParaValue(pSysTable, 35, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            long precision1 = Convert.ToInt64(paraValue1);
                            //执行控制点注记检查
                            PointAnnoCheck(dataCheckCls, pFeaDataset, mTable, codeName, serchRadiu1, precision1, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "控制点注记检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.高程点注记一致性检查:
                            //高程点注记检查，参数ID为30
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 30, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            //高程点注记检查搜索半径,参数ID为33
                            string paraValue2 = GetParaValue(pSysTable, 33, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            double serchRadiu2 = Convert.ToDouble(paraValue2);
                            //高程点注记检查精度控制，参数ID为36
                            paraValue2 = GetParaValue(pSysTable, 36, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            long precision2 = Convert.ToInt64(paraValue2);
                            //执行高程点注记检查
                            PointAnnoCheck(dataCheckCls, pFeaDataset, mTable, codeName, serchRadiu2, precision2, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "高程点注记检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.等高线注记一致性检查:
                            //等高线注记检查，参数ID为31
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 31, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            //等高线注记检查搜索半径,参数ID为34
                            string paraValue3 = GetParaValue(pSysTable, 34, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            double serchRadiu3 = Convert.ToDouble(paraValue3);
                            //等高线注记检查精度控制，参数ID为37
                            paraValue3 = GetParaValue(pSysTable, 37, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            long precision3 = Convert.ToInt64(paraValue3);
                            //执行等高线注记检查
                            PointAnnoCheck(dataCheckCls, pFeaDataset, mTable, codeName, serchRadiu3, precision3, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "等高线注记检查失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.等高线点线矛盾检查:
                            //高程点图层,参数ID为19(还需要改进）
                            string pointFeaclsname = GetParaValue(pSysTable, 19, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            //等高线图层,参数ID为20
                            string lineFeaclsname = GetParaValue(pSysTable, 20, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            //高程点高程字段名,参数ID为22
                            string pointFieldsname = GetParaValue(pSysTable, 22, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            //等高线高程字段名,参数ID为23
                            string lineFieldname = GetParaValue(pSysTable, 23, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            //等高线间距值,参数ID为21
                            string intervalValue = GetParaValue(pSysTable, 21, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            //高程点搜索半径,参数ID为38
                            string radiu = GetParaValue(pSysTable, 38, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            //执行等高线点线矛盾检查
                            dataCheckCls.PointLineElevCheck(pFeaDataset, lineFeaclsname, lineFieldname, pointFeaclsname, pointFieldsname, Convert.ToDouble(intervalValue), out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "等高线点线矛盾检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线自相交检查:
                            dataCheckCls.OrdinaryTopoCheck(pFeaDataset, esriGeometryType.esriGeometryPolyline, esriTopologyRuleType.esriTRTLineNoSelfIntersect, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线自相交检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线自重叠检查:
                            dataCheckCls.OrdinaryTopoCheck(pFeaDataset, esriGeometryType.esriGeometryPolyline, esriTopologyRuleType.esriTRTLineNoSelfOverlap, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线自重叠检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线存在悬挂点:
                            //线悬挂点检查，参数ID为6
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 6, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            //线悬挂点检查搜索容差,参数ID为38
                            string lineDangleRadiuStr = GetParaValue(pSysTable, 38, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            double lineDangleRadiu = Convert.ToDouble(lineDangleRadiuStr);
                            LineDangleCheck2(dataCheckCls, pFeaDataset, mTable, codeName, lineDangleRadiu, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线悬挂点检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线存在伪节点:
                            dataCheckCls.OrdinaryTopoCheck(pFeaDataset, esriGeometryType.esriGeometryPolyline, esriTopologyRuleType.esriTRTLineNoPseudos, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线伪节点检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.同层线重叠检查:
                            //同层线重叠检查，参数ID为14
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 14, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            SpecialFeaClsTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTLineNoOverlap, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "同层线重叠检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.同层线相交检查:
                            //同层线相交检查，参数ID为15
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 15, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            SpecialFeaClsTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTLineNoIntersection, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "同层线相交检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.同层面重叠检查:
                            dataCheckCls.OrdinaryTopoCheck(pFeaDataset, esriGeometryType.esriGeometryPolygon, esriTopologyRuleType.esriTRTAreaNoOverlap, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "同层面重叠检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.异层面重叠检查:
                            //异层面重叠检查，参数ID为7
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 7, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }

                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTAreaNoOverlapArea, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "异层面重叠检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.面缝隙检查:
                            //面缝隙检查，参数ID为8
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 8, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            SpecialFeaClsTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTAreaNoGaps, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面缝隙检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.面含面检查:
                            //面含面检查，参数ID为9
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 9, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTAreaCoveredByArea, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面含面检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.点搭面检查:
                            //点搭面检查，参数ID为10
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 10, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTPointCoveredByAreaBoundary, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "点搭面检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.点搭线检查:
                            //点搭线检查，参数ID为11
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 11, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTPointCoveredByLine, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "点搭线检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.点位于线端点检查:
                            //点位于线端点检查，参数ID为12
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 12, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTPointCoveredByLineEndpoint, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "点位于线端点检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.点位于面内检查:
                            //点位于面内检查，参数ID为13
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 13, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTPointProperlyInsideArea, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面含点检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线面边界重合检查:
                            //线面边界重合检查，参数ID为16
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 16, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线面重合检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线穿面检查:
                            //线穿面检查，参数ID为17
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 17, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            LineCrossAreaCheck(dataCheckCls, pFeaDataset, mTable, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线穿面检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.异层线重叠检查:
                            //异层线重叠检查，参数ID为40
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 40, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTLineNoOverlapLine, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "异层线重叠检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.面边界面边界重合检查:
                            //面边界面边界重合检查，参数ID为41
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 41, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTAreaBoundaryCoveredByAreaBoundary, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面边界面边界重合检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.面面相互覆盖检查:
                            //面面相互覆盖检查，参数ID为42
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 42, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTAreaAreaCoverEachOther, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面面相互覆盖检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.面含点检查:
                            //面含点检查，参数ID为43
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 43, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTAreaContainPoint, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面含点检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.面边界线重合检查:
                            //面边界线重合检查，参数ID为44
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 44, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTAreaBoundaryCoveredByLine, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面边界线重合检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线线重合检查:
                            //线线重合检查，参数ID为45
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 45, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTLineCoveredByLineClass, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线线重合检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.线端点被点覆盖检查:
                            //线端点被点覆盖检查，参数ID为46
                            mTable = GetParaValueTable(pFeaDataset, pSysTable, 46, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                b = true;
                                break;
                            }
                            AreaTopoCheck2(dataCheckCls, pFeaDataset, mTable, esriTopologyRuleType.esriTRTLineEndpointCoveredByPoint, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线端点被点覆盖检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.简单线检查:
                            //简单线检查
                            dataCheckCls.OrdinaryTopoCheck(pFeaDataset, esriGeometryType.esriGeometryPolyline, esriTopologyRuleType.esriTRTLineNoMultipart, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "简单线检查失败！" + eError.Message);
                                pSysLog.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            break;
                        case enumErrorType.接边检查:
                            //设置接边检查属性
                            //FromJoinCheck = new SetJoinChecks(_AppHk );
                            //FromJoinCheck.Show();
                            break;
                        default:
                            break;
                    }
                    if (b == true)
                    {
                        continue;
                    }

                }
                #endregion

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据检查完成!");
                pSysLog.CloseDbConnection();
                pSysTable.CloseDbConnection();
                this.Close();
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                return;
            }
        }

        #region 数据检查函数

        /// <summary>
        /// 查找分类代码字段名称
        /// </summary>
        /// <param name="pSysTable"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private string GetCodeName(SysCommon.DataBase.SysTable pSysTable, out Exception eError)
        {
            eError = null;
            string codeName = "";    //分类代码字段名称
            string selStr = "select * from GeoCheckPara where 参数ID=1";
            DataTable pTable = pSysTable.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查找分类代码字段名称失败！");
            }

            if (pTable == null || pTable.Rows.Count == 0)
            {
                eError = new Exception("找不到分类代码字段名称记录!");
            }
            codeName = pTable.Rows[0]["参数值"].ToString().Trim();
            if (codeName == "")
            {
                eError = new Exception("配置表中未配置分类代码字段名，请检查！");
            }
            return codeName;
        }

        /// <summary>
        /// 查找参数值表格
        /// </summary>
        /// <param name="pFeaDataset"></param>
        /// <param name="pSysTable"></param>
        /// <param name="checkParaID">参数ID，唯一标识检查类型</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private DataTable  GetParaValueTable(IFeatureDataset pFeaDataset, SysCommon.DataBase.SysTable pSysTable,int checkParaID,out Exception eError)
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
                string feaDTName = pFeaDataset.Name;                                   //数据集名称  
                if(feaDTName.Contains("."))
                {
                    feaDTName = feaDTName.Substring(feaDTName.IndexOf('.') + 1);
                }
                string str = "select * from GeoCheckParaValue where 检查类型=" + ParaValue+" and 数据集='"+feaDTName+"'";
                mTable = pSysTable.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    eError = new Exception("查询表格错误，表名为：GeoCheckParaValue，检查类型为:" + ParaValue);
                    return null ;
                }
            }
            return mTable;
        }

        /// <summary>
        /// 根据参数ID获得参数值
        /// </summary>
        /// <param name="pSysTable"></param>
        /// <param name="checkParaID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private string GetParaValue(SysCommon.DataBase.SysTable pSysTable, int checkParaID, out Exception eError)
        {
            eError = null;
            string paraValue="";

            string selStr = "select * from GeoCheckPara where 参数ID=" + checkParaID;
            DataTable pTable = pSysTable.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查询表格错误，表名为：GeoCheckPara，参数ID为:" + checkParaID);
                return "";
            }

            if (pTable == null || pTable.Rows.Count == 0)
            {
                eError = new Exception("找不到记录，参数ID为:" + checkParaID);
                return "";
            }
            paraValue = pTable.Rows[0]["参数值"].ToString().Trim();            //参数类型
            return paraValue;
        }

        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable">参数配置表</param>
        /// <param name="eError"></param>
        private void IsNullCheck(DataCheckClass pDataCheckClass,IFeatureDataset pFeaDataset,DataTable pTable,out Exception eError)
        {
            eError = null;

            //用来保存图层名和合要检查的字段名
            Dictionary<string, List<string>> feaClsInfodic = new Dictionary<string, List<string>>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string pFeaClsName = pTable.Rows[i]["图层"].ToString().Trim();   //图层名
                string fieldName = pTable.Rows[i]["字段项"].ToString().Trim();   //要进行检查的字段名
                if (pFeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (fieldName == "")
                {
                    eError = new Exception("字段名为空!");
                    return;
                }
                if (!feaClsInfodic.ContainsKey(pFeaClsName))
                {
                    List<string> fieldList = new List<string>();
                    fieldList.Add(fieldName);
                    feaClsInfodic.Add(pFeaClsName, fieldList);
                }
                else
                {
                    if (!feaClsInfodic[pFeaClsName].Contains(fieldName))
                    {
                        feaClsInfodic[pFeaClsName].Add(fieldName);
                    }
                }
            }
            pDataCheckClass.IsNullableCheck(pFeaDataset, feaClsInfodic, out eError);
        }

        /// <summary>
        /// 线长度逻辑性检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable">参数配置表</param>
        /// <param name="codeName">分类代码字段名</param>
        /// <param name="lenMin">最小长度</param>
        /// <param name="lenMax">最大长度</param>
        /// <param name="eError"></param>
        private void LineLogicCheck(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable, string codeName,string lenMin,string lenMax, out Exception eError)
        {
            eError = null;
            Dictionary<string, string> feaClsInfodic = new Dictionary<string, string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string pFeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();  //要检查的分类代码值集合
                if (pFeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (!feaClsInfodic.ContainsKey(pFeaClsName))
                {
                    feaClsInfodic.Add(pFeaClsName, codeValue);
                }
                else
                {
                    if (codeValue != "")
                    {
                        string tempStr = feaClsInfodic[pFeaClsName] + "," + codeValue;
                        feaClsInfodic[pFeaClsName] = tempStr;
                    }
                    else
                    {
                        feaClsInfodic[pFeaClsName] = "";
                    }
                }
            }
            pDataCheckClass.LineLengthLogicCheck(pFeaDataset, codeName, feaClsInfodic, lenMin, lenMax, out eError);
        }

        /// <summary>
        /// 面面积逻辑性检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable">参数配置表</param>
        /// <param name="codeName">分类代码名</param>
        /// <param name="AreaMin">面积最小值</param>
        /// <param name="AreaMax">面积最大值</param>
        /// <param name="eError"></param>
        private void AreaCheck(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable, string codeName, string AreaMin, string AreaMax, out Exception eError)
        {
            eError = null;
            Dictionary<string, string> feaClsInfodic = new Dictionary<string, string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string pFeaClsName = pTable.Rows[i]["图层"].ToString().Trim();//图层名
                string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();//要检查的分类代码值集合
                if (pFeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (!feaClsInfodic.ContainsKey(pFeaClsName))
                {
                    feaClsInfodic.Add(pFeaClsName, codeValue);
                }
                else
                {
                    if (codeValue != "")
                    {
                        string tempStr = feaClsInfodic[pFeaClsName] + "," + codeValue;
                        feaClsInfodic[pFeaClsName] = tempStr;
                    }
                    else
                    {
                        feaClsInfodic[pFeaClsName] = "";
                    }
                }
            }
            pDataCheckClass.AreaLogicCheck(pFeaDataset, codeName, feaClsInfodic, AreaMin, AreaMax, out eError);
        }

        /// <summary>
        /// 高程值检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="ElevMin">最小高程值</param>
        /// <param name="ElevMax">最大高程值</param>
        /// <param name="eError"></param>
        private void ElevValueCheck(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable,string ElevMin,string ElevMax, out Exception eError)
        {
            eError = null;
            
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string pFeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                string fieldName = pTable.Rows[i]["字段项"].ToString().Trim();  //高程字段名
                if (pFeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (fieldName == "")
                {
                    eError = new Exception("字段名为空!");
                    return;
                }
                pDataCheckClass.CoutourValueCheck(pFeaDataset, pFeaClsName,fieldName, ElevMin, ElevMax, out eError);
                if (eError != null) return;
            }
        }

        /// <summary>
        /// 控制点高程注记检查、高程点高程注记检查、等高线高程注记检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="codeName">分类代码名称</param>
        /// <param name="radiu">搜索半径</param>
        /// <param name="precision">精度控制</param>
        /// <param name="eError"></param>
        private void PointAnnoCheck(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable,string codeName, double radiu, long precision, out Exception eError)
        {
             eError = null;

             for (int i = 0; i < pTable.Rows.Count; i++)
             {
                 string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                 string FieldName = pTable.Rows[i]["字段项"].ToString().Trim();  //字段名
                 string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();  //检查项
                 if ((FeaClsName == "")||(!FeaClsName.Contains(";")))
                 {
                     eError = new Exception("图层名为空或配置不正确!");
                     return;
                 }
                 if ((FieldName == "") || (!FeaClsName.Contains(";"))) 
                 {
                     eError = new Exception("字段名为空!");
                     return;
                 }
                 string oriCodeValue = "";
                 string desCodeValue = "";

                 string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                 string oriFeaClsName = feaNameArr[0].Trim();         //源要素类名称
                 string desFeaClsName = feaNameArr[1].Trim();         //目标要素类名称
                 string[] fieldNameArr = FieldName.Split(new char[] { ';' });
                 string oriFieldName = fieldNameArr[0].Trim();        //源高程字段名
                 string desFielsName = fieldNameArr[1].Trim();        //目标高程字段名

                 if (codeValue != "" && codeValue.Contains(";"))
                 {
                     string[] codeValueArr = codeValue.Split(new char[] { ';' });
                     oriCodeValue = codeValueArr[0].Trim();        //源要素类分类代码限制条件
                     desCodeValue = codeValueArr[1].Trim();        //目标要素类分类代码限制条件
                 }
                 pDataCheckClass.ElevAccordanceCheck(pFeaDataset, codeName, oriFeaClsName, oriCodeValue, oriFieldName, desFeaClsName, desCodeValue, desFielsName, radiu, precision, out eError);
                 if (eError != null) return;
             }
        }

        /// <summary>
        /// 线悬挂点检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="codeName">分类代码名称</param>
        /// <param name="tolerence">搜索容差</param>
        /// <param name="eError"></param>
        public void LineDangleCheck(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable, string codeName,double tolerence, out Exception eError)
        {
              eError = null;

              for (int i = 0; i < pTable.Rows.Count; i++)
              {
                  string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                  string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();  //分类代码值
                  if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                  {
                      eError = new Exception("图层名为空或配置不正确!");
                      return;
                  }

                  string[] feaNameArr=FeaClsName.Split(new char[]{';'});
                  string oriFeaClsName = feaNameArr[0].Trim();  //源要素类名
                  string desFeaClsName = feaNameArr[1].Trim();  //目标要素名
                  if (oriFeaClsName == desFeaClsName)
                  {
                      //同层面悬挂点检查
                      pDataCheckClass.OrdinaryTopoCheck(pFeaDataset,oriFeaClsName, esriTopologyRuleType.esriTRTLineNoDangles, out eError);
                      if (eError != null)
                      {
                          return;
                      }
                  }
                  else
                  {
                      //异层面检查
                      string oriCodeValueStr = "";
                      string desCodeValueStr = "";

                      if (codeValue != "" && codeValue.Contains(";"))
                      {
                          string[] codeValueArr = codeValue.Split(new char[] { ';' });
                          string oriCodeValue = codeValueArr[0].Trim();       
                          string desCodeValue = codeValueArr[1].Trim();        
                          oriCodeValueStr = codeName + "='" + oriCodeValue + "'"; //源要素类分类代码限制条件
                          desCodeValueStr = codeName + "='" + desCodeValue + "'"; //目标要素类分类代码限制条件
                      }

                      pDataCheckClass.LineDangleCheck(pFeaDataset, oriFeaClsName, oriCodeValueStr, desFeaClsName, desCodeValueStr, tolerence, out eError);
                      if (eError != null)
                      {
                          return;
                      }
                  }
              }
 
        }

        /// <summary>
        /// 线悬挂点检查 通用算法(列表)
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="codeName">分类代码名称</param>
        /// <param name="tolerence">搜索容差</param>
        /// <param name="eError"></param>
        public void LineDangleCheck2(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable, string codeName, double tolerence, out Exception eError)
        {
            eError = null;
            List<string> feaClsNameList = new List<string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();  //分类代码值
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return;
                }

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();  //源要素类名
                string desFeaClsName = feaNameArr[1].Trim();  //目标要素名
                if (oriFeaClsName == desFeaClsName)
                {
                    //同层面悬挂点检查，拓扑检查
                    //pDataCheckClass.OrdinaryTopoCheck(pFeaDataset, oriFeaClsName, esriTopologyRuleType.esriTRTLineNoDangles, out eError);
                    //if (eError != null)
                    //{
                    //    return;
                    //}
                    if(!feaClsNameList.Contains(oriFeaClsName))
                    {
                        feaClsNameList.Add(oriFeaClsName);
                    }
                }
                else
                {
                    //异层面检查
                    string oriCodeValueStr = "";
                    string desCodeValueStr = "";

                    if (codeValue != "" && codeValue.Contains(";"))
                    {
                        string[] codeValueArr = codeValue.Split(new char[] { ';' });
                        string oriCodeValue = codeValueArr[0].Trim();
                        string desCodeValue = codeValueArr[1].Trim();
                        oriCodeValueStr = codeName + "='" + oriCodeValue + "'"; //源要素类分类代码限制条件
                        desCodeValueStr = codeName + "='" + desCodeValue + "'"; //目标要素类分类代码限制条件
                    }

                    pDataCheckClass.LineDangleCheck(pFeaDataset, oriFeaClsName, oriCodeValueStr, desFeaClsName, desCodeValueStr, tolerence, out eError);
                    if (eError != null)
                    {
                        return;
                    }
                }
            }
            if(feaClsNameList.Count==0)
            {
                return;
            }
            //同层面悬挂点检查
            pDataCheckClass.OrdinaryTopoCheck(pFeaDataset, feaClsNameList, esriTopologyRuleType.esriTRTLineNoDangles, out eError);
            if (eError != null)
            {
                return;
            }
        }



        /// <summary>
        /// 面拓扑检查,面含面检查，面缝隙检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="topoRule"></param>
        /// <param name="eError"></param>
        public void AreaTopoCheck(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable, esriTopologyRuleType topoRule, out Exception eError)
        {
            eError = null;

            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return;
                }

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();  //源要素类名
                string desFeaClsName = feaNameArr[1].Trim();  //目标要素名
                pDataCheckClass.OrdinaryTopoCheck(pFeaDataset, oriFeaClsName, desFeaClsName, topoRule, out eError);
                if (eError != null) return;
            }
        }

        /// <summary>
        /// 面拓扑检查,面含面检查(列表)
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="topoRule"></param>
        /// <param name="eError"></param>
        public void AreaTopoCheck2(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable, esriTopologyRuleType topoRule, out Exception eError)
        {
            eError = null;
            List<string> FeaClsNameDic = new List<string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return;
                }

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();  //源要素类名
                string desFeaClsName = feaNameArr[1].Trim();  //目标要素名
                if(!FeaClsNameDic.Contains(oriFeaClsName+";"+desFeaClsName))
                {
                    FeaClsNameDic.Add(oriFeaClsName + ";" + desFeaClsName);
                }
            }
            if (FeaClsNameDic.Count == 0) return;
            pDataCheckClass.OrdinaryDicTopoCheck(pFeaDataset, FeaClsNameDic, topoRule, out eError);
            if (eError != null) return;
        }

        /// <summary>
        /// 面缝隙检查,同层线相交检查等等
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="eError"></param>
        public void SpecialFeaClsTopoCheck(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable,esriTopologyRuleType pTopoRule, out Exception eError)
        {
            eError = null;

            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if (FeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                pDataCheckClass.OrdinaryTopoCheck(pFeaDataset, FeaClsName, pTopoRule, out eError);
                if (eError != null) return;
            }
        }

        public void SpecialFeaClsTopoCheck2(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable, esriTopologyRuleType pTopoRule, out Exception eError)
        {
            eError = null;
            List<string> lstFeaClsName = new List<string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if (FeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
               if(!lstFeaClsName.Contains(FeaClsName))
               {
                   lstFeaClsName.Add(FeaClsName);
               }
            }
            if (lstFeaClsName.Count == 0) return;
            pDataCheckClass.OrdinaryTopoCheck(pFeaDataset, lstFeaClsName, pTopoRule, out eError);
            if (eError != null) return;
        }

        /// <summary>
        /// 线穿面检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="eError"></param>
        public void LineCrossAreaCheck(DataCheckClass pDataCheckClass, IFeatureDataset pFeaDataset, DataTable pTable, out Exception eError)
        {
            eError = null;

            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return;
                }

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();  //源要素类名
                string desFeaClsName = feaNameArr[1].Trim();  //目标要素名
                pDataCheckClass.CrossTopoCheck(pFeaDataset, oriFeaClsName, desFeaClsName, out eError);
                if (eError != null) return;
            }
        }

        #endregion

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}