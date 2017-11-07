namespace GeoDBATool
{
    partial class frmJoinFeaSearch
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
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btn_Loaddata = new DevComponents.DotNetBar.ButtonX();
            this.btn_ControlFields = new DevComponents.DotNetBar.ButtonX();
            this.com_Project = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btn_clear = new DevComponents.DotNetBar.ButtonX();
            this.btn_Reverse = new DevComponents.DotNetBar.ButtonX();
            this.btn_SelectAll = new DevComponents.DotNetBar.ButtonX();
            this.list_JoinLayer = new System.Windows.Forms.CheckedListBox();
            this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.rbMapLayer = new System.Windows.Forms.RadioButton();
            this.rbServer = new System.Windows.Forms.RadioButton();
            this.com_DataBase = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.text1 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.label_loadState = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.com_MapNoField = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.com_MapFrameList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.rbOutdata = new System.Windows.Forms.RadioButton();
            this.rbExistdata = new System.Windows.Forms.RadioButton();
            this.OK = new DevComponents.DotNetBar.ButtonX();
            this.Cancel = new DevComponents.DotNetBar.ButtonX();
            this.btn_Connect = new DevComponents.DotNetBar.ButtonX();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupPanel1.SuspendLayout();
            this.groupPanel4.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btn_Loaddata);
            this.groupPanel1.Controls.Add(this.btn_ControlFields);
            this.groupPanel1.Controls.Add(this.com_Project);
            this.groupPanel1.Controls.Add(this.btn_clear);
            this.groupPanel1.Controls.Add(this.btn_Reverse);
            this.groupPanel1.Controls.Add(this.btn_SelectAll);
            this.groupPanel1.Controls.Add(this.list_JoinLayer);
            this.groupPanel1.Location = new System.Drawing.Point(7, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(455, 155);
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
            this.groupPanel1.TabIndex = 0;
            this.groupPanel1.Text = "接边图层设置";
            // 
            // btn_Loaddata
            // 
            this.btn_Loaddata.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Loaddata.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Loaddata.Location = new System.Drawing.Point(5, 3);
            this.btn_Loaddata.Name = "btn_Loaddata";
            this.btn_Loaddata.Size = new System.Drawing.Size(100, 23);
            this.btn_Loaddata.TabIndex = 5;
            this.btn_Loaddata.Text = "加载图层数据";
            this.btn_Loaddata.Click += new System.EventHandler(this.btn_Loaddata_Click);
            // 
            // btn_ControlFields
            // 
            this.btn_ControlFields.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_ControlFields.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_ControlFields.Location = new System.Drawing.Point(8, 32);
            this.btn_ControlFields.Name = "btn_ControlFields";
            this.btn_ControlFields.Size = new System.Drawing.Size(97, 16);
            this.btn_ControlFields.TabIndex = 11;
            this.btn_ControlFields.Text = "属性控制";
            this.btn_ControlFields.Click += new System.EventHandler(this.btn_ControlFields_Click);
            // 
            // com_Project
            // 
            this.com_Project.DisplayMember = "Text";
            this.com_Project.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.com_Project.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_Project.FormattingEnabled = true;
            this.com_Project.ItemHeight = 15;
            this.com_Project.Location = new System.Drawing.Point(39, -21);
            this.com_Project.Name = "com_Project";
            this.com_Project.Size = new System.Drawing.Size(154, 21);
            this.com_Project.TabIndex = 0;
            this.com_Project.Visible = false;
            // 
            // btn_clear
            // 
            this.btn_clear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_clear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_clear.Location = new System.Drawing.Point(9, 98);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(96, 16);
            this.btn_clear.TabIndex = 10;
            this.btn_clear.Text = "清空";
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // btn_Reverse
            // 
            this.btn_Reverse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Reverse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Reverse.Location = new System.Drawing.Point(8, 76);
            this.btn_Reverse.Name = "btn_Reverse";
            this.btn_Reverse.Size = new System.Drawing.Size(96, 16);
            this.btn_Reverse.TabIndex = 9;
            this.btn_Reverse.Text = "反选";
            this.btn_Reverse.Click += new System.EventHandler(this.btn_Reverse_Click);
            // 
            // btn_SelectAll
            // 
            this.btn_SelectAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_SelectAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_SelectAll.Location = new System.Drawing.Point(8, 54);
            this.btn_SelectAll.Name = "btn_SelectAll";
            this.btn_SelectAll.Size = new System.Drawing.Size(97, 16);
            this.btn_SelectAll.TabIndex = 8;
            this.btn_SelectAll.Text = "全选";
            this.btn_SelectAll.Click += new System.EventHandler(this.btn_SelectAll_Click);
            // 
            // list_JoinLayer
            // 
            this.list_JoinLayer.FormattingEnabled = true;
            this.list_JoinLayer.Location = new System.Drawing.Point(111, -2);
            this.list_JoinLayer.Name = "list_JoinLayer";
            this.list_JoinLayer.Size = new System.Drawing.Size(330, 116);
            this.list_JoinLayer.TabIndex = 5;
            this.list_JoinLayer.SelectedIndexChanged += new System.EventHandler(this.list_JoinLayer_SelectedIndexChanged);
            // 
            // groupPanel4
            // 
            this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel4.Controls.Add(this.rbMapLayer);
            this.groupPanel4.Controls.Add(this.rbServer);
            this.groupPanel4.Location = new System.Drawing.Point(326, 119);
            this.groupPanel4.Name = "groupPanel4";
            this.groupPanel4.Size = new System.Drawing.Size(93, 96);
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
            this.groupPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel4.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel4.TabIndex = 6;
            this.groupPanel4.Visible = false;
            // 
            // rbMapLayer
            // 
            this.rbMapLayer.AutoSize = true;
            this.rbMapLayer.Location = new System.Drawing.Point(3, 53);
            this.rbMapLayer.Name = "rbMapLayer";
            this.rbMapLayer.Size = new System.Drawing.Size(71, 16);
            this.rbMapLayer.TabIndex = 2;
            this.rbMapLayer.TabStop = true;
            this.rbMapLayer.Text = "图层数据";
            this.rbMapLayer.UseVisualStyleBackColor = true;
            // 
            // rbServer
            // 
            this.rbServer.AutoSize = true;
            this.rbServer.Location = new System.Drawing.Point(3, 14);
            this.rbServer.Name = "rbServer";
            this.rbServer.Size = new System.Drawing.Size(71, 16);
            this.rbServer.TabIndex = 1;
            this.rbServer.TabStop = true;
            this.rbServer.Text = "库体数据";
            this.rbServer.UseVisualStyleBackColor = true;
            // 
            // com_DataBase
            // 
            this.com_DataBase.DisplayMember = "Text";
            this.com_DataBase.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.com_DataBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_DataBase.FormattingEnabled = true;
            this.com_DataBase.ItemHeight = 15;
            this.com_DataBase.Location = new System.Drawing.Point(257, 6);
            this.com_DataBase.Name = "com_DataBase";
            this.com_DataBase.Size = new System.Drawing.Size(154, 21);
            this.com_DataBase.TabIndex = 3;
            this.com_DataBase.Visible = false;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(209, 9);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(49, 18);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "库体：";
            this.labelX1.Visible = false;
            // 
            // text1
            // 
            this.text1.Location = new System.Drawing.Point(8, 10);
            this.text1.Name = "text1";
            this.text1.Size = new System.Drawing.Size(48, 18);
            this.text1.TabIndex = 1;
            this.text1.Text = "工程：";
            this.text1.Visible = false;
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.label_loadState);
            this.groupPanel2.Controls.Add(this.labelX3);
            this.groupPanel2.Controls.Add(this.com_MapNoField);
            this.groupPanel2.Controls.Add(this.labelX2);
            this.groupPanel2.Controls.Add(this.groupPanel4);
            this.groupPanel2.Controls.Add(this.com_MapFrameList);
            this.groupPanel2.Controls.Add(this.groupPanel3);
            this.groupPanel2.Location = new System.Drawing.Point(7, 161);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(455, 128);
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
            this.groupPanel2.TabIndex = 1;
            this.groupPanel2.Text = "图幅范围图设置";
            // 
            // label_loadState
            // 
            this.label_loadState.Location = new System.Drawing.Point(111, 81);
            this.label_loadState.Name = "label_loadState";
            this.label_loadState.Size = new System.Drawing.Size(319, 21);
            this.label_loadState.TabIndex = 5;
            this.label_loadState.Visible = false;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(111, 42);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(81, 18);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "图幅号字段：";
            // 
            // com_MapNoField
            // 
            this.com_MapNoField.DisplayMember = "Text";
            this.com_MapNoField.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.com_MapNoField.FormattingEnabled = true;
            this.com_MapNoField.ItemHeight = 15;
            this.com_MapNoField.Location = new System.Drawing.Point(208, 42);
            this.com_MapNoField.Name = "com_MapNoField";
            this.com_MapNoField.Size = new System.Drawing.Size(228, 21);
            this.com_MapNoField.TabIndex = 3;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(111, 9);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(81, 18);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "图层名称  ：";
            // 
            // com_MapFrameList
            // 
            this.com_MapFrameList.DisplayMember = "Text";
            this.com_MapFrameList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.com_MapFrameList.FormattingEnabled = true;
            this.com_MapFrameList.ItemHeight = 15;
            this.com_MapFrameList.Location = new System.Drawing.Point(208, 9);
            this.com_MapFrameList.Name = "com_MapFrameList";
            this.com_MapFrameList.Size = new System.Drawing.Size(228, 21);
            this.com_MapFrameList.TabIndex = 1;
            this.com_MapFrameList.SelectedIndexChanged += new System.EventHandler(this.com_MapFrameList_SelectedIndexChanged);
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.rbOutdata);
            this.groupPanel3.Controls.Add(this.rbExistdata);
            this.groupPanel3.Location = new System.Drawing.Point(12, 3);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(93, 96);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 0;
            // 
            // rbOutdata
            // 
            this.rbOutdata.AutoSize = true;
            this.rbOutdata.Location = new System.Drawing.Point(3, 51);
            this.rbOutdata.Name = "rbOutdata";
            this.rbOutdata.Size = new System.Drawing.Size(71, 16);
            this.rbOutdata.TabIndex = 1;
            this.rbOutdata.TabStop = true;
            this.rbOutdata.Text = "外部数据";
            this.rbOutdata.UseVisualStyleBackColor = true;
            this.rbOutdata.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbExistdata
            // 
            this.rbExistdata.AutoSize = true;
            this.rbExistdata.Location = new System.Drawing.Point(3, 17);
            this.rbExistdata.Name = "rbExistdata";
            this.rbExistdata.Size = new System.Drawing.Size(71, 16);
            this.rbExistdata.TabIndex = 0;
            this.rbExistdata.TabStop = true;
            this.rbExistdata.Text = "已有数据";
            this.rbExistdata.UseVisualStyleBackColor = true;
            this.rbExistdata.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // OK
            // 
            this.OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.OK.Location = new System.Drawing.Point(336, 302);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(59, 21);
            this.OK.TabIndex = 2;
            this.OK.Text = "确定";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Cancel.Location = new System.Drawing.Point(406, 302);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(59, 21);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "取消";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // btn_Connect
            // 
            this.btn_Connect.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Connect.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Connect.Location = new System.Drawing.Point(417, 6);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(45, 21);
            this.btn_Connect.TabIndex = 4;
            this.btn_Connect.Text = "连接";
            this.btn_Connect.Visible = false;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btn_Connect);
            this.splitContainer1.Panel1.Controls.Add(this.Cancel);
            this.splitContainer1.Panel1.Controls.Add(this.OK);
            this.splitContainer1.Panel1.Controls.Add(this.text1);
            this.splitContainer1.Panel1.Controls.Add(this.labelX1);
            this.splitContainer1.Panel1.Controls.Add(this.groupPanel2);
            this.splitContainer1.Panel1.Controls.Add(this.groupPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.com_DataBase);
            this.splitContainer1.Size = new System.Drawing.Size(833, 367);
            this.splitContainer1.SplitterDistance = 471;
            this.splitContainer1.TabIndex = 5;
            // 
            // frmJoinFeaSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 332);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmJoinFeaSearch";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "接边要素搜索";
            this.Load += new System.EventHandler(this.frmConSet_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel4.ResumeLayout(false);
            this.groupPanel4.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.ButtonX OK;
        private DevComponents.DotNetBar.ButtonX Cancel;
        private DevComponents.DotNetBar.LabelX text1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx com_Project;
        private DevComponents.DotNetBar.Controls.ComboBoxEx com_DataBase;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btn_Connect;
        private System.Windows.Forms.CheckedListBox list_JoinLayer;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx com_MapFrameList;
        private System.Windows.Forms.RadioButton rbOutdata;
        private System.Windows.Forms.RadioButton rbExistdata;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        private System.Windows.Forms.RadioButton rbMapLayer;
        private System.Windows.Forms.RadioButton rbServer;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx com_MapNoField;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.ButtonX btn_clear;
        private DevComponents.DotNetBar.ButtonX btn_Reverse;
        private DevComponents.DotNetBar.ButtonX btn_SelectAll;
        private DevComponents.DotNetBar.LabelX label_loadState;
        private DevComponents.DotNetBar.ButtonX btn_ControlFields;
        private DevComponents.DotNetBar.ButtonX btn_Loaddata;


    }
}