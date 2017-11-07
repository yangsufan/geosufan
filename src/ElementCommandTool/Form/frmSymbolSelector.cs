using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace ElementCommandTool
{
    public partial class frmSymbolSelector : DevComponents.DotNetBar.Office2007Form
    {
        public IStyleGalleryItem m_styleGalleryItem;
        private esriSymbologyStyleClass m_pStyle;

        public frmSymbolSelector()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            numLineOffSet.Minimum = -50;
            numLineOffSet.Maximum = 50;
            numLineWidth.Minimum = (decimal)0.1;
            numLineWidth.Maximum = 12;
            numOutLineWidth.Minimum = 0;
            numOutLineWidth.Maximum = 12;
            numPtSize.Minimum = (decimal)0.1;
            numPtSize.Maximum = 32;
            numPtXOffSet.Minimum = -32;
            numPtXOffSet.Maximum = 32;
            numPtYOffSet.Minimum = -32;
            numPtYOffSet.Maximum = 32;
            numTextSize.Minimum = 5;
            numTextSize.Maximum = 72;
            numTextSize.DecimalPlaces = 2;
            //numTextSize.Value = 10;
        }

        /// <summary>
        /// 从符号库ServerStyle选择符号
        /// </summary>
        /// <param name="styleClass">符号类型</param>
        /// <param name="pSymbol">输入的符号，可以是空</param>
        /// <returns></returns>
        public IStyleGalleryItem GetItem(esriSymbologyStyleClass styleClass, object pSymbol)
        {
            SetGroupVisible(5);
            m_pStyle = styleClass;
            axSymbologyControl1.StyleClass = m_pStyle;
            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(styleClass);
            if (pSymbol != null)
            {
                try
                {
                    m_styleGalleryItem = new ServerStyleGalleryItemClass();
                    m_styleGalleryItem.Item = pSymbol;
                    m_styleGalleryItem.Name = "tempSymbo";

                    PreviewImage(m_styleGalleryItem);
                    symbologyStyleClass.AddItem(m_styleGalleryItem, 0);
                    symbologyStyleClass.SelectItem(0);
                }
                catch
                {
                    
                }
            }
            this.ShowDialog();

            return m_styleGalleryItem;
        }

        private void axSymbologyControl1_OnItemSelected(object sender, ISymbologyControlEvents_OnItemSelectedEvent e)
        {
            m_styleGalleryItem = (IStyleGalleryItem)e.styleGalleryItem;
            PreviewColorSet();            
        }

        private void PreviewColorSet()
        {
            PreviewImage(m_styleGalleryItem);
            grpSymbol.Tag = m_styleGalleryItem.Item;

            Color pColor;
            IRgbColor pRgbColor;
            if (m_styleGalleryItem.Item is IMarkerSymbol)
            {
                IMarkerSymbol pMrkSymbol = (IMarkerSymbol)m_styleGalleryItem.Item;
                pRgbColor = PublicClass.GetRGBColor(pMrkSymbol.Color);
                pColor = Color.FromArgb(pRgbColor.Transparency, pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);

                colorPoint.Color = pColor;
                colorPoint.ForeColor = pColor;

                if (pRgbColor.Transparency == 0)
                {
                    colorPoint.Color = Color.Transparent;
                    colorPoint.ForeColor = Color.Transparent;
                }

                numPtSize.Value = (decimal)pMrkSymbol.Size;
                numPtXOffSet.Value = (decimal)pMrkSymbol.XOffset;
                numPtYOffSet.Value = (decimal)pMrkSymbol.YOffset;
                SetGroupVisible(1);
            }
            else if (m_styleGalleryItem.Item is ILineSymbol)
            {
                ILineSymbol pLineSymbol = (ILineSymbol)m_styleGalleryItem.Item;
                pRgbColor = PublicClass.GetRGBColor(pLineSymbol.Color);
                pColor = Color.FromArgb(pRgbColor.Transparency, pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);

                colorLine.Color = pColor;
                colorLine.ForeColor = pColor;

                if (pRgbColor.Transparency == 0)
                {
                    colorLine.Color = Color.Transparent;
                    colorLine.ForeColor = Color.Transparent;
                }

                numLineWidth.Value = (decimal)pLineSymbol.Width;
                ILineProperties pLineProp = pLineSymbol as ILineProperties;
                if (pLineProp != null)
                    numLineOffSet.Value = (decimal)pLineProp.Offset;
                else
                    numLineOffSet.Value = 0;
                SetGroupVisible(2);
            }
            else if (m_styleGalleryItem.Item is IFillSymbol)
            {
                IFillSymbol pFillSymbol = (IFillSymbol)m_styleGalleryItem.Item;
                pRgbColor = PublicClass.GetRGBColor(pFillSymbol.Color);
                pColor = Color.FromArgb(pRgbColor.Transparency, pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);

                colorPolygon.Color = pColor;
                colorPolygon.ForeColor = pColor;

                if (pRgbColor.Transparency == 0)
                {
                    colorPolygon.Color = Color.Transparent;
                    colorPolygon.ForeColor = Color.Transparent; ;
                }

                if (pFillSymbol.Outline != null)
                {
                    ILineSymbol pOutLine = pFillSymbol.Outline;
                    pRgbColor = PublicClass.GetRGBColor(pOutLine.Color);
                    pColor = Color.FromArgb(pRgbColor.Transparency, pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);

                    colorBorder.Color = pColor;
                    colorBorder.ForeColor = pColor;

                    if (pRgbColor.Transparency == 0)
                    {
                        colorBorder.Color = Color.Transparent;
                        colorBorder.ForeColor = Color.Transparent;
                    }

                    numOutLineWidth.Value = (decimal)pOutLine.Width;
                }
                else
                {
                    btnOutLineColor.BackColor = this.BackColor;
                    numOutLineWidth.Value = 0;
                }
                SetGroupVisible(3);
            }
            else if (m_styleGalleryItem.Item is ITextSymbol)
            {
                ITextSymbol pTextSymbol = (ITextSymbol)m_styleGalleryItem.Item;
                pRgbColor = PublicClass.GetRGBColor(pTextSymbol.Color);
                pColor = Color.FromArgb(pRgbColor.Transparency, pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);

                colorAnno.Color = pColor;
                colorAnno.ForeColor = pColor;
                SetGroupVisible(4);

                stdole.IFontDisp pFontDisp = pTextSymbol.Font;
                toolBarStyle.Buttons["toolBlod"].Pushed = pFontDisp.Bold;
                toolBarStyle.Buttons["toolItalic"].Pushed = pFontDisp.Italic;
                toolBarStyle.Buttons["toolUnderline"].Pushed = pFontDisp.Underline;

                ToolBarButton btn;
                for (int i = 0; i < toolBarAlign.Buttons.Count; i++)
                {
                    btn = toolBarAlign.Buttons[i];
                    btn.Pushed = false;
                }

                if (pTextSymbol.HorizontalAlignment == esriTextHorizontalAlignment.esriTHALeft)
                {
                    btn = toolBarAlign.Buttons["toolLeft"];
                    btn.Pushed = true;
                }
                else if (pTextSymbol.HorizontalAlignment == esriTextHorizontalAlignment.esriTHACenter)
                {
                    btn = toolBarAlign.Buttons["toolCenter"];
                    btn.Pushed = true;
                }
                else if (pTextSymbol.HorizontalAlignment == esriTextHorizontalAlignment.esriTHARight)
                {
                    btn = toolBarAlign.Buttons["toolRight"];
                    btn.Pushed = true;
                }
                else
                {
                    btn = toolBarAlign.Buttons["toolFull"];
                    btn.Pushed = true;
                }

                cmbTextFont.Text = pFontDisp.Name;
                numTextSize.Value = (decimal)pTextSymbol.Size;
                anGle.Value = (decimal)pTextSymbol.Angle;
                IFormattedTextSymbol pFTextSymbol = pTextSymbol as IFormattedTextSymbol;
                if (pFTextSymbol != null)
                {
                    if (pFTextSymbol.Background != null)
                    {
                        if (pFTextSymbol.Background is IBalloonCallout)
                        {
                            fillGroup.Visible = true;
                            IBalloonCallout pBallCallout = pFTextSymbol.Background as IBalloonCallout;
                            IFillSymbol pFillSymbol = pBallCallout.Symbol;
                            if (pFillSymbol != null)
                            {
                                pRgbColor = pFillSymbol.Color as IRgbColor;
                                l_ColorForPolygon.BackColor = ColorTranslator.FromOle(pRgbColor.RGB);

                                ILineSymbol pLineSymbol = pFillSymbol.Outline;
                                if (pLineSymbol != null)
                                {
                                    pRgbColor = pLineSymbol.Color as IRgbColor;
                                    l_ColorForLine.ForeColor = ColorTranslator.FromOle(pRgbColor.RGB);
                                    txt_LineWidth.Text = pLineSymbol.Width.ToString();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                SetGroupVisible(5);
            }
            cmbTextFont.SelectedIndexChanged+=new EventHandler(cmbTextFont_SelectedIndexChanged);
            numTextSize.ValueChanged += new EventHandler(numTextSize_ValueChanged);
            txt_LineWidth.TextChanged += new EventHandler(txt_LineWidth_TextChanged);
            anGle.ValueChanged += new EventHandler(anGle_ValueChanged);
        }

        private void SetGroupVisible(int type)
        {
            switch (type)
            {
                case 1:
                    grpPoint.Visible = true;
                    grpLine.Visible = false;
                    grpPoly.Visible = false;
                    grpText.Visible = false;
                    break;
                case 2:
                    grpPoint.Visible = false;
                    grpLine.Visible = true;
                    grpPoly.Visible = false;
                    grpText.Visible = false;
                    break;
                case 3:
                    grpPoint.Visible = false;
                    grpLine.Visible = false;
                    grpPoly.Visible = true;
                    grpText.Visible = false;
                    break;
                case 4:
                    grpPoint.Visible = false;
                    grpLine.Visible = false;
                    grpPoly.Visible = false;
                    grpText.Visible = true;
                    break;
                case 5:
                    grpPoint.Visible = false;
                    grpLine.Visible = false;
                    grpPoly.Visible = false;
                    grpText.Visible = false;
                    break;
            }
        }

        private void PreviewImage(IStyleGalleryItem item)
        {
            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
            if (symbologyStyleClass.StyleClass == esriSymbologyStyleClass.esriStyleClassScaleBars)
            {
                stdole.IPictureDisp picture = symbologyStyleClass.PreviewItem(item, pictureBox1.Width*2, pictureBox1.Height);
                System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                pictureBox1.Image = image;
            }
            else
            {
                stdole.IPictureDisp picture = symbologyStyleClass.PreviewItem(item, pictureBox1.Width, pictureBox1.Height);
                System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                pictureBox1.Image = image;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_styleGalleryItem = null;
            this.Hide();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void frmSymbolSelector_Load(object sender, EventArgs e)
        {
            string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
            if (sInstall == "") //added by chulili 2012-11-13 平台由ArcGIS9.3换成ArcGIS10，相应的注册表路径要修改
            {
                sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
            }
            if (sInstall == "")
            {
                sInstall =ReadRegistry("SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
            }   //added by chulili 2012-11-13  end
            DirectoryInfo pDic = new DirectoryInfo(sInstall + "\\Styles\\");
            FileInfo[] files = pDic.GetFiles("*.ServerStyle");
            cmbStyle.Items.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                cmbStyle.Items.Add(sInstall + "\\Styles\\" + files[i].ToString());
            }
            int index = cmbStyle.Items.IndexOf(sInstall + "\\Styles\\ESRI.ServerStyle");
            if (index != -1) cmbStyle.SelectedIndex = index;
            List<string> TextStyle = new List<string>();
            System.Drawing.Text.InstalledFontCollection CollectionFont = new System.Drawing.Text.InstalledFontCollection();
            FontFamily[] Familys = CollectionFont.Families;
            for (int i = 0; i < Familys.Length; i++)
            {
                TextStyle.Add(Familys[i].Name);
            }
            cmbTextFont.Items.AddRange(TextStyle.ToArray());
        }

        private string ReadRegistry(string sKey)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sKey, true);
            if (rk == null) return "";

            return (string)rk.GetValue("InstallDir");
        }

        private void btnAddStyle_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Engine符号库文件 (*.ServerStyle)|*.ServerStyle|ArcGIS符号库文件 (*.Style)|*.Style";
            openFileDialog1.Title = "选择符号库";
            openFileDialog1.ShowReadOnly = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                cmbStyle.Items.Add(openFileDialog1.FileName);
                cmbStyle.Text = openFileDialog1.FileName;
            }
        }

        private void cmbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            axSymbologyControl1.Clear();
            if (cmbStyle.SelectedItem.ToString().Contains("ServerStyle"))
                axSymbologyControl1.LoadStyleFile(cmbStyle.SelectedItem.ToString());
            else
                axSymbologyControl1.LoadDesktopStyleFile(cmbStyle.SelectedItem.ToString());
            axSymbologyControl1.StyleClass = m_pStyle;
            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
        }

        private void ChangeMrkSymbol()
        {
            IMarkerSymbol pMrkSymbol = grpSymbol.Tag as IMarkerSymbol;
            if (pMrkSymbol == null) return;

            pMrkSymbol.Size = (double)numPtSize.Value;
            Color pColor = colorPoint.Color;
            pMrkSymbol.Color = PublicClass.GetRGBColor(colorPoint);

            pMrkSymbol.XOffset = (double)numPtXOffSet.Value;
            pMrkSymbol.YOffset = (double)numPtYOffSet.Value;
            grpSymbol.Tag = pMrkSymbol;
            IStyleGalleryItem item = new ServerStyleGalleryItemClass();
            item.Name = "tempSymbol";
            item.Item = pMrkSymbol;

            PreviewImage(item);
            m_styleGalleryItem = item;
        }

        private void ChangeLineSymbol()
        {
            ILineSymbol pLineSymbol = grpSymbol.Tag as ILineSymbol;
            if (pLineSymbol == null) return;

            Color pColor = colorLine.Color;
            pLineSymbol.Color = PublicClass.GetRGBColor(colorLine);

            pLineSymbol.Width = (double)numLineWidth.Value;
            ILineProperties pLineProp = pLineSymbol as ILineProperties;
            if (pLineProp != null)
                pLineProp.Offset = (double)numLineOffSet.Value;

            grpSymbol.Tag = pLineSymbol;
            IStyleGalleryItem item = new ServerStyleGalleryItemClass();
            item.Name = "tempSymbol";
            item.Item = pLineSymbol;

            PreviewImage(item);
            m_styleGalleryItem = item;
        }

        private void ChangeFillSymbol()
        {
            IFillSymbol pFillSymbol = grpSymbol.Tag as IFillSymbol;
            if (pFillSymbol == null) return;

            Color pColor = colorPolygon.Color;

            pFillSymbol.Color = PublicClass.GetRGBColor(colorPolygon);

            if (numOutLineWidth.Value < (decimal)0.01)
            {
                if (pFillSymbol.Outline != null)
                    pFillSymbol.Outline.Width = 0;
            }
            else
            {
                ISimpleLineSymbol pOutLine = new SimpleLineSymbolClass();

                pColor = colorBorder.Color;
                pOutLine.Color = PublicClass.GetRGBColor(colorBorder);

                pOutLine.Width = (double)numOutLineWidth.Value;
                pOutLine.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = (ILineSymbol)pOutLine;
            }

            grpSymbol.Tag = pFillSymbol;
            IStyleGalleryItem item = new ServerStyleGalleryItemClass();
            item.Name = "tempSymbol";
            item.Item = pFillSymbol;

            PreviewImage(item);
            m_styleGalleryItem = item;
        }

        private void ChangeTextSymbol()
        {
            ITextSymbol pTextSymbol = grpSymbol.Tag as ITextSymbol;
            if (pTextSymbol == null) return;

            //得到注记颜色
            System.Drawing.Color pColor = btnTextColor.BackColor;
            pColor = colorAnno.Color;
            if (colorAnno.Color == Color.Transparent)
            {
                pTextSymbol.Color.Transparency = 0;
            }
            else
            {
                pTextSymbol.Color.Transparency = 255;
            }
            pTextSymbol.Color = PublicClass.GetRGBColor(pColor.R, pColor.G, pColor.B);

            //得到注记字体格式
            stdole.IFontDisp pFontDisp = (stdole.IFontDisp)(new stdole.StdFontClass());
            pFontDisp.Name = cmbTextFont.Text;
            pFontDisp.Bold = toolBarStyle.Buttons["toolBlod"].Pushed;
            pFontDisp.Italic = toolBarStyle.Buttons["toolItalic"].Pushed;
            pFontDisp.Underline = toolBarStyle.Buttons["toolUnderline"].Pushed;
            pFontDisp.Size = numTextSize.Value;

            pTextSymbol.Font = pFontDisp;

            //得到注记对齐格式
            if (toolBarAlign.Buttons["toolLeft"].Pushed)
                pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            else if (toolBarAlign.Buttons["toolCenter"].Pushed)
                pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            else if (toolBarAlign.Buttons["toolRight"].Pushed)
                pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
            else
                pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull;

            pTextSymbol.Angle = (double)anGle.Value;
            //设置背景
            IFormattedTextSymbol pFTextSymbol = pTextSymbol as IFormattedTextSymbol;
            if (pFTextSymbol != null)
            {
                if (pFTextSymbol.Background != null)
                {
                    if (pFTextSymbol.Background is IBalloonCallout)
                    {
                        IBalloonCallout pBallCallout = pFTextSymbol.Background as IBalloonCallout;
                        IFillSymbol pFillSymbol = pBallCallout.Symbol;
                        if (pFillSymbol != null)
                        {
                            pFillSymbol.Color = PublicClass.GetRGBColor(l_ColorForPolygon.BackColor.R, l_ColorForPolygon.BackColor.G, l_ColorForPolygon.BackColor.B);

                            ILineSymbol pLineSymbol = pFillSymbol.Outline;
                            if (pLineSymbol != null)
                            {
                                pLineSymbol.Color = PublicClass.GetRGBColor(l_ColorForLine.ForeColor.R, l_ColorForLine.ForeColor.G, l_ColorForLine.ForeColor.B);
                                if (!string.IsNullOrEmpty(txt_LineWidth.Text))
                                {
                                    pLineSymbol.Width = double.Parse(txt_LineWidth.Text);
                                }
                                pFillSymbol.Outline = pLineSymbol;
                            }
                        }
                    }
                }
            }
            grpSymbol.Tag = pTextSymbol;
            IStyleGalleryItem item = new ServerStyleGalleryItemClass();
            item.Name = "tempSymbol";
            item.Item = pTextSymbol;

            PreviewImage(item);
            m_styleGalleryItem = item;
        }

        private void numPtSize_ValueChanged(object sender, EventArgs e)
        {
            ChangeMrkSymbol();
        }

        private void numPtXOffSet_ValueChanged(object sender, EventArgs e)
        {
            ChangeMrkSymbol();
        }

        private void numPtYOffSet_ValueChanged(object sender, EventArgs e)
        {
            ChangeMrkSymbol();
        }

        private void numLineWidth_ValueChanged(object sender, EventArgs e)
        {
            ChangeLineSymbol();
        }

        private void numLineOffSet_ValueChanged(object sender, EventArgs e)
        {
            ChangeLineSymbol();
        }

        private void numOutLineWidth_ValueChanged(object sender, EventArgs e)
        {
            ChangeFillSymbol();
        }

        private void cmbTextFont_ValueChanged(object sender, EventArgs e)
        {
            ChangeTextSymbol();
        }

        private void toolBarStyle_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ToolBarButton btn = e.Button;
            btn.Pushed = !btn.Pushed;
            ChangeTextSymbol();
        }

        private void toolBarAlign_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ToolBarButton btn;
            for (int i = 0; i < toolBarAlign.Buttons.Count; i++)
            {
                btn = toolBarAlign.Buttons[i];
                btn.Pushed = false;
            }
            btn = e.Button;
            btn.Pushed = true;
            ChangeTextSymbol();
        }

        private void cmbTextFont_ValueMemberChanged(object sender, EventArgs e)
        {
            ChangeTextSymbol();
        }

        private void colorAnno_Changed(object sender, EventArgs e)
        {
            ChangeTextSymbol();
        }

        private void colorBorder_Changed(object sender, EventArgs e)
        {
            ChangeFillSymbol();
        }

        private void colorLine_Changed(object sender, EventArgs e)
        {
            ChangeLineSymbol();
        }

        private void colorPoint_Changed(object sender, EventArgs e)
        {
            ChangeMrkSymbol();
        }

        private void colorPolygon_Changed(object sender, EventArgs e)
        {
            ChangeFillSymbol();
        }

        private void cmb_ColorForLine_Click(object sender, EventArgs e)
        {
            l_ColorForLine.ForeColor = GetColor(l_ColorForLine.ForeColor);
            ChangeTextSymbol();
        }

        private void cmb_ColorForPolygon_Click(object sender, EventArgs e)
        {
            l_ColorForPolygon.BackColor = GetColor(l_ColorForPolygon.BackColor);
            ChangeTextSymbol();
        }

        private Color GetColor(Color pColor)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.AnyColor = true;
            colorDlg.Color = pColor;
            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return colorDlg.Color;
            }
            else
            {
                return pColor;
            }
        }

        private void cmbTextFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTextSymbol();
        }

        void numTextSize_ValueChanged(object sender, EventArgs e)
        {
            ChangeTextSymbol();
        }

        void txt_LineWidth_TextChanged(object sender, EventArgs e)
        {
            ChangeTextSymbol();
        }

        void anGle_ValueChanged(object sender, EventArgs e)
        {
            ChangeTextSymbol();
        }
    }
}
