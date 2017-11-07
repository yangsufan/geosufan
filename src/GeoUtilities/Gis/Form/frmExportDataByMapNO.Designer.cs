namespace GeoUtilities.Gis.Form
{
    partial class frmExportDataByMapNO
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExportDataByMapNO));
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtMapNO = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtDesPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtDLGSrcPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnOpenDLGFolder = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnAdd = new DevComponents.DotNetBar.ButtonX();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.chkBoxDLG = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkBoxDEM = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkBoxDOM = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chckBoxDDBH = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtDDBH = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnOpenDEMFolder = new DevComponents.DotNetBar.ButtonX();
            this.txtDEMSrcPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnOpenDOMFolder = new DevComponents.DotNetBar.ButtonX();
            this.txtDOMSrcPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lstMapNOs = new System.Windows.Forms.CheckedListBox();
            this.bttinstead = new DevComponents.DotNetBar.ButtonX();
            this.bttAll = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 22);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(62, 17);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "图幅号：";
            // 
            // txtMapNO
            // 
            // 
            // 
            // 
            this.txtMapNO.Border.Class = "TextBoxBorder";
            this.txtMapNO.Location = new System.Drawing.Point(76, 18);
            this.txtMapNO.Name = "txtMapNO";
            this.txtMapNO.Size = new System.Drawing.Size(361, 21);
            this.txtMapNO.TabIndex = 1;
            this.txtMapNO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMapNO_KeyDown);
            // 
            // txtDesPath
            // 
            // 
            // 
            // 
            this.txtDesPath.Border.Class = "TextBoxBorder";
            this.txtDesPath.Location = new System.Drawing.Point(99, 284);
            this.txtDesPath.Name = "txtDesPath";
            this.txtDesPath.ReadOnly = true;
            this.txtDesPath.Size = new System.Drawing.Size(320, 21);
            this.txtDesPath.TabIndex = 3;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(10, 287);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(92, 17);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "拷贝目标路径：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(404, 316);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 25);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(312, 317);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 25);
            this.btnOK.TabIndex = 31;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(9, 12);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(115, 17);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "源数据文件路径：";
            // 
            // txtDLGSrcPath
            // 
            // 
            // 
            // 
            this.txtDLGSrcPath.Border.Class = "TextBoxBorder";
            this.txtDLGSrcPath.Location = new System.Drawing.Point(61, 38);
            this.txtDLGSrcPath.Name = "txtDLGSrcPath";
            this.txtDLGSrcPath.ReadOnly = true;
            this.txtDLGSrcPath.Size = new System.Drawing.Size(361, 21);
            this.txtDLGSrcPath.TabIndex = 1;
            // 
            // btnOpenDLGFolder
            // 
            this.btnOpenDLGFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenDLGFolder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOpenDLGFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenDLGFolder.Image")));
            this.btnOpenDLGFolder.Location = new System.Drawing.Point(422, 38);
            this.btnOpenDLGFolder.Name = "btnOpenDLGFolder";
            this.btnOpenDLGFolder.Size = new System.Drawing.Size(27, 21);
            this.btnOpenDLGFolder.TabIndex = 33;
            this.btnOpenDLGFolder.Text = "...";
            this.btnOpenDLGFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(447, 284);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(27, 21);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "...";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(447, 18);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(27, 21);
            this.btnAdd.TabIndex = 36;
            this.btnAdd.Tooltip = "批量导入图幅号(*.txt)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.Location = new System.Drawing.Point(447, 45);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(27, 21);
            this.btnDel.TabIndex = 37;
            this.btnDel.Text = "...";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // chkBoxDLG
            // 
            this.chkBoxDLG.BackColor = System.Drawing.Color.Transparent;
            this.chkBoxDLG.Location = new System.Drawing.Point(9, 35);
            this.chkBoxDLG.Name = "chkBoxDLG";
            this.chkBoxDLG.Size = new System.Drawing.Size(46, 23);
            this.chkBoxDLG.TabIndex = 39;
            this.chkBoxDLG.Text = "DLG";
            // 
            // chkBoxDEM
            // 
            this.chkBoxDEM.BackColor = System.Drawing.Color.Transparent;
            this.chkBoxDEM.Location = new System.Drawing.Point(9, 64);
            this.chkBoxDEM.Name = "chkBoxDEM";
            this.chkBoxDEM.Size = new System.Drawing.Size(43, 23);
            this.chkBoxDEM.TabIndex = 40;
            this.chkBoxDEM.Text = "DEM";
            // 
            // chkBoxDOM
            // 
            this.chkBoxDOM.BackColor = System.Drawing.Color.Transparent;
            this.chkBoxDOM.Location = new System.Drawing.Point(9, 93);
            this.chkBoxDOM.Name = "chkBoxDOM";
            this.chkBoxDOM.Size = new System.Drawing.Size(46, 23);
            this.chkBoxDOM.TabIndex = 41;
            this.chkBoxDOM.Text = "DOM";
            // 
            // chckBoxDDBH
            // 
            this.chckBoxDDBH.Location = new System.Drawing.Point(10, 318);
            this.chckBoxDDBH.Name = "chckBoxDDBH";
            this.chckBoxDDBH.Size = new System.Drawing.Size(92, 23);
            this.chckBoxDDBH.TabIndex = 42;
            this.chckBoxDDBH.Text = "订单编号：";
            this.chckBoxDDBH.CheckedChanged += new System.EventHandler(this.chckBoxDDBH_CheckedChanged);
            // 
            // txtDDBH
            // 
            // 
            // 
            // 
            this.txtDDBH.Border.Class = "TextBoxBorder";
            this.txtDDBH.Enabled = false;
            this.txtDDBH.Location = new System.Drawing.Point(99, 317);
            this.txtDDBH.Name = "txtDDBH";
            this.txtDDBH.Size = new System.Drawing.Size(178, 21);
            this.txtDDBH.TabIndex = 43;
            // 
            // btnOpenDEMFolder
            // 
            this.btnOpenDEMFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenDEMFolder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOpenDEMFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenDEMFolder.Image")));
            this.btnOpenDEMFolder.Location = new System.Drawing.Point(422, 66);
            this.btnOpenDEMFolder.Name = "btnOpenDEMFolder";
            this.btnOpenDEMFolder.Size = new System.Drawing.Size(27, 21);
            this.btnOpenDEMFolder.TabIndex = 45;
            this.btnOpenDEMFolder.Text = "...";
            this.btnOpenDEMFolder.Click += new System.EventHandler(this.btnOpenDEMFolder_Click);
            // 
            // txtDEMSrcPath
            // 
            // 
            // 
            // 
            this.txtDEMSrcPath.Border.Class = "TextBoxBorder";
            this.txtDEMSrcPath.Location = new System.Drawing.Point(61, 66);
            this.txtDEMSrcPath.Name = "txtDEMSrcPath";
            this.txtDEMSrcPath.ReadOnly = true;
            this.txtDEMSrcPath.Size = new System.Drawing.Size(361, 21);
            this.txtDEMSrcPath.TabIndex = 44;
            // 
            // btnOpenDOMFolder
            // 
            this.btnOpenDOMFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenDOMFolder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOpenDOMFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenDOMFolder.Image")));
            this.btnOpenDOMFolder.Location = new System.Drawing.Point(422, 93);
            this.btnOpenDOMFolder.Name = "btnOpenDOMFolder";
            this.btnOpenDOMFolder.Size = new System.Drawing.Size(27, 21);
            this.btnOpenDOMFolder.TabIndex = 47;
            this.btnOpenDOMFolder.Text = "...";
            this.btnOpenDOMFolder.Click += new System.EventHandler(this.btnOpenDOMFolder_Click);
            // 
            // txtDOMSrcPath
            // 
            // 
            // 
            // 
            this.txtDOMSrcPath.Border.Class = "TextBoxBorder";
            this.txtDOMSrcPath.Location = new System.Drawing.Point(61, 93);
            this.txtDOMSrcPath.Name = "txtDOMSrcPath";
            this.txtDOMSrcPath.ReadOnly = true;
            this.txtDOMSrcPath.Size = new System.Drawing.Size(361, 21);
            this.txtDOMSrcPath.TabIndex = 46;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txtDLGSrcPath);
            this.groupPanel1.Controls.Add(this.btnOpenDOMFolder);
            this.groupPanel1.Controls.Add(this.btnOpenDLGFolder);
            this.groupPanel1.Controls.Add(this.txtDOMSrcPath);
            this.groupPanel1.Controls.Add(this.chkBoxDLG);
            this.groupPanel1.Controls.Add(this.btnOpenDEMFolder);
            this.groupPanel1.Controls.Add(this.chkBoxDEM);
            this.groupPanel1.Controls.Add(this.txtDEMSrcPath);
            this.groupPanel1.Controls.Add(this.chkBoxDOM);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Location = new System.Drawing.Point(10, 148);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(464, 128);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 48;
            // 
            // lstMapNOs
            // 
            this.lstMapNOs.FormattingEnabled = true;
            this.lstMapNOs.Location = new System.Drawing.Point(74, 45);
            this.lstMapNOs.Name = "lstMapNOs";
            this.lstMapNOs.Size = new System.Drawing.Size(361, 100);
            this.lstMapNOs.TabIndex = 49;
            // 
            // bttinstead
            // 
            this.bttinstead.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttinstead.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttinstead.Location = new System.Drawing.Point(441, 109);
            this.bttinstead.Name = "bttinstead";
            this.bttinstead.Size = new System.Drawing.Size(39, 21);
            this.bttinstead.TabIndex = 51;
            this.bttinstead.Text = "反选";
            this.bttinstead.Click += new System.EventHandler(this.bttinstead_Click);
            // 
            // bttAll
            // 
            this.bttAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttAll.Location = new System.Drawing.Point(441, 82);
            this.bttAll.Name = "bttAll";
            this.bttAll.Size = new System.Drawing.Size(39, 21);
            this.bttAll.TabIndex = 52;
            this.bttAll.Text = "全选";
            this.bttAll.Click += new System.EventHandler(this.bttAll_Click);
            // 
            // frmExportDataByMapNO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 353);
            this.Controls.Add(this.bttAll);
            this.Controls.Add(this.bttinstead);
            this.Controls.Add(this.lstMapNOs);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.txtDDBH);
            this.Controls.Add(this.chckBoxDDBH);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtDesPath);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtMapNO);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExportDataByMapNO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据提取";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmExportDataByMapNO_Load);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMapNO;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDesPath;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDLGSrcPath;
        private DevComponents.DotNetBar.ButtonX btnOpenDLGFolder;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnAdd;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkBoxDLG;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkBoxDEM;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkBoxDOM;
        private DevComponents.DotNetBar.Controls.CheckBoxX chckBoxDDBH;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDDBH;
        private DevComponents.DotNetBar.ButtonX btnOpenDEMFolder;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDEMSrcPath;
        private DevComponents.DotNetBar.ButtonX btnOpenDOMFolder;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDOMSrcPath;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.CheckedListBox lstMapNOs;
        private DevComponents.DotNetBar.ButtonX bttinstead;
        private DevComponents.DotNetBar.ButtonX bttAll;
    }
}