using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace ElementCommandTool
{
    [Guid("3EE01858-4CD3-4a8f-B974-ABD4000DEB1F")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ElementCommandTool.AddPictureCommand")]
    public sealed class AddPictureCommand : BaseCommand
    {
        private IHookHelper m_pHookHelper;

        public AddPictureCommand()
        {
            base.m_category = "NJGIS";
            base.m_caption = "插入图片";
            base.m_message = "插入图片";
            base.m_toolTip = "插入图片";
            base.m_name = "NJ_AddPicture";

            try
            {
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_pHookHelper == null)
                m_pHookHelper = new HookHelperClass();

            m_pHookHelper.Hook = hook;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_pHookHelper.PageLayout == null) return;
            InsertPicture();
        }

        #endregion

        private void InsertPicture()
        {
            try
            {
                OpenFileDialog filedlg = new OpenFileDialog();
                filedlg.Filter = "图像文件 (*.jpg;*.bmp;*.gif;*.tif;*.emf)|*.jpg;*.bmp;*.gif;*.tif;*.emf";
                filedlg.Title = "选择图像文件";

                if (filedlg.ShowDialog() == DialogResult.OK)
                {
                    string sPath = filedlg.FileName;
                    int pos = sPath.LastIndexOf('.');
                    string sExt = sPath.Substring(pos + 1);

                    IPictureElement3 pPicElement = null;
                    switch (sExt.ToLower())
                    {
                        case "jpg":
                            pPicElement = new JpgPictureElementClass();
                            break;
                        case "bmp":
                            pPicElement = new BmpPictureElementClass();
                            break;
                        case "gif":
                            pPicElement = new GifPictureElementClass();
                            break;
                        case "tif":
                            pPicElement = new TifPictureElementClass();
                            break;
                        case "emf":
                            pPicElement = new EmfPictureElementClass();
                            break;
                        default:
                            break;
                    }
                    if (pPicElement == null) return;
                    pPicElement.ImportPictureFromFile(sPath);
                    pPicElement.SavePictureInDocument = true;
                    pPicElement.MaintainAspectRatio = true;

                    IElementProperties pElementProp = (IElementProperties)pPicElement;
                    pElementProp.Name = sPath;

                    double picWidth = 0, picHeight = 0;
                    pPicElement.QueryIntrinsicSize(ref picWidth, ref picHeight);  //得到图片的象素点

                    //象素点转换成厘米
                    picWidth = picWidth / 37.79;
                    picHeight = picHeight / 37.79;

                    //将厘米转换为当前Page单位
                    ConvertUnit(m_pHookHelper.PageLayout.Page.Units, ref picWidth, ref picHeight);

                    IPolygon pPoly = new PolygonClass();
                    IPointCollection pPntCln = (IPointCollection)pPoly;
                    object obj = System.Type.Missing;
                    IPoint pTmpPoint = new PointClass();
                    pTmpPoint.PutCoords(0, 0);
                    pPntCln.AddPoint(pTmpPoint, ref obj, ref obj);

                    pTmpPoint = new PointClass();
                    pTmpPoint.PutCoords(picWidth, 0);
                    pPntCln.AddPoint(pTmpPoint, ref obj, ref obj);

                    pTmpPoint = new PointClass();
                    pTmpPoint.PutCoords(picWidth, picHeight);
                    pPntCln.AddPoint(pTmpPoint, ref obj, ref obj);

                    pTmpPoint = new PointClass();
                    pTmpPoint.PutCoords(0, picHeight);
                    pPntCln.AddPoint(pTmpPoint, ref obj, ref obj);

                    pTmpPoint = new PointClass();
                    pTmpPoint.PutCoords(0, 0);
                    pPntCln.AddPoint(pTmpPoint, ref obj, ref obj);

                    double pageWidth, pageHeight;
                    m_pHookHelper.PageLayout.Page.QuerySize(out pageWidth, out pageHeight);

                    ITransform2D pTrans = (ITransform2D)pPoly;
                    pTrans.Move(pageWidth / 2 - picWidth / 2, pageHeight / 2 - picHeight / 2);

                    IElement pElement = null;
                    pElement = (IElement)pPicElement;
                    pElement.Geometry = pPoly;

                    IGraphicsContainer pContainer = (IGraphicsContainer)m_pHookHelper.PageLayout;
                    IGraphicsContainerSelect pGraphicsSel = pContainer as IGraphicsContainerSelect;
                    pGraphicsSel.UnselectAllElements();
                    pGraphicsSel.SelectElement(pElement);
                    pContainer.AddElement(pElement, 0);
                    m_pHookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pElement, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("插入图片失败：" + ex.Message, "提示");
            }
        }

        //将厘米转换为其他单位
        private void ConvertUnit(esriUnits units, ref double x, ref double y)
        {
            switch (units)
            {
                case esriUnits.esriDecimeters:
                    x /= 10;
                    y /= 10;
                    break;
                case esriUnits.esriFeet:
                    x /= 30.48;
                    y /= 30.48;
                    break;
                case esriUnits.esriInches:
                    x /= 2.54;
                    y /= 2.54;
                    break;
                case esriUnits.esriKilometers:
                    x /= 100000;
                    y /= 100000;
                    break;
                case esriUnits.esriMeters:
                    x /= 100;
                    y /= 100;
                    break;
                case esriUnits.esriMiles:
                    x /= 160934.4;
                    y /= 160934.4;
                    break;
                case esriUnits.esriMillimeters:
                    x *= 10;
                    y *= 10;
                    break;
                case esriUnits.esriNauticalMiles:
                    x /= 185200;
                    y /= 185200;
                    break;
                case esriUnits.esriPoints:
                    x *= 28.346456;
                    y *= 28.346456;
                    break;
                case esriUnits.esriYards:
                    x /= 91.44;
                    y /= 91.44;
                    break;
                default:
                    break;
            }
        }
    }
}
