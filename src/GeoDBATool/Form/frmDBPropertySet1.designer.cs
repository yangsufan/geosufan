namespace GeoDBATool
{
    partial class frmDBPropertySet1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comBoxType = new System.Windows.Forms.ComboBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtInstance = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbDataset = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnClearFeacls = new System.Windows.Forms.Button();
            this.btnAddFeaCls = new System.Windows.Forms.Button();
            this.LstCheckoutFeaCls = new System.Windows.Forms.ListBox();
            this.LstAllFeaCls = new System.Windows.Forms.ListBox();
            this.btnConn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDB = new System.Windows.Forms.TextBox();
            this.btnDB = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "类  型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "服务器：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "实例名：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "用  户：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(203, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "密  码：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "版  本：";
            // 
            // comBoxType
            // 
            this.comBoxType.FormattingEnabled = true;
            this.comBoxType.Location = new System.Drawing.Point(71, 6);
            this.comBoxType.Name = "comBoxType";
            this.comBoxType.Size = new System.Drawing.Size(125, 20);
            this.comBoxType.TabIndex = 7;
            this.comBoxType.SelectedIndexChanged += new System.EventHandler(this.comBoxType_SelectedIndexChanged);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(262, 6);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(149, 21);
            this.txtServer.TabIndex = 8;
            this.txtServer.Text = "172.17.2.20";
            // 
            // txtInstance
            // 
            this.txtInstance.Location = new System.Drawing.Point(71, 33);
            this.txtInstance.Name = "txtInstance";
            this.txtInstance.Size = new System.Drawing.Size(125, 21);
            this.txtInstance.TabIndex = 9;
            this.txtInstance.Text = "5151";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(73, 62);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(123, 21);
            this.txtUser.TabIndex = 10;
            this.txtUser.Text = "dlguser2";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(262, 61);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(149, 21);
            this.txtPassword.TabIndex = 11;
            this.txtPassword.Text = "11111";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(73, 92);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(275, 21);
            this.txtVersion.TabIndex = 12;
            this.txtVersion.Text = "SDE.DEFAULT";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(354, 90);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(61, 23);
            this.btnTest.TabIndex = 13;
            this.btnTest.Text = "连  接";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(362, 368);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "确  定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbDataset
            // 
            this.cmbDataset.FormattingEnabled = true;
            this.cmbDataset.Location = new System.Drawing.Point(74, 126);
            this.cmbDataset.Name = "cmbDataset";
            this.cmbDataset.Size = new System.Drawing.Size(274, 20);
            this.cmbDataset.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "数据集：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddAll);
            this.groupBox2.Controls.Add(this.btnClearFeacls);
            this.groupBox2.Controls.Add(this.btnAddFeaCls);
            this.groupBox2.Controls.Add(this.LstCheckoutFeaCls);
            this.groupBox2.Controls.Add(this.LstAllFeaCls);
            this.groupBox2.Location = new System.Drawing.Point(14, 163);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(409, 199);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "要素类选择";
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(167, 50);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(75, 23);
            this.btnAddAll.TabIndex = 23;
            this.btnAddAll.Text = "全部添加";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnClearFeacls
            // 
            this.btnClearFeacls.Location = new System.Drawing.Point(167, 79);
            this.btnClearFeacls.Name = "btnClearFeacls";
            this.btnClearFeacls.Size = new System.Drawing.Size(75, 23);
            this.btnClearFeacls.TabIndex = 22;
            this.btnClearFeacls.Text = "清除所有";
            this.btnClearFeacls.UseVisualStyleBackColor = true;
            this.btnClearFeacls.Click += new System.EventHandler(this.btnClearFeacls_Click);
            // 
            // btnAddFeaCls
            // 
            this.btnAddFeaCls.Location = new System.Drawing.Point(167, 21);
            this.btnAddFeaCls.Name = "btnAddFeaCls";
            this.btnAddFeaCls.Size = new System.Drawing.Size(75, 23);
            this.btnAddFeaCls.TabIndex = 20;
            this.btnAddFeaCls.Text = "添加";
            this.btnAddFeaCls.UseVisualStyleBackColor = true;
            this.btnAddFeaCls.Click += new System.EventHandler(this.btnAddFeaCls_Click);
            // 
            // LstCheckoutFeaCls
            // 
            this.LstCheckoutFeaCls.FormattingEnabled = true;
            this.LstCheckoutFeaCls.HorizontalScrollbar = true;
            this.LstCheckoutFeaCls.ItemHeight = 12;
            this.LstCheckoutFeaCls.Location = new System.Drawing.Point(251, 21);
            this.LstCheckoutFeaCls.Name = "LstCheckoutFeaCls";
            this.LstCheckoutFeaCls.Size = new System.Drawing.Size(146, 172);
            this.LstCheckoutFeaCls.TabIndex = 19;
            // 
            // LstAllFeaCls
            // 
            this.LstAllFeaCls.FormattingEnabled = true;
            this.LstAllFeaCls.HorizontalScrollbar = true;
            this.LstAllFeaCls.ItemHeight = 12;
            this.LstAllFeaCls.Location = new System.Drawing.Point(6, 21);
            this.LstAllFeaCls.Name = "LstAllFeaCls";
            this.LstAllFeaCls.Size = new System.Drawing.Size(155, 172);
            this.LstAllFeaCls.TabIndex = 18;
            // 
            // btnConn
            // 
            this.btnConn.Location = new System.Drawing.Point(354, 124);
            this.btnConn.Name = "btnConn";
            this.btnConn.Size = new System.Drawing.Size(61, 23);
            this.btnConn.TabIndex = 18;
            this.btnConn.Text = "确  定";
            this.btnConn.UseVisualStyleBackColor = true;
            this.btnConn.Click += new System.EventHandler(this.btnConn_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(204, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "数据库：";
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(262, 34);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(117, 21);
            this.txtDB.TabIndex = 20;
            // 
            // btnDB
            // 
            this.btnDB.Location = new System.Drawing.Point(380, 33);
            this.btnDB.Name = "btnDB";
            this.btnDB.Size = new System.Drawing.Size(31, 23);
            this.btnDB.TabIndex = 21;
            this.btnDB.Text = "...";
            this.btnDB.UseVisualStyleBackColor = true;
            this.btnDB.Click += new System.EventHandler(this.btnDB_Click);
            // 
            // frmDBPropertySet1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 397);
            this.Controls.Add(this.btnDB);
            this.Controls.Add(this.txtDB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnConn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbDataset);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtInstance);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.comBoxType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDBPropertySet1";
            this.ShowIcon = false;
            this.Text = "设置数据库连接";
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comBoxType;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtInstance;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbDataset;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnClearFeacls;
        private System.Windows.Forms.Button btnAddFeaCls;
        private System.Windows.Forms.ListBox LstCheckoutFeaCls;
        private System.Windows.Forms.ListBox LstAllFeaCls;
        private System.Windows.Forms.Button btnConn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDB;
        private System.Windows.Forms.Button btnDB;
    }
}