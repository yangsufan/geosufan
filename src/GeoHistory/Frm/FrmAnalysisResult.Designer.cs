namespace GeoHistory
{
    partial class FrmAnalysisResult
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.shi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xiang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xbh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newdl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newlz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.olddl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oldlz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewX1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.shi,
            this.xian,
            this.xiang,
            this.cun,
            this.xbh,
            this.newdl,
            this.newlz,
            this.olddl,
            this.oldlz,
            this.mj});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.Size = new System.Drawing.Size(711, 262);
            this.dataGridViewX1.TabIndex = 0;
            this.dataGridViewX1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellContentClick);
            // 
            // shi
            // 
            this.shi.HeaderText = "市";
            this.shi.Name = "shi";
            this.shi.Width = 70;
            // 
            // xian
            // 
            this.xian.HeaderText = "县";
            this.xian.Name = "xian";
            this.xian.Width = 70;
            // 
            // xiang
            // 
            this.xiang.HeaderText = "乡";
            this.xiang.Name = "xiang";
            this.xiang.Width = 70;
            // 
            // cun
            // 
            this.cun.HeaderText = "村";
            this.cun.Name = "cun";
            this.cun.Width = 70;
            // 
            // xbh
            // 
            this.xbh.HeaderText = "小斑号";
            this.xbh.Name = "xbh";
            this.xbh.Width = 50;
            // 
            // newdl
            // 
            this.newdl.HeaderText = "地类(新)";
            this.newdl.Name = "newdl";
            this.newdl.Width = 62;
            // 
            // newlz
            // 
            this.newlz.HeaderText = "林种(新)";
            this.newlz.Name = "newlz";
            this.newlz.Width = 62;
            // 
            // olddl
            // 
            this.olddl.HeaderText = "地类(旧)";
            this.olddl.Name = "olddl";
            this.olddl.Width = 62;
            // 
            // oldlz
            // 
            this.oldlz.HeaderText = "林种(旧)";
            this.oldlz.Name = "oldlz";
            this.oldlz.Width = 62;
            // 
            // mj
            // 
            this.mj.HeaderText = "面积(平方米)";
            this.mj.Name = "mj";
            this.mj.Width = 90;
            // 
            // FrmAnalysisResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 262);
            this.Controls.Add(this.dataGridViewX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAnalysisResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "对比结果";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn shi;
        private System.Windows.Forms.DataGridViewTextBoxColumn xian;
        private System.Windows.Forms.DataGridViewTextBoxColumn xiang;
        private System.Windows.Forms.DataGridViewTextBoxColumn cun;
        private System.Windows.Forms.DataGridViewTextBoxColumn xbh;
        private System.Windows.Forms.DataGridViewTextBoxColumn newdl;
        private System.Windows.Forms.DataGridViewTextBoxColumn newlz;
        private System.Windows.Forms.DataGridViewTextBoxColumn olddl;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldlz;
        private System.Windows.Forms.DataGridViewTextBoxColumn mj;
    }
}