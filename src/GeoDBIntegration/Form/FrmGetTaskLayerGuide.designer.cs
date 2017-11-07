namespace GeoDBIntegration
{
    partial class FrmGetTaskLayerGuide
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetTaskLayerGuide));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioInputNewLayer = new System.Windows.Forms.RadioButton();
            this.RadioSDELayer = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmb_Type = new System.Windows.Forms.ComboBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.txtServer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txt_servername = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.txtVersion = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.txtDataBase = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.txtPassWord = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.btn1_Next = new DevComponents.DotNetBar.ButtonX();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.combox_SelectLayer = new System.Windows.Forms.ComboBox();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btn2_Pre = new DevComponents.DotNetBar.ButtonX();
            this.btn2_next = new DevComponents.DotNetBar.ButtonX();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CancleAss = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.comboBox_USER = new System.Windows.Forms.ComboBox();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axMapControl2 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_OK = new DevComponents.DotNetBar.ButtonX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.cmbProject = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, -20);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(620, 365);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.btn1_Next);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(612, 340);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioInputNewLayer);
            this.groupBox2.Controls.Add(this.RadioSDELayer);
            this.groupBox2.Location = new System.Drawing.Point(15, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(151, 261);
            this.groupBox2.TabIndex = 37;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "任务范围选择：";
            // 
            // radioInputNewLayer
            // 
            this.radioInputNewLayer.AutoSize = true;
            this.radioInputNewLayer.ForeColor = System.Drawing.SystemColors.Desktop;
            this.radioInputNewLayer.Location = new System.Drawing.Point(5, 62);
            this.radioInputNewLayer.Name = "radioInputNewLayer";
            this.radioInputNewLayer.Size = new System.Drawing.Size(119, 16);
            this.radioInputNewLayer.TabIndex = 38;
            this.radioInputNewLayer.TabStop = true;
            this.radioInputNewLayer.Text = "导入新的范围图层";
            this.radioInputNewLayer.UseVisualStyleBackColor = true;
            this.radioInputNewLayer.CheckedChanged += new System.EventHandler(this.radioInputNewLayer_CheckedChanged);
            // 
            // RadioSDELayer
            // 
            this.RadioSDELayer.AutoSize = true;
            this.RadioSDELayer.ForeColor = System.Drawing.SystemColors.Desktop;
            this.RadioSDELayer.Location = new System.Drawing.Point(6, 28);
            this.RadioSDELayer.Name = "RadioSDELayer";
            this.RadioSDELayer.Size = new System.Drawing.Size(119, 16);
            this.RadioSDELayer.TabIndex = 37;
            this.RadioSDELayer.TabStop = true;
            this.RadioSDELayer.Text = "数据库中范围图层";
            this.RadioSDELayer.UseVisualStyleBackColor = true;
            this.RadioSDELayer.CheckedChanged += new System.EventHandler(this.RadioSDELayer_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "设置任务图层数据源：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmb_Type);
            this.groupBox1.Controls.Add(this.labelX1);
            this.groupBox1.Controls.Add(this.labelX15);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.txt_servername);
            this.groupBox1.Controls.Add(this.labelX5);
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.labelX4);
            this.groupBox1.Controls.Add(this.btnServer);
            this.groupBox1.Controls.Add(this.txtDataBase);
            this.groupBox1.Controls.Add(this.labelX13);
            this.groupBox1.Controls.Add(this.txtPassWord);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.labelX16);
            this.groupBox1.Controls.Add(this.labelX18);
            this.groupBox1.Location = new System.Drawing.Point(172, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(423, 261);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据源设置：";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cmb_Type
            // 
            this.cmb_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Type.FormattingEnabled = true;
            this.cmb_Type.Location = new System.Drawing.Point(77, 26);
            this.cmb_Type.Name = "cmb_Type";
            this.cmb_Type.Size = new System.Drawing.Size(333, 20);
            this.cmb_Type.TabIndex = 64;
            this.cmb_Type.SelectedIndexChanged += new System.EventHandler(this.cmb_Type_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(15, 28);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(50, 18);
            this.labelX1.TabIndex = 63;
            this.labelX1.Text = "类  型:";
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            this.labelX15.Location = new System.Drawing.Point(15, 183);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(56, 18);
            this.labelX15.TabIndex = 62;
            this.labelX15.Text = "密  码 :";
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.Border.Class = "TextBoxBorder";
            this.txtServer.Location = new System.Drawing.Point(77, 56);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(333, 21);
            this.txtServer.TabIndex = 61;
            this.txtServer.WatermarkText = "服务器ip地址";
            this.txtServer.WordWrap = false;
            // 
            // txt_servername
            // 
            // 
            // 
            // 
            this.txt_servername.Border.Class = "TextBoxBorder";
            this.txt_servername.Location = new System.Drawing.Point(77, 87);
            this.txt_servername.Name = "txt_servername";
            this.txt_servername.Size = new System.Drawing.Size(333, 21);
            this.txt_servername.TabIndex = 51;
            this.txt_servername.WatermarkText = "数据库实例名称";
            this.txt_servername.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.Location = new System.Drawing.Point(15, 90);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(56, 18);
            this.labelX5.TabIndex = 60;
            this.labelX5.Text = "服务名 :";
            // 
            // txtVersion
            // 
            // 
            // 
            // 
            this.txtVersion.Border.Class = "TextBoxBorder";
            this.txtVersion.Location = new System.Drawing.Point(77, 211);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(333, 21);
            this.txtVersion.TabIndex = 56;
            this.txtVersion.WatermarkText = "版本信息";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(15, 214);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(56, 18);
            this.labelX4.TabIndex = 59;
            this.labelX4.Text = "版  本 :";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnServer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnServer.Location = new System.Drawing.Point(384, 118);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(26, 21);
            this.btnServer.TabIndex = 53;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // txtDataBase
            // 
            // 
            // 
            // 
            this.txtDataBase.Border.Class = "TextBoxBorder";
            this.txtDataBase.Location = new System.Drawing.Point(77, 118);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(310, 21);
            this.txtDataBase.TabIndex = 52;
            this.txtDataBase.WatermarkText = "数据库名或本地库路径";
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            this.labelX13.Location = new System.Drawing.Point(15, 121);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(56, 18);
            this.labelX13.TabIndex = 58;
            this.labelX13.Text = "数据库 :";
            // 
            // txtPassWord
            // 
            // 
            // 
            // 
            this.txtPassWord.Border.Class = "TextBoxBorder";
            this.txtPassWord.Location = new System.Drawing.Point(77, 180);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(333, 21);
            this.txtPassWord.TabIndex = 55;
            this.txtPassWord.WatermarkText = "访问密码";
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(77, 149);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(333, 21);
            this.txtUser.TabIndex = 54;
            this.txtUser.WatermarkText = "访问用户名";
            // 
            // labelX16
            // 
            this.labelX16.AutoSize = true;
            this.labelX16.Location = new System.Drawing.Point(15, 152);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(56, 18);
            this.labelX16.TabIndex = 50;
            this.labelX16.Text = "用  户 :";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            this.labelX18.Location = new System.Drawing.Point(15, 59);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(56, 18);
            this.labelX18.TabIndex = 49;
            this.labelX18.Text = "服务器 :";
            // 
            // btn1_Next
            // 
            this.btn1_Next.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn1_Next.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn1_Next.Location = new System.Drawing.Point(474, 301);
            this.btn1_Next.Name = "btn1_Next";
            this.btn1_Next.Size = new System.Drawing.Size(121, 25);
            this.btn1_Next.TabIndex = 34;
            this.btn1_Next.Text = "下一步";
            this.btn1_Next.Click += new System.EventHandler(this.btn1_Next_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.progressBar1);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.btn2_Pre);
            this.tabPage2.Controls.Add(this.btn2_next);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(612, 340);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(18, 309);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(151, 17);
            this.progressBar1.TabIndex = 41;
            this.progressBar1.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.combox_SelectLayer);
            this.groupBox3.Controls.Add(this.axMapControl1);
            this.groupBox3.Controls.Add(this.labelX2);
            this.groupBox3.Location = new System.Drawing.Point(12, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(592, 267);
            this.groupBox3.TabIndex = 40;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "选择图层：";
            // 
            // combox_SelectLayer
            // 
            this.combox_SelectLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combox_SelectLayer.FormattingEnabled = true;
            this.combox_SelectLayer.Location = new System.Drawing.Point(6, 44);
            this.combox_SelectLayer.Name = "combox_SelectLayer";
            this.combox_SelectLayer.Size = new System.Drawing.Size(151, 20);
            this.combox_SelectLayer.TabIndex = 66;
            this.combox_SelectLayer.SelectedIndexChanged += new System.EventHandler(this.combox_SelectLayer_SelectedIndexChanged);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axMapControl1.Location = new System.Drawing.Point(163, 20);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(417, 241);
            this.axMapControl1.TabIndex = 65;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(6, 20);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(50, 18);
            this.labelX2.TabIndex = 64;
            this.labelX2.Text = "图  层:";
            // 
            // btn2_Pre
            // 
            this.btn2_Pre.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn2_Pre.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn2_Pre.Location = new System.Drawing.Point(347, 301);
            this.btn2_Pre.Name = "btn2_Pre";
            this.btn2_Pre.Size = new System.Drawing.Size(121, 25);
            this.btn2_Pre.TabIndex = 39;
            this.btn2_Pre.Text = "上一步";
            this.btn2_Pre.Click += new System.EventHandler(this.btn2_Pre_Click);
            // 
            // btn2_next
            // 
            this.btn2_next.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn2_next.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn2_next.Location = new System.Drawing.Point(474, 301);
            this.btn2_next.Name = "btn2_next";
            this.btn2_next.Size = new System.Drawing.Size(121, 25);
            this.btn2_next.TabIndex = 38;
            this.btn2_next.Text = "下一步";
            this.btn2_next.Click += new System.EventHandler(this.btn2_next_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 12);
            this.label2.TabIndex = 37;
            this.label2.Text = "选择源数据中的一个图层作为任务范围图层：：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.btn_OK);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(612, 340);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelX6);
            this.groupBox4.Controls.Add(this.cmbProject);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.CancleAss);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.labelX3);
            this.groupBox4.Controls.Add(this.comboBox_USER);
            this.groupBox4.Controls.Add(this.axToolbarControl1);
            this.groupBox4.Controls.Add(this.axMapControl2);
            this.groupBox4.Location = new System.Drawing.Point(12, 28);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(594, 267);
            this.groupBox4.TabIndex = 69;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "作业区域分配：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 74;
            this.label7.Text = "灰色：未分配范围";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 12);
            this.label6.TabIndex = 73;
            this.label6.Text = "蓝色：其他已分配范围";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 12);
            this.label5.TabIndex = 72;
            this.label5.Text = "绿色：当前作业员范围";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 71;
            this.label4.Text = "说明：";
            // 
            // CancleAss
            // 
            this.CancleAss.Location = new System.Drawing.Point(88, 140);
            this.CancleAss.Name = "CancleAss";
            this.CancleAss.Size = new System.Drawing.Size(69, 23);
            this.CancleAss.TabIndex = 70;
            this.CancleAss.Text = "取消分配";
            this.CancleAss.UseVisualStyleBackColor = true;
            this.CancleAss.Click += new System.EventHandler(this.CancleAss_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 140);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 23);
            this.button1.TabIndex = 69;
            this.button1.Text = "分配";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Location = new System.Drawing.Point(6, 90);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(50, 18);
            this.labelX3.TabIndex = 68;
            this.labelX3.Text = "作业员:";
            // 
            // comboBox_USER
            // 
            this.comboBox_USER.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_USER.FormattingEnabled = true;
            this.comboBox_USER.Location = new System.Drawing.Point(6, 114);
            this.comboBox_USER.Name = "comboBox_USER";
            this.comboBox_USER.Size = new System.Drawing.Size(151, 20);
            this.comboBox_USER.TabIndex = 67;
            this.comboBox_USER.SelectedIndexChanged += new System.EventHandler(this.comboBox_USER_SelectedIndexChanged);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.axToolbarControl1.Location = new System.Drawing.Point(172, 20);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(28, 241);
            this.axToolbarControl1.TabIndex = 39;
            // 
            // axMapControl2
            // 
            this.axMapControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axMapControl2.Location = new System.Drawing.Point(206, 20);
            this.axMapControl2.Name = "axMapControl2";
            this.axMapControl2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl2.OcxState")));
            this.axMapControl2.Size = new System.Drawing.Size(377, 241);
            this.axMapControl2.TabIndex = 38;
            this.axMapControl2.OnAfterDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterDrawEventHandler(this.axMapControl2_OnAfterDraw);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 12);
            this.label3.TabIndex = 68;
            this.label3.Text = "选中图层中的范围分配至用户：";
            // 
            // btn_OK
            // 
            this.btn_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_OK.Location = new System.Drawing.Point(474, 301);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(121, 25);
            this.btn_OK.TabIndex = 67;
            this.btn_OK.Text = "完成";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.Location = new System.Drawing.Point(6, 29);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(50, 18);
            this.labelX6.TabIndex = 76;
            this.labelX6.Text = "项  目:";
            // 
            // cmbProject
            // 
            this.cmbProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(6, 53);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(151, 20);
            this.cmbProject.TabIndex = 75;
            this.cmbProject.SelectedIndexChanged += new System.EventHandler(this.cmbProject_SelectedIndexChanged);
            // 
            // FrmGetTaskLayerGuide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 344);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetTaskLayerGuide";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "任务分配向导";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        public DevComponents.DotNetBar.ButtonX btn1_Next;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_servername;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersion;
        private DevComponents.DotNetBar.LabelX labelX4;
        public DevComponents.DotNetBar.ButtonX btnServer;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDataBase;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassWord;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUser;
        private DevComponents.DotNetBar.LabelX labelX16;
        private DevComponents.DotNetBar.LabelX labelX18;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServer;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX15;
        private System.Windows.Forms.ComboBox cmb_Type;
        private System.Windows.Forms.GroupBox groupBox2;
        protected internal System.Windows.Forms.RadioButton radioInputNewLayer;
        protected internal System.Windows.Forms.RadioButton RadioSDELayer;
        private System.Windows.Forms.Label label2;
        public DevComponents.DotNetBar.ButtonX btn2_next;
        public DevComponents.DotNetBar.ButtonX btn2_Pre;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.ComboBox combox_SelectLayer;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
        public DevComponents.DotNetBar.ButtonX btn_OK;
        private System.Windows.Forms.GroupBox groupBox4;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl2;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private System.Windows.Forms.ComboBox comboBox_USER;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button CancleAss;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private DevComponents.DotNetBar.LabelX labelX6;
        private System.Windows.Forms.ComboBox cmbProject;

    }
}