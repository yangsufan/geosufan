using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSymbology.Form
{
    public class frmSymbolEdit : DevComponents.DotNetBar.Office2007Form
    {
        private IEditItem m_EditItem;
        private Class.SymbolObject m_SymbolObject;

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonBrowse;
        private DevComponents.DotNetBar.ButtonX buttonCancel;
        private DevComponents.DotNetBar.ButtonX buttonOK;
        private DevComponents.DotNetBar.AdvPropertyGrid advGridSymbol;
        private DevComponents.DotNetBar.LabelX labelPreview;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbStyleFiles;
        private ESRI.ArcGIS.Controls.AxSymbologyControl axSymbologyControl1;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSymbolEdit));
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonBrowse = new DevComponents.DotNetBar.ButtonX();
            this.axSymbologyControl1 = new ESRI.ArcGIS.Controls.AxSymbologyControl();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.advGridSymbol = new DevComponents.DotNetBar.AdvPropertyGrid();
            this.labelPreview = new DevComponents.DotNetBar.LabelX();
            this.cmbStyleFiles = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.axSymbologyControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advGridSymbol)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(4, 8);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(50, 18);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "符号库:";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonBrowse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonBrowse.Location = new System.Drawing.Point(480, 5);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(33, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "浏览";
            this.buttonBrowse.Click += new System.EventHandler(this.Button_Click);
            // 
            // axSymbologyControl1
            // 
            this.axSymbologyControl1.Location = new System.Drawing.Point(4, 34);
            this.axSymbologyControl1.Name = "axSymbologyControl1";
            this.axSymbologyControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSymbologyControl1.OcxState")));
            this.axSymbologyControl1.Size = new System.Drawing.Size(313, 338);
            this.axSymbologyControl1.TabIndex = 3;
            this.axSymbologyControl1.OnItemSelected += new ESRI.ArcGIS.Controls.ISymbologyControlEvents_Ax_OnItemSelectedEventHandler(this.SymbologyControl_OnItemSelected);
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.Location = new System.Drawing.Point(465, 349);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(48, 23);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.Location = new System.Drawing.Point(411, 349);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(48, 23);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.Button_Click);
            // 
            // advGridSymbol
            // 
            this.advGridSymbol.Location = new System.Drawing.Point(322, 88);
            this.advGridSymbol.Margin = new System.Windows.Forms.Padding(0);
            this.advGridSymbol.Name = "advGridSymbol";
            this.advGridSymbol.PropertySort = DevComponents.DotNetBar.ePropertySort.Alphabetical;
            this.advGridSymbol.Size = new System.Drawing.Size(192, 207);
            this.advGridSymbol.TabIndex = 15;
            this.advGridSymbol.ToolbarVisible = false;
            this.advGridSymbol.PropertyValueChanged += new System.ComponentModel.PropertyChangedEventHandler(this.advGridSymbol_PropertyValueChanged);
            // 
            // labelPreview
            // 
            this.labelPreview.BackColor = System.Drawing.SystemColors.Window;
            this.labelPreview.ImagePosition = DevComponents.DotNetBar.eImagePosition.Right;
            this.labelPreview.Location = new System.Drawing.Point(323, 34);
            this.labelPreview.Name = "labelPreview";
            this.labelPreview.Size = new System.Drawing.Size(190, 55);
            this.labelPreview.TabIndex = 16;
            this.labelPreview.Text = " 符号预览:";
            // 
            // cmbStyleFiles
            // 
            this.cmbStyleFiles.DisplayMember = "Text";
            this.cmbStyleFiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbStyleFiles.FormattingEnabled = true;
            this.cmbStyleFiles.ItemHeight = 15;
            this.cmbStyleFiles.Location = new System.Drawing.Point(50, 5);
            this.cmbStyleFiles.Name = "cmbStyleFiles";
            this.cmbStyleFiles.Size = new System.Drawing.Size(424, 21);
            this.cmbStyleFiles.TabIndex = 17;
            this.cmbStyleFiles.SelectedIndexChanged += new System.EventHandler(this.cmbStyleFiles_SelectedIndexChanged);
            // 
            // frmSymbolEdit
            // 
            this.ClientSize = new System.Drawing.Size(517, 378);
            this.Controls.Add(this.cmbStyleFiles);
            this.Controls.Add(this.labelPreview);
            this.Controls.Add(this.advGridSymbol);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.axSymbologyControl1);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSymbolEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "符号配置";
            ((System.ComponentModel.ISupportInitialize)(this.axSymbologyControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advGridSymbol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private string m_EditType;
        public frmSymbolEdit(IEditItem _EditItem, ESRI.ArcGIS.Display.ISymbol _EditValue, string editType)
        {
            InitializeComponent();
            m_SymbolObject = Class.SymbolObject.CreateClassSymbol(_EditValue);
            m_EditType = editType;
            string sInstall = ModuleCommon.ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
            if (sInstall == "") //added by chulili 2012-11-13 平台由ArcGIS9.3换成ArcGIS10，相应的注册表路径要修改
            {
                sInstall = ModuleCommon.ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
            }
            if (sInstall == "")
            {
                sInstall = ModuleCommon.ReadRegistry("SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
            }   //added by chulili 2012-11-13  end
            System.IO.DirectoryInfo pDic = new System.IO.DirectoryInfo(sInstall + "\\Styles\\");
            System.IO.FileInfo[] files = pDic.GetFiles("*.ServerStyle");
            cmbStyleFiles.Items.Clear();
            for (int i = 0; i < files.Length; i++)
            {
                cmbStyleFiles.Items.Add(sInstall + "\\Styles\\" + files[i].ToString());
            }
            int index = cmbStyleFiles.Items.IndexOf(sInstall + "\\Styles\\ESRI.ServerStyle");
            if (index != -1) cmbStyleFiles.SelectedIndex = index;
            else if (cmbStyleFiles.Items.Count > 0) cmbStyleFiles.SelectedIndex = 0;


            m_EditItem = _EditItem;
            labelPreview.Tag = _EditValue;
            if (labelPreview.Image != null)
            {
                labelPreview.Image.Dispose();
                labelPreview.Image = null;
            }
            labelPreview.Image = ModuleCommon.Symbol2Picture(_EditValue, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);

            advGridSymbol.SelectedObject = m_SymbolObject;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ButtonX button = sender as DevComponents.DotNetBar.ButtonX;
            switch (button.Name)
            {
                case "buttonOK":
                    m_SymbolObject.ReGenerateSymbol(labelPreview.Tag as ESRI.ArcGIS.Display.ISymbol);
                    m_EditItem.DoAfterEdit(labelPreview.Tag, System.Windows.Forms.DialogResult.OK, m_EditType);
                    axSymbologyControl1.Clear();
                    this.Close();
                    break;
                case "buttonCancel":
                    //m_EditItem.DoAfterEdit(null, System.Windows.Forms.DialogResult.Cancel, m_EditType);
                    axSymbologyControl1.Clear();
                    this.Close();
                    break;
                case "buttonBrowse":
                    System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();
                    openDialog.RestoreDirectory = true;
                    openDialog.Filter = "Engine符号库文件 (*.ServerStyle)|*.ServerStyle|ArcGIS符号库文件 (*.Style)|*.Style";
                    openDialog.Title = "选择符号库";
                    openDialog.ShowReadOnly = true;
                    openDialog.Multiselect = false;
                    if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        cmbStyleFiles.Items.Add(openDialog.FileName);
                        cmbStyleFiles.Text = openDialog.FileName;
                    }
                    break;
            }
        }

        private void SymbologyControl_OnItemSelected(object sender, ESRI.ArcGIS.Controls.ISymbologyControlEvents_OnItemSelectedEvent e)
        {
            ESRI.ArcGIS.Display.IStyleGalleryItem galleryItem = e.styleGalleryItem as ESRI.ArcGIS.Display.IStyleGalleryItem;
            if (galleryItem.Item is ESRI.ArcGIS.Display.IMarkerSymbol)
            {
                m_SymbolObject.InitClassSymbol(galleryItem.Item as ESRI.ArcGIS.Display.ISymbol);
            }
            else if (galleryItem.Item is ESRI.ArcGIS.Display.ILineSymbol)
            {
                m_SymbolObject.InitClassSymbol(galleryItem.Item as ESRI.ArcGIS.Display.ISymbol);
            }
            else if (galleryItem.Item is ESRI.ArcGIS.Display.IFillSymbol)
            {
                m_SymbolObject.InitClassSymbol(galleryItem.Item as ESRI.ArcGIS.Display.ISymbol);
            }
            labelPreview.Tag = galleryItem.Item;
            if (labelPreview.Image != null)
            {
                labelPreview.Image.Dispose();
                labelPreview.Image = null;
            }
            labelPreview.Image = ModuleCommon.Symbol2Picture(galleryItem.Item as ESRI.ArcGIS.Display.ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
            for (int i = 0; i < m_SymbolObject.PropertyNames.Length; i++)
                advGridSymbol.UpdatePropertyValue(m_SymbolObject.PropertyNames[i]);
        }

        private void cmbStyleFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            axSymbologyControl1.Clear();
            if (cmbStyleFiles.SelectedItem.ToString().Contains("ServerStyle"))
                axSymbologyControl1.LoadStyleFile(cmbStyleFiles.SelectedItem.ToString());
            else
                axSymbologyControl1.LoadDesktopStyleFile(cmbStyleFiles.SelectedItem.ToString());
            axSymbologyControl1.StyleClass = m_SymbolObject.StyleClass;
        }

        private void advGridSymbol_PropertyValueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            m_SymbolObject.ReGenerateSymbol(labelPreview.Tag as ESRI.ArcGIS.Display.ISymbol);
            if (labelPreview.Image != null)
            {
                labelPreview.Image.Dispose();
                labelPreview.Image = null;
            }
            labelPreview.Image = ModuleCommon.Symbol2Picture(labelPreview.Tag as ESRI.ArcGIS.Display.ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
        }
    }
}
