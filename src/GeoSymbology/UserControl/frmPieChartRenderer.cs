using System;
using System.Collections.Generic;
using System.Text;
using DevDNB = DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

namespace GeoSymbology
{
    public class frmPieChartRenderer:System.Windows.Forms.UserControl,IEditItem,IRendererUI
    {
        #region InitializeComponent

        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsUnOverlap;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbColorRamp;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelPreviewBack;
        private DevComponents.DotNetBar.PanelEx panelEx5;
        private DevComponents.DotNetBar.Controls.ListViewEx listAllField;
        private DevComponents.DotNetBar.ButtonX buttonDown;
        private DevComponents.DotNetBar.ButtonX buttonUp;
        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.Controls.ListViewEx listAddField;
        private DevComponents.DotNetBar.ButtonX buttonAdd;
        private DevComponents.DotNetBar.ButtonX buttonDel;
        private DevComponents.Editors.DoubleInput sizeInput;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPieChartRenderer));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx5 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.buttonDel = new DevComponents.DotNetBar.ButtonX();
            this.buttonAdd = new DevComponents.DotNetBar.ButtonX();
            this.listAddField = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.listAllField = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.buttonDown = new DevComponents.DotNetBar.ButtonX();
            this.buttonUp = new DevComponents.DotNetBar.ButtonX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.sizeInput = new DevComponents.Editors.DoubleInput();
            this.chkIsUnOverlap = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmbColorRamp = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelPreviewBack = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.panelEx5.SuspendLayout();
            this.panelEx3.SuspendLayout();
            this.panelEx2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeInput)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.panelEx5);
            this.panelEx1.Controls.Add(this.panelEx2);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(465, 370);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // panelEx5
            // 
            this.panelEx5.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx5.Controls.Add(this.panelEx3);
            this.panelEx5.Controls.Add(this.buttonDown);
            this.panelEx5.Controls.Add(this.buttonUp);
            this.panelEx5.Location = new System.Drawing.Point(2, 92);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(460, 272);
            this.panelEx5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx5.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx5.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelEx5.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelEx5.Style.GradientAngle = 90;
            this.panelEx5.TabIndex = 52;
            // 
            // panelEx3
            // 
            this.panelEx3.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx3.Controls.Add(this.buttonDel);
            this.panelEx3.Controls.Add(this.buttonAdd);
            this.panelEx3.Controls.Add(this.listAddField);
            this.panelEx3.Controls.Add(this.listAllField);
            this.panelEx3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEx3.Location = new System.Drawing.Point(0, 0);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(420, 272);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 5;
            // 
            // buttonDel
            // 
            this.buttonDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonDel.Image = ((System.Drawing.Image)(resources.GetObject("buttonDel.Image")));
            this.buttonDel.Location = new System.Drawing.Point(160, 112);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(30, 25);
            this.buttonDel.TabIndex = 6;
            this.buttonDel.Click += new System.EventHandler(this.buttonDel_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAdd.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdd.Image")));
            this.buttonAdd.Location = new System.Drawing.Point(160, 81);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(30, 25);
            this.buttonAdd.TabIndex = 5;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // listAddField
            // 
            // 
            // 
            // 
            this.listAddField.Border.Class = "ListViewBorder";
            this.listAddField.Dock = System.Windows.Forms.DockStyle.Right;
            this.listAddField.FullRowSelect = true;
            this.listAddField.Location = new System.Drawing.Point(198, 0);
            this.listAddField.Name = "listAddField";
            this.listAddField.Size = new System.Drawing.Size(222, 272);
            this.listAddField.TabIndex = 2;
            this.listAddField.UseCompatibleStateImageBehavior = false;
            this.listAddField.View = System.Windows.Forms.View.Details;
            this.listAddField.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listAddField_MouseDoubleClick);
            // 
            // listAllField
            // 
            // 
            // 
            // 
            this.listAllField.Border.Class = "ListViewBorder";
            this.listAllField.Dock = System.Windows.Forms.DockStyle.Left;
            this.listAllField.FullRowSelect = true;
            this.listAllField.Location = new System.Drawing.Point(0, 0);
            this.listAllField.Name = "listAllField";
            this.listAllField.Size = new System.Drawing.Size(153, 272);
            this.listAllField.TabIndex = 1;
            this.listAllField.UseCompatibleStateImageBehavior = false;
            this.listAllField.View = System.Windows.Forms.View.Details;
            this.listAllField.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DoListViewMouseDoubleClick);
            // 
            // buttonDown
            // 
            this.buttonDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonDown.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonDown.Image")));
            this.buttonDown.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.buttonDown.Location = new System.Drawing.Point(428, 115);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(22, 35);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonUp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonUp.Image")));
            this.buttonUp.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonUp.Location = new System.Drawing.Point(428, 71);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(22, 35);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.sizeInput);
            this.panelEx2.Controls.Add(this.chkIsUnOverlap);
            this.panelEx2.Controls.Add(this.cmbColorRamp);
            this.panelEx2.Controls.Add(this.labelX6);
            this.panelEx2.Controls.Add(this.labelX4);
            this.panelEx2.Controls.Add(this.labelX5);
            this.panelEx2.Controls.Add(this.labelPreviewBack);
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(459, 82);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 47;
            // 
            // sizeInput
            // 
            // 
            // 
            // 
            this.sizeInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.sizeInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.sizeInput.Increment = 1;
            this.sizeInput.Location = new System.Drawing.Point(244, 45);
            this.sizeInput.MinValue = 0.001;
            this.sizeInput.Name = "sizeInput";
            this.sizeInput.ShowUpDown = true;
            this.sizeInput.Size = new System.Drawing.Size(63, 21);
            this.sizeInput.TabIndex = 49;
            this.sizeInput.Value = 1;
            // 
            // chkIsUnOverlap
            // 
            this.chkIsUnOverlap.AutoSize = true;
            this.chkIsUnOverlap.Location = new System.Drawing.Point(67, 48);
            this.chkIsUnOverlap.Name = "chkIsUnOverlap";
            this.chkIsUnOverlap.Size = new System.Drawing.Size(76, 18);
            this.chkIsUnOverlap.TabIndex = 48;
            this.chkIsUnOverlap.Text = "·ÀÖ¹¸²¸Ç";
            // 
            // cmbColorRamp
            // 
            this.cmbColorRamp.DisplayMember = "Text";
            this.cmbColorRamp.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbColorRamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColorRamp.FormattingEnabled = true;
            this.cmbColorRamp.ItemHeight = 21;
            this.cmbColorRamp.Location = new System.Drawing.Point(67, 11);
            this.cmbColorRamp.Name = "cmbColorRamp";
            this.cmbColorRamp.Size = new System.Drawing.Size(274, 27);
            this.cmbColorRamp.TabIndex = 2;
            this.cmbColorRamp.SelectedIndexChanged += new System.EventHandler(this.cmbColorRamp_SelectedIndexChanged);
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.Location = new System.Drawing.Point(347, 16);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(19, 31);
            this.labelX6.TabIndex = 36;
            this.labelX6.Text = "±³\r\n¾°";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(207, 48);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(31, 18);
            this.labelX4.TabIndex = 10;
            this.labelX4.Text = "´óÐ¡";
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.Location = new System.Drawing.Point(10, 16);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(56, 18);
            this.labelX5.TabIndex = 11;
            this.labelX5.Text = "ÑÕÉ«·½°¸";
            // 
            // labelPreviewBack
            // 
            this.labelPreviewBack.BackColor = System.Drawing.Color.Transparent;
            this.labelPreviewBack.Location = new System.Drawing.Point(367, 11);
            this.labelPreviewBack.Name = "labelPreviewBack";
            this.labelPreviewBack.Size = new System.Drawing.Size(80, 40);
            this.labelPreviewBack.TabIndex = 12;
            this.labelPreviewBack.Click += new System.EventHandler(this.labelPreviewBack_Click);
            // 
            // frmPieChartRenderer
            // 
            this.Controls.Add(this.panelEx1);
            this.Name = "frmPieChartRenderer";
            this.Size = new System.Drawing.Size(465, 370);
            this.panelEx1.ResumeLayout(false);
            this.panelEx5.ResumeLayout(false);
            this.panelEx3.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private esriSymbologyStyleClass m_SymbologyStyleClass;
        private bool flag = false;
        private IFeatureLayer m_Layer;
        private ISymbol[] pSymbols;
        private int selnum = 0;
        public frmPieChartRenderer()
        {
            InitializeComponent();
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            m_EditObject = null;
            listAddField.SmallImageList = new System.Windows.Forms.ImageList();
            listAddField.SmallImageList.ImageSize = new System.Drawing.Size(ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);

            System.Windows.Forms.ColumnHeader column1 = new System.Windows.Forms.ColumnHeader();
            column1.Name = "Field";
            column1.Text = "×Ö¶Î";
            column1.Width = 140;
            column1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            listAllField.Columns.Add(column1);

            System.Windows.Forms.ColumnHeader column2 = new System.Windows.Forms.ColumnHeader();
            column2.Name = "Symbol";
            column2.Text = "·ûºÅ";
            column2.Width = 80;
            column2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            listAddField.Columns.Add(column2);

            column2 = new System.Windows.Forms.ColumnHeader();
            column2.Name = "Field";
            column2.Text = "×Ö¶Î";
            column2.Width = 128;
            column2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            listAddField.Columns.Add(column2);
            List<ColorItem> colorRamps = new ModuleCommon().GetColorScheme(237, 20, "Default Schemes;Spatial Ramps");
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

        private void Button_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Control control = sender as System.Windows.Forms.Control;
            switch (control.Name)
            {
                case "buttonAdd":
                    break;
                case "buttonDel":
                    break;
                case "buttonUp":
                    break;
                case "buttonDown":
                    break;
                case "labelPreviewFore":
                    m_EditObject = control;
                    Form.frmSymbolEdit foreEdit = new GeoSymbology.Form.frmSymbolEdit(this, control.Tag as ISymbol, "");
                    foreEdit.ShowDialog();
                    break;
            }
        }

        #region IEditItem ³ÉÔ±

        private object m_EditObject;

        public void DoAfterEdit(object newValue, System.Windows.Forms.DialogResult result, string editType)
        {
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                m_EditObject = null;
                return;
            }
            if (m_EditObject is System.Windows.Forms.ListViewItem)
            {
                System.Windows.Forms.ListViewItem item = m_EditObject as System.Windows.Forms.ListViewItem;
                item.Tag = newValue;
                listAddField.SmallImageList.Images.RemoveByKey(item.Name.Replace("Item", "Symbol"));
                listAddField.SmallImageList.Images.Add(item.Name.Replace("Item", "Symbol"),
                    ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                item.ImageKey = item.Name.Replace("Item", "Symbol");
                listAddField.Refresh();
            }
            if (m_EditObject is DevComponents.DotNetBar.LabelX)
            {
                DevComponents.DotNetBar.LabelX label = m_EditObject as DevComponents.DotNetBar.LabelX;
                switch (label.Name)
                {
                    case "labelPreviewBack":
                        if (label.Image != null)
                        {
                            label.Image.Dispose();
                            label.Image = null;
                        }
                        label.Tag = newValue;
                        label.Image = ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
                        RefreshSymbol();
                        break;
                }
            }
            m_EditObject = null;
        }

        #endregion
        #region IRendererUI³ÉÔ±
        public void InitRendererObject(IFeatureLayer pFeatureLayer, IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            List<FieldInfo> fields = ModuleCommon.GetFieldsFromLayer(pFeatureLayer, true);
            m_Layer = pFeatureLayer;
            InitRendererObject(fields, pRenderer, _SymbologyStyleClass);
        }

        public void InitRendererObject(List<FieldInfo> pFields, IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            listAddField.Items.Clear();
            flag = true;
            m_SymbologyStyleClass = _SymbologyStyleClass;
          
            if (!(pRenderer is IChartRenderer))//ÅÐ¶ÏÊÇ·ñÊÇÍ¼±íäÖÈ¾
                return;
            IChartRenderer pPieChartRenderer = pRenderer as IChartRenderer;
            if (pPieChartRenderer.ColorScheme != "")//ÑÕÉ«·½°¸
                cmbColorRamp.Text = pPieChartRenderer.ColorScheme;

            if (pPieChartRenderer.BaseSymbol != null)//±³¾°
                labelPreviewBack.Tag = pPieChartRenderer.BaseSymbol;
            else
                labelPreviewBack.Tag = ModuleCommon.CreateSymbol(m_SymbologyStyleClass);
            labelPreviewBack.Image = ModuleCommon.Symbol2Picture(labelPreviewBack.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
            if (pPieChartRenderer.UseOverposter)//·ÀÖ¹¸²¸Ç
                chkIsUnOverlap.Checked = true;
            else
                chkIsUnOverlap.Checked = false;
            IPieChartRenderer pPCR=pPieChartRenderer as IPieChartRenderer;
            IMarkerSymbol pMS = pPieChartRenderer.ChartSymbol as IMarkerSymbol;
            sizeInput.Value = pMS.Size;//´óÐ¡
            IRendererFields pRenderFields = pPieChartRenderer as IRendererFields;//×Ö¶Î
            ISymbolArray pSymbolArray = pPieChartRenderer.ChartSymbol as ISymbolArray;
            listAddField.SmallImageList.Images.Clear();
            List<string> fdLst = new List<string>();//¼ÇÂ¼äÖÈ¾¶ÔÏóÒÑÓÃ×Ö¶Î
            for (int i = 0; i < pRenderFields.FieldCount; i++)
            {
                listAddField.SmallImageList.Images.Add("Symbol" + i.ToString(), ModuleCommon.Symbol2Picture(pSymbolArray.get_Symbol(i), ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
                item.Name = "Item" + i;
                item.Text = "";
                item.ImageKey = "Symbol" + i.ToString();
                item.Tag = pSymbolArray.get_Symbol(i);
                System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem.Text = pRenderFields.get_Field(i);
                item.SubItems.Add(subItem);
                listAddField.Items.Add(item);
                fdLst.Add(pRenderFields.get_Field(i));

            }
            if (listAddField.SmallImageList.Images.Count >= 0)
            {
                selnum = pFields.Count;
                pSymbols = CreateSymbols(selnum);
                int ii = 0;
                foreach (FieldInfo fi in pFields)
                {
                    listAddField.SmallImageList.Images.Add("Symbol" + ii.ToString(), ModuleCommon.Symbol2Picture(pSymbols[ii], ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                    ii++;
                }
 
            }
            foreach (FieldInfo fi in pFields)
            {
                if (fi.FieldName != "<NONE>"&&!fdLst.Contains(fi.FieldName))
                    listAllField.Items.Add(fi.FieldName);
            }
            flag = false;
        }
        
        public IFeatureRenderer Renderer
        {
            get
            {
                if(listAddField.Items.Count==0)
                   return null;
                IChartRenderer pChartRenderer = new ChartRendererClass();
                pChartRenderer.ColorScheme = cmbColorRamp.Text;//É«´øÃû³Æ
                if (labelPreviewBack.Tag != null)//±³¾°·ûºÅ
                    pChartRenderer.BaseSymbol = labelPreviewBack.Tag as ISymbol;
                if (chkIsUnOverlap.Checked)//ÊÇ·ñ¸²¸Ç
                    pChartRenderer.UseOverposter = true;
                else
                    pChartRenderer.UseOverposter = false;

                IPieChartRenderer pPCR = pChartRenderer as IPieChartRenderer;
             
                IRendererFields pRenderFields = pChartRenderer as IRendererFields;//×Ö¶Î

                ISymbolArray pSymbolArray = new PieChartSymbolClass();//Í¼±í·ûºÅ
                foreach (ListViewItem lvi in listAddField.Items)
                {
                    pRenderFields.AddField(lvi.SubItems[1].Text, "");
                    pSymbolArray.AddSymbol(lvi.Tag as ISymbol);
                }
                IMarkerSymbol pMS = pSymbolArray as IMarkerSymbol;
                if(sizeInput.Value>0)
                    pMS.Size= sizeInput.Value;//´óÐ¡
                pChartRenderer.ChartSymbol = pSymbolArray as IChartSymbol;
                return pChartRenderer as IFeatureRenderer;

            }
        }

        public enumRendererType RendererType
        {
            get { return enumRendererType.ChartRenderer; }
        }

        #endregion
        private void RefreshSymbol()
        {//Ë¢ÐÂ·ûºÅÑùÊ½
            if (flag) return;
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            listAddField.SmallImageList.Images.Clear();

            for (int i = 0; i < listAddField.Items.Count; i++)
            {
                ISymbol pSymbol = copy.Copy(labelPreviewBack.Tag) as ISymbol;
                IColor pColor = ModuleCommon.GetColor(listAddField.Items[i].Tag as ISymbol);
                ModuleCommon.ChangeSymbolColor(pSymbol, pColor);
                listAddField.Items[i].Tag = pSymbol;
                listAddField.SmallImageList.Images.Add(listAddField.Items[i].Name.Replace("Item", "Symbol"),
                    ModuleCommon.Symbol2Picture(pSymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                listAddField.Items[i].ImageKey = listAddField.Items[i].Name.Replace("Item", "Symbol");
                listAddField.Refresh();
            }
        }

        private void RefreshColorRamp()
        {//Ë¢ÐÂ·ûºÅÑÕÉ«
            if (flag) return;
            IColorRamp pColorRamp = (cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem).Tag as IColorRamp;
            bool bCreateRamp;
            int count = listAddField.Items.Count + listAllField.Items.Count;
            pColorRamp.Size = count <= 1 ? 2 : count;
            pColorRamp.CreateRamp(out bCreateRamp);
            IEnumColors enumColors = pColorRamp.Colors;
            enumColors.Reset();
            listAddField.SmallImageList.Images.Clear();

            for (int i = 0; i < listAddField.Items.Count; i++)
            {
                IColor pColor = enumColors.Next();
                if (pColor == null)
                {
                    enumColors.Reset();
                    enumColors.Next();
                }

                System.Windows.Forms.ListViewItem item = listAddField.Items[i];
                ModuleCommon.ChangeSymbolColor(item.Tag as ISymbol, pColor);
                listAddField.SmallImageList.Images.Add(item.Name.Replace("Item", "Symbol"),
                    ModuleCommon.Symbol2Picture(item.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                listAddField.Items[i].ImageKey = item.Name.Replace("Item", "Symbol");
            }
            listAddField.Refresh();
        }

        private void DoListViewMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DevComponents.DotNetBar.Controls.ListViewEx listView = sender as DevComponents.DotNetBar.Controls.ListViewEx;
            System.Windows.Forms.ListViewItem item = null;
            if (item == null) return;
            switch (listView.Name)
            {
                case "listAddField":
                    {
                        item = listAddField.GetItemAt(e.X, e.Y);
                        System.Drawing.Rectangle rec = item.GetBounds(System.Windows.Forms.ItemBoundsPortion.Entire);
                        if (e.X <= listAddField.Columns[0].Width)
                        {
                            m_EditObject = item;
                            Form.frmSymbolEdit frm = new GeoSymbology.Form.frmSymbolEdit(this, item.Tag as ISymbol, "");
                            frm.ShowDialog();
                        }
                        else if (e.X > listAddField.Columns[0].Width && e.X <=
                            listAddField.Columns[0].Width + listAddField.Columns[1].Width)
                        {
                            listAddField.Items.Remove(item);
                            item.ImageKey = "";
                            listAllField.Items.Add(item);
                        }
                    }
                    break;
                case "listAllField":
                    {
                        item = listAllField.GetItemAt(e.X, e.Y);
                        listAllField.Items.Remove(item);
                        listAddField.Items.Add(item);
                        listAddField.SmallImageList.Images.Add(item.Name.Replace("Item", "Symbol"),
                            ModuleCommon.Symbol2Picture(item.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                        item.ImageKey = item.Name.Replace("Item", "Symbol");
                    }
                    break;
            }
        }
        //yjl20110826 add
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (listAllField.SelectedItems.Count == 0)
                return;
            int i =  listAddField.Items.Count;
            foreach (ListViewItem lvi in listAllField.SelectedItems)
            {

                
                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
                item.Name = "Item" + i.ToString();
                item.Text = "";
                if (i > listAddField.SmallImageList.Images.Count)
                {
                    item.ImageKey = "Symbol0";
                    item.Tag = listAddField.Items[0].Tag;
                }
                else
                {
                    item.ImageKey = "Symbol" + i.ToString();
                    item.Tag = pSymbols[i];
                }
                System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem = lvi.SubItems[0];
                subItem.Text = lvi.Text;
                item.SubItems.Add(subItem);
                listAddField.Items.Add(item);
                listAllField.Items.Remove(lvi);
                i++;
            }
        }
        //yjl20110826 add
        private ISymbol[] CreateSymbols(int count)
        {

            IColorRamp pColorRamp = (cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem).Tag as IColorRamp;
            bool bCreateRamp;
            pColorRamp.Size = count <= 1 ? 2 : count;
            pColorRamp.CreateRamp(out bCreateRamp);
            IEnumColors enumColors = pColorRamp.Colors;
            enumColors.Reset();
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            ISymbol[] symbols = new ISymbol[count];
            ISymbol tmpSymbol = ModuleCommon.CreateSymbol(m_SymbologyStyleClass);
            for (int i = 0; i < count; i++)
            {
                symbols[i] = copy.Copy(tmpSymbol) as ISymbol;
                IColor pColor = enumColors.Next();
                if (pColor == null)
                {
                    enumColors.Reset();
                    enumColors.Next();
                }
                ModuleCommon.ChangeSymbolColor(symbols[i], pColor);
            }
            return symbols;
        }

        private void labelPreviewBack_Click(object sender, EventArgs e)
        {
            if (flag == true) return;
            m_EditObject = sender as  System.Windows.Forms.Control;
            Control control = sender as System.Windows.Forms.Control;
            Form.frmSymbolEdit backEdit = new GeoSymbology.Form.frmSymbolEdit(this, control.Tag as ISymbol, "");
            backEdit.ShowDialog();
            //RefreshValue(control.Name);//yjl20110826 add
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (listAddField.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem lvi in listAddField.SelectedItems)
            {
                listAllField.Items.Add(lvi.SubItems[1].Text);
                listAddField.Items.Remove(lvi);
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if (listAddField.SelectedItems.Count != 1) return;
            System.Windows.Forms.ListViewItem item = listAddField.SelectedItems[0];
            int index = item.Index;
            if (index <= 0) return;
            listAddField.Items.Remove(item);
            listAddField.Items.Insert(index - 1, item);
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            if (listAddField.SelectedItems.Count != 1) return;
            System.Windows.Forms.ListViewItem item = listAddField.SelectedItems[0];
            int index = item.Index;
            if (index == listAddField.Items.Count - 1) return;
            listAddField.Items.Remove(item);
            listAddField.Items.Insert(index + 1, item);
        }

        private void listAddField_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.ListViewItem item = listAddField.GetItemAt(e.X, e.Y);
            if (item == null) return;

            System.Drawing.Rectangle rec = item.GetBounds(System.Windows.Forms.ItemBoundsPortion.Entire);
            if (e.X <= listAddField.Columns[0].Width)
            {
                //·ûºÅ±à¼­
                m_EditObject = item;
                Form.frmSymbolEdit frm = new GeoSymbology.Form.frmSymbolEdit(this, item.Tag as ISymbol, "");
                frm.ShowDialog();
            }
        }

        private void cmbColorRamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshColorRamp();
            pSymbols = CreateSymbols(selnum);
        }

    }
}
