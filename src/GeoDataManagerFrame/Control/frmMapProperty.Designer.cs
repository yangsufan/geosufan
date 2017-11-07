namespace GeoDataManagerFrame
{
    partial class frmMapProperty
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
            this.labProperty = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labName = new System.Windows.Forms.Label();
            this.labDivision = new System.Windows.Forms.Label();
            this.labType = new System.Windows.Forms.Label();
            this.labYear = new System.Windows.Forms.Label();
            this.labScale = new System.Windows.Forms.Label();
            this.labLayer = new System.Windows.Forms.Label();
            this.listLayer = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labNewScale = new System.Windows.Forms.Label();
            this.labNewYear = new System.Windows.Forms.Label();
            this.labNewType = new System.Windows.Forms.Label();
            this.labNewDivision = new System.Windows.Forms.Label();
            this.labNewname = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labProperty
            // 
            this.labProperty.AutoSize = true;
            this.labProperty.Location = new System.Drawing.Point(12, 9);
            this.labProperty.Name = "labProperty";
            this.labProperty.Size = new System.Drawing.Size(53, 12);
            this.labProperty.TabIndex = 0;
            this.labProperty.Text = "图件属性";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 1);
            this.panel1.TabIndex = 1;
            // 
            // labName
            // 
            this.labName.AutoSize = true;
            this.labName.Location = new System.Drawing.Point(12, 40);
            this.labName.Name = "labName";
            this.labName.Size = new System.Drawing.Size(59, 12);
            this.labName.TabIndex = 0;
            this.labName.Text = "图件名称:";
            // 
            // labDivision
            // 
            this.labDivision.AutoSize = true;
            this.labDivision.Location = new System.Drawing.Point(12, 63);
            this.labDivision.Name = "labDivision";
            this.labDivision.Size = new System.Drawing.Size(59, 12);
            this.labDivision.TabIndex = 0;
            this.labDivision.Text = "行政区划:";
            // 
            // labType
            // 
            this.labType.AutoSize = true;
            this.labType.Location = new System.Drawing.Point(12, 86);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(35, 12);
            this.labType.TabIndex = 0;
            this.labType.Text = "专题:";
            // 
            // labYear
            // 
            this.labYear.AutoSize = true;
            this.labYear.Location = new System.Drawing.Point(12, 109);
            this.labYear.Name = "labYear";
            this.labYear.Size = new System.Drawing.Size(35, 12);
            this.labYear.TabIndex = 0;
            this.labYear.Text = "年份:";
            // 
            // labScale
            // 
            this.labScale.AutoSize = true;
            this.labScale.Location = new System.Drawing.Point(12, 132);
            this.labScale.Name = "labScale";
            this.labScale.Size = new System.Drawing.Size(47, 12);
            this.labScale.TabIndex = 0;
            this.labScale.Text = "比例尺:";
            // 
            // labLayer
            // 
            this.labLayer.AutoSize = true;
            this.labLayer.Location = new System.Drawing.Point(12, 155);
            this.labLayer.Name = "labLayer";
            this.labLayer.Size = new System.Drawing.Size(59, 12);
            this.labLayer.TabIndex = 0;
            this.labLayer.Text = "图层组成:";
            // 
            // listLayer
            // 
            this.listLayer.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.listLayer.FormattingEnabled = true;
            this.listLayer.ItemHeight = 12;
            this.listLayer.Location = new System.Drawing.Point(14, 171);
            this.listLayer.Name = "listLayer";
            this.listLayer.ScrollAlwaysVisible = true;
            this.listLayer.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listLayer.Size = new System.Drawing.Size(336, 88);
            this.listLayer.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labNewScale);
            this.panel2.Controls.Add(this.labNewYear);
            this.panel2.Controls.Add(this.labNewType);
            this.panel2.Controls.Add(this.labNewDivision);
            this.panel2.Controls.Add(this.labNewname);
            this.panel2.Location = new System.Drawing.Point(83, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(263, 122);
            this.panel2.TabIndex = 4;
            // 
            // labNewScale
            // 
            this.labNewScale.AutoSize = true;
            this.labNewScale.Location = new System.Drawing.Point(3, 101);
            this.labNewScale.Name = "labNewScale";
            this.labNewScale.Size = new System.Drawing.Size(0, 12);
            this.labNewScale.TabIndex = 0;
            // 
            // labNewYear
            // 
            this.labNewYear.AutoSize = true;
            this.labNewYear.Location = new System.Drawing.Point(3, 79);
            this.labNewYear.Name = "labNewYear";
            this.labNewYear.Size = new System.Drawing.Size(0, 12);
            this.labNewYear.TabIndex = 0;
            // 
            // labNewType
            // 
            this.labNewType.AutoSize = true;
            this.labNewType.Location = new System.Drawing.Point(3, 58);
            this.labNewType.Name = "labNewType";
            this.labNewType.Size = new System.Drawing.Size(0, 12);
            this.labNewType.TabIndex = 0;
            // 
            // labNewDivision
            // 
            this.labNewDivision.AutoSize = true;
            this.labNewDivision.Location = new System.Drawing.Point(3, 36);
            this.labNewDivision.Name = "labNewDivision";
            this.labNewDivision.Size = new System.Drawing.Size(0, 12);
            this.labNewDivision.TabIndex = 0;
            // 
            // labNewname
            // 
            this.labNewname.AutoSize = true;
            this.labNewname.Location = new System.Drawing.Point(3, 13);
            this.labNewname.Name = "labNewname";
            this.labNewname.Size = new System.Drawing.Size(0, 12);
            this.labNewname.TabIndex = 0;
            // 
            // frmMapProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 286);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.listLayer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labLayer);
            this.Controls.Add(this.labScale);
            this.Controls.Add(this.labYear);
            this.Controls.Add(this.labType);
            this.Controls.Add(this.labDivision);
            this.Controls.Add(this.labName);
            this.Controls.Add(this.labProperty);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(368, 320);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(368, 320);
            this.Name = "frmMapProperty";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图件属性";
            this.Load += new System.EventHandler(this.frmMapProperty_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labProperty;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labName;
        private System.Windows.Forms.Label labDivision;
        private System.Windows.Forms.Label labType;
        private System.Windows.Forms.Label labYear;
        private System.Windows.Forms.Label labScale;
        private System.Windows.Forms.Label labLayer;
        private System.Windows.Forms.ListBox listLayer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labNewScale;
        private System.Windows.Forms.Label labNewYear;
        private System.Windows.Forms.Label labNewType;
        private System.Windows.Forms.Label labNewDivision;
        private System.Windows.Forms.Label labNewname;
    }
}