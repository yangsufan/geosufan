namespace GeoPageLayout
{
    partial class FrmUser
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gpPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.cBoxExXZQ = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.cBoxExZTLX = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnXCancel = new DevComponents.DotNetBar.ButtonX();
            this.gpPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpPanel1
            // 
            this.gpPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.gpPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.gpPanel1.Controls.Add(this.listView1);
            this.gpPanel1.Location = new System.Drawing.Point(12, 69);
            this.gpPanel1.Name = "gpPanel1";
            this.gpPanel1.Size = new System.Drawing.Size(163, 192);
            // 
            // 
            // 
            this.gpPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gpPanel1.Style.BackColorGradientAngle = 90;
            this.gpPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gpPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpPanel1.Style.BorderBottomWidth = 1;
            this.gpPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gpPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpPanel1.Style.BorderLeftWidth = 1;
            this.gpPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpPanel1.Style.BorderRightWidth = 1;
            this.gpPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gpPanel1.Style.BorderTopWidth = 1;
            this.gpPanel1.Style.CornerDiameter = 4;
            this.gpPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gpPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gpPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gpPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.gpPanel1.TabIndex = 0;
            this.gpPanel1.Text = "groupPanel1";
            // 
            // listView1
            // 
            this.listView1.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(157, 170);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "出图图层";
            this.columnHeader3.Width = 157;
            // 
            // cBoxExXZQ
            // 
            this.cBoxExXZQ.DisplayMember = "Text";
            this.cBoxExXZQ.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxExXZQ.FormattingEnabled = true;
            this.cBoxExXZQ.ItemHeight = 15;
            this.cBoxExXZQ.Items.AddRange(new object[] {
            this.comboItem1});
            this.cBoxExXZQ.Location = new System.Drawing.Point(212, 108);
            this.cBoxExXZQ.Name = "cBoxExXZQ";
            this.cBoxExXZQ.Size = new System.Drawing.Size(121, 21);
            this.cBoxExXZQ.TabIndex = 3;
            this.cBoxExXZQ.Text = "东莞市";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "东莞市";
            // 
            // cBoxExZTLX
            // 
            this.cBoxExZTLX.DisplayMember = "Text";
            this.cBoxExZTLX.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxExZTLX.FormattingEnabled = true;
            this.cBoxExZTLX.ItemHeight = 15;
            this.cBoxExZTLX.Location = new System.Drawing.Point(15, 32);
            this.cBoxExZTLX.Name = "cBoxExZTLX";
            this.cBoxExZTLX.Size = new System.Drawing.Size(318, 21);
            this.cBoxExZTLX.TabIndex = 2;
            this.cBoxExZTLX.SelectedIndexChanged += new System.EventHandler(this.cBoxExZTLX_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(191, 88);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "labelX2";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "labelX1";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(212, 195);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "buttonX1";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnXCancel
            // 
            this.btnXCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXCancel.Location = new System.Drawing.Point(212, 235);
            this.btnXCancel.Name = "btnXCancel";
            this.btnXCancel.Size = new System.Drawing.Size(75, 23);
            this.btnXCancel.TabIndex = 5;
            this.btnXCancel.Text = "buttonX2";
            this.btnXCancel.Click += new System.EventHandler(this.btnXCancel_Click);
            // 
            // FrmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 298);
            this.Controls.Add(this.cBoxExXZQ);
            this.Controls.Add(this.btnXCancel);
            this.Controls.Add(this.cBoxExZTLX);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.gpPanel1);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmUser";
            this.Load += new System.EventHandler(this.FrmUser_Load);
            this.gpPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel gpPanel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxExXZQ;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxExZTLX;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnXCancel;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private DevComponents.Editors.ComboItem comboItem1;
    }
}