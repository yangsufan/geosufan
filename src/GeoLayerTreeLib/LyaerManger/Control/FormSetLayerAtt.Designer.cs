namespace GeoLayerTreeLib.LayerManager
{
    partial class FormSetLayerAtt
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
            this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtMaxScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtMinScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.checkBoxMinScale = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.checkBoxMaxScale = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkSelected = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkQuery = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkEdit = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkView = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkLoad = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel4
            // 
            this.groupPanel4.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel4.Controls.Add(this.txtMaxScale);
            this.groupPanel4.Controls.Add(this.txtMinScale);
            this.groupPanel4.Controls.Add(this.checkBoxMinScale);
            this.groupPanel4.Controls.Add(this.checkBoxMaxScale);
            this.groupPanel4.Controls.Add(this.chkSelected);
            this.groupPanel4.Controls.Add(this.chkQuery);
            this.groupPanel4.Controls.Add(this.chkEdit);
            this.groupPanel4.Controls.Add(this.chkView);
            this.groupPanel4.Controls.Add(this.chkLoad);
            this.groupPanel4.Location = new System.Drawing.Point(8, 8);
            this.groupPanel4.Name = "groupPanel4";
            this.groupPanel4.Size = new System.Drawing.Size(454, 73);
            // 
            // 
            // 
            this.groupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel4.Style.BackColorGradientAngle = 90;
            this.groupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderBottomWidth = 1;
            this.groupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderLeftWidth = 1;
            this.groupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderRightWidth = 1;
            this.groupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderTopWidth = 1;
            this.groupPanel4.Style.CornerDiameter = 4;
            this.groupPanel4.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel4.TabIndex = 62;
            // 
            // txtMaxScale
            // 
            this.txtMaxScale.AcceptsTab = true;
            // 
            // 
            // 
            this.txtMaxScale.Border.Class = "TextBoxBorder";
            this.txtMaxScale.Enabled = false;
            this.txtMaxScale.Location = new System.Drawing.Point(318, 39);
            this.txtMaxScale.Name = "txtMaxScale";
            this.txtMaxScale.Size = new System.Drawing.Size(120, 21);
            this.txtMaxScale.TabIndex = 73;
            // 
            // txtMinScale
            // 
            // 
            // 
            // 
            this.txtMinScale.Border.Class = "TextBoxBorder";
            this.txtMinScale.Enabled = false;
            this.txtMinScale.Location = new System.Drawing.Point(92, 39);
            this.txtMinScale.Name = "txtMinScale";
            this.txtMinScale.Size = new System.Drawing.Size(120, 21);
            this.txtMinScale.TabIndex = 72;
            // 
            // checkBoxMinScale
            // 
            this.checkBoxMinScale.Location = new System.Drawing.Point(2, 39);
            this.checkBoxMinScale.Name = "checkBoxMinScale";
            this.checkBoxMinScale.Size = new System.Drawing.Size(98, 24);
            this.checkBoxMinScale.TabIndex = 75;
            this.checkBoxMinScale.Text = "最小比例尺：";
            this.checkBoxMinScale.CheckedChanged += new System.EventHandler(this.checkBoxMinScale_CheckedChanged);
            // 
            // checkBoxMaxScale
            // 
            this.checkBoxMaxScale.Location = new System.Drawing.Point(225, 39);
            this.checkBoxMaxScale.Name = "checkBoxMaxScale";
            this.checkBoxMaxScale.Size = new System.Drawing.Size(98, 24);
            this.checkBoxMaxScale.TabIndex = 74;
            this.checkBoxMaxScale.Text = "最大比例尺：";
            this.checkBoxMaxScale.CheckedChanged += new System.EventHandler(this.checkBoxMaxScale_CheckedChanged);
            // 
            // chkSelected
            // 
            this.chkSelected.Location = new System.Drawing.Point(352, 8);
            this.chkSelected.Name = "chkSelected";
            this.chkSelected.Size = new System.Drawing.Size(92, 24);
            this.chkSelected.TabIndex = 68;
            this.chkSelected.Text = "可选择";
            // 
            // chkQuery
            // 
            this.chkQuery.Location = new System.Drawing.Point(263, 8);
            this.chkQuery.Name = "chkQuery";
            this.chkQuery.Size = new System.Drawing.Size(94, 24);
            this.chkQuery.TabIndex = 67;
            this.chkQuery.Text = "可查询";
            // 
            // chkEdit
            // 
            this.chkEdit.Location = new System.Drawing.Point(167, 8);
            this.chkEdit.Name = "chkEdit";
            this.chkEdit.Size = new System.Drawing.Size(91, 24);
            this.chkEdit.TabIndex = 66;
            this.chkEdit.Text = "可编辑";
            // 
            // chkView
            // 
            this.chkView.Location = new System.Drawing.Point(83, 8);
            this.chkView.Name = "chkView";
            this.chkView.Size = new System.Drawing.Size(82, 24);
            this.chkView.TabIndex = 65;
            this.chkView.Text = "显示";
            // 
            // chkLoad
            // 
            this.chkLoad.Location = new System.Drawing.Point(2, 8);
            this.chkLoad.Name = "chkLoad";
            this.chkLoad.Size = new System.Drawing.Size(79, 24);
            this.chkLoad.TabIndex = 64;
            this.chkLoad.Text = "加载";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(392, 88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.TabIndex = 64;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(306, 88);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.TabIndex = 63;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormSetLayerAtt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 116);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupPanel4);
            this.Name = "FormSetLayerAtt";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量设置图层属性";
            this.groupPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMaxScale;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMinScale;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSelected;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkQuery;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkEdit;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkView;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkLoad;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxMinScale;
        private DevComponents.DotNetBar.Controls.CheckBoxX checkBoxMaxScale;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
    }
}