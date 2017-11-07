namespace GeoDataManagerFrame
{
    partial class FrmMapBookMark
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
            this.colHName = new System.Windows.Forms.ColumnHeader();
            this.btnXZoomTo = new DevComponents.DotNetBar.ButtonX();
            this.btnXPanTo = new DevComponents.DotNetBar.ButtonX();
            this.btnXCreate = new DevComponents.DotNetBar.ButtonX();
            this.btnXUp = new DevComponents.DotNetBar.ButtonX();
            this.btnXTop = new DevComponents.DotNetBar.ButtonX();
            this.btnXBottom = new DevComponents.DotNetBar.ButtonX();
            this.btnXDown = new DevComponents.DotNetBar.ButtonX();
            this.btnXDelete = new DevComponents.DotNetBar.ButtonX();
            this.btnXDeleteAll = new DevComponents.DotNetBar.ButtonX();
            this.btnXClose = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // listViewEx1
            // 
            // 
            // 
            // 
            this.listViewEx1.Border.Class = "ListViewBorder";
            this.listViewEx1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHName});
            this.listViewEx1.Location = new System.Drawing.Point(12, 12);
            this.listViewEx1.Name = "listViewEx1";
            this.listViewEx1.Size = new System.Drawing.Size(187, 242);
            this.listViewEx1.TabIndex = 0;
            this.listViewEx1.UseCompatibleStateImageBehavior = false;
            this.listViewEx1.View = System.Windows.Forms.View.Details;
            this.listViewEx1.SelectedIndexChanged += new System.EventHandler(this.listViewEx1_SelectedIndexChanged);
            this.listViewEx1.DoubleClick += new System.EventHandler(this.listViewEx1_DoubleClick);
            this.listViewEx1.Click += new System.EventHandler(this.listViewEx1_Click);
            // 
            // colHName
            // 
            this.colHName.Text = "书签名称";
            this.colHName.Width = 200;
            // 
            // btnXZoomTo
            // 
            this.btnXZoomTo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXZoomTo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXZoomTo.Enabled = false;
            this.btnXZoomTo.Location = new System.Drawing.Point(205, 12);
            this.btnXZoomTo.Name = "btnXZoomTo";
            this.btnXZoomTo.Size = new System.Drawing.Size(75, 23);
            this.btnXZoomTo.TabIndex = 1;
            this.btnXZoomTo.Text = "切换";
            this.btnXZoomTo.DoubleClick += new System.EventHandler(this.btnXZoomTo_DoubleClick);
            this.btnXZoomTo.Click += new System.EventHandler(this.btnXZoomTo_Click);
            // 
            // btnXPanTo
            // 
            this.btnXPanTo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXPanTo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXPanTo.Location = new System.Drawing.Point(101, 211);
            this.btnXPanTo.Name = "btnXPanTo";
            this.btnXPanTo.Size = new System.Drawing.Size(75, 23);
            this.btnXPanTo.TabIndex = 2;
            this.btnXPanTo.Text = "切换";
            this.btnXPanTo.Visible = false;
            // 
            // btnXCreate
            // 
            this.btnXCreate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXCreate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXCreate.Location = new System.Drawing.Point(205, 41);
            this.btnXCreate.Name = "btnXCreate";
            this.btnXCreate.Size = new System.Drawing.Size(75, 23);
            this.btnXCreate.TabIndex = 3;
            this.btnXCreate.Text = "创建";
            this.btnXCreate.Click += new System.EventHandler(this.btnXCreate_Click);
            // 
            // btnXUp
            // 
            this.btnXUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXUp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXUp.Enabled = false;
            this.btnXUp.Location = new System.Drawing.Point(205, 70);
            this.btnXUp.Name = "btnXUp";
            this.btnXUp.Size = new System.Drawing.Size(33, 23);
            this.btnXUp.TabIndex = 4;
            this.btnXUp.Text = "上移";
            this.btnXUp.Click += new System.EventHandler(this.btnXUp_Click);
            // 
            // btnXTop
            // 
            this.btnXTop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXTop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXTop.Enabled = false;
            this.btnXTop.Location = new System.Drawing.Point(247, 70);
            this.btnXTop.Name = "btnXTop";
            this.btnXTop.Size = new System.Drawing.Size(33, 23);
            this.btnXTop.TabIndex = 5;
            this.btnXTop.Text = "置顶";
            this.btnXTop.Click += new System.EventHandler(this.btnXTop_Click);
            // 
            // btnXBottom
            // 
            this.btnXBottom.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXBottom.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXBottom.Enabled = false;
            this.btnXBottom.Location = new System.Drawing.Point(247, 99);
            this.btnXBottom.Name = "btnXBottom";
            this.btnXBottom.Size = new System.Drawing.Size(33, 23);
            this.btnXBottom.TabIndex = 7;
            this.btnXBottom.Text = "置底";
            this.btnXBottom.Click += new System.EventHandler(this.btnXBottom_Click);
            // 
            // btnXDown
            // 
            this.btnXDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXDown.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXDown.Enabled = false;
            this.btnXDown.Location = new System.Drawing.Point(205, 99);
            this.btnXDown.Name = "btnXDown";
            this.btnXDown.Size = new System.Drawing.Size(33, 23);
            this.btnXDown.TabIndex = 6;
            this.btnXDown.Text = "下移";
            this.btnXDown.Click += new System.EventHandler(this.btnXDown_Click);
            // 
            // btnXDelete
            // 
            this.btnXDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXDelete.Enabled = false;
            this.btnXDelete.Location = new System.Drawing.Point(205, 128);
            this.btnXDelete.Name = "btnXDelete";
            this.btnXDelete.Size = new System.Drawing.Size(75, 23);
            this.btnXDelete.TabIndex = 8;
            this.btnXDelete.Text = "删除";
            this.btnXDelete.Click += new System.EventHandler(this.btnXDelete_Click);
            // 
            // btnXDeleteAll
            // 
            this.btnXDeleteAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXDeleteAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXDeleteAll.Location = new System.Drawing.Point(205, 157);
            this.btnXDeleteAll.Name = "btnXDeleteAll";
            this.btnXDeleteAll.Size = new System.Drawing.Size(75, 23);
            this.btnXDeleteAll.TabIndex = 9;
            this.btnXDeleteAll.Text = "全删";
            this.btnXDeleteAll.Click += new System.EventHandler(this.btnXDeleteAll_Click);
            // 
            // btnXClose
            // 
            this.btnXClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXClose.Location = new System.Drawing.Point(205, 231);
            this.btnXClose.Name = "btnXClose";
            this.btnXClose.Size = new System.Drawing.Size(75, 23);
            this.btnXClose.TabIndex = 10;
            this.btnXClose.Text = "关闭";
            this.btnXClose.Click += new System.EventHandler(this.btnXClose_Click);
            // 
            // FrmMapBookMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.btnXClose);
            this.Controls.Add(this.btnXDeleteAll);
            this.Controls.Add(this.btnXDelete);
            this.Controls.Add(this.btnXBottom);
            this.Controls.Add(this.btnXDown);
            this.Controls.Add(this.btnXTop);
            this.Controls.Add(this.btnXUp);
            this.Controls.Add(this.btnXCreate);
            this.Controls.Add(this.btnXZoomTo);
            this.Controls.Add(this.listViewEx1);
            this.Controls.Add(this.btnXPanTo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMapBookMark";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图书签管理";
            this.Load += new System.EventHandler(this.FrmMapBookMark_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMapBookMark_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx listViewEx1;
        private DevComponents.DotNetBar.ButtonX btnXZoomTo;
        private DevComponents.DotNetBar.ButtonX btnXPanTo;
        private DevComponents.DotNetBar.ButtonX btnXCreate;
        private DevComponents.DotNetBar.ButtonX btnXUp;
        private DevComponents.DotNetBar.ButtonX btnXTop;
        private DevComponents.DotNetBar.ButtonX btnXBottom;
        private DevComponents.DotNetBar.ButtonX btnXDown;
        private DevComponents.DotNetBar.ButtonX btnXDelete;
        private DevComponents.DotNetBar.ButtonX btnXDeleteAll;
        private DevComponents.DotNetBar.ButtonX btnXClose;
        private System.Windows.Forms.ColumnHeader colHName;
    }
}