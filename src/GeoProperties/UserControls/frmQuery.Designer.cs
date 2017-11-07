namespace GeoProperties.UserControls
{
    partial class frmQuery
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
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.panelExpression = new System.Windows.Forms.Panel();
            this.richTextExpression = new System.Windows.Forms.RichTextBox();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnLoad = new DevComponents.DotNetBar.ButtonX();
            this.btnExample = new DevComponents.DotNetBar.ButtonX();
            this.btnVerify = new DevComponents.DotNetBar.ButtonX();
            this.btnClear = new DevComponents.DotNetBar.ButtonX();
            this.barExpression = new DevComponents.DotNetBar.Bar();
            this.labelItem5 = new DevComponents.DotNetBar.LabelItem();
            this.panelValue = new System.Windows.Forms.Panel();
            this.btnUniqueValues = new DevComponents.DotNetBar.ButtonX();
            this.lstValue = new System.Windows.Forms.ListBox();
            this.barValue = new DevComponents.DotNetBar.Bar();
            this.labelItem4 = new DevComponents.DotNetBar.LabelItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnNot = new DevComponents.DotNetBar.ButtonX();
            this.btnIs = new DevComponents.DotNetBar.ButtonX();
            this.btnParentheses = new DevComponents.DotNetBar.ButtonX();
            this.btnOr = new DevComponents.DotNetBar.ButtonX();
            this.btnAnd = new DevComponents.DotNetBar.ButtonX();
            this.btnLike = new DevComponents.DotNetBar.ButtonX();
            this.btnX = new DevComponents.DotNetBar.ButtonX();
            this.btnNotEqual = new DevComponents.DotNetBar.ButtonX();
            this.btnSmallerEqual = new DevComponents.DotNetBar.ButtonX();
            this.btnBiggerEqual = new DevComponents.DotNetBar.ButtonX();
            this.btnW = new DevComponents.DotNetBar.ButtonX();
            this.btnSmaller = new DevComponents.DotNetBar.ButtonX();
            this.btnBigger = new DevComponents.DotNetBar.ButtonX();
            this.btnEqual = new DevComponents.DotNetBar.ButtonX();
            this.bar3 = new DevComponents.DotNetBar.Bar();
            this.labelItem3 = new DevComponents.DotNetBar.LabelItem();
            this.panelSyllable = new System.Windows.Forms.Panel();
            this.lstSyllable = new System.Windows.Forms.ListBox();
            this.barSyllable = new DevComponents.DotNetBar.Bar();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.panelExpression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barExpression)).BeginInit();
            this.panelValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barValue)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar3)).BeginInit();
            this.panelSyllable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barSyllable)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(254, 344);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 20);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(188, 344);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 20);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panelExpression
            // 
            this.panelExpression.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelExpression.Controls.Add(this.richTextExpression);
            this.panelExpression.Controls.Add(this.btnExample);
            this.panelExpression.Controls.Add(this.btnVerify);
            this.panelExpression.Controls.Add(this.btnClear);
            this.panelExpression.Controls.Add(this.barExpression);
            this.panelExpression.Location = new System.Drawing.Point(-1, 222);
            this.panelExpression.Name = "panelExpression";
            this.panelExpression.Size = new System.Drawing.Size(336, 116);
            this.panelExpression.TabIndex = 10;
            // 
            // richTextExpression
            // 
            this.richTextExpression.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextExpression.Location = new System.Drawing.Point(0, 21);
            this.richTextExpression.Name = "richTextExpression";
            this.richTextExpression.Size = new System.Drawing.Size(332, 63);
            this.richTextExpression.TabIndex = 7;
            this.richTextExpression.Text = "";
            this.richTextExpression.TextChanged += new System.EventHandler(this.richTextExpression_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(112, 344);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 20);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoad.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoad.Location = new System.Drawing.Point(46, 344);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(60, 20);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "加载";
            this.btnLoad.Visible = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnExample
            // 
            this.btnExample.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExample.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExample.Location = new System.Drawing.Point(253, 89);
            this.btnExample.Name = "btnExample";
            this.btnExample.Size = new System.Drawing.Size(60, 20);
            this.btnExample.TabIndex = 4;
            this.btnExample.Text = "示例";
            this.btnExample.Click += new System.EventHandler(this.btnExample_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnVerify.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnVerify.Location = new System.Drawing.Point(187, 89);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(60, 20);
            this.btnVerify.TabIndex = 3;
            this.btnVerify.Text = "验证";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClear.Location = new System.Drawing.Point(121, 90);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(60, 20);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "清除";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // barExpression
            // 
            this.barExpression.Dock = System.Windows.Forms.DockStyle.Top;
            this.barExpression.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem5});
            this.barExpression.Location = new System.Drawing.Point(0, 0);
            this.barExpression.Name = "barExpression";
            this.barExpression.Size = new System.Drawing.Size(332, 21);
            this.barExpression.Stretch = true;
            this.barExpression.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barExpression.TabIndex = 0;
            this.barExpression.TabStop = false;
            this.barExpression.Text = "bar5";
            // 
            // labelItem5
            // 
            this.labelItem5.Name = "labelItem5";
            this.labelItem5.Text = "表达式";
            // 
            // panelValue
            // 
            this.panelValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelValue.Controls.Add(this.btnUniqueValues);
            this.panelValue.Controls.Add(this.lstValue);
            this.panelValue.Controls.Add(this.barValue);
            this.panelValue.Location = new System.Drawing.Point(152, 91);
            this.panelValue.Name = "panelValue";
            this.panelValue.Size = new System.Drawing.Size(183, 128);
            this.panelValue.TabIndex = 9;
            // 
            // btnUniqueValues
            // 
            this.btnUniqueValues.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUniqueValues.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUniqueValues.Location = new System.Drawing.Point(34, 99);
            this.btnUniqueValues.Name = "btnUniqueValues";
            this.btnUniqueValues.Size = new System.Drawing.Size(100, 20);
            this.btnUniqueValues.TabIndex = 2;
            this.btnUniqueValues.Text = "列出可能值";
            this.btnUniqueValues.Click += new System.EventHandler(this.btnUniqueValues_Click);
            // 
            // lstValue
            // 
            this.lstValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstValue.FormattingEnabled = true;
            this.lstValue.ItemHeight = 12;
            this.lstValue.Location = new System.Drawing.Point(0, 21);
            this.lstValue.Name = "lstValue";
            this.lstValue.Size = new System.Drawing.Size(179, 76);
            this.lstValue.TabIndex = 1;
            this.lstValue.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstValue_MouseDoubleClick);
            // 
            // barValue
            // 
            this.barValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.barValue.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem4});
            this.barValue.Location = new System.Drawing.Point(0, 0);
            this.barValue.Name = "barValue";
            this.barValue.Size = new System.Drawing.Size(179, 21);
            this.barValue.Stretch = true;
            this.barValue.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barValue.TabIndex = 0;
            this.barValue.TabStop = false;
            this.barValue.Text = "bar4";
            // 
            // labelItem4
            // 
            this.labelItem4.Name = "labelItem4";
            this.labelItem4.Text = "值";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnNot);
            this.panel3.Controls.Add(this.btnIs);
            this.panel3.Controls.Add(this.btnParentheses);
            this.panel3.Controls.Add(this.btnOr);
            this.panel3.Controls.Add(this.btnAnd);
            this.panel3.Controls.Add(this.btnLike);
            this.panel3.Controls.Add(this.btnX);
            this.panel3.Controls.Add(this.btnNotEqual);
            this.panel3.Controls.Add(this.btnSmallerEqual);
            this.panel3.Controls.Add(this.btnBiggerEqual);
            this.panel3.Controls.Add(this.btnW);
            this.panel3.Controls.Add(this.btnSmaller);
            this.panel3.Controls.Add(this.btnBigger);
            this.panel3.Controls.Add(this.btnEqual);
            this.panel3.Controls.Add(this.bar3);
            this.panel3.Location = new System.Drawing.Point(-1, 91);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(150, 128);
            this.panel3.TabIndex = 8;
            // 
            // btnNot
            // 
            this.btnNot.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNot.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnNot.Location = new System.Drawing.Point(39, 98);
            this.btnNot.Name = "btnNot";
            this.btnNot.Size = new System.Drawing.Size(30, 19);
            this.btnNot.TabIndex = 16;
            this.btnNot.Text = "Not";
            this.btnNot.Click += new System.EventHandler(this.btnNot_Click);
            // 
            // btnIs
            // 
            this.btnIs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnIs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnIs.Location = new System.Drawing.Point(3, 98);
            this.btnIs.Name = "btnIs";
            this.btnIs.Size = new System.Drawing.Size(30, 19);
            this.btnIs.TabIndex = 15;
            this.btnIs.Text = "Is";
            this.btnIs.Click += new System.EventHandler(this.btnIs_Click);
            // 
            // btnParentheses
            // 
            this.btnParentheses.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnParentheses.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnParentheses.Location = new System.Drawing.Point(111, 75);
            this.btnParentheses.Name = "btnParentheses";
            this.btnParentheses.Size = new System.Drawing.Size(30, 19);
            this.btnParentheses.TabIndex = 14;
            this.btnParentheses.Text = "()";
            this.btnParentheses.Click += new System.EventHandler(this.btnParentheses_Click);
            // 
            // btnOr
            // 
            this.btnOr.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOr.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOr.Location = new System.Drawing.Point(75, 75);
            this.btnOr.Name = "btnOr";
            this.btnOr.Size = new System.Drawing.Size(30, 19);
            this.btnOr.TabIndex = 13;
            this.btnOr.Text = "Or";
            this.btnOr.Click += new System.EventHandler(this.btnOr_Click);
            // 
            // btnAnd
            // 
            this.btnAnd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAnd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAnd.Location = new System.Drawing.Point(39, 75);
            this.btnAnd.Name = "btnAnd";
            this.btnAnd.Size = new System.Drawing.Size(30, 19);
            this.btnAnd.TabIndex = 12;
            this.btnAnd.Text = "And";
            this.btnAnd.Click += new System.EventHandler(this.btnAnd_Click);
            // 
            // btnLike
            // 
            this.btnLike.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLike.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLike.Location = new System.Drawing.Point(3, 75);
            this.btnLike.Name = "btnLike";
            this.btnLike.Size = new System.Drawing.Size(30, 19);
            this.btnLike.TabIndex = 11;
            this.btnLike.Text = "Like";
            this.btnLike.Click += new System.EventHandler(this.btnLike_Click);
            // 
            // btnX
            // 
            this.btnX.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnX.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnX.Location = new System.Drawing.Point(111, 50);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(30, 19);
            this.btnX.TabIndex = 10;
            this.btnX.Text = "*";
            this.btnX.Click += new System.EventHandler(this.btnX_Click);
            // 
            // btnNotEqual
            // 
            this.btnNotEqual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNotEqual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnNotEqual.Location = new System.Drawing.Point(75, 50);
            this.btnNotEqual.Name = "btnNotEqual";
            this.btnNotEqual.Size = new System.Drawing.Size(30, 19);
            this.btnNotEqual.TabIndex = 9;
            this.btnNotEqual.Text = "<>";
            this.btnNotEqual.Click += new System.EventHandler(this.btnNotEqual_Click);
            // 
            // btnSmallerEqual
            // 
            this.btnSmallerEqual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSmallerEqual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSmallerEqual.Location = new System.Drawing.Point(39, 50);
            this.btnSmallerEqual.Name = "btnSmallerEqual";
            this.btnSmallerEqual.Size = new System.Drawing.Size(30, 19);
            this.btnSmallerEqual.TabIndex = 8;
            this.btnSmallerEqual.Text = "<=";
            this.btnSmallerEqual.Click += new System.EventHandler(this.btnSmallerEqual_Click);
            // 
            // btnBiggerEqual
            // 
            this.btnBiggerEqual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBiggerEqual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBiggerEqual.Location = new System.Drawing.Point(3, 50);
            this.btnBiggerEqual.Name = "btnBiggerEqual";
            this.btnBiggerEqual.Size = new System.Drawing.Size(30, 19);
            this.btnBiggerEqual.TabIndex = 7;
            this.btnBiggerEqual.Text = ">=";
            this.btnBiggerEqual.Click += new System.EventHandler(this.btnBiggerEqual_Click);
            // 
            // btnW
            // 
            this.btnW.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnW.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnW.Location = new System.Drawing.Point(111, 25);
            this.btnW.Name = "btnW";
            this.btnW.Size = new System.Drawing.Size(30, 19);
            this.btnW.TabIndex = 6;
            this.btnW.Text = "?";
            this.btnW.Click += new System.EventHandler(this.btnW_Click);
            // 
            // btnSmaller
            // 
            this.btnSmaller.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSmaller.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSmaller.Location = new System.Drawing.Point(39, 25);
            this.btnSmaller.Name = "btnSmaller";
            this.btnSmaller.Size = new System.Drawing.Size(30, 19);
            this.btnSmaller.TabIndex = 5;
            this.btnSmaller.Text = "<";
            this.btnSmaller.Click += new System.EventHandler(this.btnSmaller_Click);
            // 
            // btnBigger
            // 
            this.btnBigger.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBigger.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBigger.Location = new System.Drawing.Point(3, 25);
            this.btnBigger.Name = "btnBigger";
            this.btnBigger.Size = new System.Drawing.Size(30, 19);
            this.btnBigger.TabIndex = 4;
            this.btnBigger.Text = ">";
            this.btnBigger.Click += new System.EventHandler(this.btnBigger_Click);
            // 
            // btnEqual
            // 
            this.btnEqual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEqual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnEqual.Location = new System.Drawing.Point(75, 25);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(30, 19);
            this.btnEqual.TabIndex = 3;
            this.btnEqual.Text = "=";
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
            // 
            // bar3
            // 
            this.bar3.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar3.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem3});
            this.bar3.Location = new System.Drawing.Point(0, 0);
            this.bar3.Name = "bar3";
            this.bar3.Size = new System.Drawing.Size(146, 21);
            this.bar3.Stretch = true;
            this.bar3.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.bar3.TabIndex = 0;
            this.bar3.TabStop = false;
            this.bar3.Text = "bar3";
            // 
            // labelItem3
            // 
            this.labelItem3.Name = "labelItem3";
            this.labelItem3.Text = "操作符";
            // 
            // panelSyllable
            // 
            this.panelSyllable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSyllable.Controls.Add(this.lstSyllable);
            this.panelSyllable.Controls.Add(this.barSyllable);
            this.panelSyllable.Location = new System.Drawing.Point(-1, -1);
            this.panelSyllable.Name = "panelSyllable";
            this.panelSyllable.Size = new System.Drawing.Size(336, 89);
            this.panelSyllable.TabIndex = 7;
            // 
            // lstSyllable
            // 
            this.lstSyllable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSyllable.FormattingEnabled = true;
            this.lstSyllable.ItemHeight = 12;
            this.lstSyllable.Location = new System.Drawing.Point(0, 21);
            this.lstSyllable.Name = "lstSyllable";
            this.lstSyllable.Size = new System.Drawing.Size(332, 64);
            this.lstSyllable.TabIndex = 1;
            this.lstSyllable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSyllable_MouseDoubleClick);
            // 
            // barSyllable
            // 
            this.barSyllable.Dock = System.Windows.Forms.DockStyle.Top;
            this.barSyllable.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem2});
            this.barSyllable.Location = new System.Drawing.Point(0, 0);
            this.barSyllable.Name = "barSyllable";
            this.barSyllable.Size = new System.Drawing.Size(332, 21);
            this.barSyllable.Stretch = true;
            this.barSyllable.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.barSyllable.TabIndex = 0;
            this.barSyllable.TabStop = false;
            this.barSyllable.Text = "bar2";
            // 
            // labelItem2
            // 
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "字段";
            // 
            // frmQuery
            // 
            this.ClientSize = new System.Drawing.Size(335, 366);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.panelExpression);
            this.Controls.Add(this.panelValue);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panelSyllable);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQuery";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "条件";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmQuery_Load);
            this.panelExpression.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barExpression)).EndInit();
            this.panelValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barValue)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bar3)).EndInit();
            this.panelSyllable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.barSyllable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private System.Windows.Forms.Panel panelExpression;
        private System.Windows.Forms.RichTextBox richTextExpression;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnLoad;
        private DevComponents.DotNetBar.ButtonX btnExample;
        private DevComponents.DotNetBar.ButtonX btnVerify;
        private DevComponents.DotNetBar.ButtonX btnClear;
        private DevComponents.DotNetBar.Bar barExpression;
        private DevComponents.DotNetBar.LabelItem labelItem5;
        private System.Windows.Forms.Panel panelValue;
        private DevComponents.DotNetBar.ButtonX btnUniqueValues;
        private System.Windows.Forms.ListBox lstValue;
        private DevComponents.DotNetBar.Bar barValue;
        private DevComponents.DotNetBar.LabelItem labelItem4;
        private System.Windows.Forms.Panel panel3;
        private DevComponents.DotNetBar.ButtonX btnNot;
        private DevComponents.DotNetBar.ButtonX btnIs;
        private DevComponents.DotNetBar.ButtonX btnParentheses;
        private DevComponents.DotNetBar.ButtonX btnOr;
        private DevComponents.DotNetBar.ButtonX btnAnd;
        private DevComponents.DotNetBar.ButtonX btnLike;
        private DevComponents.DotNetBar.ButtonX btnX;
        private DevComponents.DotNetBar.ButtonX btnNotEqual;
        private DevComponents.DotNetBar.ButtonX btnSmallerEqual;
        private DevComponents.DotNetBar.ButtonX btnBiggerEqual;
        private DevComponents.DotNetBar.ButtonX btnW;
        private DevComponents.DotNetBar.ButtonX btnSmaller;
        private DevComponents.DotNetBar.ButtonX btnBigger;
        private DevComponents.DotNetBar.ButtonX btnEqual;
        private DevComponents.DotNetBar.Bar bar3;
        private DevComponents.DotNetBar.LabelItem labelItem3;
        private System.Windows.Forms.Panel panelSyllable;
        private System.Windows.Forms.ListBox lstSyllable;
        private DevComponents.DotNetBar.Bar barSyllable;
        private DevComponents.DotNetBar.LabelItem labelItem2;
    }
}