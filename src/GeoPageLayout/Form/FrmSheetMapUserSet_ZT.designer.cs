using ESRI.ArcGIS.Controls;
namespace GeoPageLayout
{
    partial class FrmSheetMapUserSet_ZT
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
            pAxMapControl.OnMouseDown -= new
                IMapControlEvents2_Ax_OnMouseDownEventHandler(pAxMapControl_OnMouseDown);
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
            this.txtMapNo = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cBoxScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelRatio = new DevComponents.DotNetBar.LabelX();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cBoxZT = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // txtMapNo
            // 
            // 
            // 
            // 
            this.txtMapNo.Border.Class = "TextBoxBorder";
            this.txtMapNo.Location = new System.Drawing.Point(50, 41);
            this.txtMapNo.Name = "txtMapNo";
            this.txtMapNo.Size = new System.Drawing.Size(194, 21);
            this.txtMapNo.TabIndex = 1;
            this.txtMapNo.TextChanged += new System.EventHandler(this.txtResolution_TextChanged);
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
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(138, 111);
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
            this.btnCancel.Location = new System.Drawing.Point(194, 111);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(2, 41);
            this.labelX2.Name = "labelX2";
            this.labelX2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelX2.Size = new System.Drawing.Size(42, 23);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "图幅号";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "提示:可点击地图获取图幅号";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(2, 70);
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
            this.cBoxZT.Location = new System.Drawing.Point(50, 72);
            this.cBoxZT.Name = "cBoxZT";
            this.cBoxZT.Size = new System.Drawing.Size(194, 21);
            this.cBoxZT.TabIndex = 10;
            // 
            // FrmSheetMapUserSet_ZT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 168);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cBoxZT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelRatio);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cBoxScale);
            this.Controls.Add(this.txtMapNo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSheetMapUserSet_ZT";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "森林资源现状图分幅设置";
            this.Load += new System.EventHandler(this.FrmSheetMapUserSet_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSheetMapUserSet_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtMapNo;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxScale;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelRatio;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxZT;

    }
}