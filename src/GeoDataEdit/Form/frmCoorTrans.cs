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
using GeoDataCenterFunLib;



namespace GeoDataEdit
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.06.22
    /// 说明：读取控制点进行坐标转换的窗体
    /// </summary>
    public partial class frmCoorTrans : DevComponents.DotNetBar.Office2007Form
    {
        public frmCoorTrans()
        {
            InitializeComponent();
            this.Height = 343;
        }

        private string shpPath = "";
        private IWorkspace pWorkspace = null;
        private IFeatureClass pFeaClass=null;
        private List<IPoint> pSrcPts = null;//源控制点集合
        private List<IPoint> pToPts = null;//目标控制点集合
        private Dictionary<string, string> pResult;//记录成功或失败的要素类
        private ITransformation pTransformation;//转换类
        private IWorkspaceFactory pWF;

        private void buttonX1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "控制点文件|*.txt";
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtCtrlPtPath.Text = openFileDialog1.FileName;
                fillDGView(openFileDialog1.OpenFile());
                initTransformation();
 
            }

            txtCtrlPtPath.ForeColor = Color.Black;
          

        }
        //填充dgv
        private void fillDGView(Stream inStream)
        {
            StreamReader sr = new StreamReader(inStream);
            dgvCtrlPt.Rows.Clear();
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                string[] strs = str.Split(',', ':');
                if (strs.Length != 6)
                    continue;
                dgvCtrlPt.Rows.Add(strs[0], strs[1], strs[2], strs[3], strs[4], strs[5]);
 
            }
            
        }
        //构造转换类
        private void initTransformation()
        {
            pSrcPts = new List<IPoint>();
            pToPts = new List<IPoint>();
            pTransformation = new AffineTransformation2DClass();
            IAffineTransformation2D3GEN pAffineTrans3 = pTransformation as IAffineTransformation2D3GEN;
            
            for (int i = 0; i < dgvCtrlPt.Rows.Count; i++)//行
            {
                IPoint ptSrc = new PointClass();//源点
                ptSrc.PutCoords(Convert.ToDouble(dgvCtrlPt[0, i].Value), Convert.ToDouble(dgvCtrlPt[1, i].Value));
                pSrcPts.Add(ptSrc);
                IPoint ptTo = new PointClass();//目标点
                ptTo.PutCoords(Convert.ToDouble(dgvCtrlPt[3, i].Value), Convert.ToDouble(dgvCtrlPt[4, i].Value));
                pToPts.Add(ptTo);
            }
            IPoint[] aSrcPts = pSrcPts.ToArray();
            IPoint[] aToPoint = pToPts.ToArray();
            pAffineTrans3.DefineFromControlPoints(ref aSrcPts, ref aToPoint);
            for (int i = 0; i < dgvCtrlPt.Rows.Count; i++)//行
            {
                double fromPtError=0,toPtError=0;
                pAffineTrans3.GetControlPointError(i,ref fromPtError,ref toPtError);//获取每个控制点残差
                dgvCtrlPt[6, i].Value = fromPtError.ToString("F06");
            }
            double fromRMSerror=0,toRMSerror=0;
            pAffineTrans3.GetRMSError(ref fromRMSerror, ref toRMSerror);
            if(fromRMSerror<0.05)
                lblRMS.Text = "标准差（RMS）：" + fromRMSerror.ToString("F06")+".可以进行转换！";
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
            IGeoDataset pGD = inFC as IGeoDataset;
            IEnvelope ppp = pGD.Extent;
        }
        //打开shp文件
        private IFeatureClass openShp()
        {
            if (txtCtrlPtPath.Text == "") 
                return null;
            string wsPath=System.IO.Path.GetDirectoryName(txtCtrlPtPath.Text);
            string shpName=System.IO.Path.GetFileName(txtCtrlPtPath.Text);
            string exten=System.IO.Path.GetExtension(txtCtrlPtPath.Text);
            pWorkspace = new ShapefileWorkspaceFactoryClass().OpenFromFile(wsPath,0);
            IFeatureClass pFeatureClass=(pWorkspace as IFeatureWorkspace).OpenFeatureClass(shpName.Remove(shpName.Length-4));
            return pFeatureClass;
            
        }
    

        private void btnOK_Click(object sender, EventArgs e)
        {

            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在转换,请稍后");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = false;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            
            Application.DoEvents();
            bool result = false;
            if (txtCtrlPtPath.Text == "打开一个控制点文件（txt）" || txtSrcPath.Text == "源数据工作空间路径" || txtToPath.Text == "转换后的数据的工作空间路径")
            {
                vProgress.Close();
                return;
            }
            try
            {
            if(rdoSHP.Checked)
            {
                DirectoryInfo dir = new DirectoryInfo(txtSrcPath.Text);
                FileInfo[] files = dir.GetFiles();
                if (!Directory.Exists(txtToPath.Text))
                    Directory.CreateDirectory(txtToPath.Text);//判断目录存在否
                foreach (FileInfo file in files)
                {
                    file.CopyTo(System.IO.Path.Combine(txtToPath.Text, file.Name), true);
                }
                pWF = new ShapefileWorkspaceFactoryClass();
            }
            else if(rdoGDB.Checked)
            {
                DirectoryInfo dir2 = new DirectoryInfo(txtSrcPath.Text);
                FileInfo[] files2 = dir2.GetFiles();
                if (!Directory.Exists(txtToPath.Text))
                    Directory.CreateDirectory(txtToPath.Text);//判断目录存在否
                else
                {
                    if (MessageBox.Show("文件已存在。确定覆盖？", "提示", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information) != DialogResult.OK)
                        return;
                    Directory.Delete(txtToPath.Text, true);
                    Directory.CreateDirectory(txtToPath.Text);

                }
                foreach (FileInfo file in files2)
                {
                    file.CopyTo(System.IO.Path.Combine(txtToPath.Text, file.Name), true);
                }
                pWF = new FileGDBWorkspaceFactoryClass();
            }
            else
            {
                File.Copy(txtSrcPath.Text, txtToPath.Text, true);
                pWF = new AccessWorkspaceFactoryClass();
            }

                pResult = new Dictionary<string, string>();
                lstViewResult.Items.Clear();
                pWorkspace = pWF.OpenFromFile(txtToPath.Text, 0);
                IWorkspaceEdit pWorkSpaceEdit = pWorkspace as IWorkspaceEdit;
                pWorkSpaceEdit.StartEditing(false);
                IEnumDataset enumDS = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                IDataset pDs = enumDS.Next();
                while (pDs != null)
                {
                    try
                    {
                        IFeatureClass pFC = pDs as IFeatureClass;
                        coordTransfermation(pFC, pTransformation);
                        pResult.Add(pDs.Name, "转换成功");
                    }
                    catch(Exception ex)
                    {
                        pResult.Add(pDs.Name, "转换失败/"+ex.Message);
                        if (ex.Message == "The coordinates or measures are out of bounds.")
                           pResult[pDs.Name]="转换失败/控制点坐标超出该要素类坐标域边界";
                        pDs.Delete();
 
                    }
                    finally
                    {
                        pDs = enumDS.Next();
                    }
                }
                IEnumDataset enumDS1 = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset pDs1 = enumDS1.Next();
                while (pDs1 != null)
                {
                    IEnumDataset pED = pDs1.Subsets;
                    IDataset pDs2 = pED.Next();
                    while (pDs2 != null)
                    {
                        try
                        {
                            IFeatureClass pFC2 = pDs2 as IFeatureClass;
                            coordTransfermation(pFC2, pTransformation);
                            pResult.Add(pDs2.Name, "转换成功");
                        }
                        catch (Exception ex)
                        {

                            pResult.Add(pDs2.Name, "转换失败/" + ex.Message);
                            if (ex.Message == "The coordinates or measures are out of bounds.")
                                pResult[pDs2.Name] = "转换失败/控制点坐标超出该要素类坐标域边界";
                            pDs2.Delete();
                        }
                        finally
                        {
                            pDs2 = pED.Next();
                        }
                    }
                    pDs1 = enumDS1.Next();
                }
                pWorkSpaceEdit.StopEditing(true);
                //vProgress.Close();
                //result = true;
                //if (result == true)
                //{
                //    MessageBox.Show("坐标转换成功完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //}
                foreach (KeyValuePair<string, string> kvp in pResult)
                {
                    ListViewItem lvi = lstViewResult.Items.Add(kvp.Key);
                    lvi.SubItems.Add(kvp.Value);
                }
                lstViewResult.Refresh();
                this.Height = 529;

            }
            catch (Exception ex)
            {
                

                MessageBox.Show("坐标转换失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                vProgress.Close();

            }
            finally
            {
                vProgress.Close();
                pWorkspace = null;
                pWF = null;
            }


                lblRMS.Text = "";
            Application.DoEvents();
            
        }
   
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvCtrlPt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        private void btnSrcPath_Click(object sender, EventArgs e)
        {
           
                if(rdoGDB.Checked||rdoSHP.Checked)
                {
                    FolderBrowserDialog pFBD = new FolderBrowserDialog();

                    if (pFBD.ShowDialog() == DialogResult.OK)
                    {
                        txtSrcPath.Text = pFBD.SelectedPath;
                        txtSrcPath.ForeColor = Color.Black;
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
                        txtSrcPath.ForeColor = Color.Black;
                    }
                }
        }

        private void btnToPath_Click(object sender, EventArgs e)
        {
               if(rdoGDB.Checked||rdoSHP.Checked)
                {
                    FolderBrowserDialog pFBD = new FolderBrowserDialog();

                    if (pFBD.ShowDialog() == DialogResult.OK)
                    {
                        txtToPath.Text = pFBD.SelectedPath;
                        txtToPath.ForeColor = Color.Black;
                    }
               }
               else
               {
                   SaveFileDialog savFD=new SaveFileDialog();
                    savFD.Filter = "目标数据工作空间(mdb)|*.mdb";
                    savFD.FileName = "";
                    if (savFD.ShowDialog() == DialogResult.OK)
                    {
                        txtToPath.Text = savFD.FileName;
                        txtToPath.ForeColor = Color.Black;
                    }
               }      
        }

        private void btnOpenxml_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "参数配置文件（xml）|*.xml";
                openFileDialog1.FileName = "";
                openFileDialog1.Multiselect = false;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(openFileDialog1.FileName);
                    XmlNode xn1 = xmlDoc.FirstChild.NextSibling;
                    XmlNode xn2 = xn1.FirstChild;
                    XmlNode xn3 = xn2.FirstChild;
                    txtCtrlPtPath.Text = xn3.Attributes[0].Value;
                    txtCtrlPtPath.ForeColor = Color.Black;
                    Stream strm = new System.IO.FileStream(txtCtrlPtPath.Text, FileMode.Open, FileAccess.Read);
                    fillDGView(strm);
                    initTransformation();
                    strm.Close();
                    XmlNode xn3_2 = xn3.NextSibling;
                    cboxSrcType.SelectedIndex = cboxSrcType.FindStringExact(xn3_2.Attributes[0].Value);
                    XmlNode xn3_3 = xn3_2.NextSibling;
                    txtSrcPath.Text = xn3_3.Attributes[0].Value;
                    txtSrcPath.ForeColor = Color.Black;
                    XmlNode xn3_4 = xn3_3.NextSibling;
                    txtToPath.Text = xn3_4.Attributes[0].Value;
                    txtToPath.ForeColor = Color.Black;
                    xmlDoc = null;
                }
            }
            catch
            {
 
            }
            
        }

        private void btnSaveXml_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog pSFD = new SaveFileDialog();
                pSFD.Filter = "参数配置文件（xml）|*.xml";
              
                if (pSFD.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(Application.StartupPath + "\\..\\res\\xml\\coorTrancfg.xml", pSFD.FileName, true);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(pSFD.FileName);
                    XmlNode xn1 = xmlDoc.FirstChild.NextSibling;
                    XmlNode xn2 = xn1.FirstChild;
                    XmlNode xn3 = xn2.FirstChild;
                    xn3.Attributes[0].Value = txtCtrlPtPath.Text;
                    XmlNode xn3_2 = xn3.NextSibling;
                    xn3_2.Attributes[0].Value = cboxSrcType.SelectedItem.ToString();
                    XmlNode xn3_3 = xn3_2.NextSibling;
                    xn3_3.Attributes[0].Value = txtSrcPath.Text;
                    XmlNode xn3_4 = xn3_3.NextSibling;
                    xn3_4.Attributes[0].Value = txtToPath.Text;

                    xmlDoc.Save(pSFD.FileName);
                    xmlDoc = null;
                }
            }
            catch
            {
 
            }
        }

        private void cboxSrcType_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void txtCtrlPtPath_Enter(object sender, EventArgs e)
        {
            if ((sender as DevComponents.DotNetBar.Controls.TextBoxX).Name == "txtCtrlPtPath"
                && (sender as DevComponents.DotNetBar.Controls.TextBoxX).Text == "打开一个控制点文件（txt）")
            {
                txtCtrlPtPath.Text = "";
                txtCtrlPtPath.ForeColor = Color.Black;
            }
            if ((sender as DevComponents.DotNetBar.Controls.TextBoxX).Name == "txtSrcPath"
                && (sender as DevComponents.DotNetBar.Controls.TextBoxX).Text == "源数据工作空间路径")
            {
                txtSrcPath.Text = "";
                txtSrcPath.ForeColor = Color.Black;
            }
            if ((sender as DevComponents.DotNetBar.Controls.TextBoxX).Name == "txtToPath"
                && (sender as DevComponents.DotNetBar.Controls.TextBoxX).Text == "转换后的数据的工作空间路径")
            {
                txtToPath.Text = "";
                txtToPath.ForeColor = Color.Black;
            }
        }

        private void txtCtrlPtPath_Leave(object sender, EventArgs e)
        {
            if ((sender as DevComponents.DotNetBar.Controls.TextBoxX).Name == "txtCtrlPtPath"
                && (sender as DevComponents.DotNetBar.Controls.TextBoxX).Text == "")
            {
                txtCtrlPtPath.Text = "打开一个控制点文件（txt）";
                txtCtrlPtPath.ForeColor = Color.Gray;
            }
            if ((sender as DevComponents.DotNetBar.Controls.TextBoxX).Name == "txtSrcPath"
                && (sender as DevComponents.DotNetBar.Controls.TextBoxX).Text == "")
            {txtSrcPath.Text = "源数据工作空间路径";
                txtSrcPath.ForeColor=Color.Gray;
            }
            if ((sender as DevComponents.DotNetBar.Controls.TextBoxX).Name == "txtToPath"
                && (sender as DevComponents.DotNetBar.Controls.TextBoxX).Text == "")
            {txtToPath.Text = "转换后的数据的工作空间路径";
                txtToPath.ForeColor=Color.Gray;
             }
        }

        private void frmRdCtrlPt_Load(object sender, EventArgs e)
        {
            cboxSrcType.SelectedIndex = 0;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Height = 343;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (lstViewResult.Items.Count == 0)
                return;
            SaveFileDialog pSFD = new SaveFileDialog();
            pSFD.Filter = "文本文件(.txt)|*.txt";
            if (pSFD.ShowDialog() == DialogResult.OK)
            {
                Stream tmpStream = pSFD.OpenFile();
                StreamWriter sw = new StreamWriter(tmpStream);
                for (int i = 0; i < lstViewResult.Items.Count; i++)
                {
                    sw.WriteLine(lstViewResult.Items[i].Text + "\t" + lstViewResult.Items[i].SubItems[1].Text);
                }
                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
                sw.Close();
                tmpStream.Close();
                pSFD.Dispose();
            }
        }

        private void rdoSHP_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoSHP.Checked)
            {
                txtSrcPath.Text = "";
                txtToPath.Text = "";
            }
        }

        private void rdoMDB_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoMDB.Checked)
            {
                txtSrcPath.Text = "";
                txtToPath.Text = "";
            }
        }

        private void rdoGDB_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdoGDB.Checked)
            {
                txtSrcPath.Text = "";
                txtToPath.Text = "";
            }
        }

     


    }
}