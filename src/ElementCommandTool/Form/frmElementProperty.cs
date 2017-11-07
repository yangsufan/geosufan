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
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace ElementCommandTool
{
    public partial class frmElementProperty : DevComponents.DotNetBar.Office2007Form
    {
        private IElement m_pOrgElement;
        private IBoundsProperties m_pBoundProp;
        private ITransform2D m_pTransform;
        private IDisplay m_pDisplay;
        private IGraphicsContainer m_pGraphicCtn;
        private IActiveView m_pActiveView;
        private IPageLayoutControl m_pPageLayoutControl;

        public string LowerLeftX
        {
            set { this.numLowerLeftX.Text = value; }
            get { return this.numLowerLeftX.Text.ToString(); }
        }

        public string NumHeight
        {
            set { this.numHeight.Text = value; }
            get { return this.numHeight.Text.ToString(); }
        }

        public string NumWidth
        {
            set { this.numWidth.Text = value; }
            get { return this.numWidth.Text.ToString(); }
        }

        //调用类型//考虑是在制图的过程中添加，还是在设计模板的时候添加
        /// <summary>
        /// 1:设计模板时，2：制图时
        /// </summary>
        private int m_AddMode;

        public frmElementProperty(int AddMode)
        {
            InitializeComponent();
            //初始化时，向类型中添加变量类型和常量类型
            m_AddMode = AddMode;
            if (m_AddMode > 2 || m_AddMode < 1)
                m_AddMode = 2;//默认的时候为制图类型
            if (m_AddMode == 2)
                this.GroupType.Visible = false;
        }

        public IPageLayoutControl set_PageLayoutControl
        {
            set
            {
                m_pPageLayoutControl = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (true)
            {
                if (SetElementProp())
                {
                    this.Hide();
                }
            }
            else
            {
                this.Hide();
            }
        }

        private void btnCannel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        bool applyed = false;
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (SetElementProp())
            {
                applyed = true;
            }
            else
            {
                applyed = false;
            }
        }

        public void StartEditElement(IActiveView pView, ref IElement pElement)
        {
            if (pElement == null) return;
            m_pOrgElement = pElement;
            m_pBoundProp = (IBoundsProperties)m_pOrgElement;
            m_pTransform = (ITransform2D)m_pOrgElement;

            m_pActiveView = pView;
            m_pDisplay = pView.ScreenDisplay;
            m_pGraphicCtn = pView.GraphicsContainer;
            SetUI(m_pOrgElement);
            GetProperty(m_pDisplay, m_pOrgElement);
            //this.ShowDialog();
        }
        private string ReadRegistry(string sKey)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sKey, true);
            if (rk == null) return "";

            return (string)rk.GetValue("InstallDir");
        }
        private void SetUI(IElement pElement)
        {
            tabProperty.Tabs["tabFrame"].Visible = false;
            chkRatio.Checked = m_pBoundProp.FixedAspectRatio;
            if (pElement is IMarkerElement)
            {
                //panelLine.Visible = false;
                //panelPoly.Visible = false;
                panelText.Visible = false;
                picLine.Visible = false;
                picPoly.Visible = false;
                picPoint.Visible = true;

                btnLine.Visible = false;
                btnPoint.Visible = true;
                btnPoly.Visible = false;

                picArrow.Visible = false;
                picScale.Visible = false;
                picScaleText.Visible = false;

                btnArrow.Visible = false;
                btnScale.Visible = false;
                btnScaleText.Visible = false;
                //panelPoint.Visible = true;
                //panelPoint.BringToFront ();
                picPoint.BringToFront();
                tabProperty.Tabs["tabArea"].Visible = false;
                chkRatio.Enabled = false;
            }
            else if (pElement is ILineElement)
            {
                panelText.Visible = false;
                picLine.Visible = true;
                picPoly.Visible = false;
                picPoint.Visible = false;

                btnLine.Visible = true;
                btnPoint.Visible = false;
                btnPoly.Visible = false;

                picArrow.Visible = false;
                picScale.Visible = false;
                picScaleText.Visible = false;

                btnArrow.Visible = false;
                btnScale.Visible = false;
                btnScaleText.Visible = false;

                picLine.BringToFront();
                tabProperty.Tabs["tabArea"].Visible = false;
                chkRatio.Enabled = true;
            }
            else if (pElement is IFillShapeElement)
            {
                panelText.Visible = false;
                picLine.Visible = false;
                picPoly.Visible = true;
                picPoint.Visible = false;

                btnLine.Visible = false;
                btnPoint.Visible = false;
                btnPoly.Visible = true;

                picArrow.Visible = false;
                picScale.Visible = false;
                picScaleText.Visible = false;

                btnArrow.Visible = false;
                btnScale.Visible = false;
                btnScaleText.Visible = false;

                picPoly.BringToFront();
                chkRatio.Enabled = true;
            }
            else if (pElement is ITextElement)
            {
                panelText.Visible = true;
                picLine.Visible = false;
                picPoly.Visible = false;
                picPoint.Visible = false;

                btnLine.Visible = false;
                btnPoint.Visible = false;
                btnPoly.Visible = false;
                
                picArrow.Visible = false;
                picScale.Visible = false;
                picScaleText.Visible = false;

                btnArrow.Visible = false;
                btnScale.Visible = false;
                btnScaleText.Visible = false;

                panelText.BringToFront();
                tabProperty.Tabs["tabArea"].Visible = false;
                chkRatio.Enabled = false;
            }
            else if (pElement is IInkGraphic)
            {
                tabProperty.Tabs["tabArea"].Visible = false;
                tabProperty.Tabs["tabSymbol"].Visible = false;
            }

            if (pElement is IFrameElement)
            {
                tabProperty.Tabs["tabArea"].Visible = false;
                tabProperty.Tabs["tabSymbol"].Visible = false;
                //判断指北针,比例尺,比例文本
                IMapSurroundFrame pMapSurroundFrame = pElement as IMapSurroundFrame;
                if (pMapSurroundFrame != null)
                {
                    IMapSurround pMapSurround = pMapSurroundFrame.MapSurround;
                    if (pMapSurround != null)
                    {
                        tabProperty.Tabs["tabSymbol"].Visible = true;
                        panelText.Visible = false;
                        if (pMapSurround is INorthArrow)
                        {
                            panelText.Visible = false;
                            picLine.Visible = false;
                            picPoly.Visible = false;
                            picPoint.Visible = false;

                            btnLine.Visible = false;
                            btnPoint.Visible = false;
                            btnPoly.Visible = false;

                            picArrow.Visible = true;
                            picScale.Visible = false;
                            picScaleText.Visible = false;

                            btnArrow.Visible = true;
                            btnScale.Visible = false;
                            btnScaleText.Visible = false;

                            picArrow.BringToFront();
                            chkRatio.Enabled = true;
                        }
                        else if (pMapSurround is IScaleBar)
                        {
                            panelText.Visible = false;
                            picLine.Visible = false;
                            picPoly.Visible = false;
                            picPoint.Visible = false;

                            btnLine.Visible = false;
                            btnPoint.Visible = false;
                            btnPoly.Visible = false;

                            picArrow.Visible = false;
                            picScale.Visible = true;
                            picScaleText.Visible = false;

                            btnArrow.Visible = false;
                            btnScale.Visible = true;
                            btnScaleText.Visible = false;

                            picScale.BringToFront();
                            chkRatio.Enabled = true;
                        }
                        else if (pMapSurround is IScaleText)
                        {
                            panelText.Visible = false;
                            picLine.Visible = false;
                            picPoly.Visible = false;
                            picPoint.Visible = false;

                            btnLine.Visible = false;
                            btnPoint.Visible = false;
                            btnPoly.Visible = false;

                            picArrow.Visible = false;
                            picScale.Visible = false;
                            picScaleText.Visible = true;

                            btnArrow.Visible = false;
                            btnScale.Visible = false;
                            btnScaleText.Visible = true;

                            picScaleText.BringToFront();
                            chkRatio.Enabled = true;
                        }
                    }
                }
                else if(pElement is IMapFrame)
                {
                    tabProperty.Tabs["tabFrame"].Visible = true;
                }
                //初始化窗体
                //Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\ESRI\\CoreRuntime", true);
                //if (rk == null) return;

                //string sInstall = (string)rk.GetValue("InstallDir");
                string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
                if (sInstall == "") //added by chulili 2012-11-13 平台由ArcGIS9.3换成ArcGIS10，相应的注册表路径要修改
                {
                    sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
                }
                if (sInstall == "")
                {
                    sInstall = ReadRegistry("SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
                }   //added by chulili 2012-11-13  end
                if (sInstall == "")
                {
                    MessageBox.Show("系统没有安装Engine Runtime", "提示");
                    return;
                }

                string sPath = sInstall + "\\Styles\\ESRI.ServerStyle";

                axSymbologyControl1.Clear();
                axSymbologyControl1.LoadStyleFile(sPath);

                //添加边框符号
                IStyleGalleryItem pItem;
                stdole.IPictureDisp picture;
                System.Drawing.Image image;
                DevComponents.Editors.ComboItem valueItem;
                int i = 0;

                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassBorders;
                ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
                int cnt = symbologyStyleClass.get_ItemCount(symbologyStyleClass.StyleCategory);
                cmbBorder.Items.Clear();
                cmbBorder.Items.Add("<None>");
                for (i = 0; i < cnt; i++)
                {
                    pItem = symbologyStyleClass.GetItem(i);
                    picture = symbologyStyleClass.PreviewItem(pItem, cmbBorder.Width - 80, cmbBorder.Height);
                    image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                    valueItem = new DevComponents.Editors.ComboItem();
                    valueItem.Text = pItem.Name;
                    valueItem.Tag = pItem.Item;
                    valueItem.Image = image;
                    cmbBorder.Items.Add(valueItem);
                }

                //添加背景符号
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassBackgrounds;
                symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
                cnt = symbologyStyleClass.get_ItemCount(symbologyStyleClass.StyleCategory);
                cmbBack.Items.Clear();
                cmbBack.Items.Add("<None>");
                for (i = 0; i < cnt; i++)
                {
                    pItem = symbologyStyleClass.GetItem(i);
                    picture = symbologyStyleClass.PreviewItem(pItem, cmbBack.Width - 80, cmbBack.Height);
                    image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                    valueItem = new DevComponents.Editors.ComboItem();
                    valueItem.Text = pItem.Name;
                    valueItem.Tag = pItem.Item;
                    valueItem.Image = image;
                    cmbBack.Items.Add(valueItem);
                }

                //添加阴影符号
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassShadows;
                symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
                cnt = symbologyStyleClass.get_ItemCount(symbologyStyleClass.StyleCategory);
                cmbShadow.Items.Clear();
                cmbShadow.Items.Add("<None>");
                for (i = 0; i < cnt; i++)
                {
                    pItem = symbologyStyleClass.GetItem(i);
                    picture = symbologyStyleClass.PreviewItem(pItem, cmbShadow.Width - 80, cmbShadow.Height);
                    image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                    valueItem = new DevComponents.Editors.ComboItem();
                    valueItem.Text = pItem.Name;
                    valueItem.Tag = pItem.Item;
                    valueItem.Image = image;
                    cmbShadow.Items.Add(valueItem);
                }
            }
        }

        //支持6中element，点，线，面，注记，Inkgraphic，MapFrame
        private bool GetProperty(IDisplay pDisplay, IElement pElement)
        {
            try
            {
                //获得ＦｒａｍｅＥＬＥｍｅｎｔ的属性
                if (pElement is IFrameElement)
                {
                    IFrameProperties pFrameProperty = (IFrameProperties)pElement;
                    IFrameDecoration pFrameDecoration = null;
                    IClone pClone;
                    int i;
                    bool bExist = false;

                    //得到地图边框属性
                    if (pFrameProperty.Border != null)
                    {
                        pFrameDecoration = (IFrameDecoration)pFrameProperty.Border;
                        //判断和列表框中的符号是否相同
                        bExist = false;
                        for (i = 0; i < cmbBorder.Items.Count; i++)
                        {
                            pClone = (IClone)pFrameProperty.Border;
                            DevComponents.Editors.ComboItem item = cmbBorder.Items[i] as DevComponents.Editors.ComboItem;
                            if (item == null) continue;
                            if (pClone.Equals(item.Tag))
                            {
                                bExist = true;
                                break;
                            }
                        }
                        if (bExist)
                            cmbBorder.SelectedIndex = i;
                        else
                            PreViewCustom(cmbBorder, pFrameProperty.Border, esriSymbologyStyleClass.esriStyleClassBorders);

                        GetDecorationColor(pFrameDecoration, btnBorderColor);
                        numBorderGapX.Value = (decimal)pFrameDecoration.HorizontalSpacing;
                        numBorderGapY.Value = (decimal)pFrameDecoration.VerticalSpacing;
                        numBorderRound.Value = (decimal)pFrameDecoration.CornerRounding;
                    }

                    //得到地图背景属性
                    if (pFrameProperty.Background != null)
                    {
                        pFrameDecoration = (IFrameDecoration)((IFrameElement)pElement).Background;
                        //判断和列表框中的符号是否相同
                        bExist = false;
                        for (i = 0; i < cmbBack.Items.Count; i++)
                        {
                            pClone = (IClone)pFrameProperty.Background;
                            DevComponents.Editors.ComboItem item = cmbBack.Items[i] as DevComponents.Editors.ComboItem;
                            if (item == null) continue;
                            if (pClone.Equals(item.Tag))
                            {
                                bExist = true;
                                break;
                            }
                        }
                        if (bExist)
                            cmbBack.SelectedIndex = i;
                        else
                            PreViewCustom(cmbBack, pFrameProperty.Background, esriSymbologyStyleClass.esriStyleClassBackgrounds);

                        GetDecorationColor(pFrameDecoration, btnBackColor);

                        numBackGapX.Value = (decimal)pFrameDecoration.HorizontalSpacing;
                        numBackGapY.Value = (decimal)pFrameDecoration.VerticalSpacing;
                        numBackRound.Value = (decimal)pFrameDecoration.CornerRounding;
                    }

                    //得到地图阴影属性
                    if (pFrameProperty.Shadow != null)
                    {
                        pFrameDecoration = (IFrameDecoration)pFrameProperty.Shadow;
                        //判断和列表框中的符号是否相同
                        bExist = false;
                        for (i = 0; i < cmbShadow.Items.Count; i++)
                        {
                            pClone = (IClone)pFrameProperty.Shadow;
                            DevComponents.Editors.ComboItem item = cmbShadow.Items[i] as DevComponents.Editors.ComboItem;
                            if (item == null) continue;
                            if (pClone.Equals(item.Tag))
                            {
                                bExist = true;
                                break;
                            }
                        }
                        if (bExist)
                            cmbShadow.SelectedIndex = i;
                        else
                            PreViewCustom(cmbShadow, pFrameProperty.Shadow, esriSymbologyStyleClass.esriStyleClassShadows);

                        GetDecorationColor(pFrameDecoration, btnShadowColor);

                        numShadowGapX.Value = (decimal)pFrameDecoration.HorizontalSpacing;
                        numShadowGapY.Value = (decimal)pFrameDecoration.VerticalSpacing;
                        numShadowRound.Value = (decimal)pFrameDecoration.CornerRounding;
                    }
                }
                IElementProperties3 pElementProp = pElement as IElementProperties3;
                txtName.Text = pElementProp.Name;
                IEnvelope pBounds = new EnvelopeClass();
                pElement.QueryBounds(pDisplay, pBounds);
                IArea pArea = (IArea)pBounds;
                numCenterX.Text = pArea.Centroid.X.ToString("f4");
                numCenterY.Text = pArea.Centroid.Y.ToString("f4");
                numHeight.Text = pBounds.Height.ToString("f4");
                numWidth.Text = pBounds.Width.ToString("f4");
                numLowerLeftX.Text = pBounds.LowerLeft.X.ToString("f4");
                numLowerLeftY.Text = pBounds.LowerLeft.Y.ToString("f4");

                if (pElement is IMarkerElement)
                {
                    IMarkerElement pMrkElement = pElement as IMarkerElement;
                    picPoint.Tag = pMrkElement.Symbol;
                    PreViewSymbol((ISymbol)picPoint.Tag, ref picPoint);
                }
                else if (pElement is ILineElement)
                {
                    ILineElement pLinElement = pElement as ILineElement;
                    picLine.Tag = pLinElement.Symbol;
                    PreViewSymbol((ISymbol)picLine.Tag, ref picLine);
                }
                else if (pElement is IFillShapeElement)
                {
                    IFillShapeElement pFillElement = pElement as IFillShapeElement;
                    picPoly.Tag = pFillElement.Symbol;
                    PreViewSymbol((ISymbol)picPoly.Tag, ref picPoly);

                    //得到面积
                    pArea = (IArea)pElement.Geometry;
                    numArea.Text = pArea.Area.ToString("0.00");
                    //得到周长
                    IPolygon pPolygon = new PolygonClass();
                    pElement.QueryOutline(m_pDisplay, pPolygon);
                    IRing pOutRing = new RingClass();
                    pPolygon.QueryExteriorRings(ref pOutRing);
                    numLength.Text = pOutRing.Length.ToString("0.00");
                }
                else if (pElement is ITextElement)
                {
                    ITextElement pTxtElement = pElement as ITextElement;
                    txtText.Text = pTxtElement.Text;
                    picText.Tag = pTxtElement.Symbol;
                    PreViewSymbol((ISymbol)picText.Tag, ref picText);
                }
                if (pElement is IFrameElement)
                {
                    //判断指北针,比例尺,比例文本
                    IMapSurroundFrame pMapSurroundFrame = pElement as IMapSurroundFrame;
                    if (pMapSurroundFrame != null)
                    {
                        IMapSurround pMapSurround = pMapSurroundFrame.MapSurround;
                        if (pMapSurround != null)
                        {
                            if (pMapSurround is INorthArrow)
                            {
                                picArrow.Tag = pMapSurround;
                                PreViewSymbol(pMapSurround, ref picArrow);
                            }
                            else if (pMapSurround is IScaleBar)
                            {
                                picScale.Tag = pMapSurround;
                                PreViewSymbol(pMapSurround, ref picScale);
                            }
                            else if (pMapSurround is IScaleText)
                            {
                                picScaleText.Tag = pMapSurround;
                                PreViewSymbol(pMapSurround, ref picScaleText);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("显示属性出错:" + ex.Message);
                return false;
            }

        }

        private void PreViewCustom(DevComponents.DotNetBar.Controls.ComboBoxEx cmb, object item, esriSymbologyStyleClass styleClass)
        {
            IStyleGalleryItem pStyleItem = new ServerStyleGalleryItemClass();
            pStyleItem.Name = "Custom";
            pStyleItem.Item = item;
            stdole.IPictureDisp picture;
            System.Drawing.Image image;

            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(styleClass);
            picture = symbologyStyleClass.PreviewItem(pStyleItem, cmb.Width - 80, cmb.Height);
            image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));

            DevComponents.Editors.ComboItem valueItem;
            valueItem = new DevComponents.Editors.ComboItem();
            valueItem.Text = "Custom";
            valueItem.Tag = item;
            valueItem.Image = image;
            cmb.Items.Add(valueItem);
            cmb.SelectedIndex = cmb.Items.Count - 1;
        }

        //获取颜色
        private void GetDecorationColor(IFrameDecoration pFrameDecoration, Button btn)
        {
            try
            {
                IRgbColor pRgbColor = null;
                Color pColor;

                pRgbColor = (IRgbColor)pFrameDecoration.Color;
                pColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);
                btn.BackColor = pColor;
            }
            catch
            {
                btn.BackColor = this.BackColor;
            }
        }

        //选择颜色
        private Color GetColor(Color pColor)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.AnyColor = true;
            colorDlg.Color = pColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                return colorDlg.Color;
            }
            else
            {
                return pColor;
            }
        }

        private bool PreViewSymbol(object pSymbol, ref System.Windows.Forms.PictureBox btn)
        {
            btn.Image = Symbol2Picture(pSymbol, btn.Width, btn.Height);
            return true;
        }

        private System.Drawing.Image Symbol2Picture(object pSymbol, int width, int height)
        {
            if (pSymbol == null || width < 1 || height < 1) return null;
            if (pSymbol is IMarkerSymbol)
            {
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
            }
            else if (pSymbol is ILineSymbol)
            {
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassLineSymbols;
            }
            else if (pSymbol is IFillSymbol)
            {
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassFillSymbols;
            }
            else if (pSymbol is ITextSymbol)
            {
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassTextSymbols;
            }
            else if (pSymbol is INorthArrow)
            {
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassNorthArrows;
            }
            else if (pSymbol is IScaleBar)
            {
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassScaleBars;
            }
            else if (pSymbol is IScaleText)
            {
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassScaleTexts;
            }

            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
            symbologyStyleClass.RemoveAll();
            IStyleGalleryItem pStyleItem = new ServerStyleGalleryItemClass();

            pStyleItem.Name = "tempSymbol";
            pStyleItem.Item = pSymbol; 
            //symbologyStyleClass.AddItem(pStyleItem, 0);
            stdole.IPictureDisp picture = symbologyStyleClass.PreviewItem(pStyleItem, width, height);
            if (pSymbol is IScaleBar)
            {
                picture = symbologyStyleClass.PreviewItem(pStyleItem, width*2, height);
            }
            else if (pSymbol is IScaleText)
            {
                picture = symbologyStyleClass.PreviewItem(pStyleItem, width * 2, height);
            }
            System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
            return image;
        }

        public bool SetElementProp()
        {
            try
            {
                //if (txtName.Text.Trim() == "")
                //{
                //    MessageBox.Show("请输入变量名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);yjl0616
                //    return false;
                //}
                IElementProperties3 pElementProp = m_pOrgElement as IElementProperties3;

                if (pElementProp!=null)
                {
                    pElementProp.Name = txtName.Text;
                }

                //对于ＦｒａｍｅＥｌｅｍｅｎｔ的情况
                if (m_pOrgElement is IFrameElement)
                {
                    IFrameProperties pFrameProperty = null;
                    IFrameDecoration pFrameDecoration = null;
                    DevComponents.Editors.ComboItem item=cmbBorder.SelectedItem as DevComponents.Editors.ComboItem;
                    if (cmbBorder.SelectedIndex != -1 && item!= null)
                    {
                        IBorder pBorder = (IBorder)(item.Tag);
                        pFrameDecoration = (IFrameDecoration)pBorder;
                        pFrameDecoration.HorizontalSpacing = (double)numBorderGapX.Value;
                        pFrameDecoration.VerticalSpacing = (double)numBorderGapY.Value;
                        pFrameDecoration.CornerRounding = (short)numBorderRound.Value;
                        ((IFrameElement)m_pOrgElement).Border = pBorder;
                    }
                    else
                    {
                        ((IFrameElement)m_pOrgElement).Border = null;
                    }
                    item = cmbBack.SelectedItem as DevComponents.Editors.ComboItem;
                    if (cmbBack.SelectedIndex != -1 && item != null)
                    {
                        IBackground pBackground = (IBackground)(item.Tag);
                        pFrameDecoration = (IFrameDecoration)pBackground;
                        pFrameDecoration.HorizontalSpacing = (double)numBackGapX.Value;
                        pFrameDecoration.VerticalSpacing = (double)numBackGapY.Value;
                        pFrameDecoration.CornerRounding = (short)numBackRound.Value;
                        ((IFrameElement)m_pOrgElement).Background = pBackground;
                    }
                    else
                    {
                        ((IFrameElement)m_pOrgElement).Background = null;
                    }
                    item = cmbShadow.SelectedItem as DevComponents.Editors.ComboItem;
                    if (cmbShadow.SelectedIndex != -1 && item != null)
                    {
                        IShadow pShadow = (IShadow)(item.Tag);
                        pFrameDecoration = (IFrameDecoration)pShadow;
                        pFrameDecoration.HorizontalSpacing = (double)numShadowGapX.Value;
                        pFrameDecoration.VerticalSpacing = (double)numShadowGapY.Value;
                        pFrameDecoration.CornerRounding = (short)numShadowRound.Value;
                        pFrameProperty = (IFrameProperties)m_pOrgElement;
                        pFrameProperty.Shadow = pShadow;
                    }
                    else
                    {
                        pFrameProperty = (IFrameProperties)m_pOrgElement;
                        pFrameProperty.Shadow = null;
                    }
                }
                IEnvelope pBounds = new EnvelopeClass();
                m_pOrgElement.QueryBounds(m_pDisplay, pBounds);

                //设置偏移
                double dx, dy;
                IPoint pFromPoint = pBounds.LowerLeft;

                if (chkUseOffDist.Checked)
                {
                    dx = Convert.ToDouble(numLowerLeftX.Text);
                    dy = Convert.ToDouble(numLowerLeftY.Text);
                }
                else
                {
                    dx = Convert.ToDouble(numLowerLeftX.Text) - pFromPoint.X;
                    dy = Convert.ToDouble(numLowerLeftY.Text) - pFromPoint.Y;
                }
                m_pTransform.Move(dx, dy);

                //设置缩放
                pFromPoint = pBounds.LowerLeft;
                if (chkUsePercent.Checked)
                {
                    dx = ((Convert.ToDouble(numWidth.Text) / 100) * pBounds.Width) / pBounds.Width;
                    dy = ((Convert.ToDouble(numHeight.Text) / 100) * pBounds.Width) / pBounds.Height;
                }
                else
                {
                    dx = Convert.ToDouble(numWidth.Text) / pBounds.Width;
                    dy = Convert.ToDouble(numHeight.Text) / pBounds.Height;
                }
                m_pTransform.Scale(pFromPoint, dx, dy);

                //设置符号
                if (m_pOrgElement is IMarkerElement)
                {
                    IMarkerElement pMrkElement = m_pOrgElement as IMarkerElement;
                    IMarkerSymbol pSymbol = (IMarkerSymbol)picPoint.Tag;
                    double dblSize = pMrkElement.Symbol.Size;
                    pMrkElement.Symbol = (IMarkerSymbol)picPoint.Tag;
                    pSymbol.Size = dblSize;
                    pMrkElement.Symbol = pSymbol;
                }
                else if (m_pOrgElement is ILineElement)
                {
                    ILineElement pLinElement = m_pOrgElement as ILineElement;
                    pLinElement.Symbol = (ILineSymbol)picLine.Tag;
                }
                else if (m_pOrgElement is IFillShapeElement)
                {
                    IFillShapeElement pFillElement = m_pOrgElement as IFillShapeElement;
                    pFillElement.Symbol = (IFillSymbol)picPoly.Tag;
                }
                else if (m_pOrgElement is ITextElement)
                {
                    ITextElement pTxtElement = m_pOrgElement as ITextElement;
                    pTxtElement.Text = txtText.Text;
                    pTxtElement.Symbol = (ITextSymbol)picText.Tag;
                }

                if (m_pOrgElement is IFrameElement)
                {
                    //判断指北针,比例尺,比例文本
                    IMapSurroundFrame pMapSurroundFrame = m_pOrgElement as IMapSurroundFrame;
                    if (pMapSurroundFrame != null)
                    {
                        IMapSurround pMapSurround = pMapSurroundFrame.MapSurround;
                        if (pMapSurround != null)
                        {
                            if (pMapSurround is INorthArrow)
                            {
                                pMapSurroundFrame.MapSurround = (IMapSurround)picArrow.Tag;
                                pMapSurroundFrame.MapSurround.Name = "指北针";
                            }
                            else if (pMapSurround is IScaleBar)
                            {
                                pMapSurroundFrame.MapSurround = (IMapSurround)picScale.Tag;
                                pMapSurroundFrame.MapSurround.Name = "比例尺";
                            }
                            else if (pMapSurround is IScaleText)
                            {
                                pMapSurroundFrame.MapSurround = (IMapSurround)picScaleText.Tag;
                                pMapSurroundFrame.MapSurround.Name = "比例尺";
                            }
                        }
                    }
                }

                m_pGraphicCtn.UpdateElement(m_pOrgElement);
                m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                if (chkUseOffDist.Checked)
                {
                    numLowerLeftX.Text = "0";
                    numLowerLeftY.Text = "0";
                }
                if (chkUsePercent.Checked)
                {
                    numHeight.Text = "100";
                    numWidth.Text = "100";
                }
                return true;
            }
            catch
            {
                //MessageBox.Show("保存属性时出现错误，错误描述为:" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //
        }

        private void chkUsePercent_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUsePercent.Checked)
            {
                numHeight.Text = "100";
                numWidth.Text = "100";
            }
            else
            {
                IEnvelope pBound = new EnvelopeClass();
                m_pOrgElement.QueryBounds(m_pDisplay, pBound);
                numHeight.Text = pBound.Height.ToString();
                numWidth.Text = pBound.Width.ToString();
            }
        }

        private void chkUseOffDist_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseOffDist.Checked)
            {
                numLowerLeftX.Text = "0";
                numLowerLeftY.Text = "0";
            }
            else
            {
                IEnvelope pBound = new EnvelopeClass();
                m_pOrgElement.QueryBounds(m_pDisplay, pBound);
                numLowerLeftX.Text = pBound.LowerLeft.X.ToString("f4");
                numLowerLeftY.Text = pBound.LowerLeft.Y.ToString("f4");
            }
        }

        private void numWidth_TextChanged(object sender, EventArgs e)
        {
            if (chkRatio.Checked)
            {
                if (chkUsePercent.Checked)
                {
                    numHeight.Text = numWidth.Text;
                }
                else
                {
                    IEnvelope pBound = new EnvelopeClass();
                    m_pOrgElement.QueryBounds(m_pDisplay, pBound);
                    numHeight.Text = ((Convert.ToDouble(numWidth.Text) / pBound.Width) * pBound.Height).ToString();
                }
            }
        }

        private void numHeight_TextChanged(object sender, EventArgs e)
        {
            if (chkRatio.Checked)
            {
                if (chkUsePercent.Checked)
                {
                    numWidth.Text = numHeight.Text;
                }
                else
                {
                    IEnvelope pBound = new EnvelopeClass();
                    m_pOrgElement.QueryBounds(m_pDisplay, pBound);
                    numWidth.Text = ((Convert.ToDouble(numHeight.Text) / pBound.Height) * pBound.Width).ToString();
                }
            }
        }

        private void btnBorderColor_Click(object sender, EventArgs e)
        {
            btnBorderColor.BackColor = GetColor(btnBorderColor.BackColor);
            ChangeSymbol(cmbBorder, btnBorderColor.BackColor, esriSymbologyStyleClass.esriStyleClassBorders);
        }

        private void btnBackColor_Click(object sender, EventArgs e)
        {
            btnBackColor.BackColor = GetColor(btnBackColor.BackColor);
            ChangeSymbol(cmbBack, btnBackColor.BackColor, esriSymbologyStyleClass.esriStyleClassBackgrounds);
        }

        private void btnShadowColor_Click(object sender, EventArgs e)
        {
            btnShadowColor.BackColor = GetColor(btnShadowColor.BackColor);
            ChangeSymbol(cmbShadow, btnShadowColor.BackColor, esriSymbologyStyleClass.esriStyleClassShadows);
        }

        private void ChangeSymbol(DevComponents.DotNetBar.Controls.ComboBoxEx cmb, Color pColor, esriSymbologyStyleClass styleClass)
        {
            try
            {
                DevComponents.Editors.ComboItem item=cmb.SelectedItem as DevComponents.Editors.ComboItem;
                object obj=item.Tag;
                if (obj== null) return;
                IFrameDecoration pFrameDecoration = (IFrameDecoration)obj;
                IRgbColor pRgbColor = new RgbColorClass();
                pRgbColor.Red = pColor.R;
                pRgbColor.Green = pColor.G;
                pRgbColor.Blue = pColor.B;

                pFrameDecoration.Color = (IColor)pRgbColor;
                IStyleGalleryItem pItem = new ServerStyleGalleryItemClass();
                pItem.Name = "temp";
                pItem.Item = pFrameDecoration;
                stdole.IPictureDisp picture;
                System.Drawing.Image image;

                ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(styleClass);
                picture = symbologyStyleClass.PreviewItem(pItem, cmb.Width - 80, cmb.Height);
                image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                item.Image = image;
            }
            catch
            {
            }
        }

        private void cmbBorder_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem item=cmbBorder.SelectedItem as DevComponents.Editors.ComboItem;
            if (item == null) return;
            IFrameDecoration pFrameDecoration = (IFrameDecoration)(item.Tag);
            GetDecorationColor(pFrameDecoration, btnBorderColor);
        }

        private void cmbBorder_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbBorder.SelectedIndex != -1 && cmbBorder.Text != "<None>")
            {
                numBorderGapX.Enabled = true;
                numBorderGapY.Enabled = true;
                numBorderRound.Enabled = true;
                btnBorderColor.Enabled = true;
            }
            else
            {
                numBorderGapX.Enabled = false;
                numBorderGapY.Enabled = false;
                numBorderRound.Enabled = false;
                btnBorderColor.BackColor = this.BackColor;
                btnBorderColor.Enabled = false;
            }
        }

        private void cmbBack_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem item = cmbBack.SelectedItem as DevComponents.Editors.ComboItem;
            if (item == null) return;
            IFrameDecoration pFrameDecoration = (IFrameDecoration)(item.Tag);
            GetDecorationColor(pFrameDecoration, btnBackColor);
        }

        private void cmbBack_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbBack.SelectedIndex != -1 && cmbBack.Text != "<None>")
            {
                numBackGapX.Enabled = true;
                numBackGapY.Enabled = true;
                numBackRound.Enabled = true;
                btnBackColor.Enabled = true;
            }
            else
            {
                numBackGapX.Enabled = false;
                numBackGapY.Enabled = false;
                numBackRound.Enabled = false;
                btnBackColor.BackColor = this.BackColor;
                btnBackColor.Enabled = false;
            }
        }

        private void cmbShadow_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem item = cmbShadow.SelectedItem as DevComponents.Editors.ComboItem;
            if (item == null) return;
            IFrameDecoration pFrameDecoration = (IFrameDecoration)(item.Tag);
            GetDecorationColor(pFrameDecoration, btnShadowColor);
        }

        private void cmbShadow_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbShadow.SelectedIndex != -1 && cmbShadow.Text != "<None>")
            {
                numShadowGapX.Enabled = true;
                numShadowGapY.Enabled = true;
                numShadowRound.Enabled = true;
                btnShadowColor.Enabled = true;
            }
            else
            {
                numShadowGapX.Enabled = false;
                numShadowGapY.Enabled = false;
                numShadowRound.Enabled = false;
                btnShadowColor.BackColor = this.BackColor;
                btnShadowColor.Enabled = false;
            }
        }

        private void btnText_Click(object sender, EventArgs e)
        {
            picText_Click(null, null);
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            picPoint_Click(null, null);
        }

        private void btnPoly_Click(object sender, EventArgs e)
        {
            picPoly_Click(null, null);
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            picLine_Click(null, null);
        }

        private void picText_Click(object sender, EventArgs e)
        {
            if (picText.Tag == null) return;
            frmSymbolSelector frm = new frmSymbolSelector();
            IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassTextSymbols, (ISymbol)picText.Tag);
            if (item != null)
            {
                PreViewSymbol(item.Item, ref picText);
                picText.Tag = item.Item;
            }
        }

        private void picPoly_Click(object sender, EventArgs e)
        {
            if (picPoly.Tag == null) return;
            frmSymbolSelector frm = new frmSymbolSelector();
            IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassFillSymbols, (ISymbol)picPoly.Tag);
            if (item != null)
            {
                PreViewSymbol(item.Item, ref picPoly);
                picPoly.Tag = item.Item;
            }
        }

        private void picLine_Click(object sender, EventArgs e)
        {
            if (picLine.Tag == null) return;
            frmSymbolSelector frm = new frmSymbolSelector();
            IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassLineSymbols, (ISymbol)picLine.Tag);
            if (item != null)
            {
                PreViewSymbol(item.Item, ref picLine);
                picLine.Tag = item.Item;
            }
        }

        private void picPoint_Click(object sender, EventArgs e)
        {
            if (picPoint.Tag == null) return;
            frmSymbolSelector frm = new frmSymbolSelector();
            IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, (ISymbol)picPoint.Tag);
            if (item != null)
            {
                PreViewSymbol(item.Item, ref picPoint);
                picPoint.Tag = item.Item;
            }
        }

        private void btnArrow_Click(object sender, EventArgs e)
        {
            picArrow_Click(null, null);
        }

        private void picArrow_Click(object sender, EventArgs e)
        {
            if (picArrow.Tag == null) return;
            frmSymbolSelector frm = new frmSymbolSelector();
            IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassNorthArrows, picArrow.Tag);
            if (item != null)
            {
                PreViewSymbol(item.Item, ref picArrow);
                picArrow.Tag = item.Item;
            }
        }

        private void picScale_Click(object sender, EventArgs e)
        {
            if (picScale.Tag == null) return;
            frmSymbolSelector frm = new frmSymbolSelector();
            IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassScaleBars,picScale.Tag);
            if (item != null)
            {
                PreViewSymbol(item.Item, ref picScale);
                picScale.Tag = item.Item;
            }
        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            picScale_Click(null, null);
        }

        private void picScaleText_Click(object sender, EventArgs e)
        {
            if (picScaleText.Tag == null) return;
            frmSymbolSelector frm = new frmSymbolSelector();
            IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassScaleTexts,picScaleText.Tag);
            if (item != null)
            {
                PreViewSymbol(item.Item, ref picScaleText);
                picScaleText.Tag = item.Item;
            }
        }

        private void btnScaleText_Click(object sender, EventArgs e)
        {
            picScaleText_Click(null, null);
        }

        private void btnBorderColor_Click_1(object sender, EventArgs e)
        {

        }

    }
}
