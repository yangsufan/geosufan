using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoDataCenterFunLib
{
    public partial class frmPolygonToTxt : DevComponents.DotNetBar.Office2007Form
    {
        public frmPolygonToTxt()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            IPolygon pGon = null;
            List<string> listpoint = new List<string>();
            string path = textBoxX.Text;
            string pathtxt=textBoxY.Text;
            string pathlog = Application.StartupPath + "\\..\\Log";

            //判断文件是否存在  不存在就创建
            if (!Directory.Exists(pathlog))
            {
                System.IO.DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(pathlog);
                pathlog += "\\TransLog.txt";
                System.IO.FileStream pFileStream = System.IO.File.Create(pathlog);
            }
            else
            {
                pathlog += "\\TransLog.txt";
                if (!File.Exists(pathlog))
                {
                    System.IO.FileStream pFileStream = System.IO.File.Create(pathlog);
                    pFileStream.Close();
                }
            }

            if(path.Trim()=="")
            {
                MessageBox.Show("请选择shp文件路径","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (pathtxt.Trim() == "")
            {
                MessageBox.Show("请选择坐标文件存放路径","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            IWorkspaceFactory pwf = new ShapefileWorkspaceFactoryClass();
            string filepath = System.IO.Path.GetDirectoryName(path);
            string filename = path.Substring(path.LastIndexOf("\\") + 1);
            IFeatureWorkspace pFeatureworkspace = (IFeatureWorkspace)pwf.OpenFromFile(filepath, 0);
            IFeatureClass pFeatureclass = pFeatureworkspace.OpenFeatureClass(filename);
            //System.IO.FileStream fs = new System.IO.FileStream(pathtxt, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //fs.Close();
            StreamWriter sw = new StreamWriter(pathtxt, false, Encoding.GetEncoding("gb2312"));
            StreamWriter swlog = new StreamWriter(pathlog, true, Encoding.GetEncoding("gb2312"));
            string strTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.ToLongTimeString();
            swlog.WriteLine(strTime+"-->从SHP面文件转换坐标串文本文件");//记录日志
            swlog.WriteLine("源文件："+path+",目标文件："+pathtxt);
            if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                IFeature pFeature = pCursor.NextFeature();
                while (pFeature != null)
                {
                    pGon = pFeature.Shape as IPolygon;
                    if (pGon == null) continue;
                    ITopologicalOperator iTop = pGon as ITopologicalOperator;
                    if (!iTop.IsSimple) //有自相交，出错记录日志
                    {
                        swlog.WriteLine("转换错误：图层" + pFeatureclass.AliasName + "要素ID为" + pFeature.OID + "有错误");
                        swlog.Close();
                        MessageBox.Show("转换出错，详细日志查看"+pathlog, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (!pGon.IsClosed) //不闭合 ，出错记录日志
                    {
                        swlog.WriteLine("转换错误：图层" + pFeatureclass.AliasName + "要素ID为" + pFeature.OID + "有不闭合");
                        swlog.Close();
                        MessageBox.Show("转换出错，详细日志查看" + pathlog, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    listpoint = PolygonToString(pGon as IPolygon4);
                    for (int i = 0; i < listpoint.Count; i++)
                    {
                        if (i == listpoint.Count - 1)
                        {
                            sw.WriteLine(listpoint[i].Substring(0,listpoint[i].Length-2)+"D;");
                        }
                        else
                        {
                            sw.WriteLine(listpoint[i]);
                        }
                    }
                    pFeature = pCursor.NextFeature();
                }
            }
            //else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
            //{
            //    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
            //    IFeature pFeature = pCursor.NextFeature();
            //    if (pFeature != null)
            //    {

            //        IPolyline pPolyline = pFeature.Shape as IPolyline;
            //        pGon = GetPolygonFormLine(pPolyline);
            //        if (pGon.IsClosed == false)
            //        {
            //            MessageBox.Show("选择的线要素不能构成封闭多边形！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //            return null;
            //        }
            //    }
            //}
            else
            {
                MessageBox.Show("请选择一个面要素文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sw.Close();
            sw = null;
            MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            swlog.WriteLine("转换成功");
            swlog.Close();

        }

        private void btn_SelTxt_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "文本文件|*.txt";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            textBoxY.Text = dlg.FileName;
        }

        private void btn_SelShp_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "shp数据|*.shp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            textBoxX.Text = dlg.FileName;
        }

        public List<string> PolygonToString(IPolygon4 polygon)
        {

            List<string> list = new List<string>();
            IGeometryBag exteriorRingGeometryBag = polygon.ExteriorRingBag;
            IGeometryCollection exteriorRingGeometryCollection = exteriorRingGeometryBag as IGeometryCollection;
            for (int i = 0; i < exteriorRingGeometryCollection.GeometryCount; i++)
            {
                IGeometry exteriorRingGeometry = exteriorRingGeometryCollection.get_Geometry(i);
                
                IGeometryBag interiorRingGeometryBag = polygon.get_InteriorRingBag(exteriorRingGeometry as IRing);
                IGeometryCollection interiorRingGeometryCollection = interiorRingGeometryBag as IGeometryCollection; ;
                for (int k = 0; k < interiorRingGeometryCollection.GeometryCount; k++)
                {
                    IGeometry interiorRingGeometry = interiorRingGeometryCollection.get_Geometry(k);
                    IPointCollection interiorRingPointCollection = interiorRingGeometry as IPointCollection;
                    for (int m = 0; m < interiorRingPointCollection.PointCount; m++)
                    {
                        if (m == interiorRingPointCollection.PointCount - 1)
                        {
                            list.Add(PointToString(interiorRingPointCollection.get_Point(m)) + "-;");
                        }
                        else
                        {
                            list.Add(PointToString(interiorRingPointCollection.get_Point(m))+";");
                        }
                    }

                }

                IPointCollection exteriorRingPointCollection = exteriorRingGeometry as IPointCollection;
                for (int j = 0; j < exteriorRingPointCollection.PointCount; j++)
                {
                    if (j == exteriorRingPointCollection.PointCount - 1)
                    {
                        list.Add(PointToString(exteriorRingPointCollection.get_Point(j)) + "+;");
                    }
                    else
                    {
                        list.Add(PointToString(exteriorRingPointCollection.get_Point(j))+";");
                    }
                }

            }
            return list;

        }

        private string PointToString(IPoint point)
        {

            return (point.X + ";" + point.Y + ";");

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
