namespace GeoDBATool
{
    partial class frmJoinSetting 
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
            this.con_DisTo = new DevComponents.Editors.DoubleInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.con_SearchTo = new DevComponents.Editors.DoubleInput();
            this.con_AngleTo = new DevComponents.Editors.DoubleInput();
            this.con_LengthTo = new DevComponents.Editors.DoubleInput();
            this.OK = new DevComponents.DotNetBar.ButtonX();
            this.Cancel = new DevComponents.DotNetBar.ButtonX();
            this.com_jointype = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.check_RemovePoPnt = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.check_Simplify = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.com_MergeAtrSet = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.logcheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.label_LogPath = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.con_DisTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.con_SearchTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.con_AngleTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.con_LengthTo)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // con_DisTo
            // 
            // 
            // 
            // 
            this.con_DisTo.BackgroundStyle.Class = "DateTimeInputBackground";
            this.con_DisTo.Increment = 1;
            this.con_DisTo.Location = new System.Drawing.Point(121, 3);
            this.con_DisTo.MaxValue = 40;
            this.con_DisTo.MinValue = 0;
            this.con_DisTo.Name = "con_DisTo";
            this.con_DisTo.ShowUpDown = true;
            this.con_DisTo.Size = new System.Drawing.Size(93, 21);
            this.con_DisTo.TabIndex = 0;
            this.con_DisTo.Value = 40;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(3, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(111, 20);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "距离容差（米）：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(220, 4);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(111, 20);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "搜索距离（米）：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(3, 38);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(111, 20);
            this.labelX3.TabIndex = 3;
            this.labelX3.Text = "角度容差（°）：";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(220, 38);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(111, 20);
            this.labelX4.TabIndex = 4;
            this.labelX4.Text = "长度容差（米）：";
            // 
            // con_SearchTo
            // 
            // 
            // 
            // 
            this.con_SearchTo.BackgroundStyle.Class = "DateTimeInputBackground";
            this.con_SearchTo.Increment = 1;
            this.con_SearchTo.Location = new System.Drawing.Point(337, 4);
            this.con_SearchTo.MaxValue = 40;
            this.con_SearchTo.MinValue = 0;
            this.con_SearchTo.Name = "con_SearchTo";
            this.con_SearchTo.ShowUpDown = true;
            this.con_SearchTo.Size = new System.Drawing.Size(93, 21);
            this.con_SearchTo.TabIndex = 5;
            this.con_SearchTo.Value = 40;
            // 
            // con_AngleTo
            // 
            // 
            // 
            // 
            this.con_AngleTo.BackgroundStyle.Class = "DateTimeInputBackground";
            this.con_AngleTo.Increment = 1;
            this.con_AngleTo.Location = new System.Drawing.Point(121, 38);
            this.con_AngleTo.MaxValue = 180;
            this.con_AngleTo.MinValue = 0;
            this.con_AngleTo.Name = "con_AngleTo";
            this.con_AngleTo.ShowUpDown = true;
            this.con_AngleTo.Size = new System.Drawing.Size(93, 21);
            this.con_AngleTo.TabIndex = 6;
            this.con_AngleTo.Value = 1;
            // 
            // con_LengthTo
            // 
            // 
            // 
            // 
            this.con_LengthTo.BackgroundStyle.Class = "DateTimeInputBackground";
            this.con_LengthTo.Increment = 1;
            this.con_LengthTo.Location = new System.Drawing.Point(337, 38);
            this.con_LengthTo.MaxValue = 1000;
            this.con_LengthTo.MinValue = 0;
            this.con_LengthTo.Name = "con_LengthTo";
            this.con_LengthTo.ShowUpDown = true;
            this.con_LengthTo.Size = new System.Drawing.Size(93, 21);
            this.con_LengthTo.TabIndex = 7;
            // 
            // OK
            // 
            this.OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.OK.Location = new System.Drawing.Point(324, 266);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(54, 20);
            this.OK.TabIndex = 8;
            this.OK.Text = "确定";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Cancel.Location = new System.Drawing.Point(384, 266);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(54, 20);
            this.Cancel.TabIndex = 9;
            this.Cancel.Text = "取消";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // com_jointype
            // 
            this.com_jointype.DisplayMember = "Text";
            this.com_jointype.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.com_jointype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_jointype.FormattingEnabled = true;
            this.com_jointype.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.com_jointype.ItemHeight = 15;
            this.com_jointype.Location = new System.Drawing.Point(121, 73);
            this.com_jointype.Name = "com_jointype";
            this.com_jointype.Size = new System.Drawing.Size(93, 21);
            this.com_jointype.TabIndex = 10;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(3, 73);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(111, 20);
            this.labelX5.TabIndex = 11;
            this.labelX5.Text = "接 边 类 型   ：";
            // 
            // check_RemovePoPnt
            // 
            this.check_RemovePoPnt.Location = new System.Drawing.Point(220, 73);
            this.check_RemovePoPnt.Name = "check_RemovePoPnt";
            this.check_RemovePoPnt.Size = new System.Drawing.Size(174, 20);
            this.check_RemovePoPnt.TabIndex = 12;
            this.check_RemovePoPnt.Text = "删除多边形上多余点";
            // 
            // check_Simplify
            // 
            this.check_Simplify.Location = new System.Drawing.Point(220, 99);
            this.check_Simplify.Name = "check_Simplify";
            this.check_Simplify.Size = new System.Drawing.Size(174, 20);
            this.check_Simplify.TabIndex = 13;
            this.check_Simplify.Text = "要素简单化";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Controls.Add(this.check_Simplify);
            this.groupPanel1.Controls.Add(this.con_DisTo);
            this.groupPanel1.Controls.Add(this.check_RemovePoPnt);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.com_jointype);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.con_SearchTo);
            this.groupPanel1.Controls.Add(this.con_AngleTo);
            this.groupPanel1.Controls.Add(this.con_LengthTo);
            this.groupPanel1.Location = new System.Drawing.Point(1, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(446, 153);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 14;
            this.groupPanel1.Text = "接边参数设置";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.com_MergeAtrSet);
            this.groupPanel2.Controls.Add(this.labelX6);
            this.groupPanel2.Location = new System.Drawing.Point(1, 174);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(446, 56);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 15;
            this.groupPanel2.Text = "融合参数设置";
            // 
            // com_MergeAtrSet
            // 
            this.com_MergeAtrSet.DisplayMember = "Text";
            this.com_MergeAtrSet.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.com_MergeAtrSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_MergeAtrSet.FormattingEnabled = true;
            this.com_MergeAtrSet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.com_MergeAtrSet.ItemHeight = 15;
            this.com_MergeAtrSet.Location = new System.Drawing.Point(120, 2);
            this.com_MergeAtrSet.Name = "com_MergeAtrSet";
            this.com_MergeAtrSet.Size = new System.Drawing.Size(93, 21);
            this.com_MergeAtrSet.TabIndex = 13;
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(3, 3);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(111, 20);
            this.labelX6.TabIndex = 12;
            this.labelX6.Text = "属性字段处理  ：";
            // 
            // logcheck
            // 
            this.logcheck.Location = new System.Drawing.Point(3, 242);
            this.logcheck.Name = "logcheck";
            this.logcheck.Size = new System.Drawing.Size(100, 19);
            this.logcheck.TabIndex = 16;
            this.logcheck.Text = "保存日志";
            this.logcheck.Click += new System.EventHandler(this.logcheck_Click);
            this.logcheck.CheckedChanged += new System.EventHandler(this.logcheck_CheckedChanged);
            // 
            // label_LogPath
            // 
            this.label_LogPath.Location = new System.Drawing.Point(95, 242);
            this.label_LogPath.Name = "label_LogPath";
            this.label_LogPath.Size = new System.Drawing.Size(351, 18);
            this.label_LogPath.TabIndex = 17;
            this.label_LogPath.Visible = false;
            // 
            // frmJoinSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 297);
            this.Controls.Add(this.label_LogPath);
            this.Controls.Add(this.logcheck);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmJoinSetting";
            this.ShowIcon = false;
            this.Text = "接边参数设置";
            this.Load += new System.EventHandler(this.frmConSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.con_DisTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.con_SearchTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.con_AngleTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.con_LengthTo)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.Editors.DoubleInput con_DisTo;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.Editors.DoubleInput con_SearchTo;
        private DevComponents.Editors.DoubleInput con_AngleTo;
        private DevComponents.Editors.DoubleInput con_LengthTo;
        private DevComponents.DotNetBar.ButtonX OK;
        private DevComponents.DotNetBar.ButtonX Cancel;
        private DevComponents.DotNetBar.Controls.ComboBoxEx com_jointype;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.CheckBoxX check_RemovePoPnt;
        private DevComponents.DotNetBar.Controls.CheckBoxX check_Simplify;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx com_MergeAtrSet;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.CheckBoxX logcheck;
        private DevComponents.DotNetBar.LabelX label_LogPath;

    }
}