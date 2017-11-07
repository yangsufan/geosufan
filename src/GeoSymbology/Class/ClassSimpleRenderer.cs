using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;

namespace GeoSymbology
{

    public class ClassSimpleRenderer : ClassRenderer,IEditItem
    {
        private StringValue m_RendererName;
        /// <summary>
        /// Ãû³Æ
        /// </summary>
        public StringValue RendererName
        {
            get { return m_RendererName; }
            set { m_RendererName = value; }
        }

        private StringValue m_RendererDesc;
        /// <summary>
        /// ÃèÊö
        /// </summary>
        public StringValue RendererDesc
        {
            get { return m_RendererDesc; }
            set { m_RendererDesc = value; }
        }

        public ClassSimpleRenderer():base()
        {
        }

        public override void InitRendererObject(IFeatureRenderer _Renderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            m_SymbologyStyleClass = _SymbologyStyleClass;

            ISimpleRenderer _SimpleRenderer = null;
            if ((_Renderer is ISimpleRenderer) == false)
            {
                _SimpleRenderer = new SimpleRendererClass();
                _SimpleRenderer.Label = "";
                _SimpleRenderer.Description = "";
                _SimpleRenderer.Symbol = ModuleCommon.CreateSymbol(_SymbologyStyleClass);
            }
            else
            {
                _SimpleRenderer = _Renderer as ISimpleRenderer;
            }

            #region RendererName

            m_RendererName = new StringValue();
            m_RendererName.ControlName = "RendererName";
            m_RendererName.Caption = "Ãû³Æ";
            m_RendererName.ControlWidth = 100;
            m_RendererName.ControlHeight = 23;
            m_RendererName.DataValue = _SimpleRenderer.Label;

            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            node.Text = m_RendererName.Caption;
            node.Name = "node" + m_RendererName.ControlName;
            DevComponents.AdvTree.Cell cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetTextBox(m_RendererName);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);

            #endregion

            #region RendererDesc

            m_RendererDesc = new StringValue();
            m_RendererDesc.ControlName = "RendererDesc";
            m_RendererDesc.Caption = "ÃèÊö";
            m_RendererDesc.ControlWidth = 100;
            m_RendererDesc.ControlHeight = 23;
            m_RendererDesc.DataValue = _SimpleRenderer.Description;

            node = new DevComponents.AdvTree.Node();
            node.Text = m_RendererDesc.Caption;
            node.Name = "node" + m_RendererDesc.ControlName;
            cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetTextBox(m_RendererDesc);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);

            #endregion

            #region ForeSymbol

            m_ForeSymbol = new SymbolValue();
            m_ForeSymbol.ControlName = "SimpleSymbol";
            m_ForeSymbol.Caption = "·ûºÅ";
            m_ForeSymbol.ControlWidth = 100;
            m_ForeSymbol.ControlHeight = 50;
            m_ForeSymbol.DataValue = _SimpleRenderer.Symbol;

            node = new DevComponents.AdvTree.Node();
            node.Text = m_ForeSymbol.Caption;
            node.Name = "node" + m_ForeSymbol.ControlName;
            cell = new DevComponents.AdvTree.Cell();
            cell.HostedControl = GetSymbolButton(m_ForeSymbol);
            node.Cells.Add(cell);
            m_TreeProperty.Nodes.Add(node);

            #endregion
        }

        public override void DoButtonClick(DevComponents.DotNetBar.ButtonX button)
        {
            switch (button.Name)
            {
                case "SimpleSymbol":
                    {
                        m_EditObject = button;
                        Form.frmSymbolEdit frm = new GeoSymbology.Form.frmSymbolEdit(this, m_ForeSymbol.DataValue,"");
                        frm.ShowDialog();
                    }
                    break;
            }
        }

        public override void DoListValueItemMouseDoubleClick(int x, int y)
        {
            
        }

        public override IFeatureRenderer FeatureRenderer
        {
            get
            {
                ISimpleRenderer renderer = new SimpleRendererClass();
                renderer.Label = m_TreeProperty.Nodes[0].Cells[1].HostedControl.Text;
                renderer.Description = m_TreeProperty.Nodes[2].Cells[1].HostedControl.Text;
                renderer.Symbol = m_ForeSymbol.DataValue;
                return renderer as IFeatureRenderer;
            }
        }

        public override void RefreshValueItem()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #region IEditItem ³ÉÔ±

        private object m_EditObject;

        public void DoAfterEdit(object newValue, System.Windows.Forms.DialogResult result,string editType)
        {
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                m_EditObject = null;
                return;
            }

            if (m_EditObject is DevComponents.DotNetBar.ButtonX)
            {
                DevComponents.DotNetBar.ButtonX button = m_EditObject as DevComponents.DotNetBar.ButtonX;
                m_ForeSymbol.DataValue = newValue as ISymbol;
                if (button.Image != null)
                {
                    button.Image.Dispose();
                    button.Image = null;
                }
                button.Image = ModuleCommon.Symbol2Picture(m_ForeSymbol.DataValue, 80, 40);
            }
        }

        #endregion
    }
}
