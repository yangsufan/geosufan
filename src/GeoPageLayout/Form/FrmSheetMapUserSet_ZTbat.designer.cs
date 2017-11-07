using ESRI.ArcGIS.Controls;
namespace GeoPageLayout
{
    partial class FrmSheetMapUserSet_ZTbat
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
            this.cBoxScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.labelRatio = new DevComponents.DotNetBar.LabelX();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cBoxZT = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // cBoxScale
            // 
            this.cBoxScale.DisplayMember = "Text";
            this.cBoxScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxScale.FormattingEnabled = true;
            this.cBoxScale.ItemHeight = 15;
            this.cBoxScale.Location = new System.Drawing.Point(50, 12);
            this.cBoxScale.Name = "cBoxScale";
            this.cBoxScale.Size = new System.Drawing.Size(194, 21);
            this.cBoxScale.TabIndex = 2;
            this.cBoxScale.SelectedIndexChanged += new System.EventHandler(this.cBoxScale_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(138, 78);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(50, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(194, 78);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelRatio
            // 
            this.labelRatio.Location = new System.Drawing.Point(2, 12);
            this.labelRatio.Name = "labelRatio";
            this.labelRatio.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelRatio.Size = new System.Drawing.Size(42, 23);
            this.labelRatio.TabIndex = 8;
            this.labelRatio.Text = "比例尺";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(2, 37);
            this.labelX1.Name = "labelX1";
            this.labelX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelX1.Size = new System.Drawing.Size(42, 23);
            this.labelX1.TabIndex = 11;
            this.labelX1.Text = "专  题";
            // 
            // cBoxZT
            // 
            this.cBoxZT.DisplayMember = "Text";
            this.cBoxZT.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxZT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxZT.FormattingEnabled = true;
            this.cBoxZT.ItemHeight = 15;
            this.cBoxZT.Location = new System.Drawing.Point(50, 39);
            this.cBoxZT.Name = "cBoxZT";
            this.cBoxZT.Size = new System.Drawing.Size(194, 21);
            this.cBoxZT.TabIndex = 10;
            // 
            // FrmSheetMapUserSet_ZTbat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 117);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cBoxZT);
            this.Controls.Add(this.labelRatio);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cBoxScale);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSheetMapUserSet_ZTbat";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "森林资源现状图专题设置";
            this.Load += new System.EventHandler(this.FrmSheetMapUserSet_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSheetMapUserSet_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxScale;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX labelRatio;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxZT;

    }
}