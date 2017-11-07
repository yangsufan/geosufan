namespace GeoDBATool
{
    partial class FrmInputTempToProduction
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
            this.btnInputTempData = new DevComponents.DotNetBar.ButtonX();
            this.cbProNameList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.lblState = new DevComponents.DotNetBar.LabelX();
            this.prbInputTempData = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.SuspendLayout();
            // 
            // btnInputTempData
            // 
            this.btnInputTempData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnInputTempData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnInputTempData.Location = new System.Drawing.Point(302, 82);
            this.btnInputTempData.Name = "btnInputTempData";
            this.btnInputTempData.Size = new System.Drawing.Size(93, 31);
            this.btnInputTempData.TabIndex = 0;
            this.btnInputTempData.Text = "入库操作";
            this.btnInputTempData.Click += new System.EventHandler(this.btnInputTempData_Click);
            // 
            // cbProNameList
            // 
            this.cbProNameList.DisplayMember = "Text";
            this.cbProNameList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbProNameList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProNameList.FormattingEnabled = true;
            this.cbProNameList.ItemHeight = 19;
            this.cbProNameList.Location = new System.Drawing.Point(12, 41);
            this.cbProNameList.Name = "cbProNameList";
            this.cbProNameList.Size = new System.Drawing.Size(383, 25);
            this.cbProNameList.TabIndex = 1;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(109, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "选择入库工程";
            // 
            // lblState
            // 
            this.lblState.Location = new System.Drawing.Point(0, 110);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(200, 18);
            this.lblState.TabIndex = 3;
            // 
            // prbInputTempData
            // 
            this.prbInputTempData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prbInputTempData.Location = new System.Drawing.Point(0, 133);
            this.prbInputTempData.Name = "prbInputTempData";
            this.prbInputTempData.Size = new System.Drawing.Size(407, 11);
            this.prbInputTempData.TabIndex = 4;
            this.prbInputTempData.Text = "progressBarX1";
            // 
            // FrmInputTempToProduction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 144);
            this.Controls.Add(this.prbInputTempData);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cbProNameList);
            this.Controls.Add(this.btnInputTempData);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInputTempToProduction";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "临时库入库";
            this.Load += new System.EventHandler(this.FrmInputTempToProduction_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnInputTempData;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbProNameList;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX lblState;
        private DevComponents.DotNetBar.Controls.ProgressBarX prbInputTempData;
    }
}