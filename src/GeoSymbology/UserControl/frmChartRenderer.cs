using System;
using System.Collections.Generic;
using System.Text;
using DevDNB = DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace GeoSymbology
{
    public class frmChartRenderer:System.Windows.Forms.UserControl,IEditItem,IRendererUI
    {
        #region InitializeComponent

        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsUnOverlap;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbColorRamp;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelPreviewFore;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChartRenderer));
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
            this.chkIsUnOverlap = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.cmbColorRamp = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelPreviewFore = new DevComponents.DotNetBar.LabelX();
            this.sizeInput = new DevComponents.Editors.DoubleInput();
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
            // panelEx5
            // 
            this.panelEx5.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx5.Controls.Add(this.panelEx3);
            this.panelEx5.Controls.Add(this.buttonDown);
            this.panelEx5.Controls.Add(this.buttonUp);
            this.panelEx5.Location = new System.Drawing.Point(2, 107);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(420, 240);
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
            this.panelEx3.Size = new System.Drawing.Size(392, 240);
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
            this.buttonDel.Location = new System.Drawing.Point(147, 112);
            this.buttonDel.Name = "buttonDel";
            this.buttonDel.Size = new System.Drawing.Size(30, 25);
            this.buttonDel.TabIndex = 6;
            // 
            // buttonAdd
            // 
            this.buttonAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAdd.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdd.Image")));
            this.buttonAdd.Location = new System.Drawing.Point(147, 80);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(30, 25);
            this.buttonAdd.TabIndex = 5;
            // 
            // listAddField
            // 
            // 
            // 
            // 
            this.listAddField.Border.Class = "ListViewBorder";
            this.listAddField.Dock = System.Windows.Forms.DockStyle.Right;
            this.listAddField.FullRowSelect = true;
            this.listAddField.Location = new System.Drawing.Point(181, 0);
            this.listAddField.Name = "listAddField";
            this.listAddField.Size = new System.Drawing.Size(211, 240);
            this.listAddField.TabIndex = 2;
            this.listAddField.UseCompatibleStateImageBehavior = false;
            this.listAddField.View = System.Windows.Forms.View.Details;
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
            this.listAllField.Size = new System.Drawing.Size(143, 240);
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
            this.buttonDown.Location = new System.Drawing.Point(395, 115);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(22, 35);
            this.buttonDown.TabIndex = 4;
            // 
            // buttonUp
            // 
            this.buttonUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonUp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonUp.Image")));
            this.buttonUp.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonUp.Location = new System.Drawing.Point(395, 71);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(22, 35);
            this.buttonUp.TabIndex = 3;
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
            this.panelEx2.Controls.Add(this.labelPreviewFore);
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(420, 100);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 47;
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
            this.cmbColorRamp.Size = new System.Drawing.Size(237, 27);
            this.cmbColorRamp.TabIndex = 2;
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.Location = new System.Drawing.Point(310, 16);
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
            // labelPreviewFore
            // 
            this.labelPreviewFore.BackColor = System.Drawing.Color.Transparent;
            this.labelPreviewFore.Location = new System.Drawing.Point(330, 11);
            this.labelPreviewFore.Name = "labelPreviewFore";
            this.labelPreviewFore.Size = new System.Drawing.Size(80, 40);
            this.labelPreviewFore.TabIndex = 12;
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
            this.sizeInput.Size = new System.Drawing.Size(60, 21);
            this.sizeInput.TabIndex = 49;
            this.sizeInput.Value = 1;
            // 
            // frmChartRenderer
            // 
            this.Controls.Add(this.panelEx1);
            this.Name = "frmChartRenderer";
            this.Size = new System.Drawing.Size(425, 350);
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

        public frmChartRenderer()
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
            column2.Width = 130;
            column2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            listAddField.Columns.Add(column2);

            List<ColorItem> colorRamps = new ModuleCommon().GetColorScheme(217, 20, "Default Schemes;Spatial Ramps");
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
                    case "labelPreviewFore":
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

        public void InitRendererObject(IFeatureLayer pFeatureLayer, IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            List<FieldInfo> fields = ModuleCommon.GetFieldsFromLayer(pFeatureLayer, true);
            m_Layer = pFeatureLayer;
            InitRendererObject(fields, pRenderer, _SymbologyStyleClass);
        }

        public void InitRendererObject(List<FieldInfo> pFields, IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            flag = true;
            m_SymbologyStyleClass = _SymbologyStyleClass;
            flag = false;
        }
        
        public IFeatureRenderer Renderer
        {
            get
            {
                return null;
            }
        }

        public enumRendererType RendererType
        {
            get { return enumRendererType.BreakColorRenderer; }
        }

        private void RefreshSymbol()
        {//Ë¢ÐÂ·ûºÅÑùÊ½
            if (flag) return;
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            listAddField.SmallImageList.Images.Clear();

            for (int i = 0; i < listAllField.Items.Count; i++)
            {
                ISymbol pSymbol = copy.Copy(labelPreviewFore.Tag) as ISymbol;
                IColor pColor = ModuleCommon.GetColor(listAllField.Items[i].Tag as ISymbol);
                ModuleCommon.ChangeSymbolColor(pSymbol, pColor);
                listAllField.Items[i].Tag = pSymbol;
            }

            for (int i = 0; i < listAddField.Items.Count; i++)
            {
                ISymbol pSymbol = copy.Copy(labelPreviewFore.Tag) as ISymbol;
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

            for (int i = 0; i < listAllField.Items.Count; i++)
            {
                IColor pColor = enumColors.Next();
                if (pColor == null)
                {
                    enumColors.Reset();
                    enumColors.Next();
                }
                System.Windows.Forms.ListViewItem item = listAllField.Items[i];
                ModuleCommon.ChangeSymbolColor(item.Tag as ISymbol, pColor);
            }

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
    }
}
