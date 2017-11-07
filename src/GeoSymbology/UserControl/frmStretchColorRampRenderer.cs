using System;
using System.Collections.Generic;
using System.Text;
using DevDNB = DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using System.Drawing;

namespace GeoSymbology
{
    public class frmStretchColorRampRenderer: System.Windows.Forms.UserControl, IEditItem,IRasterRendererUI 
    {
        #region InitializeComponent

        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.Editors.DoubleInput maxValueInput;
        private DevComponents.Editors.DoubleInput minValueInput;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbColorRamp;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBackgroundColor;
        private DevComponents.DotNetBar.LabelX labelNoData;
        private DevComponents.DotNetBar.ColorPickerButton colorPickerBackGround;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ColorPickerButton colorPickerNoData;
        private DevComponents.Editors.DoubleInput dbBackgrdVal;
        private System.Windows.Forms.PictureBox pBoxColorRamp;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkReverse;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStretchColorRampRenderer));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.checkReverse = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.pBoxColorRamp = new System.Windows.Forms.PictureBox();
            this.dbBackgrdVal = new DevComponents.Editors.DoubleInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.colorPickerNoData = new DevComponents.DotNetBar.ColorPickerButton();
            this.labelNoData = new DevComponents.DotNetBar.LabelX();
            this.colorPickerBackGround = new DevComponents.DotNetBar.ColorPickerButton();
            this.checkBackgroundColor = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmbColorRamp = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.maxValueInput = new DevComponents.Editors.DoubleInput();
            this.minValueInput = new DevComponents.Editors.DoubleInput();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxColorRamp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbBackgrdVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValueInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minValueInput)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.panelEx2);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(425, 350);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.labelX4);
            this.panelEx2.Controls.Add(this.checkReverse);
            this.panelEx2.Controls.Add(this.pBoxColorRamp);
            this.panelEx2.Controls.Add(this.dbBackgrdVal);
            this.panelEx2.Controls.Add(this.labelX1);
            this.panelEx2.Controls.Add(this.colorPickerNoData);
            this.panelEx2.Controls.Add(this.labelNoData);
            this.panelEx2.Controls.Add(this.colorPickerBackGround);
            this.panelEx2.Controls.Add(this.checkBackgroundColor);
            this.panelEx2.Controls.Add(this.cmbColorRamp);
            this.panelEx2.Controls.Add(this.maxValueInput);
            this.panelEx2.Controls.Add(this.minValueInput);
            this.panelEx2.Controls.Add(this.labelX2);
            this.panelEx2.Controls.Add(this.labelX3);
            this.panelEx2.Controls.Add(this.labelX5);
            this.panelEx2.Location = new System.Drawing.Point(3, 2);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(419, 347);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 46;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(16, 14);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(44, 18);
            this.labelX4.TabIndex = 22;
            this.labelX4.Text = "颜色：";
            // 
            // checkReverse
            // 
            this.checkReverse.Location = new System.Drawing.Point(317, 181);
            this.checkReverse.Name = "checkReverse";
            this.checkReverse.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkReverse.Size = new System.Drawing.Size(99, 30);
            this.checkReverse.TabIndex = 21;
            this.checkReverse.Text = "色带反向";
            this.checkReverse.CheckedChanged += new System.EventHandler(this.checkReverse_CheckedChanged);
            // 
            // pBoxColorRamp
            // 
            this.pBoxColorRamp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pBoxColorRamp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pBoxColorRamp.Location = new System.Drawing.Point(71, 14);
            this.pBoxColorRamp.Name = "pBoxColorRamp";
            this.pBoxColorRamp.Size = new System.Drawing.Size(25, 75);
            this.pBoxColorRamp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBoxColorRamp.TabIndex = 20;
            this.pBoxColorRamp.TabStop = false;
            // 
            // dbBackgrdVal
            // 
            // 
            // 
            // 
            this.dbBackgrdVal.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dbBackgrdVal.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.dbBackgrdVal.Increment = 1;
            this.dbBackgrdVal.Location = new System.Drawing.Point(129, 144);
            this.dbBackgrdVal.Name = "dbBackgrdVal";
            this.dbBackgrdVal.Size = new System.Drawing.Size(62, 21);
            this.dbBackgrdVal.TabIndex = 19;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(230, 149);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(80, 21);
            this.labelX1.TabIndex = 18;
            this.labelX1.Text = "的颜色为";
            // 
            // colorPickerNoData
            // 
            this.colorPickerNoData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.colorPickerNoData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.colorPickerNoData.Image = ((System.Drawing.Image)(resources.GetObject("colorPickerNoData.Image")));
            this.colorPickerNoData.Location = new System.Drawing.Point(99, 190);
            this.colorPickerNoData.Name = "colorPickerNoData";
            this.colorPickerNoData.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.colorPickerNoData.Size = new System.Drawing.Size(37, 23);
            this.colorPickerNoData.TabIndex = 17;
            // 
            // labelNoData
            // 
            this.labelNoData.Location = new System.Drawing.Point(16, 190);
            this.labelNoData.Name = "labelNoData";
            this.labelNoData.Size = new System.Drawing.Size(80, 21);
            this.labelNoData.TabIndex = 16;
            this.labelNoData.Text = "无效数据颜色";
            // 
            // colorPickerBackGround
            // 
            this.colorPickerBackGround.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.colorPickerBackGround.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.colorPickerBackGround.Image = ((System.Drawing.Image)(resources.GetObject("colorPickerBackGround.Image")));
            this.colorPickerBackGround.Location = new System.Drawing.Point(351, 144);
            this.colorPickerBackGround.Name = "colorPickerBackGround";
            this.colorPickerBackGround.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.colorPickerBackGround.Size = new System.Drawing.Size(37, 23);
            this.colorPickerBackGround.TabIndex = 15;
            // 
            // checkBackgroundColor
            // 
            this.checkBackgroundColor.Location = new System.Drawing.Point(16, 144);
            this.checkBackgroundColor.Name = "checkBackgroundColor";
            this.checkBackgroundColor.Size = new System.Drawing.Size(99, 30);
            this.checkBackgroundColor.TabIndex = 13;
            this.checkBackgroundColor.Text = "显示背景值:";
            this.checkBackgroundColor.CheckedChanged += new System.EventHandler(this.checkBackgroundColor_CheckedChanged);
            // 
            // cmbColorRamp
            // 
            this.cmbColorRamp.DisplayMember = "Text";
            this.cmbColorRamp.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbColorRamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColorRamp.FormattingEnabled = true;
            this.cmbColorRamp.ItemHeight = 21;
            this.cmbColorRamp.Location = new System.Drawing.Point(71, 91);
            this.cmbColorRamp.Name = "cmbColorRamp";
            this.cmbColorRamp.Size = new System.Drawing.Size(317, 27);
            this.cmbColorRamp.TabIndex = 2;
            this.cmbColorRamp.SelectedIndexChanged += new System.EventHandler(this.cmbColorRamp_SelectedIndexChanged);
            // 
            // maxValueInput
            // 
            // 
            // 
            // 
            this.maxValueInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.maxValueInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.maxValueInput.Enabled = false;
            this.maxValueInput.Increment = 1;
            this.maxValueInput.Location = new System.Drawing.Point(267, 11);
            this.maxValueInput.Name = "maxValueInput";
            this.maxValueInput.Size = new System.Drawing.Size(122, 21);
            this.maxValueInput.TabIndex = 3;
            // 
            // minValueInput
            // 
            // 
            // 
            // 
            this.minValueInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.minValueInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.minValueInput.Enabled = false;
            this.minValueInput.Increment = 1;
            this.minValueInput.Location = new System.Drawing.Point(267, 65);
            this.minValueInput.Name = "minValueInput";
            this.minValueInput.Size = new System.Drawing.Size(122, 21);
            this.minValueInput.TabIndex = 2;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(216, 68);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(44, 18);
            this.labelX2.TabIndex = 8;
            this.labelX2.Text = "最小值";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Location = new System.Drawing.Point(216, 14);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(44, 18);
            this.labelX3.TabIndex = 9;
            this.labelX3.Text = "最大值";
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.Location = new System.Drawing.Point(16, 100);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(56, 18);
            this.labelX5.TabIndex = 11;
            this.labelX5.Text = "颜色方案";
            // 
            // frmStretchColorRampRenderer
            // 
            this.Controls.Add(this.panelEx1);
            this.Name = "frmStretchColorRampRenderer";
            this.Size = new System.Drawing.Size(425, 350);
            this.panelEx1.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxColorRamp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbBackgrdVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValueInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minValueInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private bool flag = false;
        private ILayer  m_Layer;
        public frmStretchColorRampRenderer()
        {
            InitializeComponent();
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            m_EditObject = null;

            List<ColorItem> colorRamps = new ModuleCommon().GetColorScheme(297, 20, "Default Ramps;Dichromatic Ramps");
            for (int i = 0; i < colorRamps.Count; i++)
            {
                DevComponents.Editors.ComboItem item = new DevComponents.Editors.ComboItem();
                item.Image = colorRamps[i].ColorImage;
                item.Text = colorRamps[i].Name;
                item.Tag = colorRamps[i].ColorRamp;
                cmbColorRamp.Items.Add(item);
            }
            flag = true;
            cmbColorRamp.SelectedIndex = 0;
            flag = false;
        }

        private void Control_Click(object sender, EventArgs e)
        {
            if (flag == true) return;
            System.Windows.Forms.Control control = sender as System.Windows.Forms.Control;
            switch (control.Name)
            {
                case "labelPreviewFore":
                    m_EditObject = control;
                    Form.frmSymbolEdit foreEdit = new GeoSymbology.Form.frmSymbolEdit(this, control.Tag as ISymbol, "");
                    foreEdit.ShowDialog();
                    break;
            }
        }


        
        #region IEditItem 成员

        private object m_EditObject;

        public void DoAfterEdit(object newValue, System.Windows.Forms.DialogResult result, string editType)
        {
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                m_EditObject = null;
                return;
            }
            if (m_EditObject is System.Windows.Forms.ListViewItem.ListViewSubItem)
            {
                System.Windows.Forms.ListViewItem.ListViewSubItem subItem =
                    m_EditObject as System.Windows.Forms.ListViewItem.ListViewSubItem;
                if (subItem.Name.Contains("Range"))//范围编辑
                {
                    #region Range
                    //更新当前编辑对象（Range）的值
                    subItem.Tag = newValue;
                    subItem.Text = subItem.Text.Split('-')[0] + "-" + newValue.ToString();
                    //更新对应的Label的值
                    string nameIndex = subItem.Name.Replace("Range", "");
                    #endregion
                }
                else if (subItem.Name.Contains("Label"))//标签编辑
                {
                    if (newValue.ToString() != "")
                    {
                        subItem.Tag = newValue;
                        subItem.Text = newValue.ToString();
                    }
                }
            }
            if (m_EditObject is System.Windows.Forms.ListViewItem)
            {
                System.Windows.Forms.ListViewItem item = m_EditObject as System.Windows.Forms.ListViewItem;
                item.Tag = newValue;
                ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
                item.ImageKey = "Symbol" + item.Index.ToString();
            }
            if (m_EditObject is DevComponents.DotNetBar.LabelX)
            {
                DevComponents.DotNetBar.LabelX label = m_EditObject as DevComponents.DotNetBar.LabelX;
                switch (label.Name)
                {
                    case "labelPreviewFore":
                        if (label.Image != null)
                        {
                            label.Image.Dispose();
                            label.Image = null;
                        }
                        label.Tag = newValue;
                        label.Image = ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
                        //RefreshSymbol();
                        break;
                }
            }
            m_EditObject = null;
        }

        #endregion

        public void InitRasterRendererObject(ILayer  pLayer, IRasterRenderer  pRenderer)
        {
            m_Layer = pLayer;
            InitRasterRendererObject(pLayer as IRasterLayer, pRenderer);
        }
        //yjl 20110827 modify 根据渲染对象初始化界面
        public void InitRasterRendererObject(IRasterLayer inRL, IRasterRenderer  pRenderer)
        {
            flag = true;
            IRaster pRaster=inRL.Raster;
            IRasterBandCollection pRBC=pRaster as IRasterBandCollection;
            IRasterBand pRB=pRBC.Item(0);//获取第一波段
            bool hasStatis=false;
            pRB.HasStatistics(out hasStatis);
            if(!hasStatis)
                pRB.ComputeStatsAndHist();//统计直方图
            IRasterStatistics pRStatis=pRB.Statistics;
            minValueInput.Value=pRStatis.Minimum;//最大最小值
            maxValueInput.Value=pRStatis.Maximum;

            IRasterStretchColorRampRenderer pRSCRRenderer = pRenderer as IRasterStretchColorRampRenderer;
            if (pRSCRRenderer.ColorScheme != "")
                cmbColorRamp.Text = pRSCRRenderer.ColorScheme;//颜色方案
            DevComponents.Editors.ComboItem selItem=cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem;
            Bitmap bmp = new Bitmap(selItem.Image);
            bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pBoxColorRamp.Image= bmp;
            //txtMaxLb.Text=pRSCRRenderer.LabelHigh;//标签
            //txtMinLb.Text=pRSCRRenderer.LabelLow;
            IRasterStretch pRStretch = pRSCRRenderer as IRasterStretch;
            if (pRStretch.Background)//背景值
            {
                checkBackgroundColor.Checked = true;
                dbBackgrdVal.Value = pRStretch.get_BackgroundValues();
                IColor pColor = pRStretch.BackgroundColor;//esri color
                System.Drawing.Color bakColor = cWinColor(pColor);
                colorPickerBackGround.SelectedColor = bakColor;//背景值颜色
            }
            else
            {
                checkBackgroundColor.Checked = false;
                dbBackgrdVal.Enabled = false;
            }
            if (pRStretch.Invert)//色带方向
                checkReverse.Checked = true;
            else
                checkReverse.Checked = false;
            IRasterDisplayProps pRDP = pRSCRRenderer as IRasterDisplayProps;
            System.Drawing.Color nodataColor = cWinColor(pRDP.NoDataColor);
            colorPickerNoData.SelectedColor = nodataColor;//无效值颜色

            
            flag = false;
        }
        public void InitRasterRendererObject(List<FieldInfo> pFieldInfo, IRasterRenderer pRenderer)
        {
            flag = true;

            IRasterStretchColorRampRenderer pBreakRenderer = pRenderer as IRasterStretchColorRampRenderer;

            flag = false;
        }
        //yjl20110827 modify 栅格渲染对象属性
        public IRasterRenderer  RasterRenderer
        {
            get 
            {
                IRaster pRaster = (m_Layer as IRasterLayer).Raster;
                IRasterStretchColorRampRenderer pRenderer = new RasterStretchColorRampRendererClass();
                pRenderer.BandIndex = 0;//波段
                pRenderer.ColorScheme = cmbColorRamp.Text;//颜色方案
                //pRenderer.LabelHigh = txtMaxLb.Text;//标签
                //pRenderer.LabelLow = txtMinLb.Text;
                IRasterStretch pRStretch = pRenderer as IRasterStretch;
                if (checkBackgroundColor.Checked)//背景颜色
                {
                    pRStretch.Background = true;
                    double bakVal=dbBackgrdVal.Value;
                    pRStretch.set_BackgroundValues(ref bakVal);
                    System.Drawing.Color color=colorPickerBackGround.SelectedColor;
                    pRStretch.BackgroundColor = cEsriColor(color);

                }
                else
                {
                    pRStretch.Background = false;
 
                }
                IRasterDisplayProps pRDP = pRenderer as IRasterDisplayProps;
                pRDP.NoDataColor = cEsriColor(colorPickerNoData.SelectedColor);//无效值颜色
                if (checkReverse.Checked)//色带方向
                {
                    pRStretch.Invert = true;
                }
                else
                {
                    pRStretch.Invert = false;
 
                }
                IRasterRenderer pRR = pRenderer as IRasterRenderer;
                pRR.Raster = pRaster;
                if (pRR.Updated)
                    pRR.Update();
 
                return pRenderer as IRasterRenderer;
            }
        }
        //yjl 20110827 add 
        private IColor cEsriColor(System.Drawing.Color color)
        {
            IRgbColor rgb = new RgbColorClass();
            rgb.Red=Convert.ToInt32(color.R);
            rgb.Green=Convert.ToInt32(color.G);
            rgb.Blue=Convert.ToInt32(color.B);
            return rgb as IColor;
 
        }
        //yjl 20110827 add 
        private System.Drawing.Color cWinColor(IColor incolor)
        {
            if (!(incolor is IRgbColor))
                return System.Drawing.Color.AliceBlue;
            IRgbColor color = incolor as IRgbColor;
            System.Drawing.Color rgb = System.Drawing.Color.FromArgb(
                color.Red,color.Green,color.Blue);

            //rgb. = Convert.ToByte(color.Red);
            //rgb.G = Convert.ToByte(color.Green);
            //rgb.B = Convert.ToByte(color.Blue);
            return rgb;

        }
        public enumRasterRendererType RasterRendererType
        {
            get { return enumRasterRendererType.StretchColorRampRenderer; }
        }


        private void RefreshColorRamp()
        {//刷新符号颜色
            if (flag) return;
            IColorRamp pColorRamp = (cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem).Tag as IColorRamp;
            bool bCreateRamp;
            pColorRamp.Size = 5;
            pColorRamp.CreateRamp(out bCreateRamp);
            IEnumColors enumColors = pColorRamp.Colors;
            enumColors.Reset();
        }

        private void checkBackgroundColor_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBackgroundColor.Checked)
                dbBackgrdVal.Enabled = true;
            else
                dbBackgrdVal.Enabled = false;
        }

        private void checkReverse_CheckedChanged(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem selItem = cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem;
            Bitmap bmp = new Bitmap(selItem.Image);
            if (checkReverse.Checked)
            {
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pBoxColorRamp.Image = bmp;
            }
            else
            {
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                pBoxColorRamp.Image = bmp;
 
            }
        }

        private void cmbColorRamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem selItem = cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem;
            Bitmap bmp = new Bitmap(selItem.Image);
            if (checkReverse.Checked)
            {
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pBoxColorRamp.Image = bmp;
            }
            else
            {
                bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                pBoxColorRamp.Image = bmp;

            }
        }
    }
}
