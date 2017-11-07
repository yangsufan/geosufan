namespace GeoDBATool
{
    partial class FrmGetGeometry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetGeometry));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.label1 = new System.Windows.Forms.Label();
            this.combox_layers = new System.Windows.Forms.ComboBox();
            this.btn_Cancle = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.Btn_getRangeByText = new System.Windows.Forms.Button();
            this.btn_InputLayer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_Ip = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(42, 52);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(450, 301);
            this.axMapControl1.TabIndex = 0;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(460, 359);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(8, 52);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(28, 301);
            this.axToolbarControl1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 371);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "图幅范围图层：";
            // 
            // combox_layers
            // 
            this.combox_layers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combox_layers.FormattingEnabled = true;
            this.combox_layers.Location = new System.Drawing.Point(117, 365);
            this.combox_layers.Name = "combox_layers";
            this.combox_layers.Size = new System.Drawing.Size(337, 20);
            this.combox_layers.TabIndex = 0;
            this.combox_layers.SelectedIndexChanged += new System.EventHandler(this.combox_layers_SelectedIndexChanged);
            // 
            // btn_Cancle
            // 
            this.btn_Cancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancle.Location = new System.Drawing.Point(417, 401);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancle.TabIndex = 4;
            this.btn_Cancle.Text = "取 消";
            this.btn_Cancle.UseVisualStyleBackColor = true;
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(326, 401);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 5;
            this.btn_OK.Text = "确 定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // Btn_getRangeByText
            // 
            this.Btn_getRangeByText.Location = new System.Drawing.Point(8, 401);
            this.Btn_getRangeByText.Name = "Btn_getRangeByText";
            this.Btn_getRangeByText.Size = new System.Drawing.Size(75, 23);
            this.Btn_getRangeByText.TabIndex = 6;
            this.Btn_getRangeByText.Text = "导入范围";
            this.Btn_getRangeByText.UseVisualStyleBackColor = true;
            this.Btn_getRangeByText.Click += new System.EventHandler(this.Btn_getRangeByText_Click);
            // 
            // btn_InputLayer
            // 
            this.btn_InputLayer.Location = new System.Drawing.Point(89, 401);
            this.btn_InputLayer.Name = "btn_InputLayer";
            this.btn_InputLayer.Size = new System.Drawing.Size(75, 23);
            this.btn_InputLayer.TabIndex = 7;
            this.btn_InputLayer.Text = "导入图层";
            this.btn_InputLayer.UseVisualStyleBackColor = true;
            this.btn_InputLayer.Click += new System.EventHandler(this.btn_InputLayer_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_Ip);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(8, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 36);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "发布服务器设置：";
            // 
            // txt_Ip
            // 
            // 
            // 
            // 
            this.txt_Ip.Border.Class = "TextBoxBorder";
            this.txt_Ip.Location = new System.Drawing.Point(143, 11);
            this.txt_Ip.Name = "txt_Ip";
            this.txt_Ip.Size = new System.Drawing.Size(320, 21);
            this.txt_Ip.TabIndex = 1;
            this.txt_Ip.WatermarkText = "数据发布服务器的IP地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "发布服务器IP地址：";
            // 
            // FrmGetGeometry
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancle;
            this.ClientSize = new System.Drawing.Size(501, 431);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_InputLayer);
            this.Controls.Add(this.Btn_getRangeByText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.combox_layers);
            this.Controls.Add(this.btn_Cancle);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmGetGeometry";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择更新发布的图幅范围:";
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.Button btn_Cancle;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox combox_layers;
        private System.Windows.Forms.Button Btn_getRangeByText;
        private System.Windows.Forms.Button btn_InputLayer;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_Ip;
        private System.Windows.Forms.Label label2;





    }
}

