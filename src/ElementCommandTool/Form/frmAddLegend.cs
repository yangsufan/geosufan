using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using stdole;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;

namespace ElementCommandTool
{
    public partial class frmAddLegend : DevComponents.DotNetBar.Office2007Form
    {
        private IMapSurroundFrame m_pMapSurroundFrame = null;
        private IActiveView m_pView = null;
        private bool m_IsAdd = false;
        //当前Legend
        private int m_Step = 0;
        IDisplay m_pDisplay;
        ITransform2D m_pTransform;
        private bool m_bSizeChanged = false;
        public frmAddLegend(bool bIsAdd)
        {
            //相关委托
            InitializeComponent();
            txtPath1.GotFocus += new EventHandler(txtPath_GotFocus);
            txtPath2.GotFocus += new EventHandler(txtPath_GotFocus);
            txtPath3.GotFocus += new EventHandler(txtPath_GotFocus);
            txtPath4.GotFocus += new EventHandler(txtPath_GotFocus);
            txtPath5.GotFocus += new EventHandler(txtPath_GotFocus);
            txtPath6.GotFocus += new EventHandler(txtPath_GotFocus);
            txtPath7.GotFocus += new EventHandler(txtPath_GotFocus);
            txtPath8.GotFocus += new EventHandler(txtPath_GotFocus);
            txtPath9.GotFocus += new EventHandler(txtPath_GotFocus);
            this.cmbBorder.SelectionChangeCommitted += new System.EventHandler(this.cmbBorder_SelectionChangeCommitted);
            this.cmbBack.SelectionChangeCommitted += new System.EventHandler(this.cmbBack_SelectionChangeCommitted);
            this.cmbShadow.SelectionChangeCommitted += new System.EventHandler(this.cmbShadow_SelectionChangeCommitted);
            this.cmbBorder.SelectedValueChanged += new System.EventHandler(this.cmbBorder_SelectionChanged);
            this.cmbBack.SelectedValueChanged += new System.EventHandler(this.cmbBack_SelectionChanged);
            this.cmbShadow.SelectedValueChanged += new System.EventHandler(this.cmbShadow_SelectionChanged);

            //移动各个对象到指定位置
            grpFrame.Left = tabLegend.Left;
            grpItems.Left = tabLegend.Left;
            grpTitle.Left = tabLegend.Left;
            grpSize.Left = tabLegend.Left;
            grpFrame.Top = tabLegend.Top;
            grpItems.Top = tabLegend.Top;
            grpTitle.Top = tabLegend.Top;
            grpSize.Top = tabLegend.Top;

            m_IsAdd = bIsAdd;
            if (bIsAdd == false)
            {
                tabLegend.Visible = true;
                tabItem2.AttachedControl.Controls.Add(grpItems);
                tabItem3.AttachedControl.Controls.Add(grpTitle);
                tabItem4.AttachedControl.Controls.Add(grpFrame);
                tabItem5.AttachedControl.Controls.Add(grpSize);

                btnNext.Text = "确　定";
                btnLast.Visible = false;
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            m_Step = m_Step - 1;
            switch (m_Step)
            {
                case 0:
                    grpItems.BringToFront();
                    btnLast.Enabled = false;
                    break;
                case 1:
                    grpTitle.BringToFront();
                    btnLast.Enabled = true;
                    break;
                case 2:
                    grpFrame.BringToFront();
                    btnLast.Enabled = true;
                    break;
            }

            btnNext.Text = "下一步";
            btnNext.Enabled = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (btnNext.Text == "确　定")
            {
                CProgress pgss = new CProgress("正在应用图例设置，请稍候...");
                pgss.EnableCancel = false;
                pgss.ShowDescription = false;
                pgss.FakeProgress = true;
                pgss.TopMost = true;
                pgss.ShowProgress();
                this.DialogResult = DialogResult.OK;
                
                UpdateProperties();
                pgss.Close();
                this.Hide();
                return;
            }

            m_Step = m_Step + 1;
            switch (m_Step)
            {
                case 4:
                    this.DialogResult = DialogResult.OK;
                    UpdateProperties();
                    this.Hide();
                    break;
                case 3:
                    grpSize.BringToFront();
                    btnNext.Text = "完 成";
                    //this.DialogResult = DialogResult.OK;

                    break;
                case 1:
                    grpTitle.BringToFront();
                    break;
                case 2:
                    grpFrame.BringToFront();
                    break;

            }
            btnLast.Enabled = true;
        }

        private void frmAddLegend_Load(object sender, EventArgs e)
        {
           
            if (m_IsAdd == false)
            {
                tabLegend.BringToFront();
            }
            else
            {
                ILegend pLegend = (ILegend)m_pMapSurroundFrame.MapSurround;
                pLegend.Title = "图例";//yjl20111010 add 
                IElementProperties3 pElementProp = m_pMapSurroundFrame as IElementProperties3;
                pElementProp.Name = "图例";
                grpItems.BringToFront();
            }
            GetProperties();
            btnLast.Enabled = false;
        }
        private string ReadRegistry(string sKey)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sKey, true);
            if (rk == null) return "";

            return (string)rk.GetValue("InstallDir");
        }
        //获得当前Legend的属性
        private void GetProperties()
        {
            if (m_pMapSurroundFrame != null && m_pMapSurroundFrame.MapSurround is ILegend)
            {
                //==================================获得Ｌｅｇｅｎｄ的主要属性==============================
                ILegend pLegend = (ILegend)m_pMapSurroundFrame.MapSurround;
                txtLegend.Text = pLegend.Title;
                //设置Title的Symbol
                ILegendFormat pLegendFormat = pLegend.Format;
                this.txtPath1.Text = pLegendFormat.TitleGap.ToString("f4");
                this.txtPath2.Text = pLegendFormat.VerticalItemGap.ToString("f4");
                this.txtPath3.Text = pLegendFormat.HorizontalItemGap.ToString("f4");
                txtPath4.Text = pLegendFormat.HeadingGap.ToString("f4");
                txtPath5.Text = pLegendFormat.TextGap.ToString("f4");
                txtPath6.Text = pLegendFormat.VerticalPatchGap.ToString("f4");
                txtPath7.Text = pLegendFormat.HorizontalPatchGap.ToString("f4");
                txtPath8.Text = pLegendFormat.LayerNameGap.ToString("f4");
                txtPath9.Text = pLegendFormat.GroupGap.ToString("f4");
                btnLegendSymbol.Tag = pLegendFormat.TitleSymbol;
                //是否显示标题
                chkVisible.Checked = pLegendFormat.ShowTitle;
                //==========================================================================================

                //========================================图例样式==================================================
                //Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\ESRI\\CoreRuntime", true);
                //string sInstall = (string)rk.GetValue("InstallDir");
                string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
                
                if (sInstall == "") //added by chulili 2012-11-13 平台由ArcGIS9.3换成ArcGIS10，相应的注册表路径要修改
                {
                    sInstall =ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
                }
                if (sInstall == "")
                {
                    sInstall =ReadRegistry("SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
                }   //added by chulili 2012-11-13  end
                if (sInstall == "")
                {
                    MessageBox.Show("系统没有安装Engine Runtime", "提示");
                    return;
                }
                string sPath = sInstall + "\\Styles\\ESRI.ServerStyle";

                axSymbologyControl1.Clear();
                axSymbologyControl1.LoadStyleFile(sPath);
                //面状样式
                int i = 0;
                DevComponents.Editors.ComboItem valueItem;
                System.Drawing.Image image;
                ISymbologyStyleClass symbologyStyleClass;
                IStyleGalleryItem pItem;
                IPictureDisp picture;
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassAreaPatches;
                symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
                int cnt = symbologyStyleClass.get_ItemCount(symbologyStyleClass.StyleCategory);
                cmbAreaPatch.Items.Clear();
                IPatch pPatch = (IPatch)pLegendFormat.DefaultAreaPatch;
                for (i = 0; i < cnt; i++)
                {
                    pItem = symbologyStyleClass.GetItem(i);
                    picture = symbologyStyleClass.PreviewItem(pItem, cmbAreaPatch.Width - 50, cmbAreaPatch.Height);
                    image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                    valueItem = new DevComponents.Editors.ComboItem();
                    //valueItem.Text = pItem.Name;//yjl20111010 add 屏蔽英文
                    IPatch pPatch1 = (IPatch)pItem.Item;
                    pPatch1.Name = pItem.Name;
                    valueItem.Tag = pPatch1;
                    valueItem.Image= image;
                    cmbAreaPatch.Items.Add(valueItem);

                    if (pPatch.Name == pItem.Name)
                        cmbAreaPatch.SelectedIndex = i;
                    if (cmbAreaPatch.SelectedIndex == -1) cmbAreaPatch.SelectedIndex = 0;
                }

                //线状样式
                pPatch = (IPatch)pLegendFormat.DefaultLinePatch;
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassLinePatches;
                symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
                cnt = symbologyStyleClass.get_ItemCount(symbologyStyleClass.StyleCategory);
                cmbLinePath.Items.Clear();

                for (i = 0; i < cnt; i++)
                {
                    pItem = symbologyStyleClass.GetItem(i);
                    picture = symbologyStyleClass.PreviewItem(pItem, cmbLinePath.Width - 50, cmbLinePath.Height);
                    image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                    valueItem = new DevComponents.Editors.ComboItem();
                    //valueItem.Text = pItem.Name;//yjl20111010 add 屏蔽英文
                    IPatch pPatch1 = (IPatch)pItem.Item;
                    pPatch1.Name = pItem.Name;
                    valueItem.Tag = pPatch1;
                    valueItem.Image = image;
                    cmbLinePath.Items.Add(valueItem);
                    if (pPatch.Name == pItem.Name)
                        cmbLinePath.SelectedIndex = i;

                    if (cmbLinePath.SelectedIndex == -1) cmbLinePath.SelectedIndex = 0;
                }

                //大小
                this.txtPatchDefaultHeight.Text = pLegendFormat.DefaultPatchHeight.ToString("f4");
                txtPatchDefaultWidth.Text = pLegendFormat.DefaultPatchWidth.ToString("f4");
                //==========================================================================================
                ////====================================边框样式=============================================
                axSymbologyControl1.StyleClass = esriSymbologyStyleClass.esriStyleClassBorders;
                symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
                cnt = symbologyStyleClass.get_ItemCount(symbologyStyleClass.StyleCategory);
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

                //获得当前的边框样式
                IFrameElement pFrameElement = (IFrameElement)m_pMapSurroundFrame;
                IFrameProperties pFrameProperty = (IFrameProperties)pFrameElement;
                IFrameDecoration pFrameDecoration = null;
                IClone pClone;

                bool bExist = false;
                //得到地图边框属性
                if (pFrameProperty.Border != null)
                {
                    pFrameDecoration = (IFrameDecoration)pFrameProperty.Border;
                    //判断和列表框中的符号是否相同
                    bExist = false;
                    pClone = (IClone)pFrameProperty.Border;
                    for (i = 0; i < cmbBorder.Items.Count; i++)
                    {
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
                    pFrameDecoration = (IFrameDecoration)pFrameElement.Background;
                    //判断和列表框中的符号是否相同
                    bExist = false;
                    pClone = (IClone)pFrameProperty.Background;
                    for (i = 0; i < cmbBack.Items.Count; i++)
                    {
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
                    pClone = (IClone)pFrameProperty.Shadow;
                    for (i = 0; i < cmbShadow.Items.Count; i++)
                    {
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
                //==========================================================================================

                //===================================图例项属性==============================================
                lstLayers.Items.Clear();
                lstLegends.Items.Clear();
                ILegendItem pLegendItem;

                IMap pMap = m_pView.FocusMap;
                ILayer pLayer;
                ILayer pGLayer;
                for (i = 0; i < pMap.LayerCount; i++)
                {
                    pLayer = pMap.get_Layer(i);
                    if (pLayer is IFDOGraphicsLayer || pLayer is IDimensionLayer) continue;
                    if (pLayer is ICompositeLayer)
                    {
                        ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                        for (int j = 0; j < pCLayer.Count; j++)
                        {
                            pGLayer = pCLayer.get_Layer(j);
                            if (pGLayer is IFeatureLayer)
                            {
                                if (!lstLayers.Items.Contains(pGLayer.Name))
                                {
                                    lstLayers.Items.Add(pGLayer.Name);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pLayer is IFeatureLayer)
                        {
                            if (!lstLayers.Items.Contains(pLayer.Name))
                            {
                                lstLayers.Items.Add(pLayer.Name);
                            }
                        }
                    }
                }
                //ＬｅｇｅｎｄＩｔｅｍ
                int iLegendColumn = 1;
                for (i = 0; i < pLegend.ItemCount; i++)
                {
                    pLegendItem = pLegend.get_Item(i);
                    if (i == 0)
                    {
                        btnLayerItemSymbol.Tag = pLegendItem.LegendClassFormat.LabelSymbol;
                    }
                    pLayer = pLegendItem.Layer;
                    //pLegendItem.Name = pLayer.Name;
                    lstLegends.Items.Add(pLayer.Name);
                    lstLayers.Items.Remove(pLayer.Name);
                    if (pLegendItem.NewColumn == true)
                    {
                        iLegendColumn++;
                    }
                    iLegendColumn += pLegendItem.Columns - 1;

                }
                this.numCol.Value = iLegendColumn;
                //==========================================================================================

                //===================================位置和大小属性==============================================

                IElementProperties3 pElementProp = m_pMapSurroundFrame as IElementProperties3;
                txtName.Text = pElementProp.Name;

                //如果是空,则创建一个默认的位置
                IElement pElement = (IElement)m_pMapSurroundFrame;
                if (pElement.Geometry.IsEmpty)
                {
                    IElement pMainMapElement = (IElement)m_pMapSurroundFrame.MapFrame;
                    IEnvelope pMainEnv = pMainMapElement.Geometry.Envelope;
                    IEnvelope pEnv = new EnvelopeClass();
                    IQuerySize pQS = m_pMapSurroundFrame.MapSurround as IQuerySize;
                    double w = 0, h = 0;
                    pQS.QuerySize(ref w, ref h);
                    double ratio2 = w / h;
                    pEnv.PutCoords(pMainEnv.XMax - 1.5, pMainEnv.YMin + 1.5, pMainEnv.XMax - 1.5 + 8, pMainEnv.YMin + 1.5+8/ratio2);
                    pElement.Geometry = pEnv;
                }

                //===================================比率属性==============================================
              
                IEnvelope pBounds = new EnvelopeClass();
                uC_AnchorPoint1.DirectAnchor(pElementProp.AnchorPoint);
                pElement.QueryBounds(m_pDisplay, pBounds);
                numHeight.Text = pBounds.Height.ToString("f4");
                numWidth.Text = pBounds.Width.ToString("f4");
                IPoint anchorPoint = getAnchorPoint(pBounds, pElement);
                numLowerLeftX.Text = anchorPoint.X.ToString("f4");
                numLowerLeftY.Text = anchorPoint.Y.ToString("f4");
                //IQuerySize pQS1 = m_pMapSurroundFrame.MapSurround as IQuerySize;
                //double w1 = 0, h1 = 0;
                //pQS1.QuerySize(ref w1, ref h1);
                //double wcm = w1 * 2.54 / 72, hcm = h1 * 2.54 / 72;
                //double ratio21 = wcm / hcm;
                //==========================================================================================
                
            }
        }

        /// <summary>
        /// 重新设置属性
        /// </summary>
        private void UpdateProperties()
        {

            IElement legendEle = m_pMapSurroundFrame as IElement;
            IElementProperties3 legendElePro = legendEle as IElementProperties3;
            IEnvelope pOriBound=legendEle.Geometry.Envelope;//原始大小
            
            IFrameProperties pFrameProperty = null;
            IFrameDecoration pFrameDecoration = null;
            IFrameElement pFrameElement = (IFrameElement)m_pMapSurroundFrame;
            ILegend pLegend;
            ILegendFormat pLegendFormat;
            DevComponents.Editors.ComboItem item = null;
            IElementProperties3 pElementProp = m_pMapSurroundFrame as IElementProperties3;
            if (pElementProp != null)
            {
                pElementProp.Name = txtName.Text;
            }
            pLegend = (ILegend)m_pMapSurroundFrame.MapSurround;
            pLegend.Title = txtLegend.Text;
            //====================================更新图层属性============================================
            //首先清除所有原始的内容
            pLegend.Map = m_pMapSurroundFrame.MapFrame.Map;
            pLegend.ClearItems();
            
            ILayer pLayer;
            int i = 0, j = 0, k = 0, n = 0;
            ILegendInfo pLegendInfo;
            int iLegendCount = 0;
            //for (i = 0; i < lstLegends.Items.Count; i++)
            //{
            //    pLayer = null;
            //    pLayer = GetLayerByName(lstLegends.Items[i].ToString());

            //    if (pLayer != null)
            //    {
            //        pLegendInfo = (ILegendInfo)pLayer;


            //        for (j = 0; j < pLegendInfo.LegendGroupCount; j++)
            //        {
            //            iLegendCount += pLegendInfo.get_LegendGroup(j).ClassCount;
            //        }
            //    }
            //}


            for (i = 0; i < lstLegends.Items.Count; i++)
            {
                pLayer = null;
                pLayer = GetLayerByName(lstLegends.Items[i].ToString());
                ILegendItem pLegendItem;
                if (pLayer != null)
                {
                    pLegendItem = new HorizontalLegendItemClass();

                    pLegendItem.Layer = pLayer;

                    //分列显示的问题


                    //pLegendInfo = (ILegendInfo)pLegendItem.Layer;
                    //for (j = 0; j < pLegendInfo.LegendGroupCount; j++)
                    //{
                    //    for (k = 0; k < pLegendInfo.get_LegendGroup(j).ClassCount; k++)
                    //    {

                    //        if (n % (iLegendCount / Convert.ToInt32(numCol.Value)) == 0 && n > 0 && (k > 0 || pLegendInfo.get_LegendGroup(j).ClassCount == 1))
                    //        {
                    //            pLegendItem.Columns++;
                    //            pLegendItem.NewColumn = false;
                    //        }
                    //        n = n + 1;
                    //    }
                    //}
                    pLegend.AddItem(pLegendItem);
                }
            }//for

            //====================================更新边框属性============================================

            if (cmbBorder.SelectedItem != null)
            {
                item = cmbBorder.SelectedItem as DevComponents.Editors.ComboItem;
                if (cmbBorder.SelectedIndex != -1 && item != null)
                {
                    IBorder pBorder = (IBorder)(item.Tag);
                    pFrameDecoration = (IFrameDecoration)pBorder;
                    pFrameDecoration.HorizontalSpacing = (double)numBorderGapX.Value;
                    pFrameDecoration.VerticalSpacing = (double)numBorderGapY.Value;
                    pFrameDecoration.CornerRounding = (short)numBorderRound.Value;
                    pFrameElement.Border = pBorder;
                }
                else
                {
                    pFrameElement.Border = null;
                }
            }
            if (cmbBack.SelectedItem != null)
            {
                item = cmbBack.SelectedItem as DevComponents.Editors.ComboItem;
                if (cmbBack.SelectedIndex != -1 && item != null)
                {
                    IBackground pBackground = (IBackground)(item.Tag);
                    pFrameDecoration = (IFrameDecoration)pBackground;
                    pFrameDecoration.HorizontalSpacing = (double)numBackGapX.Value;
                    pFrameDecoration.VerticalSpacing = (double)numBackGapY.Value;
                    pFrameDecoration.CornerRounding = (short)numBackRound.Value;
                    pFrameElement.Background = pBackground;
                }
                else
                {
                    pFrameElement.Background = null;
                }
            }
            if (cmbShadow.SelectedItem != null)
            {
                item = cmbShadow.SelectedItem as DevComponents.Editors.ComboItem;
                if (cmbShadow.SelectedIndex != -1 && item != null)
                {
                    IShadow pShadow = (IShadow)(item.Tag);
                    pFrameDecoration = (IFrameDecoration)pShadow;
                    pFrameDecoration.HorizontalSpacing = (double)numShadowGapX.Value;
                    pFrameDecoration.VerticalSpacing = (double)numShadowGapY.Value;
                    pFrameDecoration.CornerRounding = (short)numShadowRound.Value;

                    pFrameProperty = (IFrameProperties)m_pMapSurroundFrame;
                    pFrameProperty.Shadow = pShadow;
                }
                else
                {
                    pFrameProperty = (IFrameProperties)m_pMapSurroundFrame;
                    pFrameProperty.Shadow = null;
                }
            }
           

            //=====================================更新基本属性=========================================
            
            //设置Title的Symbol
            pLegend.AutoAdd = false;
            pLegend.AutoVisibility = false;
            pLegend.AutoReorder = false;

            pLegendFormat = pLegend.Format;
            pLegendFormat.TitlePosition = esriRectanglePosition.esriTopSide;
            pLegendFormat.TitleGap = Convert.ToDouble(this.txtPath1.Text);
            pLegendFormat.VerticalItemGap = Convert.ToDouble(this.txtPath2.Text);
            pLegendFormat.HorizontalItemGap = Convert.ToDouble(this.txtPath3.Text);
            pLegendFormat.HeadingGap = Convert.ToDouble(txtPath4.Text);
            pLegendFormat.TextGap = Convert.ToDouble(txtPath5.Text);
            pLegendFormat.VerticalPatchGap = Convert.ToDouble(txtPath6.Text);
            pLegendFormat.HorizontalPatchGap = Convert.ToDouble(txtPath7.Text);
            pLegendFormat.LayerNameGap = Convert.ToDouble(txtPath8.Text);
            pLegendFormat.GroupGap = Convert.ToDouble(txtPath9.Text);
            pLegendFormat.ShowTitle = chkVisible.Checked;
            pLegendFormat.TitleSymbol = (ITextSymbol)btnLegendSymbol.Tag;
            //==========================================================================================
            //=====================================更新图例ITem属性=====================================
            pLegendFormat.DefaultPatchWidth = Convert.ToDouble(txtPatchDefaultWidth.Text);
            pLegendFormat.DefaultPatchHeight = Convert.ToDouble(txtPatchDefaultHeight.Text);
            IPatch pPatch;
            pPatch = (IPatch)(cmbAreaPatch.SelectedItem as DevComponents.Editors.ComboItem).Tag;
            pPatch.PreserveAspectRatio = false;
            pLegendFormat.DefaultAreaPatch = (IAreaPatch)pPatch;
            pPatch = (IPatch)(cmbLinePath.SelectedItem as DevComponents.Editors.ComboItem).Tag;
            pPatch.PreserveAspectRatio = false;
            pLegendFormat.DefaultLinePatch = (ILinePatch)pPatch;


            for (int itm = 0; itm < pLegend.ItemCount; itm++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(itm);
               
                //图例项样式

                pLegendItem.ShowLabels = true;
                pLegendItem.ShowLayerName = false;
                pLegendItem.ShowHeading = false;
                pLegendItem.KeepTogether = false;
                pLegendItem.LayerNameSymbol = (ITextSymbol)btnLayerItemSymbol.Tag;
                pLegendItem.HeadingSymbol = (ITextSymbol)btnLayerItemSymbol.Tag;


                pLegendItem.LegendClassFormat.LabelSymbol = (ITextSymbol)btnLayerItemSymbol.Tag;
                pLegendItem.LegendClassFormat.PatchHeight = pLegendFormat.DefaultPatchHeight;
                pLegendItem.LegendClassFormat.PatchWidth = pLegendFormat.DefaultPatchWidth;
            }

            //=====================================更新列数属性=========================================
            ILegend2 pLegend2 = pLegend as ILegend2;
            pLegend2.AdjustColumns(Convert.ToInt32(numCol.Value));
            pLegend2.Refresh();

            //=========================================================================================
            //最后设置大小,如果涉及到缩放,则自动缩放,但是由于其只能等比缩放,所以效果也不是很明显
            //====================================更新位置大小属性======================================
            

            IQuerySize pQS1 = m_pMapSurroundFrame.MapSurround as IQuerySize;
            double w1 = 0, h1 = 0;
            pQS1.QuerySize(ref w1, ref h1);
            double wcm = w1 * 2.54 / 72, hcm = h1 * 2.54 / 72;
            
            
            if (m_bSizeChanged && !chkUsePercent.Checked)//用户若设置大小，则以此为主
            {
                wcm = (Convert.ToDouble(numWidth.Text));
                hcm = (Convert.ToDouble(numHeight.Text));
            }
            IPoint oriAnchorPoint = getAnchorPoint(pOriBound, legendEle);
            double posX = oriAnchorPoint.X,
                posY = oriAnchorPoint.Y;
            pOriBound = getEnvelope(legendElePro.AnchorPoint, posX, posY, wcm, hcm);

            legendEle.Geometry = pOriBound;
            //设置局部变量
            m_pTransform = pOriBound as ITransform2D;
            //设置偏移
            IEnvelope pBounds = new EnvelopeClass();
            ((IElement)m_pMapSurroundFrame).QueryBounds(m_pDisplay, pBounds);
            //ratio = pBounds.Width / pBounds.Height;
            double dx, dy;
            IPoint pFromPoint = getAnchorPoint(pBounds,legendEle);

            if (chkUseOffDist.Checked)//设置偏移
            {
                dx = Math.Round(Convert.ToDouble(numLowerLeftX.Text), 2);
                dy = Math.Round(Convert.ToDouble(numLowerLeftY.Text), 2);
            }
            else//设置左下角位置
            {
                dx = Math.Round((Convert.ToDouble(numLowerLeftX.Text) - pFromPoint.X), 2);
                dy = Math.Round((Convert.ToDouble(numLowerLeftY.Text) - pFromPoint.Y), 2);
            }
            m_pTransform.Move(dx, dy);
            legendEle.Geometry = pOriBound;
            //设置缩放
            ((IElement)m_pMapSurroundFrame).QueryBounds(m_pDisplay, pBounds);
            pFromPoint = getAnchorPoint(pBounds, legendEle);

            if (chkUsePercent.Checked)
            {
                wcm = (Convert.ToDouble(numWidth.Text));
                hcm = (Convert.ToDouble(numHeight.Text));
                dx = Math.Round((wcm / 100), 2);
                dy = Math.Round((hcm / 100), 2);
            }
            else
            {
                dx = Math.Round(wcm / pBounds.Width, 2);
                dy = Math.Round(hcm / pBounds.Height, 2);
            }
            m_pTransform.Scale(pFromPoint, dx, dy);
            bool changed=false;
            pLegend2.FitToBounds(m_pDisplay, m_pTransform as IEnvelope, out changed);
            legendEle.Geometry = pOriBound;
            legendEle.Activate(m_pDisplay);
            m_pView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, legendEle, null);
        }
        //获取元素的锚点 yjl20111105 add
        private IPoint getAnchorPoint(IEnvelope inEnv,IElement inEle)
        {
            IPoint res = new PointClass();
            double xmin = inEnv.XMin, ymin = inEnv.YMin, xmax = inEnv.XMax, ymax = inEnv.YMax,xmid=(xmin+xmax)/2,ymid=(ymin+ymax)/2;
            IElementProperties3 pElePro = inEle as IElementProperties3;
            switch (pElePro.AnchorPoint)
            {
                case esriAnchorPointEnum.esriBottomLeftCorner:
                    res.PutCoords(xmin, ymin);
                    break;
                case esriAnchorPointEnum.esriBottomMidPoint:
                    res.PutCoords(xmid, ymin);
                    break;
                case esriAnchorPointEnum.esriBottomRightCorner:
                    res.PutCoords(xmax, ymin);
                    break;
                case esriAnchorPointEnum.esriCenterPoint:
                    res.PutCoords(xmid, ymid);
                    break;
                case esriAnchorPointEnum.esriLeftMidPoint:
                    res.PutCoords(xmin, ymid);
                    break;
                case esriAnchorPointEnum.esriRightMidPoint:
                    res.PutCoords(xmax, ymid);
                    break;
                case esriAnchorPointEnum.esriTopLeftCorner:
                    res.PutCoords(xmin, ymax);
                    break;
                case esriAnchorPointEnum.esriTopMidPoint:
                    res.PutCoords(xmid, ymax);
                    break;
                case esriAnchorPointEnum.esriTopRightCorner:
                    res.PutCoords(xmax, ymax);
                    break;
                default:
                    res.PutCoords(xmin, ymin);
                    break;


            }
            return res;
 
        }
        //由锚点构造元素的形状 yjl20111107 add
        private IEnvelope getEnvelope(esriAnchorPointEnum inAnchorPoint, double inPosX,double inPosY,double inWidth,double inHeight)
        {
            IEnvelope res = new EnvelopeClass();
            double midW = inWidth / 2,
                midH = inHeight / 2;
            switch (inAnchorPoint)
            {
                case esriAnchorPointEnum.esriBottomLeftCorner:
                    res.PutCoords(inPosX,inPosY,inPosX+inWidth,inPosY+inHeight);
                    break;
                case esriAnchorPointEnum.esriBottomMidPoint:
                    res.PutCoords(inPosX-midW, inPosY,inPosX+midW,inPosY+inHeight);
                    break;
                case esriAnchorPointEnum.esriBottomRightCorner:
                    res.PutCoords(inPosX - inWidth, inPosY, inPosX, inPosY + inHeight);
                    break;
                case esriAnchorPointEnum.esriCenterPoint:
                    res.PutCoords(inPosX - midW, inPosY-midH, inPosX + midW, inPosY + midH);
                    break;
                case esriAnchorPointEnum.esriLeftMidPoint:
                    res.PutCoords(inPosX, inPosY - midH, inPosX + inWidth, inPosY + midH);
                    break;
                case esriAnchorPointEnum.esriRightMidPoint:
                    res.PutCoords(inPosX - inWidth, inPosY - midH, inPosX, inPosY + midH);
                    break;
                case esriAnchorPointEnum.esriTopLeftCorner:
                    res.PutCoords(inPosX, inPosY - inHeight, inPosX + inWidth, inPosY);
                    break;
                case esriAnchorPointEnum.esriTopMidPoint:
                    res.PutCoords(inPosX-midW, inPosY - inHeight, inPosX + midW, inPosY);
                    break;
                case esriAnchorPointEnum.esriTopRightCorner:
                    res.PutCoords(inPosX - inWidth, inPosY - inHeight, inPosX, inPosY);
                    break;
                default:
                    res.PutCoords(inPosX, inPosY, inPosX + inWidth, inPosY + inHeight);
                    break;


            }
            return res;

        }
        //yjl 20111022 modify in SG
        //private void UpdateProperties()
        //{
        //    IFrameProperties pFrameProperty = null;
        //    IFrameDecoration pFrameDecoration = null;
        //    IFrameElement pFrameElement = (IFrameElement)m_pMapSurroundFrame;
        //    ILegend pLegend = (ILegend)m_pMapSurroundFrame.MapSurround;
        //    ILegendFormat pLegendFormat;
        //    DevComponents.Editors.ComboItem item = null;
        //    IElement pEle = m_pMapSurroundFrame as IElement;
        //    IElementProperties3 pElementProp = m_pMapSurroundFrame as IElementProperties3;
        //    //首先清除所有原始的内容
        //    pLegend.Map = m_pMapSurroundFrame.MapFrame.Map;
        //    pLegend.ClearItems();
        //    ILegendItem pLegendItem;
        //    ILayer pLayer;
        //    int i = 0, j = 0, k = 0, n = 0;
        //    ILegendInfo pLegendInfo;
        //    int iLegendCount = 0;
        //    for (i = 0; i < lstLegends.Items.Count; i++)
        //    {
        //        pLayer = null;
        //        pLayer = GetLayerByName(lstLegends.Items[i].ToString());

        //        if (pLayer != null)
        //        {
        //            pLegendInfo = (ILegendInfo)pLayer;


        //            for (j = 0; j < pLegendInfo.LegendGroupCount; j++)
        //            {
        //                iLegendCount += pLegendInfo.get_LegendGroup(j).ClassCount;
        //            }
        //        }
        //    }


        //    for (i = 0; i < lstLegends.Items.Count; i++)
        //    {
        //        pLayer = null;
        //        pLayer = GetLayerByName(lstLegends.Items[i].ToString());

        //        if (pLayer != null)
        //        {
        //            pLegendItem = new HorizontalLegendItemClass();

        //            pLegendItem.Layer = pLayer;
        //            pLegendItem.KeepTogether = false;

        //            //每一列的数量
        //            pLegendItem.LayerNameSymbol = (ITextSymbol)btnLayerItemSymbol.Tag;
        //            pLegendItem.HeadingSymbol = (ITextSymbol)btnLayerItemSymbol.Tag;
        //            pLegendItem.LegendClassFormat.LabelSymbol = (ITextSymbol)btnLayerItemSymbol.Tag;

        //            pLegendItem.ShowLabels = true;
        //            pLegendItem.ShowLayerName = false;
        //            pLegendItem.ShowDescriptions = false;
        //            pLegendItem.HeadingSymbol = (ITextSymbol)btnLayerItemSymbol.Tag;
        //            pLegendItem.ShowHeading = false;
        //            pLegendItem.KeepTogether = false;
        //            //pLegendItem .HeadingSymbol = ((ILegendItem )btnLegendItemStyle.Tag).;
        //            //分列显示的问题


        //            //pLegendInfo = (ILegendInfo)pLegendItem.Layer;
        //            //for (j = 0; j < pLegendInfo.LegendGroupCount; j++)
        //            //{
        //            //    for (k = 0; k < pLegendInfo.get_LegendGroup(j).ClassCount; k++)
        //            //    {

        //            //        if (n % (iLegendCount / Convert.ToInt32(numCol.Value)) == 0 && n > 0 && (k > 0 || pLegendInfo.get_LegendGroup(j).ClassCount == 1))
        //            //        {
        //            //            pLegendItem.Columns++;
        //            //            pLegendItem.NewColumn = false;
        //            //        }
        //            //        n = n + 1;
        //            //    }
        //            //}               
        //            pLegend.AddItem(pLegendItem);
        //        }
        //    }//for
        //    pEle.Activate(m_pDisplay);
        //    //====================================更新边框属性============================================

        //    if (pElementProp != null)
        //    {
        //        pElementProp.Name = txtName.Text;
        //    }

        //    if (cmbBorder.SelectedItem != null)
        //    {
        //        item = cmbBorder.SelectedItem as DevComponents.Editors.ComboItem;
        //        if (cmbBorder.SelectedIndex != -1 && item != null)
        //        {
        //            IBorder pBorder = (IBorder)(item.Tag);
        //            pFrameDecoration = (IFrameDecoration)pBorder;
        //            pFrameDecoration.HorizontalSpacing = (double)numBorderGapX.Value;
        //            pFrameDecoration.VerticalSpacing = (double)numBorderGapY.Value;
        //            pFrameDecoration.CornerRounding = (short)numBorderRound.Value;
        //            pFrameElement.Border = pBorder;
        //        }
        //        else
        //        {
        //            pFrameElement.Border = null;
        //        }
        //    }
        //    if (cmbBack.SelectedItem != null)
        //    {
        //        item = cmbBack.SelectedItem as DevComponents.Editors.ComboItem;
        //        if (cmbBack.SelectedIndex != -1 && item != null)
        //        {
        //            IBackground pBackground = (IBackground)(item.Tag);
        //            pFrameDecoration = (IFrameDecoration)pBackground;
        //            pFrameDecoration.HorizontalSpacing = (double)numBackGapX.Value;
        //            pFrameDecoration.VerticalSpacing = (double)numBackGapY.Value;
        //            pFrameDecoration.CornerRounding = (short)numBackRound.Value;
        //            pFrameElement.Background = pBackground;
        //        }
        //        else
        //        {
        //            pFrameElement.Background = null;
        //        }
        //    }
        //    if (cmbShadow.SelectedItem != null)
        //    {
        //        item = cmbShadow.SelectedItem as DevComponents.Editors.ComboItem;
        //        if (cmbShadow.SelectedIndex != -1 && item != null)
        //        {
        //            IShadow pShadow = (IShadow)(item.Tag);
        //            pFrameDecoration = (IFrameDecoration)pShadow;
        //            pFrameDecoration.HorizontalSpacing = (double)numShadowGapX.Value;
        //            pFrameDecoration.VerticalSpacing = (double)numShadowGapY.Value;
        //            pFrameDecoration.CornerRounding = (short)numShadowRound.Value;

        //            pFrameProperty = (IFrameProperties)m_pMapSurroundFrame;
        //            pFrameProperty.Shadow = pShadow;
        //        }
        //        else
        //        {
        //            pFrameProperty = (IFrameProperties)m_pMapSurroundFrame;
        //            pFrameProperty.Shadow = null;
        //        }
        //    }
        //    pEle.Activate(m_pDisplay);

        //    //=====================================更新基本属性=========================================

        //    pLegend.Title = txtLegend.Text;
        //    //设置Title的Symbol
        //    pLegend.AutoAdd = false;
        //    pLegend.AutoVisibility = false;
        //    pLegend.AutoReorder = false;

        //    pLegendFormat = pLegend.Format;
        //    pLegendFormat.TitlePosition = esriRectanglePosition.esriTopSide;
        //    pLegendFormat.TitleGap = Microsoft.VisualBasic.Conversion.Val(this.txtPath1.Text);
        //    pLegendFormat.VerticalItemGap = Microsoft.VisualBasic.Conversion.Val(this.txtPath2.Text);
        //    pLegendFormat.HorizontalItemGap = Microsoft.VisualBasic.Conversion.Val(this.txtPath3.Text);
        //    pLegendFormat.HeadingGap = Microsoft.VisualBasic.Conversion.Val(txtPath4.Text);
        //    pLegendFormat.TextGap = Microsoft.VisualBasic.Conversion.Val(txtPath5.Text);
        //    pLegendFormat.VerticalPatchGap = Microsoft.VisualBasic.Conversion.Val(txtPath6.Text);
        //    pLegendFormat.HorizontalPatchGap = Microsoft.VisualBasic.Conversion.Val(txtPath7.Text);
        //    pLegendFormat.LayerNameGap = Microsoft.VisualBasic.Conversion.Val(txtPath8.Text);
        //    pLegendFormat.GroupGap = Microsoft.VisualBasic.Conversion.Val(txtPath9.Text);
        //    pLegendFormat.ShowTitle = chkVisible.Checked;
        //    pLegendFormat.TitleSymbol = (ITextSymbol)btnLegendSymbol.Tag;
        //    pEle.Activate(m_pDisplay);
        //    //==========================================================================================
        //    //=====================================更新图例ITem属性=====================================
        //    pLegendFormat.DefaultPatchWidth = Convert.ToDouble(txtPatchDefaultWidth.Text);
        //    pLegendFormat.DefaultPatchHeight = Convert.ToDouble(txtPatchDefaultHeight.Text);
        //    IPatch pPatch;
        //    pPatch = (IPatch)(cmbAreaPatch.SelectedItem as DevComponents.Editors.ComboItem).Tag;

        //    pLegendFormat.DefaultAreaPatch = (IAreaPatch)pPatch;
        //    pLegendFormat.DefaultLinePatch = (ILinePatch)(cmbLinePath.SelectedItem as DevComponents.Editors.ComboItem).Tag;
        //    pEle.Activate(m_pDisplay);

        //    ILegend2 pLegend2 = pLegend as ILegend2;
        //    pLegend2.AdjustColumns(Convert.ToInt32(numCol.Value));
        //    pEle.Activate(m_pDisplay);
        //    //=========================================================================================
        //    //最后设置大小,如果涉及到缩放,则自动缩放,但是由于其只能等比缩放,所以效果也不是很明显
        //    //====================================更新位置大小属性======================================
        //    //设置偏移
        //    //得到比率
        //    //m_pView.ScreenDisplay.DisplayTransformation.Units = esriUnits.esriPoints;
        //    //IPageLayout pPL = m_pView as IPageLayout;
        //    //pPL.Page.Units = esriUnits.esriPoints;
        //    ESRI.ArcGIS.Carto.IQuerySize querySize = m_pMapSurroundFrame.MapSurround as ESRI.ArcGIS.Carto.IQuerySize; // Dynamic Cast
        //    System.Double w = 0;
        //    System.Double h = 0;
        //    querySize.QuerySize(ref w, ref h);
        //    System.Double aspectRatio = w / h;
        //    w *= 2.54 / 72;
        //    h *= 2.54 / 72;
        //    IEnvelope pBounds = new EnvelopeClass();
        //    ((IElement)m_pMapSurroundFrame).QueryBounds(m_pDisplay, pBounds);
        //    double dx, dy;
        //    IPoint pFromPoint = pBounds.LowerLeft;
        //    pBounds.PutCoords(pFromPoint.X, pFromPoint.Y, pFromPoint.X + w, pFromPoint.Y + h);
        //    if (chkUseOffDist.Checked)//设置偏移
        //    {
        //        dx = Convert.ToDouble(numLowerLeftX.Text);
        //        dy = Convert.ToDouble(numLowerLeftY.Text);
        //    }
        //    else//设置左下角位置
        //    {
        //        dx = Convert.ToDouble(numLowerLeftX.Text) - pFromPoint.X;
        //        dy = Convert.ToDouble(numLowerLeftY.Text) - pFromPoint.Y;
        //    }
        //    m_pTransform.Move(dx, dy);
        //    ((IElement)m_pMapSurroundFrame).Geometry = pBounds;
        //    pEle.Activate(m_pDisplay);
        //    //设置缩放
        //    ((IElement)m_pMapSurroundFrame).QueryBounds(m_pDisplay, pBounds);
        //    pFromPoint = pBounds.LowerLeft;
        //    pBounds.PutCoords(pFromPoint.X, pFromPoint.Y, pFromPoint.X + w, pFromPoint.Y + h);
        //    if (chkUsePercent.Checked)
        //    {
        //        dx = (Convert.ToDouble(numWidth.Text) / 100);
        //        dy = (Convert.ToDouble(numHeight.Text) / 100);
        //    }
        //    else
        //    {
        //        dx = Convert.ToDouble(numWidth.Text) / pBounds.Width;
        //        dy = Convert.ToDouble(numHeight.Text) / pBounds.Height;
        //    }
        //    m_pTransform.Scale(pFromPoint, dx, dy);
        //    ((IElement)m_pMapSurroundFrame).Geometry = pBounds;
        //    pEle.Activate(m_pDisplay);
        //    m_pView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pEle, null);

        //}
        private ILayer GetLayerByName(string sName)
        {
            ILayer pLayer;
            for (int i = 0; i < m_pView.FocusMap.LayerCount; i++)
            {
                pLayer = m_pView.FocusMap.get_Layer(i);
                if (pLayer is ICompositeLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        pLayer = pCLayer.get_Layer(j);
                        if (pLayer is IFeatureLayer)
                        {
                            if (pLayer.Name == sName) return pLayer;
                        }
                    }
                }
                else if (pLayer is IFeatureLayer)
                {
                    if (pLayer.Name == sName) return pLayer;
                }
            }
            return null;
        }

        private ITextSymbol GetTextSymbol()
        {
            return null;
        }

        //预览自定义的符号
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

        private void btnCannel_Click(object sender, EventArgs e)
        {
            m_pMapSurroundFrame = null;
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        public IMapSurroundFrame GetMapLegend(IActiveView pView)
        {
            m_pView = pView;
            m_pDisplay = m_pView.ScreenDisplay;
            //这个涉及到大小的问题,在这里统一设置Legend的单位为Point

            //m_pDisplay.DisplayTransformation.Units = esriUnits.esriPoints;
            //创建一个默认的mapsurroundframe
            //ILegend pLegend = new LegendClass_2();
            //IMapSurround pMapSurround = (IMapSurround)pLegend;
            ////pMapSurround.Map = m_pView.FocusMap;
            IGraphicsContainer pGraphicsContainer = m_pView.GraphicsContainer;
            IMapFrame pMapFrame = (IMapFrame)pGraphicsContainer.FindFrame(m_pView.FocusMap);
            UID vUid = new UIDClass();
            vUid.Value = "{7A3F91E4-B9E3-11d1-8756-0000F8751720}";
            IMapSurround pMapSurround1 = new LegendClass_2();
            m_pMapSurroundFrame = pMapFrame.CreateSurroundFrame(vUid, pMapSurround1);
            IElement pElement = (IElement)m_pMapSurroundFrame;

            this.ShowDialog();

            if (this.DialogResult == DialogResult.OK)
            {
                pGraphicsContainer.AddElement(pElement, 0);
                UpdateProperties();
            }
            return m_pMapSurroundFrame;
        }

        public void SetMapSurroundFrame(IMapSurroundFrame pMapSurroundFrame)
        {
            m_pMapSurroundFrame = pMapSurroundFrame;
            m_pView = (IActiveView)m_pMapSurroundFrame.MapFrame.Map;
            m_pDisplay = m_pView.ScreenDisplay;

            this.ShowDialog();
        }

        /// <summary>
        /// 切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPath_GotFocus(object sender, EventArgs e)
        {
            if (sender == txtPath1)

                picPatch.Image = ElementCommandTool.Properties.Resources.patch1;

            else if (sender == txtPath2)
                picPatch.Image = ElementCommandTool.Properties.Resources.patch2;

            else if (sender == txtPath3)
                picPatch.Image = ElementCommandTool.Properties.Resources.patch3;

            else if (sender == txtPath4)
                picPatch.Image = ElementCommandTool.Properties.Resources.patch4;

            else if (sender == txtPath5)
                picPatch.Image = ElementCommandTool.Properties.Resources.patch5;

            else if (sender == txtPath6)
                picPatch.Image = ElementCommandTool.Properties.Resources.patch6;

            else if (sender == txtPath7)
                picPatch.Image = ElementCommandTool.Properties.Resources.patch7;
            else if (sender == txtPath8)
                picPatch.Image = ElementCommandTool.Properties.Resources.patch8;
            else if (sender == txtPath9)
                picPatch.Image = ElementCommandTool.Properties.Resources.patch9;
        }

        private void cmbBorder_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem item = cmbBorder.SelectedItem as DevComponents.Editors.ComboItem;
            if (item == null) return;
            object obj = item.Tag;
            if (obj== null) return;
            IFrameDecoration pFrameDecoration = (IFrameDecoration)obj;
            GetDecorationColor(pFrameDecoration, btnBorderColor);
        }

        private void cmbBack_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem item = cmbBack.SelectedItem as DevComponents.Editors.ComboItem;
            if (item == null) return;
            object obj = item.Tag;
            if (obj == null) return;
            IFrameDecoration pFrameDecoration = (IFrameDecoration)obj;
            GetDecorationColor(pFrameDecoration, btnBackColor);
        }

        private void cmbShadow_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem item = cmbShadow.SelectedItem as DevComponents.Editors.ComboItem;
            if (item == null) return;
            object obj = item.Tag;
            if (obj == null) return;
            IFrameDecoration pFrameDecoration = (IFrameDecoration)obj;
            GetDecorationColor(pFrameDecoration, btnShadowColor);
        }

        private void cmbBorder_SelectionChanged(object sender, EventArgs e)
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

        private void cmbBack_SelectionChanged(object sender, EventArgs e)
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

        private void cmbShadow_SelectionChanged(object sender, EventArgs e)
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

        private void ChangeSymbol(DevComponents.DotNetBar.Controls.ComboBoxEx cmb, Color pColor, esriSymbologyStyleClass styleClass)
        {
            try
            {
                DevComponents.Editors.ComboItem item=cmb.SelectedItem as DevComponents.Editors.ComboItem;
                object obj = item.Tag;
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

        private void chkUseOffDist_CheckedChanged(object sender, EventArgs e)
        {
            IElement pElemnet = (IElement)m_pMapSurroundFrame;
            if (chkUseOffDist.Checked)
            {
                numLowerLeftX.Text = "0";
                numLowerLeftY.Text = "0";
            }
            else
            {
                IEnvelope pBound = new EnvelopeClass();
                pElemnet.QueryBounds(m_pDisplay, pBound);
                IPoint anchorPoint = getAnchorPoint(pBound, pElemnet);
                numLowerLeftX.Text = anchorPoint.X.ToString("f4");
                numLowerLeftY.Text = anchorPoint.Y.ToString("f4");
            }
        }

        private void chkUsePercent_CheckedChanged(object sender, EventArgs e)
        {
            IElement pElemnet = (IElement)m_pMapSurroundFrame;
            if (chkUsePercent.Checked)
            {
                numHeight.Text = "100";
                numWidth.Text = "100";
            }
            else
            {
                IEnvelope pBound = new EnvelopeClass();
                pElemnet.QueryBounds(m_pDisplay, pBound);
                numHeight.Text = pBound.Height.ToString("f4");
                numWidth.Text = pBound.Width.ToString("f4");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void AddItem()
        {
            if (lstLayers.SelectedIndex == -1) return;
            string sSel = lstLayers.SelectedItem.ToString();
            lstLegends.Items.Add(sSel);
            //创建一个ＬｅｇｅｎｄＩｔｅｍ
            lstLayers.Items.RemoveAt(lstLayers.SelectedIndex);
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            for (; lstLayers.Items.Count > 0; )
            {
                lstLegends.Items.Add(lstLayers.Items[0]);
                lstLayers.Items.RemoveAt(0);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DelItem();
        }

        private void DelItem()
        {
            if (lstLegends.SelectedIndex == -1) return;
            string sSel = lstLegends.SelectedItem.ToString();
            lstLayers.Items.Add(sSel);
            lstLegends.Items.RemoveAt(lstLegends.SelectedIndex);
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            for (; lstLegends.Items.Count > 0; )
            {
                lstLayers.Items.Add(lstLegends.Items[0]);
                lstLegends.Items.RemoveAt(0);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int cnt = lstLegends.Items.Count;
            if (cnt == 1) return;
            if (lstLegends.SelectedIndex <= 0) return;

            string sSel = lstLegends.SelectedItem.ToString();
            //这个获得对应的Ｌｅｇｅｎｄ

            int selIndex = lstLegends.SelectedIndex;

            lstLegends.Items.RemoveAt(lstLegends.SelectedIndex);


            lstLegends.Items.Insert(selIndex - 1, sSel);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int cnt = lstLegends.Items.Count;
            if (cnt == 1) return;
            if (lstLegends.SelectedIndex == -1 || lstLegends.SelectedIndex == (cnt - 1)) return;

            string sSel = lstLegends.SelectedItem.ToString();
            int selIndex = lstLegends.SelectedIndex;

            lstLegends.Items.RemoveAt(lstLegends.SelectedIndex);
            lstLegends.Items.Insert(selIndex + 1, sSel);
        }

        private void btnLayerItemSymbol_Click(object sender, EventArgs e)
        {
            try
            {
                frmSymbolSelector frm = new frmSymbolSelector();

                IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassTextSymbols, (ISymbol)btnLayerItemSymbol.Tag);
                if (frm.DialogResult != DialogResult.OK) return;
                if (item != null)
                {
                    btnLayerItemSymbol.Tag = item.Item;

                }
            }
            catch
            {
            }
        }

        private void btnLegendItemStyle_Click(object sender, EventArgs e)
        {
            //默认的
            frmSymbolSelector frm = new frmSymbolSelector();
            if (btnLegendItemStyle.Tag == null || btnLegendItemStyle.Tag is ILegendItem)
            {
                IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassLegendItems, (ISymbol)btnLegendItemStyle.Tag);
                if (frm.DialogResult != DialogResult) return;
                if (item != null)
                {
                    btnLegendItemStyle.Tag = item.Item;
                }
            }
        }

        private void btnLegendSymbol_Click(object sender, EventArgs e)
        {
            frmSymbolSelector frm = new frmSymbolSelector();
            IStyleGalleryItem item = frm.GetItem(esriSymbologyStyleClass.esriStyleClassTextSymbols, (ISymbol)btnLegendSymbol.Tag);
            if (item != null)
            {
                btnLegendSymbol.Tag = item.Item;
            }
        }

        private void CheckNumberKeyPress(string sExistValue, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 46)
            {
                if (sExistValue.IndexOf('.') >= 0)
                {
                    e.KeyChar = (char)Keys.None;
                }
                else
                    return;
            }
            //限制只能输入数字建和退格键
            if (((int)e.KeyChar > 57 | (int)e.KeyChar < 48) & (int)e.KeyChar != 8)
            {
                e.KeyChar = (char)Keys.None;
            }
            else if (((int)e.KeyChar != 8) & ((int)e.KeyChar != 46))
            {
                string AngleValue = sExistValue + e.KeyChar;
                if ((Convert.ToDouble(AngleValue) < 0) | (Convert.ToDouble(AngleValue) > 360))
                {
                    e.KeyChar = (char)Keys.None;
                }
                else
                    return;
            }
        }

        private void txtPath1_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath1.Text, e);
        }

        private void txtPath2_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath2.Text, e);
        }

        private void txtPath3_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath3.Text, e);
        }

        private void txtPath4_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath4.Text, e);
        }

        private void txtPath5_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath5.Text, e);
        }

        private void txtPath6_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath6.Text, e);
        }

        private void txtPath7_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath7.Text, e);
        }

        private void txtPath8_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath8.Text, e);
        }

        private void txtPath9_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckNumberKeyPress(txtPath9.Text, e);
        }

        private void numWidth_TextChanged(object sender, EventArgs e)
        {
            //numHeight.Text = (Convert.ToDouble(numWidth.Text) / ratio).ToString("F2");
        }

        private void numHeight_TextChanged(object sender, EventArgs e)
        {
            //numWidth.Text = (Convert.ToDouble(numHeight.Text) * ratio).ToString("F2");
        }

        private void numHeight_MouseEnter(object sender, EventArgs e)
        {
            m_bSizeChanged = true;
        }

        private void btnSetDefaultValue_Click(object sender, EventArgs e)
        {
            this.txtPath1.Text = "8";
            this.txtPath2.Text = "5";
            this.txtPath3.Text = "5";
            txtPath4.Text = "5";
            txtPath5.Text = "5";
            txtPath6.Text = "5";
            txtPath7.Text = "5";
            txtPath8.Text = "5";
            txtPath9.Text = "5";
        }


    }
}
