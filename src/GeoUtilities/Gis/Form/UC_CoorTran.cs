using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;


using ESRI.ArcGIS.Geodatabase;

using ESRI.ArcGIS.DataSourcesFile;

using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
//using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;

using ESRI.ArcGIS.DataSourcesGDB;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using SysCommon.Error;

namespace GeoUtilities
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110822
    /// 说明：读取控制点进行坐标转换的控件
    /// 空间参考文件可选，目标数据不可覆盖。
    /// </summary>
    public partial class UC_CoorTran : UserControl
    {
        public UC_CoorTran()
        {
            InitializeComponent();
        }
        private IWorkspace pSrcWorkspace = null;
        private IWorkspace pToWorkspace = null;
        private List<IPoint> pSrcPts = null;//源控制点集合
        private List<IPoint> pToPts = null;//目标控制点集合
        private Dictionary<string, string> pResult;//记录成功或失败的要素类
        private ITransformation pTransformation;//转换类
        private ISpatialReference pSR;//空间参考文件获取的空间参考
        private IWorkspaceFactory pWF;
        private string toPath="";
        private DateTime start, stop;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "控制点文件|*.txt";
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtCtrlPtPath.Text = openFileDialog1.FileName;
                fillDGView(openFileDialog1.OpenFile());
                try
                {
                    initTransformation();
                }
                catch (Exception ex)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                }

            }

            txtCtrlPtPath.ForeColor = Color.Black;

        }
        //填充dgv
        private void fillDGView(Stream inStream)
        {
            StreamReader sr = new StreamReader(inStream);
            dgvCtrlPt.Rows.Clear();
            int id = 1;
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                string[] strs = str.Split(',', ':');
                if (strs.Length != 6 && strs.Length!=7)
                    continue;
                if(strs.Length==6)
                    dgvCtrlPt.Rows.Add(id.ToString(),strs[0], strs[1], strs[2], strs[3], strs[4], strs[5]);
                if (strs.Length == 7)
                    dgvCtrlPt.Rows.Add(strs[0], strs[1], strs[2], strs[3], strs[4], strs[5],strs[6]);
                id++;

            }

        }
        //构造转换类
        private void initTransformation()
        {
            pSrcPts = new List<IPoint>();
            pToPts = new List<IPoint>();
            for (int i = 0; i < dgvCtrlPt.Rows.Count; i++)//行
            {
                IPoint ptSrc = new PointClass();//源点
                ptSrc.PutCoords(Convert.ToDouble(dgvCtrlPt[1, i].Value), Convert.ToDouble(dgvCtrlPt[2, i].Value));
                pSrcPts.Add(ptSrc);
                IPoint ptTo = new PointClass();//目标点
                ptTo.PutCoords(Convert.ToDouble(dgvCtrlPt[4, i].Value), Convert.ToDouble(dgvCtrlPt[5, i].Value));
                pToPts.Add(ptTo);
            }
            IPoint[] aSrcPts = pSrcPts.ToArray();
            IPoint[] aToPoint = pToPts.ToArray();
            pTransformation = new AffineTransformation2DClass();
            IAffineTransformation2D3GEN pAffineTrans3 = pTransformation as IAffineTransformation2D3GEN;
            pAffineTrans3.DefineFromControlPoints(ref aSrcPts, ref aToPoint);
            for (int i = 0; i < dgvCtrlPt.Rows.Count; i++)//行
            {
                double fromPtError = 0, toPtError = 0;
                pAffineTrans3.GetControlPointError(i, ref fromPtError, ref toPtError);//获取每个控制点残差
                dgvCtrlPt[7, i].Value = fromPtError.ToString("F06");
            }
            double fromRMSerror = 0, toRMSerror = 0;
            pAffineTrans3.GetRMSError(ref fromRMSerror, ref toRMSerror);
            if (fromRMSerror < 0.05)
                lblRMS.Text = "标准差（RMS）：" + fromRMSerror.ToString("F06") + ".可以进行转换！";
            else
                lblRMS.Text = "标准差（RMS）：" + fromRMSerror.ToString("F06") + ".误差太大，请修正控制点数据！";
        }
        /// <summary>
        /// 实现对要素类的坐标的仿射变换
        /// </summary>
        /// <param name="inFC">要素类</param>
        /// <param name="inTransformation">转换类</param>
        private void coordTransfermation(IFeatureClass inFC, ITransformation inTransformation)
        {

            IFeatureCursor pFCursor = inFC.Update(null, false);
            IFeature pFeature = pFCursor.NextFeature();
            while (pFeature != null)
            {
                IGeometry shpTransformed = pFeature.ShapeCopy;
                ITransform2D pTransform2D = shpTransformed as ITransform2D;
                pTransform2D.Transform(esriTransformDirection.esriTransformForward, inTransformation);
                pFeature.Shape = shpTransformed;
                //int id = inFC.FindField("LAYER_OID");
                //if((inFC as IDataset).Name=="宗地_Project54_1")
                //pFeature.set_Value(id,"1");

                pFCursor.UpdateFeature(pFeature);
                //cursor后移
                pFeature = pFCursor.NextFeature();
            }
            Marshal.ReleaseComObject(pFCursor);//释放cursor
            ISpatialReference unKnownSR = new UnknownCoordinateSystemClass();
            IGeoDatasetSchemaEdit pGDSE = inFC as IGeoDatasetSchemaEdit;
            if (pGDSE.CanAlterSpatialReference)
                pGDSE.AlterSpatialReference(unKnownSR);//更新要素类的投影
            IFeatureClassManage pFCM = inFC as IFeatureClassManage;
            pFCM.UpdateExtent();//更新要素类的最值范围
            //IGeoDataset pGD = inFC as IGeoDataset;
            //IEnvelope ppp = pGD.Extent;
        }



        private void btnOK_Click(object sender, EventArgs e)
        {
            bool resultErr = false;
            if (txtCtrlPtPath.Text == "" || txtSrcPath.Text == "" || txtToPath.Text == "")
                return;
            if (lstFC.CheckedItems.Count == 0)
                return;
            try
            {
                initTransformation();
            }
            catch (Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                return;
            }
            if(rdoMDB.Checked)//personal GDB
            {
                try
                {
                pWF = new AccessWorkspaceFactoryClass();
                string path = txtToPath.Text.Substring(0, txtToPath.Text.LastIndexOf("\\") + 1);
                string name = txtToPath.Text.Substring(txtToPath.Text.LastIndexOf("\\") + 1);
                IWorkspaceName pToWN=pWF.Create(path,name,null,0);
                if (pToWN == null)
                    return;
              
                    pToWorkspace = pWF.OpenFromFile(txtToPath.Text, 0);
                }
                catch { }
                if (pToWorkspace == null)
                    return;

            }
            else if (rdoGDB.Checked)//file GDB
            {
                pWF = new FileGDBWorkspaceFactoryClass();
                string path = txtToPath.Text.Substring(0, txtToPath.Text.LastIndexOf("\\") + 1);
                string name = txtToPath.Text.Substring(txtToPath.Text.LastIndexOf("\\") + 1);
                IWorkspaceName pToWN = pWF.Create(path, name, null, 0);
                if (pToWN == null)
                    return;
                try
                {
                    pToWorkspace = pWF.OpenFromFile(txtToPath.Text, 0);
                }
                catch { }
                if (pToWorkspace == null)
                    return;
 
            }
            else if (rdoSHP.Checked)//shp
            {
                pWF = new ShapefileWorkspaceFactoryClass();
                try
                {
                    pToWorkspace = pWF.OpenFromFile(txtToPath.Text, 0);
                }
                catch { }
                if (pToWorkspace == null)
                    return;
            }
            try
            {
                toPath = txtToPath.Text;//保存目标路径，因转换结束要清空目标文本框防止用户再次点击转换，查看日志需要知道目标路径
                pbCoorTran.Value = 0;
                pbCoorTran.Minimum = 0;
                pbCoorTran.Maximum = lstFC.CheckedItems.Count;
                pbCoorTran.Step = 1;
                pbCoorTran.Visible = true;
                start = DateTime.Now;//开始时间
                Application.DoEvents();
                pResult = new Dictionary<string, string>();
                IWorkspaceEdit pWorkSpaceEdit = pToWorkspace as IWorkspaceEdit;
                pWorkSpaceEdit.StartEditing(false);
                foreach (ListViewItem lvi in lstFC.CheckedItems)
                {
                    IDataset pToD = null;
                    try
                    {
                        IFeatureClass pFc = (pSrcWorkspace as IFeatureWorkspace).OpenFeatureClass(lvi.Text);
                        IDataset pD = pFc as IDataset;
                        lblRMS.Text = "正在转换要素类" + pD.Name + "...";
                        Application.DoEvents();
                        IFeatureClass pToFC = TransFeatures.CreateFeatureClass(pD.Name, pFc, pToWorkspace, null, null, pFc.ShapeType);
                        pToD = pToFC as IDataset;
                        TransFeatures.CopyFeatureAndTran(pFc.Search(null, false), pToFC, pTransformation);
                        IGeoDataset pGD = pToD as IGeoDataset;
                        IGeoDatasetSchemaEdit pGDS = pGD as IGeoDatasetSchemaEdit;
                        if (pSR!=null&&pGDS.CanAlterSpatialReference)
                            pGDS.AlterSpatialReference(pSR);//定义空间参考
                        pResult.Add(pToD.Name, "转换成功");
                        pbCoorTran.PerformStep();
                    }
                    catch (Exception ex)
                    {
                        pResult.Add(pToD.Name, "转换失败/" + ex.Message);
                        if (ex.Message == "The coordinates or measures are out of bounds.")
                            pResult[pToD.Name] = "转换失败/控制点坐标超出该要素类坐标域边界";
                        pToD.Delete();
                        resultErr = true;

                    }
                  
                    //if (rdoSHP.Checked)
                    //    pD.Copy(pD.Name, pToWorkspace);
                    //else
                    //    CopyPasteGDBData.CopyPasteGeodatabaseData(pSrcWorkspace, pToWorkspace, pD.Name, esriDatasetType.esriDTFeatureClass);

                }//foreach
                #region 20110815前代码
                //IWorkspaceEdit pWorkSpaceEdit = pToWorkspace as IWorkspaceEdit;
                //pWorkSpaceEdit.StartEditing(false);
                //IEnumDataset enumDS = pToWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                //IDataset pDs = enumDS.Next();
                //while (pDs != null)
                //{
                //    lblRMS.Text = "正在转换要素类"+pDs.Name+"...";
                //    try
                //    {
                //        IFeatureClass pFC = pDs as IFeatureClass;
                //        coordTransfermation(pFC, pTransformation);
                //        pResult.Add(pDs.Name, "转换成功");
                //    }
                //    catch (Exception ex)
                //    {
                //        pResult.Add(pDs.Name, "转换失败/" + ex.Message);
                //        if (ex.Message == "The coordinates or measures are out of bounds.")
                //            pResult[pDs.Name] = "转换失败/控制点坐标超出该要素类坐标域边界";
                //        pDs.Delete();
                //        resultErr = true;

                //    }
                //    finally
                //    {
                //        pbCoorTran.PerformStep();
                //        pDs = enumDS.Next();
                //    }
                //}
                //IEnumDataset enumDS1 = pToWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                //IDataset pDs1 = enumDS1.Next();
                //while (pDs1 != null)
                //{
                //    lblRMS.Text = "正在转换要素集" + pDs1.Name + "...";
                //    IEnumDataset pED = pDs1.Subsets;
                //    IDataset pDs2 = pED.Next();
                //    while (pDs2 != null)
                //    {
                //        try
                //        {
                //            IFeatureClass pFC2 = pDs2 as IFeatureClass;
                //            coordTransfermation(pFC2, pTransformation);
                //            pResult.Add(pDs2.Name, "转换成功");
                //        }
                //        catch (Exception ex)
                //        {

                //            pResult.Add(pDs2.Name, "转换失败/" + ex.Message);
                //            if (ex.Message == "The coordinates or measures are out of bounds.")
                //                pResult[pDs2.Name] = "转换失败/控制点坐标超出该要素类坐标域边界";
                //            pDs2.Delete();
                //            resultErr = true;
                //        }
                //        finally
                //        {

                //            pDs2 = pED.Next();
                //        }
                //    }
                //    pbCoorTran.PerformStep();
                //    pDs1 = enumDS1.Next();
                //}
                

                //}
                //foreach (KeyValuePair<string, string> kvp in pResult)
                //{
                //    ListViewItem lvi = lstViewResult.Items.Add(kvp.Key);
                //    lvi.SubItems.Add(kvp.Value);
                //}
                //lstViewResult.Refresh();
                //this.Height = 529;
                #endregion
                pWorkSpaceEdit.StopEditing(true);

            }
            catch (Exception ex)
            {


                MessageBox.Show("坐标转换失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            finally
            {
                //vProgress.Close();
                pToWorkspace = null;
                txtToPath.Text = "";
                pbCoorTran.Visible = false;
            }

            if(!resultErr)
               lblRMS.Text = "转换成功！";
            else
                lblRMS.Text = "转换结束！但是部分要素转换失败，请查看日志。";
            stop = DateTime.Now;//结束时间

        }

        private void dgvCtrlPt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSrcPath_Click(object sender, EventArgs e)
        {

            if (rdoGDB.Checked)
            {
                FolderBrowserDialog pFBD = new FolderBrowserDialog();

                if (pFBD.ShowDialog() == DialogResult.OK)
                {
                    txtSrcPath.Text = pFBD.SelectedPath;
                    pWF = new FileGDBWorkspaceFactoryClass();
                }
                
                
            }
            else if (rdoSHP.Checked)
            {
                FolderBrowserDialog pFBD = new FolderBrowserDialog();

                if (pFBD.ShowDialog() == DialogResult.OK)
                {
                    txtSrcPath.Text = pFBD.SelectedPath;
                    pWF = new ShapefileWorkspaceFactoryClass();
                }

            }
            else
            {
                openFileDialog1.Filter = "源数据工作空间(mdb)|*.mdb";
                openFileDialog1.FileName = "";
                openFileDialog1.Multiselect = false;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtSrcPath.Text = openFileDialog1.FileName;
                    pWF = new AccessWorkspaceFactoryClass();
                }
            }
            if (pWF == null)
                return;
            try
            {
                pSrcWorkspace = pWF.OpenFromFile(txtSrcPath.Text, 0);
            }
            catch
            { }
            if (pSrcWorkspace == null)
                return;
            LstAllLyrFile(pSrcWorkspace);
        }
        //列出全部的FC
        private void LstAllLyrFile(IWorkspace pWks)
        {
            try
            {
                lstFC.Clear();
                IFeatureWorkspace pFeaWks = pWks as IFeatureWorkspace;
                if (pFeaWks == null) return;

                ESRI.ArcGIS.Geometry.ISpatialReference pSpa = null;

                IEnumDatasetName pEnumFeaCls = pWks.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
                IDatasetName pFeaClsName = pEnumFeaCls.Next();
                while (pFeaClsName != null)
                {
                    if (pSpa == null)
                    {
                        IName pPrjName = pFeaClsName as IName;
                        IFeatureClass pFeaCls = pPrjName.Open() as IFeatureClass;
                        IGeoDataset pGeodataset = pFeaCls as IGeoDataset;

                        //获得空间参看 源的
                        //pSpa = pGeodataset.SpatialReference;
                        //m_strInPrj = ExportToESRISpatialReference(pSpa);
                    }

                    this.lstFC.Items.Add(pFeaClsName.Name);
                    pFeaClsName = pEnumFeaCls.Next();
                }

                IEnumDatasetName pEnumDataNames = pWks.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
                IDatasetName pDatasetName = pEnumDataNames.Next();
                while (pDatasetName != null)
                {
                    IEnumDatasetName pSubNames = pDatasetName.SubsetNames;
                    IDatasetName pSubName = pSubNames.Next();
                    while (pSubName != null)
                    {
                        if (pSpa == null)
                        {
                            IName pPrjName = pSubName as IName;
                            IFeatureClass pFeaCls = pPrjName.Open() as IFeatureClass;
                            IGeoDataset pGeodataset = pFeaCls as IGeoDataset;

                            //获得空间参看 源的
                            //pSpa = pGeodataset.SpatialReference;
                            //m_strInPrj = ExportToESRISpatialReference(pSpa);
                        }

                        this.lstFC.Items.Add(pSubName.Name);
                        pSubName = pSubNames.Next();
                    }

                    pDatasetName = pEnumDataNames.Next();
                }

                for (int i = 0; i < this.lstFC.Items.Count; i++)
                {
                    this.lstFC.Items[i].Checked = true;
                }
            }
            catch
            {
            }
        }
        private void btnToPath_Click(object sender, EventArgs e)
        {
            if (rdoSHP.Checked)
            {
                FolderBrowserDialog pFBD = new FolderBrowserDialog();

                if (pFBD.ShowDialog() == DialogResult.OK)
                {
                    txtToPath.Text = pFBD.SelectedPath;
                }
            }
            else if (rdoGDB.Checked)
            {
                SaveFileDialog savFD = new SaveFileDialog();
                savFD.Filter = "目标数据工作空间(gdb)|*.gdb";
                savFD.FileName = "";
                if (savFD.ShowDialog() == DialogResult.OK)
                {
                    txtToPath.Text = savFD.FileName;
                }
            }
            else
            {
                SaveFileDialog savFD = new SaveFileDialog();
                savFD.Filter = "目标数据工作空间(mdb)|*.mdb";
                savFD.FileName = "";
                if (savFD.ShowDialog() == DialogResult.OK)
                {
                    txtToPath.Text = savFD.FileName;
                }
            }
        }

      


        private void btnExport_Click(object sender, EventArgs e)
        {
            if (pResult == null)
                return;
            if (pResult.Count == 0)
                return;
            //SaveFileDialog pSFD = new SaveFileDialog();
            //pSFD.Filter = "文本文件(.txt)|*.txt";
            //if (pSFD.ShowDialog() == DialogResult.OK)
            //{
                //Stream tmpStream = pSFD.OpenFile();
            string filename=System.IO.Path.GetDirectoryName(toPath)+"\\转换日志.txt";
            Stream tmpStream = new System.IO.FileStream(filename, FileMode.Create);
                StreamWriter sw = new StreamWriter(tmpStream);
                sw.WriteLine("日志位置："+filename);
                sw.WriteLine("---------------在'"+start.ToString()+"'开始转换---------------");
                foreach (KeyValuePair<string, string> kvp in pResult)
                {
                    sw.WriteLine(kvp.Key + "\t" + kvp.Value);
                }
                sw.WriteLine("---------------在'" + stop.ToString() + "'结束转换---------------");
                
                sw.Close();
                tmpStream.Close();
                //if (MessageBox.Show("保存成功！是否现在查看？", "提示", MessageBoxButtons.YesNo,
                //        MessageBoxIcon.Information) == DialogResult.Yes)
                //{
                    Process p = new Process();
                    p.StartInfo.FileName = "notepad.exe";
                    p.StartInfo.Arguments = filename;
                    p.Start();
                  
                //}
                //pSFD.Dispose();
            //}
        }

        private void rdoSHP_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoSHP.Checked)
            {
                txtSrcPath.Text = "";
                txtToPath.Text = "";
                lstFC.Clear();
            }
        }

        private void rdoMDB_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoMDB.Checked)
            {
                txtSrcPath.Text = "";
                txtToPath.Text = "";
                lstFC.Clear();
            }
        }

        private void rdoGDB_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoGDB.Checked)
            {
                txtSrcPath.Text = "";
                txtToPath.Text = "";
                lstFC.Clear();
            }
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstFC.Items)
            {
                lvi.Checked = true;
            }
        }

        private void btnSelReverse_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstFC.Items)
            {
                if (!lvi.Checked)
                    lvi.Checked = true;
                else
                    lvi.Checked = false;
            }
        }

   

        private void btnCtrlPtPath_Leave(object sender, EventArgs e)
        {
            lblRMS.Text = "";
        }

        private void btnSaveCtrPts_Click(object sender, EventArgs e)
        {
            SaveFileDialog pSFD = new SaveFileDialog();
            pSFD.Filter = "文本文件(.txt)|*.txt";
            if (pSFD.ShowDialog() == DialogResult.OK)
            {
                Stream tmpStream = pSFD.OpenFile();
                StreamWriter sw = new StreamWriter(tmpStream);
                for (int i = 0; i < dgvCtrlPt.Rows.Count;i++ )
                {
                     string str="";
                    for (int j = 0; j < dgvCtrlPt.Columns.Count-1; j++)
                    {
                        try
                        {
                            str += dgvCtrlPt[j, i].Value.ToString() + ",";
                        }
                        catch { }
                        
                    }
                    str = str.Substring(0, str.Length - 1);
                    sw.WriteLine(str);
                }

                sw.Close();
                tmpStream.Close();
                lblRMS.Text = "导出成功！";
                //if (MessageBox.Show("保存成功！是否现在查看？", "提示", MessageBoxButtons.YesNo,
                //        MessageBoxIcon.Information) == DialogResult.Yes)
                //{
            }
        }

        private void btnEditCtrlPts_Click(object sender, EventArgs e)
        {
            if (btnEditCtrlPts.Text == "编辑")
            {
                dgvCtrlPt.ReadOnly = false;
                btnEditCtrlPts.Text = "保存";
            }
            else
            {
                dgvCtrlPt.ReadOnly = true;
                btnEditCtrlPts.Text = "编辑";
                try
                {
                    initTransformation();
                }
                catch (Exception ex)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                }
            }
        }

        private void btnPrjFile_Click(object sender, EventArgs e)
        {
            string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
            if (sInstall == "") //added by chulili 2012-11-13 平台由ArcGIS9.3换成ArcGIS10，相应的注册表路径要修改
            {
                sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
            }
            if (sInstall == "")
            {
                sInstall = ReadRegistry("SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
            }   //added by chulili 2012-11-13  end
            sInstall = sInstall + "\\Coordinate Systems";

            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Multiselect = false;
            filedialog.InitialDirectory = sInstall;
            filedialog.Filter = "*.prj|*.prj";
            filedialog.Title = "选择prj文件";
            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                string str = filedialog.FileName;
                this.txtPrjFile.Text = str;

                //直接读取坐标系统
                try
                {
                    ESRI.ArcGIS.Geometry.ISpatialReferenceFactory pPrjFac = new ESRI.ArcGIS.Geometry.SpatialReferenceEnvironmentClass();
                    pSR = pPrjFac.CreateESRISpatialReferenceFromPRJFile(str);

                }
                catch
                {

                }
            }
        }
        private string ReadRegistry(string p)
        {
            /// <summary> 
            /// 从注册表中取得指定软件的路径 

            /// </summary> 

            /// <param name="sKey"> </param> 

            /// <returns> </returns> 

            //Open the subkey for reading 

            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(p, true);

            if (rk == null) return "";

            // Get the data from a specified item in the key. 

            return (string)rk.GetValue("InstallDir");
        }
     


    }
}
