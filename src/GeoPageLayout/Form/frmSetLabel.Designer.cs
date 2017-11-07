namespace GeoPageLayout
{
    partial class frmSetLabel
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
            this.listViewEx1 = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.btnUp = new DevComponents.DotNetBar.ButtonX();
            this.btnDn = new DevComponents.DotNetBar.ButtonX();
            this.btnTp = new DevComponents.DotNetBar.ButtonX();
            this.btnBt = new DevComponents.DotNetBar.ButtonX();
            this.btnSA = new DevComponents.DotNetBar.ButtonX();
            this.btnSR = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCs = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // listViewEx1
            // 
            // 
            // 
            // 
            this.listViewEx1.Border.Class = "ListViewBorder";
            this.listViewEx1.CheckBoxes = true;
            this.listViewEx1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewEx1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listViewEx1.Location = new System.Drawing.Point(0, 0);
            this.listViewEx1.Name = "listViewEx1";
            this.listViewEx1.Size = new System.Drawing.Size(192, 265);
            this.listViewEx1.TabIndex = 0;
            this.listViewEx1.UseCompatibleStateImageBehavior = false;
            this.listViewEx1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "标注字段";
            this.columnHeader1.Width = 182;
            // 
            // btnUp
            // 
            this.btnUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUp.Location = new System.Drawing.Point(198, 12);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(49, 23);
            this.btnUp.TabIndex = 1;
            this.btnUp.Text = "上移";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDn
            // 
            this.btnDn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDn.Location = new System.Drawing.Point(198, 41);
            this.btnDn.Name = "btnDn";
            this.btnDn.Size = new System.Drawing.Size(49, 23);
            this.btnDn.TabIndex = 2;
            this.btnDn.Text = "下移";
            this.btnDn.Click += new System.EventHandler(this.btnDn_Click);
            // 
            // btnTp
            // 
            this.btnTp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTp.Location = new System.Drawing.Point(198, 70);
            this.btnTp.Name = "btnTp";
            this.btnTp.Size = new System.Drawing.Size(49, 23);
            this.btnTp.TabIndex = 3;
            this.btnTp.Text = "置顶";
            this.btnTp.Click += new System.EventHandler(this.btnTp_Click);
            // 
            // btnBt
            // 
            this.btnBt.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBt.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBt.Location = new System.Drawing.Point(198, 99);
            this.btnBt.Name = "btnBt";
            this.btnBt.Size = new System.Drawing.Size(49, 23);
            this.btnBt.TabIndex = 4;
            this.btnBt.Text = "置底";
            this.btnBt.Click += new System.EventHandler(this.btnBt_Click);
            // 
            // btnSA
            // 
            this.btnSA.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSA.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSA.Location = new System.Drawing.Point(198, 128);
            this.btnSA.Name = "btnSA";
            this.btnSA.Size = new System.Drawing.Size(49, 23);
            this.btnSA.TabIndex = 5;
            this.btnSA.Text = "全选";
            this.btnSA.Click += new System.EventHandler(this.btnSA_Click);
            // 
            // btnSR
            // 
            this.btnSR.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSR.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSR.Location = new System.Drawing.Point(198, 157);
            this.btnSR.Name = "btnSR";
            this.btnSR.Size = new System.Drawing.Size(49, 23);
            this.btnSR.TabIndex = 6;
            this.btnSR.Text = "反选";
            this.btnSR.Click += new System.EventHandler(this.btnSR_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(198, 211);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(49, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCs
            // 
            this.btnCs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCs.Location = new System.Drawing.Point(198, 239);
            this.btnCs.Name = "btnCs";
            this.btnCs.Size = new System.Drawing.Size(49, 23);
            this.btnCs.TabIndex = 8;
            this.btnCs.Text = "关闭";
            this.btnCs.Click += new System.EventHandler(this.btnCs_Click);
            // 
            // frmSetLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 265);
            this.Controls.Add(this.btnCs);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSR);
            this.Controls.Add(this.btnSA);
            this.Controls.Add(this.btnBt);
            this.Controls.Add(this.btnTp);
            this.Controls.Add(this.btnDn);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.listViewEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetLabel";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "范围标注设置";
            this.Load += new System.EventHandler(this.frmMetaMap_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx listViewEx1;
        private DevComponents.DotNetBar.ButtonX btnUp;
        private DevComponents.DotNetBar.ButtonX btnDn;
        private DevComponents.DotNetBar.ButtonX btnTp;
        private DevComponents.DotNetBar.ButtonX btnBt;
        private DevComponents.DotNetBar.ButtonX btnSA;
        private DevComponents.DotNetBar.ButtonX btnSR;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCs;
        private System.Windows.Forms.ColumnHeader columnHeader1;

    }
}