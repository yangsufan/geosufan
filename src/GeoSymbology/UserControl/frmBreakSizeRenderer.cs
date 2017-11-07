using System;
using System.Collections.Generic;
using System.Text;
using DevDNB = DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace GeoSymbology
{
    public class frmBreakSizeRenderer:System.Windows.Forms.UserControl,IEditItem,IRendererUI
    {
        #region InitializeComponent

        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.LabelX labelPreviewFore;
        private DevComponents.Editors.IntegerInput levelInput;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelPreviewBack;
        private DevComponents.DotNetBar.PanelEx panelEx4;
        private DevComponents.DotNetBar.Controls.ListViewEx listValueItem;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbField;
        private DevComponents.Editors.DoubleInput maxSizeInput;
        private DevComponents.Editors.DoubleInput minSizeInput;
        private DevComponents.Editors.DoubleInput maxValueInput;
        private DevComponents.Editors.DoubleInput minValueInput;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelBackCaption;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    
        private void InitializeComponent()
        {
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx4 = new DevComponents.DotNetBar.PanelEx();
            this.listValueItem = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.labelBackCaption = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.maxSizeInput = new DevComponents.Editors.DoubleInput();
            this.minSizeInput = new DevComponents.Editors.DoubleInput();
            this.maxValueInput = new DevComponents.Editors.DoubleInput();
            this.minValueInput = new DevComponents.Editors.DoubleInput();
            this.cmbField = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelPreviewFore = new DevComponents.DotNetBar.LabelX();
            this.levelInput = new DevComponents.Editors.IntegerInput();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelPreviewBack = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.panelEx4.SuspendLayout();
            this.panelEx2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxSizeInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSizeInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValueInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minValueInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.levelInput)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.panelEx4);
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
            // panelEx4
            // 
            this.panelEx4.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx4.Controls.Add(this.listValueItem);
            this.panelEx4.Location = new System.Drawing.Point(3, 107);
            this.panelEx4.Name = "panelEx4";
            this.panelEx4.Size = new System.Drawing.Size(459, 260);
            this.panelEx4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx4.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx4.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelEx4.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelEx4.Style.GradientAngle = 90;
            this.panelEx4.TabIndex = 49;
            // 
            // listValueItem
            // 
            // 
            // 
            // 
            this.listValueItem.Border.Class = "ListViewBorder";
            this.listValueItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listValueItem.FullRowSelect = true;
            this.listValueItem.Location = new System.Drawing.Point(0, 0);
            this.listValueItem.Name = "listValueItem";
            this.listValueItem.Size = new System.Drawing.Size(459, 260);
            this.listValueItem.TabIndex = 1;
            this.listValueItem.UseCompatibleStateImageBehavior = false;
            this.listValueItem.View = System.Windows.Forms.View.Details;
            this.listValueItem.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DoListValueItemMouseDoubleClick);
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.labelBackCaption);
            this.panelEx2.Controls.Add(this.labelX1);
            this.panelEx2.Controls.Add(this.maxSizeInput);
            this.panelEx2.Controls.Add(this.minSizeInput);
            this.panelEx2.Controls.Add(this.maxValueInput);
            this.panelEx2.Controls.Add(this.minValueInput);
            this.panelEx2.Controls.Add(this.cmbField);
            this.panelEx2.Controls.Add(this.labelX12);
            this.panelEx2.Controls.Add(this.labelX11);
            this.panelEx2.Controls.Add(this.labelX10);
            this.panelEx2.Controls.Add(this.labelX9);
            this.panelEx2.Controls.Add(this.labelPreviewFore);
            this.panelEx2.Controls.Add(this.levelInput);
            this.panelEx2.Controls.Add(this.labelX13);
            this.panelEx2.Controls.Add(this.labelX8);
            this.panelEx2.Controls.Add(this.labelPreviewBack);
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(459, 100);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 47;
            // 
            // labelBackCaption
            // 
            this.labelBackCaption.AutoSize = true;
            this.labelBackCaption.BackColor = System.Drawing.Color.Transparent;
            this.labelBackCaption.Location = new System.Drawing.Point(346, 59);
            this.labelBackCaption.Name = "labelBackCaption";
            this.labelBackCaption.Size = new System.Drawing.Size(19, 31);
            this.labelBackCaption.TabIndex = 36;
            this.labelBackCaption.Text = "背\r\n景";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(346, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(19, 31);
            this.labelX1.TabIndex = 35;
            this.labelX1.Text = "符\r\n号";
            // 
            // maxSizeInput
            // 
            // 
            // 
            // 
            this.maxSizeInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.maxSizeInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.maxSizeInput.Increment = 1;
            this.maxSizeInput.Location = new System.Drawing.Point(244, 71);
            this.maxSizeInput.MinValue = 0.01;
            this.maxSizeInput.Name = "maxSizeInput";
            this.maxSizeInput.Size = new System.Drawing.Size(90, 21);
            this.maxSizeInput.TabIndex = 4;
            this.maxSizeInput.Value = 1;
            this.maxSizeInput.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // minSizeInput
            // 
            // 
            // 
            // 
            this.minSizeInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.minSizeInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.minSizeInput.Increment = 1;
            this.minSizeInput.Location = new System.Drawing.Point(244, 41);
            this.minSizeInput.MinValue = 0.01;
            this.minSizeInput.Name = "minSizeInput";
            this.minSizeInput.Size = new System.Drawing.Size(90, 21);
            this.minSizeInput.TabIndex = 3;
            this.minSizeInput.Value = 1;
            this.minSizeInput.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // maxValueInput
            // 
            // 
            // 
            // 
            this.maxValueInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.maxValueInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.maxValueInput.Increment = 1;
            this.maxValueInput.Location = new System.Drawing.Point(56, 71);
            this.maxValueInput.Name = "maxValueInput";
            this.maxValueInput.Size = new System.Drawing.Size(112, 21);
            this.maxValueInput.TabIndex = 3;
            this.maxValueInput.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // minValueInput
            // 
            // 
            // 
            // 
            this.minValueInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.minValueInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.minValueInput.Increment = 1;
            this.minValueInput.Location = new System.Drawing.Point(56, 41);
            this.minValueInput.Name = "minValueInput";
            this.minValueInput.Size = new System.Drawing.Size(112, 21);
            this.minValueInput.TabIndex = 2;
            this.minValueInput.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // cmbField
            // 
            this.cmbField.DisplayMember = "Text";
            this.cmbField.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbField.FormattingEnabled = true;
            this.cmbField.ItemHeight = 15;
            this.cmbField.Location = new System.Drawing.Point(56, 10);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(159, 21);
            this.cmbField.TabIndex = 2;
            this.cmbField.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // labelX12
            // 
            this.labelX12.AutoSize = true;
            this.labelX12.Location = new System.Drawing.Point(11, 13);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(31, 18);
            this.labelX12.TabIndex = 19;
            this.labelX12.Text = "字段";
            // 
            // labelX11
            // 
            this.labelX11.AutoSize = true;
            this.labelX11.Location = new System.Drawing.Point(11, 72);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(44, 18);
            this.labelX11.TabIndex = 20;
            this.labelX11.Text = "最大值";
            // 
            // labelX10
            // 
            this.labelX10.AutoSize = true;
            this.labelX10.Location = new System.Drawing.Point(11, 43);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(44, 18);
            this.labelX10.TabIndex = 21;
            this.labelX10.Text = "最小值";
            // 
            // labelX9
            // 
            this.labelX9.AutoSize = true;
            this.labelX9.Location = new System.Drawing.Point(223, 13);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(31, 18);
            this.labelX9.TabIndex = 22;
            this.labelX9.Text = "等级";
            // 
            // labelPreviewFore
            // 
            this.labelPreviewFore.BackColor = System.Drawing.Color.Transparent;
            this.labelPreviewFore.Location = new System.Drawing.Point(367, 8);
            this.labelPreviewFore.Name = "labelPreviewFore";
            this.labelPreviewFore.Size = new System.Drawing.Size(80, 40);
            this.labelPreviewFore.TabIndex = 24;
            this.labelPreviewFore.Click += new System.EventHandler(this.Control_Click);
            // 
            // levelInput
            // 
            // 
            // 
            // 
            this.levelInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.levelInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.levelInput.Location = new System.Drawing.Point(260, 10);
            this.levelInput.MinValue = 1;
            this.levelInput.Name = "levelInput";
            this.levelInput.ShowUpDown = true;
            this.levelInput.Size = new System.Drawing.Size(51, 21);
            this.levelInput.TabIndex = 26;
            this.levelInput.Value = 5;
            this.levelInput.ValueChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            this.labelX13.Location = new System.Drawing.Point(174, 73);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(68, 18);
            this.labelX13.TabIndex = 30;
            this.labelX13.Text = "符号最大值";
            // 
            // labelX8
            // 
            this.labelX8.AutoSize = true;
            this.labelX8.Location = new System.Drawing.Point(174, 43);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(68, 18);
            this.labelX8.TabIndex = 31;
            this.labelX8.Text = "符号最小值";
            // 
            // labelPreviewBack
            // 
            this.labelPreviewBack.BackColor = System.Drawing.Color.Transparent;
            this.labelPreviewBack.Location = new System.Drawing.Point(367, 54);
            this.labelPreviewBack.Name = "labelPreviewBack";
            this.labelPreviewBack.Size = new System.Drawing.Size(80, 40);
            this.labelPreviewBack.TabIndex = 34;
            this.labelPreviewBack.Click += new System.EventHandler(this.Control_Click);
            // 
            // frmBreakSizeRenderer
            // 
            this.Controls.Add(this.panelEx1);
            this.Name = "frmBreakSizeRenderer";
            this.Size = new System.Drawing.Size(465, 370);
            this.panelEx1.ResumeLayout(false);
            this.panelEx4.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxSizeInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSizeInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValueInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minValueInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.levelInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private esriSymbologyStyleClass m_SymbologyStyleClass;
        private bool flag = false;
        private IFeatureLayer m_Layer;

        public frmBreakSizeRenderer()
        {
            InitializeComponent(); 
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            m_EditObject = null;
            listValueItem.SmallImageList = new System.Windows.Forms.ImageList();
            listValueItem.SmallImageList.ImageSize = new System.Drawing.Size(ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);

            System.Windows.Forms.ColumnHeader column = new System.Windows.Forms.ColumnHeader();
            column.Name = "Symbol";
            column.Text = "符号";
            column.Width = 80;
            column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            listValueItem.Columns.Add(column);

            column = new System.Windows.Forms.ColumnHeader();
            column.Name = "Range";
            column.Text = "范围";
            column.Width = 146;
            column.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            listValueItem.Columns.Add(column);

            column = new System.Windows.Forms.ColumnHeader();
            column.Name = "Label";
            column.Text = "标签";
            column.Width = 146;
            column.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            listValueItem.Columns.Add(column);
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
                    RefreshValue(control.Name);//yjl20110826 add
                    break;
                case "labelPreviewBack":
                    m_EditObject = control;
                    Form.frmSymbolEdit backEdit = new GeoSymbology.Form.frmSymbolEdit(this, control.Tag as ISymbol, "");
                    backEdit.ShowDialog();
                    RefreshValue(control.Name);//yjl20110826 add
                    break;
            }
        }

        private void DoListValueItemMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.ListViewItem item = listValueItem.GetItemAt(e.X,e.Y);
            if (item == null) return;

            System.Drawing.Rectangle rec = item.GetBounds(System.Windows.Forms.ItemBoundsPortion.Entire);
            if (e.X <= listValueItem.Columns[0].Width)
            {
                //符号编辑
                m_EditObject = item;
                Form.frmSymbolEdit frm = new GeoSymbology.Form.frmSymbolEdit(this, item.Tag as ISymbol, "");
                frm.ShowDialog();
            }
            else if (e.X > listValueItem.Columns[0].Width &&
                e.X <= (listValueItem.Columns[1].Width + listValueItem.Columns[0].Width))
            {
                m_EditObject = item.SubItems[1];
                //范围编辑
                System.Drawing.Point point = new System.Drawing.Point();
                point.X = listValueItem.Columns[0].Width;
                point.Y = rec.Top + (rec.Height - Form.frmDoubleEdit.FormWidth) / 2;
                point = listValueItem.PointToScreen(point);
                Form.frmDoubleEdit doubleEdit = new GeoSymbology.Form.frmDoubleEdit(this,
                    (double)item.SubItems[1].Tag, point, listValueItem.Columns[1].Width, "");
                doubleEdit.Show();
            }
            else if (e.X > (listValueItem.Columns[1].Width + listValueItem.Columns[0].Width)
                && e.X <= (listValueItem.Columns[0].Width + listValueItem.Columns[1].Width + listValueItem.Columns[2].Width))
            {
                m_EditObject = item.SubItems[2];
                //标签编辑
                System.Drawing.Point point = new System.Drawing.Point();
                point.X = listValueItem.Columns[0].Width + listValueItem.Columns[1].Width;
                point.Y = rec.Top + (rec.Height - Form.frmStringEdit.FormWidth) / 2;
                point = listValueItem.PointToScreen(point);
                Form.frmStringEdit stringEdit = new GeoSymbology.Form.frmStringEdit(this,
                    item.SubItems[2].Text, point, listValueItem.Columns[2].Width, "");
                stringEdit.Show();
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
                    System.Windows.Forms.ListViewItem item = listValueItem.Items[Convert.ToInt32(nameIndex)];
                    item.SubItems[2].Text = subItem.Text;
                    //更新下一条记录的Range和Label的值
                    if (item.Index + 1 < listValueItem.Items.Count)
                    {
                        System.Windows.Forms.ListViewItem nextItem = listValueItem.Items[item.Index + 1];
                        nextItem.SubItems[1].Text = newValue.ToString() + "-" + nextItem.SubItems[1].Tag.ToString();
                        nextItem.SubItems[2].Text = newValue.ToString() + "-" + nextItem.SubItems[1].Tag.ToString();
                    }
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
                listValueItem.SmallImageList.Images.RemoveByKey("Symbol" + item.Index.ToString());
                listValueItem.SmallImageList.Images.Add("Symbol" + item.Index.ToString(),
                    ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
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
                        RefreshSymbol();
                        break;
                    case "labelPreviewBack":
                        if (label.Image != null)
                        {
                            label.Image.Dispose();
                            label.Image = null;
                        }
                        label.Tag = newValue;
                        label.Image = ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
                        labelPreviewBack.Refresh();
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

            cmbField.Items.AddRange(pFields.ToArray());
            
            IClassBreaksRenderer pBreakRenderer = pRenderer as IClassBreaksRenderer;

            cmbField.Text = pBreakRenderer.Field;
            levelInput.Value = pBreakRenderer.BreakCount;
            minSizeInput.Value = 1;
            maxSizeInput.Value = 10;
            minValueInput.Value = 0;
            maxValueInput.Value = 100;
            double minBreak = pBreakRenderer.MinimumBreak;
            minBreak = Math.Round(minBreak, 4);
            for (int i = 0; i < pBreakRenderer.BreakCount; i++)
            {
                ISymbol pSymbol = pBreakRenderer.get_Symbol(i);
                string label = pBreakRenderer.get_Label(i);
                double breakValue = pBreakRenderer.get_Break(i);
                breakValue = Math.Round(breakValue, 4);
                double lastBreak = 0;
                if (i == 0)
                {
                    lastBreak = minBreak;

                    double size = ModuleCommon.GetSymbolSize(pSymbol);
                    minSizeInput.Value = size;

                    minValueInput.Value = lastBreak;
                }
                else
                {
                    lastBreak = pBreakRenderer.get_Break(i - 1);
                    lastBreak = Math.Round(lastBreak, 4);
                }

                if (i == pBreakRenderer.BreakCount - 1)
                {
                    double size = ModuleCommon.GetSymbolSize(pSymbol);
                    maxSizeInput.Value = size;

                    maxValueInput.Value = breakValue;
                }

                listValueItem.SmallImageList.Images.Add("Symbol" + i.ToString(), ModuleCommon.Symbol2Picture(pSymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));//yjl20110826 add
                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
                item.Name = "Item" + i.ToString();
                item.Text = "";
                item.ImageKey = "Symbol" + i.ToString();
                item.Tag = pSymbol;

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem.Name = "Range" + i.ToString();
                subItem.Text = (lastBreak == 0 ? "0" : lastBreak.ToString()) + "-" + (breakValue == 0 ? "0" : breakValue.ToString());
                subItem.Tag = subItem.Text;
                item.SubItems.Add(subItem);

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem1.Name = "Label" + i.ToString();
                subItem1.Text = label;
                item.SubItems.Add(subItem1);

                listValueItem.Items.Add(item);
            }
            flag = false;
            if (pBreakRenderer.BreakCount == 0)
            {
                levelInput.Value = 5;

                switch (m_SymbologyStyleClass)
                {
                    case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                    case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                        labelPreviewFore.Tag = ModuleCommon.CreateSymbol(m_SymbologyStyleClass);
                        break;
                    case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                        labelPreviewFore.Tag = ModuleCommon.CreateSymbol(esriSymbologyStyleClass.esriStyleClassMarkerSymbols);
                        break;
                }
            }
            else
            {
                labelPreviewFore.Tag = pBreakRenderer.get_Symbol(0);
            }
            labelPreviewFore.Image = ModuleCommon.Symbol2Picture(labelPreviewFore.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);

            if (m_SymbologyStyleClass == esriSymbologyStyleClass.esriStyleClassFillSymbols)
            {
                if (pBreakRenderer.BackgroundSymbol == null)
                    labelPreviewBack.Tag = ModuleCommon.CreateSymbol(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                else
                    labelPreviewBack.Tag = pBreakRenderer.BackgroundSymbol as ISymbol;
                labelPreviewBack.Visible = true;
                labelBackCaption.Visible = true;
                labelPreviewBack.Image = ModuleCommon.Symbol2Picture(labelPreviewBack.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
            }
            else
            {
                labelPreviewBack.Tag = null;
                labelPreviewBack.Visible = false;
                labelBackCaption.Visible = false;
            }
        }

        public IFeatureRenderer Renderer
        {
            get
            {
                IClassBreaksRenderer pRenderer = new ClassBreaksRendererClass();
                pRenderer.MinimumBreak = minValueInput.Value;
                pRenderer.Field = cmbField.Text;
                pRenderer.BackgroundSymbol = labelPreviewBack.Tag as IFillSymbol;

                pRenderer.BreakCount = listValueItem.Items.Count;
                for (int i = 0; i < listValueItem.Items.Count; i++)
                {
                    ISymbol pSymbol = listValueItem.Items[i].Tag as ISymbol;
                    double breakValue = (double)listValueItem.Items[i].SubItems[1].Tag;
                    string label = listValueItem.Items[i].SubItems[2].Text;

                    pRenderer.set_Break(i, breakValue);
                    pRenderer.set_Symbol(i, pSymbol);
                    pRenderer.set_Label(i, label);
                }
                return pRenderer as IFeatureRenderer;
            }
        }

        public enumRendererType RendererType
        {
            get { return enumRendererType.BreakSizeRenderer; }
        }

        private ISymbol[] CreateSymbols()
        {
            double sizeInterval = (maxSizeInput.Value - minSizeInput.Value) / levelInput.Value;

            ISymbol[] symbols = new ISymbol[levelInput.Value];
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            for (int i = 0; i < levelInput.Value; i++)
            {
                double size = minSizeInput.Value + sizeInterval * i;
                symbols[i] = copy.Copy(labelPreviewFore.Tag) as ISymbol;
                ModuleCommon.ChangeSymbolSize(symbols[i], size);
            }
            return symbols;
        }

        private void RefreshItems()
        {
            if (cmbField.Text == "<NONE>") return;
            if (levelInput.Value < 1) return;
            if (minSizeInput.Value <= 0 || maxSizeInput.Value <= 0) return;

            listValueItem.Items.Clear();
            listValueItem.SmallImageList.Images.Clear();
            double sizeInterval = (maxSizeInput.Value - minSizeInput.Value) / levelInput.Value;
            double valueInterval = GetValueInterval(cmbField.Name);
            ISymbol[] symbols = CreateSymbols();
            for (int i = 0; i < levelInput.Value; i++)
            {
                listValueItem.SmallImageList.Images.Add("Symbol" + i.ToString(), ModuleCommon.Symbol2Picture(symbols[i], ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));

                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
                item.Name = "Item" + i.ToString();
                item.Text = "";
                item.ImageKey = "Symbol" + i.ToString();
                item.Tag = symbols[i];

                double sMinValue = minValueInput.Value + valueInterval * i;
                double sMaxValue = minValueInput.Value + valueInterval * (i+1);
                sMinValue = Math.Round(sMinValue, 4);
                sMaxValue = Math.Round(sMaxValue, 4);

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem.Name = "Range" + i.ToString();
                subItem.Text = (sMinValue == 0 ? "0" : sMinValue.ToString()) + "-" + (sMaxValue == 0 ? "0" : sMaxValue.ToString());
                subItem.Tag = sMaxValue;
                item.SubItems.Add(subItem);

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem1.Name = "Label" + i.ToString();
                subItem1.Text = (sMinValue == 0 ? "0" : sMinValue.ToString()) + "-" + (sMaxValue == 0 ? "0" : sMaxValue.ToString());
                item.SubItems.Add(subItem1);

                listValueItem.Items.Add(item);
            }
            listValueItem.Refresh();
        }

        private double GetValueInterval(string controlName)
        {
            double valueInterval = (maxValueInput.Value - minValueInput.Value) / levelInput.Value;
            if (m_Layer == null || m_Layer.FeatureClass == null || controlName == "minValueInput" || controlName == "maxValueInput")
                return valueInterval;

            ESRI.ArcGIS.Geodatabase.ITable pTable = m_Layer.FeatureClass as ESRI.ArcGIS.Geodatabase.ITable;
            if (pTable.FindField(cmbField.Text) < 0) return valueInterval;
            ESRI.ArcGIS.Geodatabase.ICursor pCursor = pTable.Search(null, false);

            ESRI.ArcGIS.Geodatabase.IDataStatistics dataStatistics = new ESRI.ArcGIS.Geodatabase.DataStatisticsClass();
            dataStatistics.Field = cmbField.Text;
            dataStatistics.Cursor = pCursor;

            ESRI.ArcGIS.esriSystem.IStatisticsResults statisticsResults = dataStatistics.Statistics;
            valueInterval = (statisticsResults.Maximum - statisticsResults.Minimum) / levelInput.Value;

            flag = true;
            minValueInput.Value = statisticsResults.Minimum;
            maxValueInput.Value = statisticsResults.Maximum;
            flag = false;

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(statisticsResults);
            return valueInterval;
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            if (flag) return;
            System.Windows.Forms.Control control = sender as System.Windows.Forms.Control;
            switch (control.Name)
            {
                case "levelInput":
                    RefreshItems();
                    break;
                case "minValueInput":
                case "maxValueInput":
                case "cmbField":
                    RefreshValue(control.Name);
                    break;
                case "minSizeInput":
                case "maxSizeInput":
                    RefreshSize();
                    break;
            }
            listValueItem.Refresh();
        }

        private void RefreshSymbol()
        {//刷新符号样式
            if (flag) return;
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            listValueItem.SmallImageList.Images.Clear();
            for (int i = 0; i < listValueItem.Items.Count; i++)
            {
                ISymbol pSymbol = copy.Copy(labelPreviewFore.Tag) as ISymbol;
                double size = ModuleCommon.GetSymbolSize(listValueItem.Items[i].Tag as ISymbol);
                ModuleCommon.ChangeSymbolSize(pSymbol, size);
                listValueItem.Items[i].Tag = pSymbol;
                listValueItem.SmallImageList.Images.Add(listValueItem.Items[i].Name.Replace("Item", "Symbol"),
                    ModuleCommon.Symbol2Picture(pSymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                listValueItem.Items[i].ImageKey = listValueItem.Items[i].Name.Replace("Item", "Symbol");
            }
            listValueItem.Refresh();
        }
        
        /// <summary>
        /// 刷新值
        /// </summary>
        private void RefreshValue(string controlName)
        {
            if (flag) return;
            if (cmbField.Text == "<NONE>")
            {
                listValueItem.Items.Clear();
                listValueItem.SmallImageList.Images.Clear();
                return;
            }
            if (levelInput.Value < 1) return;
            if (listValueItem.Items.Count != levelInput.Value)
            {
                RefreshItems();
                return;
            }

            double valueInterval = GetValueInterval(controlName);
            for (int i = 0; i < listValueItem.Items.Count; i++)
            {
                System.Windows.Forms.ListViewItem item = listValueItem.Items[i];
                double sMinValue = minValueInput.Value + valueInterval * i;
                double sMaxValue = minValueInput.Value + valueInterval * (i + 1);
                sMinValue = Math.Round(sMinValue, 4);
                sMaxValue = Math.Round(sMaxValue, 4);

                item.SubItems[1].Text = (sMinValue == 0 ? "0" : sMinValue.ToString()) + "-" + (sMaxValue == 0 ? "0" : sMaxValue.ToString());
                item.SubItems[1].Tag = sMaxValue;

                item.SubItems[2].Text = (sMinValue == 0 ? "0" : sMinValue.ToString()) + "-" + (sMaxValue == 0 ? "0" : sMaxValue.ToString());
            }
        }

        private void RefreshSize()
        {
            if (flag) return;
            double sizeInterval = (maxSizeInput.Value - minSizeInput.Value) / levelInput.Value;
            listValueItem.SmallImageList.Images.Clear();
            for (int i = 0; i < listValueItem.Items.Count; i++)
            {
                System.Windows.Forms.ListViewItem item = listValueItem.Items[i];
                double size = minSizeInput.Value + sizeInterval * i;
                ModuleCommon.ChangeSymbolSize(item.Tag as ISymbol, size);
                listValueItem.SmallImageList.Images.Add(item.Name.Replace("Item", "Symbol"),
                    ModuleCommon.Symbol2Picture(item.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                item.ImageKey = item.Name.Replace("Item", "Symbol");
            }
        }
    }
}