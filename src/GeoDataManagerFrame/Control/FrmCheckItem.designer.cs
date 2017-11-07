namespace GeoDataManagerFrame
{
    partial class frmCheckItem
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.bttCanle = new DevComponents.DotNetBar.ButtonX();
            this.btnDefine = new DevComponents.DotNetBar.ButtonX();
            this.btnReturnselection = new DevComponents.DotNetBar.ButtonX();
            this.btnAllselection = new DevComponents.DotNetBar.ButtonX();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkedListBoxItem = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel2.Controls.Add(this.bttCanle, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDefine, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnReturnselection, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAllselection, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 237);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(241, 29);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // bttCanle
            // 
            this.bttCanle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttCanle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttCanle.Location = new System.Drawing.Point(181, 3);
            this.bttCanle.Name = "bttCanle";
            this.bttCanle.Size = new System.Drawing.Size(57, 23);
            this.bttCanle.TabIndex = 7;
            this.bttCanle.Text = "取消";
            this.bttCanle.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDefine
            // 
            this.btnDefine.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDefine.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDefine.Location = new System.Drawing.Point(114, 3);
            this.btnDefine.Name = "btnDefine";
            this.btnDefine.Size = new System.Drawing.Size(61, 23);
            this.btnDefine.TabIndex = 7;
            this.btnDefine.Text = "确定";
            this.btnDefine.Click += new System.EventHandler(this.btnDefine_Click);
            // 
            // btnReturnselection
            // 
            this.btnReturnselection.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReturnselection.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReturnselection.Location = new System.Drawing.Point(59, 3);
            this.btnReturnselection.Name = "btnReturnselection";
            this.btnReturnselection.Size = new System.Drawing.Size(49, 23);
            this.btnReturnselection.TabIndex = 7;
            this.btnReturnselection.Text = "反选";
            this.btnReturnselection.Click += new System.EventHandler(this.btnReturnselection_Click);
            // 
            // btnAllselection
            // 
            this.btnAllselection.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAllselection.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAllselection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAllselection.Location = new System.Drawing.Point(3, 3);
            this.btnAllselection.Name = "btnAllselection";
            this.btnAllselection.Size = new System.Drawing.Size(50, 23);
            this.btnAllselection.TabIndex = 7;
            this.btnAllselection.Text = "全选";
            this.btnAllselection.Click += new System.EventHandler(this.btnAllselection_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkedListBoxItem, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(247, 269);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // checkedListBoxItem
            // 
            this.checkedListBoxItem.CheckOnClick = true;
            this.checkedListBoxItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxItem.FormattingEnabled = true;
            this.checkedListBoxItem.Location = new System.Drawing.Point(3, 3);
            this.checkedListBoxItem.Name = "checkedListBoxItem";
            this.checkedListBoxItem.Size = new System.Drawing.Size(241, 228);
            this.checkedListBoxItem.TabIndex = 4;
            // 
            // frmCheckItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 269);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCheckItem";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "属性显示项设置";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckedListBox checkedListBoxItem;
        private DevComponents.DotNetBar.ButtonX btnAllselection;
        private DevComponents.DotNetBar.ButtonX btnReturnselection;
        private DevComponents.DotNetBar.ButtonX btnDefine;
        private DevComponents.DotNetBar.ButtonX bttCanle;



    }
}