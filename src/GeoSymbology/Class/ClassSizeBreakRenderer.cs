using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;

namespace GeoSymbology
{
    public class ClassSizeBreakRenderer : ClassRenderer, IEditItem
    {
        private ComboValue m_Field;
        /// <summary>
        /// 渐变字段
        /// </summary>
        public ComboValue Field
        {
            get { return m_Field; }
            set { m_Field = value; }
        }

        private IntegerInputValue m_SizeLevel;
        /// <summary>
        /// 渐变等级
        /// </summary>
        public IntegerInputValue SizeLevel
        {
            get { return m_SizeLevel; }
            set { m_SizeLevel = value; }
        }

        private DoubleInputValue m_MinSize;
        /// <summary>
        /// 符号最小值
        /// </summary>
        public DoubleInputValue MinSize
        {
            get { return m_MinSize; }
            set { m_MinSize = value; }
        }

        private DoubleInputValue m_MaxSize;
        /// <summary>
        /// 符号最大值
        /// </summary>
        public DoubleInputValue MaxSize
        {
            get { return m_MaxSize; }
            set { m_MaxSize = value; }
        }

        private DoubleInputValue m_MinValue;
        /// <summary>
        /// 最小值
        /// </summary>
        public DoubleInputValue MinValue
        {
            get { return m_MinValue; }
            set { m_MinValue = value; }
        }

        private DoubleInputValue m_MaxValue;
        /// <summary>
        /// 最大值
        /// </summary>
        public DoubleInputValue MaxValue
        {
            get { return m_MaxValue; }
            set { m_MaxValue = value; }
        }

        private SymbolValue m_BackSymbol;
        /// <summary>
        /// 背景符号
        /// </summary>
        public SymbolValue BackSymbol
        {
            get { return m_BackSymbol; }
            set { m_BackSymbol = value; }
        }

        public ClassSizeBreakRenderer()
            : base()
        {
            
        }

        public override void DoListValueItemMouseDoubleClick(int x, int y)
        {
            System.Windows.Forms.ListViewItem item = m_ListValueItem.GetItemAt(x, y);
            if (item == null) return;

            System.Drawing.Rectangle rec = item.GetBounds(System.Windows.Forms.ItemBoundsPortion.Entire);
            if (x <= m_ListValueItem.Columns[0].Width)
            {
                //符号编辑
                m_EditObject = item;
                Form.frmSymbolEdit frm = new GeoSymbology.Form.frmSymbolEdit(this, item.Tag as ISymbol, "");
                frm.ShowDialog();
            }
            else if (x > m_ListValueItem.Columns[0].Width &&
                x <= (m_ListValueItem.Columns[1].Width + m_ListValueItem.Columns[0].Width))
            {
                m_EditObject = item.SubItems[1];
                //范围编辑
                System.Drawing.Point point = new System.Drawing.Point();
                point.X = m_ListValueItem.Columns[0].Width;
                point.Y = rec.Top + (rec.Height - Form.frmDoubleEdit.FormWidth) / 2;
                point = m_ListValueItem.PointToScreen(point);
                Form.frmDoubleEdit doubleEdit = new GeoSymbology.Form.frmDoubleEdit(this,
                    (double)item.SubItems[1].Tag, point, m_ListValueItem.Columns[1].Width, "");
                doubleEdit.Show();
            }
            else if (x > (m_ListValueItem.Columns[1].Width + m_ListValueItem.Columns[0].Width)
                && x <= (m_ListValueItem.Columns[0].Width + m_ListValueItem.Columns[1].Width + m_ListValueItem.Columns[2].Width))
            {
                m_EditObject = item.SubItems[2];
                //标签编辑
                System.Drawing.Point point = new System.Drawing.Point();
                point.X = m_ListValueItem.Columns[0].Width + m_ListValueItem.Columns[1].Width;
                point.Y = rec.Top + (rec.Height - Form.frmStringEdit.FormWidth) / 2;
                point = m_ListValueItem.PointToScreen(point);
                Form.frmStringEdit stringEdit = new GeoSymbology.Form.frmStringEdit(this,
                    item.SubItems[2].Text, point, m_ListValueItem.Columns[2].Width, "");
                stringEdit.Show();
                stringEdit.Location = point;
            }
        }

        public override void DoButtonClick(DevComponents.DotNetBar.ButtonX button)
        {
            switch (button.Name)
            {
                case "ForeSymbol":
                    {
                        m_EditObject = button;
                        Form.frmSymbolEdit frm = new GeoSymbology.Form.frmSymbolEdit(this, m_ForeSymbol.DataValue, "");
                        frm.ShowDialog();
                    }
                    break;
                case "BackSymbol":
                    {
                        m_EditObject = button;
                        Form.frmSymbolEdit frm = new GeoSymbology.Form.frmSymbolEdit(this, m_BackSymbol.DataValue, "");
                        frm.ShowDialog();
                    }
                    break;
            }
        }

        public override IFeatureRenderer FeatureRenderer
        {
            get
            {
                IClassBreaksRenderer renderer = new ClassBreaksRendererClass();
                renderer.Field = m_Field.DataValue;
                renderer.MinimumBreak = m_MinValue.DataValue;
                for (int i = 0; i < m_ListValueItem.Items.Count; i++)
                {
                    ISymbol pSymbol = m_ListValueItem.Items[i].Tag as ISymbol;
                    string label = m_ListValueItem.Items[i].SubItems[2].Tag.ToString();
                    double range = (double)m_ListValueItem.Items[i].SubItems[1].Tag;
                    renderer.set_Break(i, range);
                    renderer.set_Label(i, label);
                    renderer.set_Symbol(i, pSymbol);
                }
                renderer.BackgroundSymbol = m_BackSymbol.DataValue as IFillSymbol;
                return renderer as IFeatureRenderer;
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
                    subItem.Tag = newValue;
                    subItem.Text = subItem.Text.Split('-')[0] + "-" + newValue.ToString();

                    string nameIndex = subItem.Name.Replace("Range", "");
                    System.Windows.Forms.ListViewItem item = m_ListValueItem.Items[Convert.ToInt32(nameIndex)];
                    item.SubItems[2].Text = subItem.Text;

                    if (item.Index + 1 < m_ListValueItem.Items.Count)
                    {
                        System.Windows.Forms.ListViewItem nextItem = m_ListValueItem.Items[item.Index + 1];
                        nextItem.SubItems[1].Text = newValue.ToString() + "-" + nextItem.SubItems[1].Tag.ToString();
                        nextItem.SubItems[2].Text = newValue.ToString() + "-" + nextItem.SubItems[1].Tag.ToString();
                    }
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
                m_ListValueItem.SmallImageList.Images.RemoveByKey("Symbol" + item.Index.ToString());
                m_ListValueItem.SmallImageList.Images.Add("Symbol" + item.Index.ToString(),
                    ModuleCommon.Symbol2Picture(newValue as ISymbol, 80, 40));
                item.ImageKey = "Symbol" + item.Index.ToString();
            }
            if (m_EditObject is DevComponents.DotNetBar.ButtonX)
            {
                DevComponents.DotNetBar.ButtonX button = m_EditObject as DevComponents.DotNetBar.ButtonX;
                switch (button.Name)
                {
                    case "ForeSymbol":
                        m_ForeSymbol.DataValue = newValue as ISymbol;
                        if (button.Image != null)
                        {
                            button.Image.Dispose();
                            button.Image = null;
                        }
                        button.Image = ModuleCommon.Symbol2Picture(m_ForeSymbol.DataValue, 80, 40);
                        RefreshValueItem();
                        break;
                    case "BackSymbol":
                        m_BackSymbol.DataValue = newValue as ISymbol;
                        if (button.Image != null)
                        {
                            button.Image.Dispose();
                            button.Image = null;
                        }
                        button.Image = ModuleCommon.Symbol2Picture(m_BackSymbol.DataValue, 80, 40);
                        break;
                }
            }
            m_EditObject = null;
        }

        #endregion

        private void InitUI()
        {
            #region MinValue

            m_MinValue = new DoubleInputValue();
            m_MinValue.ControlName = "MinValue";
            m_MinValue.Caption = "最小值";
            m_MinValue.ControlWidth = 150;
            m_MinValue.ControlHeight = 23;
            m_MinValue.MinValue = -1.7976931348623157E+308;
            m_MinValue.MaxValue = 1.7976931348623157E+308;

            #endregion

            #region MaxValue

            m_MaxValue = new DoubleInputValue();
            m_MaxValue.ControlName = "MaxValue";
            m_MaxValue.Caption = "最大值";
            m_MaxValue.ControlWidth = 150;
            m_MaxValue.ControlHeight = 23;
            m_MaxValue.MinValue = -1.7976931348623157E+308;
            m_MaxValue.MaxValue = 1.7976931348623157E+308;

            #endregion

            #region MinSize

            m_MinSize = new DoubleInputValue();
            m_MinSize.ControlName = "MinSize";
            m_MinSize.Caption = "符号最小值";
            m_MinSize.ControlWidth = 150;
            m_MinSize.ControlHeight = 23;
            m_MinSize.MinValue = 1;
            m_MinSize.MaxValue = 1.7976931348623157E+308;

            #endregion

            #region MaxSize

            m_MaxSize = new DoubleInputValue();
            m_MaxSize.ControlName = "MaxSize";
            m_MaxSize.Caption = "符号最大值";
            m_MaxSize.ControlWidth = 150;
            m_MaxSize.ControlHeight = 23;
            m_MaxSize.MinValue = 1;
            m_MaxSize.MaxValue = 1.7976931348623157E+308;

            #endregion

            #region SizeLevel

            m_SizeLevel = new IntegerInputValue();
            m_SizeLevel.ControlName = "SizeLevel";
            m_SizeLevel.Caption = "渐变等级";
            m_SizeLevel.ControlWidth = 150;
            m_SizeLevel.ControlHeight = 23;
            m_SizeLevel.MinValue = 1;
            m_SizeLevel.MaxValue = 2147483647;

            #endregion

            #region Field

            m_Field = new ComboValue();
            m_Field.ControlName = "Field";
            m_Field.Caption = "渐变字段";
            m_Field.ControlWidth = 150;
            m_Field.ControlHeight = 23;
            m_Field.DropDownWidth = 150;
            FieldInfo[] fields = new FieldInfo[10];
            for (int i = 0; i < 10; i++)
            {
                fields[i] = new FieldInfo();
                fields[i].FieldName = "Field" + i.ToString();
                fields[i].FieldDesc = "Field" + i.ToString();
                fields[i].FieldType = "Field" + i.ToString();
            }
            m_Field.Items = fields;

            #endregion

            #region ForeSymbol

            m_ForeSymbol = new SymbolValue();
            m_ForeSymbol.ControlName = "ForeSymbol";
            m_ForeSymbol.Caption = "前景符号";
            m_ForeSymbol.ControlWidth = 100;
            m_ForeSymbol.ControlHeight = 40;

            #endregion
        }

        public override void InitRendererObject(IFeatureRenderer _Renderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            m_SymbologyStyleClass = _SymbologyStyleClass;
            InitUI();

            #region InitObject

            IClassBreaksRenderer _BreakRenderer = null;
            if ((_Renderer is IClassBreaksRenderer) == false)
            {
                _BreakRenderer = new ClassBreaksRendererClass();
                if (m_SymbologyStyleClass == esriSymbologyStyleClass.esriStyleClassFillSymbols)
                    _BreakRenderer.BackgroundSymbol = ModuleCommon.CreateSymbol(esriSymbologyStyleClass.esriStyleClassFillSymbols) as IFillSymbol;
                else
                    _BreakRenderer.BackgroundSymbol = null;
                _BreakRenderer.MinimumBreak = 0;
                _BreakRenderer.Field = "";

                m_Field.DataValue = "<NONE>";
                m_SizeLevel.DataValue = 3;
                m_MinSize.DataValue = 1;
                m_MaxSize.DataValue = 6;
                m_MinValue.DataValue = 0;
                m_MaxValue.DataValue = 100;
                switch (m_SymbologyStyleClass)
                {
                    case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                    case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                        m_ForeSymbol.DataValue = ModuleCommon.CreateSymbol(m_SymbologyStyleClass);
                        m_BackSymbol = null;
                        break;
                    case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                        m_ForeSymbol.DataValue = ModuleCommon.CreateSymbol(esriSymbologyStyleClass.esriStyleClassMarkerSymbols);
                        m_BackSymbol = new SymbolValue();
                        m_BackSymbol.ControlName = "BackSymbol";
                        m_BackSymbol.Caption = "背景符号";
                        m_BackSymbol.ControlWidth = 100;
                        m_BackSymbol.ControlHeight = 40;
                        m_BackSymbol.DataValue = ModuleCommon.CreateSymbol(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                        break;
                }
            }
            else
            {
                _BreakRenderer = _Renderer as IClassBreaksRenderer;

                m_SizeLevel.DataValue = _BreakRenderer.BreakCount;
                if (_BreakRenderer.BreakCount > 0)
                {
                    m_MinValue.DataValue = _BreakRenderer.MinimumBreak;
                    m_MaxValue.DataValue = _BreakRenderer.get_Break(m_SizeLevel.DataValue - 1);

                    m_MinSize.DataValue = ModuleCommon.GetSymbolSize(_BreakRenderer.get_Symbol(0));
                    m_MaxSize.DataValue = ModuleCommon.GetSymbolSize(_BreakRenderer.get_Symbol(m_SizeLevel.DataValue - 1));

                    m_ForeSymbol.DataValue = _BreakRenderer.get_Symbol(0);
                }
                else
                {
                    m_MinValue.DataValue = 0;
                    m_MaxValue.DataValue = 100;
                    m_MinSize.DataValue = 1;
                    m_MaxSize.DataValue = 6;
                    m_ForeSymbol.DataValue = ModuleCommon.CreateSymbol(m_SymbologyStyleClass);
                }

                if (_BreakRenderer.BackgroundSymbol == null)
                    m_BackSymbol = null;
                else
                {
                    m_BackSymbol = new SymbolValue();
                    m_BackSymbol.ControlName = "BackSymbol";
                    m_BackSymbol.Caption = "背景符号";
                    m_BackSymbol.ControlWidth = 100;
                    m_BackSymbol.ControlHeight = 40;
                    m_BackSymbol.DataValue = _BreakRenderer.BackgroundSymbol as ISymbol;
                }

                if (_BreakRenderer.Field == "")
                    m_Field.DataValue = "<NONE>";
                else
                    m_Field.DataValue = _BreakRenderer.Field;
            }

            #endregion

            #region InitListView

            m_ListValueItem.SmallImageList.Images.Clear();
            for (int i = 0; i < _BreakRenderer.BreakCount; i++)
            {
                ISymbol pSymbol = _BreakRenderer.get_Symbol(i);
                m_ListValueItem.SmallImageList.Images.Add("Symbol" + i.ToString(), ModuleCommon.Symbol2Picture(pSymbol, 80, 40));
                string label = _BreakRenderer.get_Label(i);
                double range = _BreakRenderer.get_Break(i);


                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
                item.Name = "Item" + i.ToString();
                item.Text = "";
                item.ImageKey = "Symbol" + i.ToString();
                item.Tag = pSymbol;

                string strMinValue = "0";
                if (i == 0)
                {
                    strMinValue = _BreakRenderer.MinimumBreak == 0 ? "0" : _BreakRenderer.MinimumBreak.ToString(".####");
                }
                else
                {
                    double sMinValue = (double)m_ListValueItem.Items[i-1].SubItems[1].Tag;
                    strMinValue = sMinValue == 0 ? "0" : sMinValue.ToString(".####");
                }
                System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem.Name = "Range" + i.ToString();
                subItem.Text = strMinValue + "-" + (range == 0 ? "0" : range.ToString(".####"));
                subItem.Tag = range;
                item.SubItems.Add(subItem);

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem1.Name = "Label" + i.ToString();
                subItem1.Text = label;
                item.SubItems.Add(subItem1);

                m_ListValueItem.Items.Add(item);
            }

            #endregion

            #region InitTree

            #region Field
            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            node.Text = m_Field.Caption;
            node.Name = "node" + m_Field.ControlName;
            DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetComboBox(m_Field);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);
            #endregion

            #region SizeLevel
            node = new DevComponents.AdvTree.Node();
            node.Text = m_SizeLevel.Caption;
            node.Name = "node" + m_SizeLevel.ControlName;
            cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetIntegerInput(m_SizeLevel);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);
            #endregion

            #region MinSize
            node = new DevComponents.AdvTree.Node();
            node.Text = m_MinSize.Caption;
            node.Name = "node" + m_MinSize.ControlName;
            cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetDoubleInput(m_MinSize);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);
            #endregion

            #region MaxSize
            node = new DevComponents.AdvTree.Node();
            node.Text = m_MaxSize.Caption;
            node.Name = "node" + m_MaxSize.ControlName;
            cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetDoubleInput(m_MaxSize);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);
            #endregion

            #region MinValue
            node = new DevComponents.AdvTree.Node();
            node.Text = m_MinValue.Caption;
            node.Name = "node" + m_MinValue.ControlName;
            cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetDoubleInput(m_MinValue);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);
            #endregion

            #region MaxValue
            node = new DevComponents.AdvTree.Node();
            node.Text = m_MaxValue.Caption;
            node.Name = "node" + m_MaxValue.ControlName;
            cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetDoubleInput(m_MaxValue);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);
            #endregion

            #region ForeSymbol
            node = new DevComponents.AdvTree.Node();
            node.Text = m_ForeSymbol.Caption;
            node.Name = "node" + m_ForeSymbol.ControlName;
            cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetSymbolButton(m_ForeSymbol);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);
            #endregion

            #region BackSymbol
            if (m_BackSymbol != null)
            {
                node = new DevComponents.AdvTree.Node();
                node.Text = m_BackSymbol.Caption;
                node.Name = "node" + m_BackSymbol.ControlName;
                cell = new DevComponents.AdvTree.Cell();
                cell.HostedControl = GetSymbolButton(m_BackSymbol);
                node.Cells.Add(cell);
                m_TreeProperty.Nodes.Add(node);
            }
            #endregion

            #endregion
        }

        public override void RefreshValueItem()
        {
            if (m_Field.DataValue == "<NONE>") return;
            if (m_SizeLevel.DataValue < 1) return;
            if (m_MinSize.DataValue <= 0 || m_MaxSize.DataValue <= 0) return;

            m_ListValueItem.Items.Clear();
            m_ListValueItem.SmallImageList.Images.Clear();
            double sizeInterval = (m_MaxSize.DataValue - m_MinSize.DataValue) / m_SizeLevel.DataValue;
            double valueInterval = (m_MaxValue.DataValue - m_MinValue.DataValue) / m_SizeLevel.DataValue;
            ISymbol[] symbols = CreateSymbols();
            for (int i = 0; i < m_SizeLevel.DataValue; i++)
            {
                m_ListValueItem.SmallImageList.Images.Add("Symbol" + i.ToString(), ModuleCommon.Symbol2Picture(symbols[i], 80, 40));

                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
                item.Name = "Item" + i.ToString();
                item.Text = "";
                item.ImageKey = "Symbol" + i.ToString();
                item.Tag = symbols[i];

                string strMinValue = "0";
                double sMinValue = m_MinValue.DataValue;
                if (i == 0)
                {
                    strMinValue = m_MinValue.DataValue == 0 ? "0" : m_MinValue.DataValue.ToString(".####");
                }
                else
                {
                    sMinValue = m_MinValue.DataValue + valueInterval * i;
                    strMinValue = sMinValue == 0 ? "0" : sMinValue.ToString(".####");
                }

                string strMaxValue = "0";
                double sMaxValue = m_MinValue.DataValue;
                {
                    sMaxValue = m_MinValue.DataValue + valueInterval * (i + 1);
                    strMaxValue = sMaxValue == 0 ? "0" : sMaxValue.ToString(".####");
                }
                System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem.Name = "Range" + i.ToString();
                subItem.Text = strMinValue + "-" + strMaxValue;
                subItem.Tag = sMaxValue;
                item.SubItems.Add(subItem);

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem1.Name = "Label" + i.ToString();
                subItem1.Text = strMinValue + "-" + strMaxValue;
                item.SubItems.Add(subItem1);

                m_ListValueItem.Items.Add(item);
            }
        }

        private ISymbol[] CreateSymbols()
        {
            double sizeInterval = (m_MaxSize.DataValue - m_MinSize.DataValue) / m_SizeLevel.DataValue;

            ISymbol[] symbols = new ISymbol[m_SizeLevel.DataValue];
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            for (int i = 0; i < m_SizeLevel.DataValue; i++)
            {
                double size = m_MinSize.DataValue + sizeInterval * i;
                symbols[i] = copy.Copy(m_ForeSymbol.DataValue) as ISymbol;
                ModuleCommon.ChangeSymbolSize(symbols[i], size);
            }
            return symbols;
        }
    }
}
