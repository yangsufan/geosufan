namespace GeoDataCenterFunLib
{
    partial class frmAnalyseInLibMap
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
            this.comboBoxArea = new System.Windows.Forms.ComboBox();
            this.comboBoxScale = new System.Windows.Forms.ComboBox();
            this.groupBoxArea = new System.Windows.Forms.GroupBox();
            this.groupBoxYear = new System.Windows.Forms.GroupBox();
            this.comboBoxYear = new System.Windows.Forms.ComboBox();
            this.groupBoxScale = new System.Windows.Forms.GroupBox();
            this.groupBoxSelect = new System.Windows.Forms.GroupBox();
            this.checkBoxAnachild = new System.Windows.Forms.CheckBox();
            this.checkBoxDelold = new System.Windows.Forms.CheckBox();
            this.checkBoxArea = new System.Windows.Forms.CheckBox();
            this.checkBoxScale = new System.Windows.Forms.CheckBox();
            this.checkBoxYear = new System.Windows.Forms.CheckBox();
            this.btn_Analys = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxArea.SuspendLayout();
            this.groupBoxYear.SuspendLayout();
            this.groupBoxScale.SuspendLayout();
            this.groupBoxSelect.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxArea
            // 
            this.comboBoxArea.DropDownHeight = 1;
            this.comboBoxArea.DropDownWidth = 1;
            this.comboBoxArea.IntegralHeight = false;
            this.comboBoxArea.Location = new System.Drawing.Point(6, 20);
            this.comboBoxArea.Name = "comboBoxArea";
            this.comboBoxArea.Size = new System.Drawing.Size(208, 20);
            this.comboBoxArea.TabIndex = 0;
            this.comboBoxArea.Click += new System.EventHandler(this.comboBoxArea_Click);
            // 
            // comboBoxScale
            // 
            this.comboBoxScale.FormattingEnabled = true;
            this.comboBoxScale.Location = new System.Drawing.Point(6, 19);
            this.comboBoxScale.Name = "comboBoxScale";
            this.comboBoxScale.Size = new System.Drawing.Size(208, 20);
            this.comboBoxScale.TabIndex = 1;
            // 
            // groupBoxArea
            // 
            this.groupBoxArea.Controls.Add(this.comboBoxArea);
            this.groupBoxArea.Enabled = false;
            this.groupBoxArea.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxArea.Location = new System.Drawing.Point(124, 57);
            this.groupBoxArea.Name = "groupBoxArea";
            this.groupBoxArea.Size = new System.Drawing.Size(242, 52);
            this.groupBoxArea.TabIndex = 2;
            this.groupBoxArea.TabStop = false;
            this.groupBoxArea.Text = "数据单元";
            // 
            // groupBoxYear
            // 
            this.groupBoxYear.Controls.Add(this.comboBoxYear);
            this.groupBoxYear.Enabled = false;
            this.groupBoxYear.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxYear.Location = new System.Drawing.Point(124, 107);
            this.groupBoxYear.Name = "groupBoxYear";
            this.groupBoxYear.Size = new System.Drawing.Size(242, 52);
            this.groupBoxYear.TabIndex = 0;
            this.groupBoxYear.TabStop = false;
            this.groupBoxYear.Text = "年度";
            // 
            // comboBoxYear
            // 
            this.comboBoxYear.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxYear.Location = new System.Drawing.Point(6, 15);
            this.comboBoxYear.Name = "comboBoxYear";
            this.comboBoxYear.Size = new System.Drawing.Size(208, 22);
            this.comboBoxYear.TabIndex = 1;
            // 
            // groupBoxScale
            // 
            this.groupBoxScale.Controls.Add(this.comboBoxScale);
            this.groupBoxScale.Enabled = false;
            this.groupBoxScale.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxScale.Location = new System.Drawing.Point(124, 166);
            this.groupBoxScale.Name = "groupBoxScale";
            this.groupBoxScale.Size = new System.Drawing.Size(242, 52);
            this.groupBoxScale.TabIndex = 0;
            this.groupBoxScale.TabStop = false;
            this.groupBoxScale.Text = "比例尺";
            // 
            // groupBoxSelect
            // 
            this.groupBoxSelect.Controls.Add(this.checkBoxAnachild);
            this.groupBoxSelect.Controls.Add(this.checkBoxDelold);
            this.groupBoxSelect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxSelect.Location = new System.Drawing.Point(42, 287);
            this.groupBoxSelect.Name = "groupBoxSelect";
            this.groupBoxSelect.Size = new System.Drawing.Size(343, 10);
            this.groupBoxSelect.TabIndex = 0;
            this.groupBoxSelect.TabStop = false;
            this.groupBoxSelect.Text = "选项";
            this.groupBoxSelect.Visible = false;
            // 
            // checkBoxAnachild
            // 
            this.checkBoxAnachild.AutoSize = true;
            this.checkBoxAnachild.Location = new System.Drawing.Point(24, 52);
            this.checkBoxAnachild.Name = "checkBoxAnachild";
            this.checkBoxAnachild.Size = new System.Drawing.Size(180, 18);
            this.checkBoxAnachild.TabIndex = 4;
            this.checkBoxAnachild.Text = "分析该行政区所辖的区域";
            this.checkBoxAnachild.UseVisualStyleBackColor = true;
            // 
            // checkBoxDelold
            // 
            this.checkBoxDelold.AutoSize = true;
            this.checkBoxDelold.Location = new System.Drawing.Point(24, 20);
            this.checkBoxDelold.Name = "checkBoxDelold";
            this.checkBoxDelold.Size = new System.Drawing.Size(250, 18);
            this.checkBoxDelold.TabIndex = 3;
            this.checkBoxDelold.Text = "删除[地图入库信息表]中的原有信息";
            this.checkBoxDelold.UseVisualStyleBackColor = true;
            // 
            // checkBoxArea
            // 
            this.checkBoxArea.AutoSize = true;
            this.checkBoxArea.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxArea.Location = new System.Drawing.Point(14, 71);
            this.checkBoxArea.Name = "checkBoxArea";
            this.checkBoxArea.Size = new System.Drawing.Size(108, 16);
            this.checkBoxArea.TabIndex = 0;
            this.checkBoxArea.Text = "以数据单元为主";
            this.checkBoxArea.UseVisualStyleBackColor = true;
            this.checkBoxArea.CheckedChanged += new System.EventHandler(this.checkBoxArea_CheckedChanged);
            // 
            // checkBoxScale
            // 
            this.checkBoxScale.AutoSize = true;
            this.checkBoxScale.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxScale.Location = new System.Drawing.Point(14, 185);
            this.checkBoxScale.Name = "checkBoxScale";
            this.checkBoxScale.Size = new System.Drawing.Size(96, 16);
            this.checkBoxScale.TabIndex = 1;
            this.checkBoxScale.Text = "以比例尺为主";
            this.checkBoxScale.UseVisualStyleBackColor = true;
            this.checkBoxScale.CheckedChanged += new System.EventHandler(this.checkBoxScale_CheckedChanged);
            // 
            // checkBoxYear
            // 
            this.checkBoxYear.AutoSize = true;
            this.checkBoxYear.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxYear.Location = new System.Drawing.Point(14, 128);
            this.checkBoxYear.Name = "checkBoxYear";
            this.checkBoxYear.Size = new System.Drawing.Size(84, 16);
            this.checkBoxYear.TabIndex = 2;
            this.checkBoxYear.Text = "以年度为主";
            this.checkBoxYear.UseVisualStyleBackColor = true;
            this.checkBoxYear.CheckedChanged += new System.EventHandler(this.checkBoxYear_CheckedChanged);
            // 
            // btn_Analys
            // 
            this.btn_Analys.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Analys.Location = new System.Drawing.Point(167, 238);
            this.btn_Analys.Name = "btn_Analys";
            this.btn_Analys.Size = new System.Drawing.Size(79, 31);
            this.btn_Analys.TabIndex = 3;
            this.btn_Analys.Text = "开始分析";
            this.btn_Analys.UseVisualStyleBackColor = true;
            this.btn_Analys.Click += new System.EventHandler(this.btn_Analys_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Cancel.Location = new System.Drawing.Point(287, 238);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(79, 31);
            this.btn_Cancel.TabIndex = 3;
            this.btn_Cancel.Text = "取  消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(42, 287);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(343, 10);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选项";
            this.groupBox2.Visible = false;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(24, 52);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(180, 18);
            this.checkBox3.TabIndex = 4;
            this.checkBox3.Text = "分析该行政区所辖的区域";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(24, 20);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(250, 18);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "删除[地图入库信息表]中的原有信息";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(130, 22);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(208, 20);
            this.comboBoxSource.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 41;
            this.label1.Text = "选择分析数据源:";
            // 
            // frmAnalyseInLibMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 281);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxSource);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Analys);
            this.Controls.Add(this.groupBoxYear);
            this.Controls.Add(this.groupBoxScale);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxSelect);
            this.Controls.Add(this.checkBoxScale);
            this.Controls.Add(this.groupBoxArea);
            this.Controls.Add(this.checkBoxYear);
            this.Controls.Add(this.checkBoxArea);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAnalyseInLibMap";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "入库图形数据分析";
            this.Load += new System.EventHandler(this.frmAnalyseInLibMap_Load);
            this.groupBoxArea.ResumeLayout(false);
            this.groupBoxYear.ResumeLayout(false);
            this.groupBoxScale.ResumeLayout(false);
            this.groupBoxSelect.ResumeLayout(false);
            this.groupBoxSelect.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxArea;
        private System.Windows.Forms.ComboBox comboBoxScale;
        private System.Windows.Forms.GroupBox groupBoxArea;
        private System.Windows.Forms.GroupBox groupBoxYear;
        private System.Windows.Forms.GroupBox groupBoxScale;
        private System.Windows.Forms.GroupBox groupBoxSelect;
        private System.Windows.Forms.CheckBox checkBoxAnachild;
        private System.Windows.Forms.CheckBox checkBoxYear;
        private System.Windows.Forms.CheckBox checkBoxScale;
        private System.Windows.Forms.CheckBox checkBoxArea;
        private System.Windows.Forms.CheckBox checkBoxDelold;
        private System.Windows.Forms.ComboBox comboBoxYear;
        private System.Windows.Forms.Button btn_Analys;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.ComboBox comboBoxSource;
        private System.Windows.Forms.Label label1;
    }
}