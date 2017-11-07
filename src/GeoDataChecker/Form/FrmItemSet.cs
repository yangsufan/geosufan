using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OracleClient;
using DevComponents.DotNetBar;
using Microsoft.Win32;
using System.IO;//注册表操作要引用的空间
using System.Text.RegularExpressions;

namespace GeoDataChecker
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 :DevComponents.DotNetBar.Office2007RibbonForm
	{
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.TabControl tabControl1;
        private TabControlPanel tabControlPanel1;
        private PanelEx panelEx2;
        private Label label1;
        private PanelEx panelEx3;
        private RichTextBox richTextBox2;
        private TabItem tabItem1;
        private TabControlPanel tabControlPanel2;
        private PanelEx panelEx4;
        private Label label2;
        private PanelEx panelEx7;
        private RichTextBox richTextBox4;
        private TabItem tabItem2;
        private TabControlPanel tabControlPanel3;
        private PanelEx panelEx5;
        private Label label3;
        private PanelEx panelEx6;
        private RichTextBox richTextBox6;
        private TabItem tabItem3;
        private DevComponents.DotNetBar.TabControl tabControl2;
        private TabControlPanel tabControlPanel4;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX3;
        private TabItem tabItem4;
        private TabControlPanel tabControlPanel5;
        private TabItem tabItem5;
        private ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1;
        private LabelX labelX3;
        private LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX2;
        private LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX4;
        private LabelX labelX5;
        private LabelX labelX6;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx2;
        private ButtonX buttonX2;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX5;
        private LabelX labelX1;
        private OpenFileDialog openFileDialog1;
        private IContainer components;
        private PictureBox pictureBox1;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private string m_Path;   //系统环境变量

        public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            //数据库类型选择按钮默认从本地xml文件中读取
            #region 从本地xml文件“DataCheckSet.xml”中读取初始化参数信息，设置界面



            #endregion

            //获取本机上Oracle的连接字符串，并初始化下拉列表中的内容
            #region 获取Oracle数据库的连接字符串信息
            RegistryKey regLocalMachine = Registry.LocalMachine;
            RegistryKey regSYSTEM = regLocalMachine.OpenSubKey("SYSTEM", true);//打开HKEY_LOCAL_MACHINE下的SYSTEM
            RegistryKey regControlSet001 = regSYSTEM.OpenSubKey("ControlSet001", true);//打开ControlSet001 
            RegistryKey regControl = regControlSet001.OpenSubKey("Control", true);//打开Control
            RegistryKey regManager = regControl.OpenSubKey("Session Manager", true);//打开Control

            RegistryKey regEnvironment = regManager.OpenSubKey("Environment", true);//打开MSSQLServer下的MSSQLServer
            m_Path = regEnvironment.GetValue("path").ToString();//读取path的值

            //根据分号截取字符串获得路径
            string[] Path = m_Path.Split(';');

            //tns文件路径
            string TnsnamesFilePath=null;

            //遍历数组查找有效的tnsnames.ora文件路径
            #region 查找获得tnsnames.ora文件路径
            for (int i = 0; i < Path.Length-1; i++)
            {
                if (System.IO.File.Exists(Path[i]+"\\.."+@"\NETWORK\ADMIN\tnsnames.ora"))
                {
                    TnsnamesFilePath=Path[i]+"\\.."+@"\NETWORK\ADMIN\tnsnames.ora";
                }
                
            }

            #region 如果获得文件路径则读取里面的网络连接标识符，并初始化控件下拉框

            if (TnsnamesFilePath != null) 
            {
                #region 读取文本中的内容
                StreamReader objReader = new StreamReader(TnsnamesFilePath);

                string sLine = "";
                string pTxt = "";
                ArrayList arrText = new ArrayList();

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();

                    if (sLine != null)
                    {
                        //“#”“（”“）”开头的行去掉
                        if (!(sLine.IndexOf("#")>-1 || (sLine.IndexOf("(") > -1) || (sLine.IndexOf(")") > -1)))
                        {
                            pTxt = pTxt + sLine;

                            if (sLine != null)
                            {
                                arrText.Add(sLine);
                            }
                        } 
                    }

                }
                objReader.Close();

                #endregion


                #region 正则表达式分离解析字符
                string[] pStringSegment = new string[100];
                //int[] pMatchposition = new int[];
                //通过正则表达式分离空格，解析每行字符
                Regex re = new Regex("\\S+", RegexOptions.None);
                MatchCollection mc = re.Matches(pTxt);
                int j = 0;

                for (int i = 0; i < mc.Count; i++) //在输入字符串中找到所有匹配
                {
                    if (!(mc[i].Value.StartsWith("=")))
                    {
                        pStringSegment[j] = mc[i].Value;
                    }
                    else if (!(mc[i].Value=="="))
                    {
                        pStringSegment[j] = mc[i].Value;
                        pStringSegment[j] = pStringSegment[j].Substring(1, pStringSegment[j].Length - 1);
                    }

                    j++;
                    //pStringSegment[i] = mc[i].Value; //将匹配的字符串添在字符串数组中
                    //pMatchposition[i] = mc[i].Index; //记录匹配字符的位置
                } 
                #endregion
                //填充数据库连接标识符下拉框
                for (int i = 0; i < 99; i++)
                {

                    if (pStringSegment[i] != null)
                    {
                        this.comboBoxEx2.Items.Add(pStringSegment[i]);
                    }
                }

            }

            TnsnamesFilePath = null;
            #endregion

            #endregion

            #endregion

            #region 隐藏验证数据库连接状态的图标
            pictureBox1.Visible = false;
            #endregion
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.tabControl1 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.tabControl2 = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel5 = new DevComponents.DotNetBar.TabControlPanel();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.textBoxX2 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxX5 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX4 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.tabItem5 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel4 = new DevComponents.DotNetBar.TabControlPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxEx2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.textBoxX1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX3 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tabItem4 = new DevComponents.DotNetBar.TabItem(this.components);
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.panelEx4 = new DevComponents.DotNetBar.PanelEx();
            this.label2 = new System.Windows.Forms.Label();
            this.panelEx7 = new DevComponents.DotNetBar.PanelEx();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.tabItem2 = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel3 = new DevComponents.DotNetBar.TabControlPanel();
            this.panelEx5 = new DevComponents.DotNetBar.PanelEx();
            this.label3 = new System.Windows.Forms.Label();
            this.panelEx6 = new DevComponents.DotNetBar.PanelEx();
            this.richTextBox6 = new System.Windows.Forms.RichTextBox();
            this.tabItem3 = new DevComponents.DotNetBar.TabItem(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl2)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabControlPanel5.SuspendLayout();
            this.tabControlPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelEx2.SuspendLayout();
            this.panelEx3.SuspendLayout();
            this.tabControlPanel2.SuspendLayout();
            this.panelEx4.SuspendLayout();
            this.panelEx7.SuspendLayout();
            this.tabControlPanel3.SuspendLayout();
            this.panelEx5.SuspendLayout();
            this.panelEx6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelEx1.Location = new System.Drawing.Point(4, 1);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(554, 34);
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(136)))), ((int)(((byte)(215)))));
            this.panelEx1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(108)))), ((int)(((byte)(191)))));
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.Style.MarginLeft = 8;
            this.panelEx1.TabIndex = 0;
            this.panelEx1.Text = "数据检查参数配置管理";
            // 
            // tabControl1
            // 
            this.tabControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.tabControl1.CanReorderTabs = true;
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Controls.Add(this.tabControlPanel2);
            this.tabControl1.Controls.Add(this.tabControlPanel3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabControl1.SelectedTabIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(554, 273);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl1.Tabs.Add(this.tabItem1);
            this.tabControl1.Tabs.Add(this.tabItem2);
            this.tabControl1.Tabs.Add(this.tabItem3);
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.buttonX1);
            this.tabControlPanel1.Controls.Add(this.tabControl2);
            this.tabControlPanel1.Controls.Add(this.panelEx2);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(554, 247);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Location = new System.Drawing.Point(454, 210);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(85, 28);
            this.buttonX1.TabIndex = 4;
            this.buttonX1.Text = "确　定";
            // 
            // tabControl2
            // 
            this.tabControl2.CanReorderTabs = true;
            this.tabControl2.Controls.Add(this.tabControlPanel4);
            this.tabControl2.Controls.Add(this.tabControlPanel5);
            this.tabControl2.Location = new System.Drawing.Point(14, 66);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControl2.SelectedTabIndex = 1;
            this.tabControl2.Size = new System.Drawing.Size(536, 138);
            this.tabControl2.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Dock;
            this.tabControl2.TabIndex = 3;
            this.tabControl2.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControl2.Tabs.Add(this.tabItem4);
            this.tabControl2.Tabs.Add(this.tabItem5);
            this.tabControl2.Text = "tabControl2";
            // 
            // tabControlPanel5
            // 
            this.tabControlPanel5.Controls.Add(this.buttonX2);
            this.tabControlPanel5.Controls.Add(this.textBoxX2);
            this.tabControlPanel5.Controls.Add(this.textBoxX5);
            this.tabControlPanel5.Controls.Add(this.labelX4);
            this.tabControlPanel5.Controls.Add(this.labelX1);
            this.tabControlPanel5.Controls.Add(this.textBoxX4);
            this.tabControlPanel5.Controls.Add(this.labelX5);
            this.tabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel5.Location = new System.Drawing.Point(0, 25);
            this.tabControlPanel5.Name = "tabControlPanel5";
            this.tabControlPanel5.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel5.Size = new System.Drawing.Size(536, 113);
            this.tabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel5.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel5.Style.GradientAngle = 90;
            this.tabControlPanel5.TabIndex = 2;
            this.tabControlPanel5.TabItem = this.tabItem5;
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Location = new System.Drawing.Point(308, 16);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(23, 21);
            this.buttonX2.TabIndex = 10;
            this.buttonX2.Text = "…";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // textBoxX2
            // 
            // 
            // 
            // 
            this.textBoxX2.Border.Class = "TextBoxBorder";
            this.textBoxX2.Location = new System.Drawing.Point(171, 69);
            this.textBoxX2.Name = "textBoxX2";
            this.textBoxX2.Size = new System.Drawing.Size(131, 21);
            this.textBoxX2.TabIndex = 11;
            this.textBoxX2.WatermarkText = "代码与图层关系表名";
            // 
            // textBoxX5
            // 
            // 
            // 
            // 
            this.textBoxX5.Border.Class = "TextBoxBorder";
            this.textBoxX5.Location = new System.Drawing.Point(171, 16);
            this.textBoxX5.Name = "textBoxX5";
            this.textBoxX5.Size = new System.Drawing.Size(131, 21);
            this.textBoxX5.TabIndex = 9;
            this.textBoxX5.WatermarkText = "本地数据库文件路径";
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(38, 71);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(135, 21);
            this.labelX4.TabIndex = 10;
            this.labelX4.Text = "代码与图层关系表名：";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(38, 17);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(110, 20);
            this.labelX1.TabIndex = 8;
            this.labelX1.Text = "本地数据库文件：";
            // 
            // textBoxX4
            // 
            // 
            // 
            // 
            this.textBoxX4.Border.Class = "TextBoxBorder";
            this.textBoxX4.Location = new System.Drawing.Point(171, 42);
            this.textBoxX4.Name = "textBoxX4";
            this.textBoxX4.Size = new System.Drawing.Size(131, 21);
            this.textBoxX4.TabIndex = 6;
            this.textBoxX4.WatermarkText = "分类代码表名";
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.Location = new System.Drawing.Point(38, 45);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(95, 19);
            this.labelX5.TabIndex = 9;
            this.labelX5.Text = "分类代码表名：";
            // 
            // tabItem5
            // 
            this.tabItem5.AttachedControl = this.tabControlPanel5;
            this.tabItem5.Name = "tabItem5";
            this.tabItem5.Text = "本地Access数据库";
            // 
            // tabControlPanel4
            // 
            this.tabControlPanel4.Controls.Add(this.pictureBox1);
            this.tabControlPanel4.Controls.Add(this.labelX6);
            this.tabControlPanel4.Controls.Add(this.comboBoxEx2);
            this.tabControlPanel4.Controls.Add(this.textBoxX1);
            this.tabControlPanel4.Controls.Add(this.labelX3);
            this.tabControlPanel4.Controls.Add(this.labelX2);
            this.tabControlPanel4.Controls.Add(this.textBoxX3);
            this.tabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel4.Location = new System.Drawing.Point(0, 25);
            this.tabControlPanel4.Name = "tabControlPanel4";
            this.tabControlPanel4.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel4.Size = new System.Drawing.Size(536, 113);
            this.tabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(254)))));
            this.tabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(188)))), ((int)(((byte)(227)))));
            this.tabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(165)))), ((int)(((byte)(199)))));
            this.tabControlPanel4.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel4.Style.GradientAngle = 90;
            this.tabControlPanel4.TabIndex = 1;
            this.tabControlPanel4.TabItem = this.tabItem4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(330, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 18);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.Location = new System.Drawing.Point(37, 15);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(110, 20);
            this.labelX6.TabIndex = 12;
            this.labelX6.Text = "远程数据库连接：";
            // 
            // comboBoxEx2
            // 
            this.comboBoxEx2.DisplayMember = "Text";
            this.comboBoxEx2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx2.FormattingEnabled = true;
            this.comboBoxEx2.Location = new System.Drawing.Point(171, 13);
            this.comboBoxEx2.Name = "comboBoxEx2";
            this.comboBoxEx2.Size = new System.Drawing.Size(153, 22);
            this.comboBoxEx2.TabIndex = 11;
            this.comboBoxEx2.WatermarkText = "远程数据库连接标识符";
            this.comboBoxEx2.SelectedIndexChanged += new System.EventHandler(this.comboBoxEx2_SelectedIndexChanged);
            // 
            // textBoxX1
            // 
            // 
            // 
            // 
            this.textBoxX1.Border.Class = "TextBoxBorder";
            this.textBoxX1.Location = new System.Drawing.Point(170, 69);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(154, 21);
            this.textBoxX1.TabIndex = 5;
            this.textBoxX1.WatermarkText = "代码与图层关系表名";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(37, 71);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(135, 21);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "代码与图层关系表名：";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(37, 45);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(95, 19);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "分类代码表名：";
            // 
            // textBoxX3
            // 
            // 
            // 
            // 
            this.textBoxX3.Border.Class = "TextBoxBorder";
            this.textBoxX3.Location = new System.Drawing.Point(170, 42);
            this.textBoxX3.Name = "textBoxX3";
            this.textBoxX3.Size = new System.Drawing.Size(154, 21);
            this.textBoxX3.TabIndex = 0;
            this.textBoxX3.WatermarkText = "分类代码表名";
            // 
            // tabItem4
            // 
            this.tabItem4.AttachedControl = this.tabControlPanel4;
            this.tabItem4.Name = "tabItem4";
            this.tabItem4.Text = "远程数据库";
            // 
            // panelEx2
            // 
            this.panelEx2.Controls.Add(this.radioButton2);
            this.panelEx2.Controls.Add(this.radioButton1);
            this.panelEx2.Controls.Add(this.label1);
            this.panelEx2.Controls.Add(this.panelEx3);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx2.Location = new System.Drawing.Point(1, 1);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(552, 59);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 0;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(420, 20);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(95, 16);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "使用本地连接";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(308, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(95, 16);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "使用远程连接";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(10, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "分类代码标准化检查设置";
            // 
            // panelEx3
            // 
            this.panelEx3.Controls.Add(this.richTextBox2);
            this.panelEx3.Location = new System.Drawing.Point(10, 9);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Padding = new System.Windows.Forms.Padding(6);
            this.panelEx3.Size = new System.Drawing.Size(230, 34);
            this.panelEx3.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx3.Style.BorderColor.Color = System.Drawing.Color.Gray;
            this.panelEx3.Style.BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.panelEx3.Style.CornerDiameter = 5;
            this.panelEx3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 0;
            this.panelEx3.Text = "panelEx3";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.richTextBox2.Location = new System.Drawing.Point(6, 6);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox2.Size = new System.Drawing.Size(218, 22);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "代码标准化，代码图层设置";
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "代码检查 &D";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Controls.Add(this.panelEx4);
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel2.Size = new System.Drawing.Size(554, 247);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            this.tabControlPanel2.TabIndex = 2;
            this.tabControlPanel2.TabItem = this.tabItem2;
            // 
            // panelEx4
            // 
            this.panelEx4.Controls.Add(this.label2);
            this.panelEx4.Controls.Add(this.panelEx7);
            this.panelEx4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx4.Location = new System.Drawing.Point(1, 1);
            this.panelEx4.Name = "panelEx4";
            this.panelEx4.Size = new System.Drawing.Size(552, 59);
            this.panelEx4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.panelEx4.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx4.Style.GradientAngle = 90;
            this.panelEx4.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(10, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(295, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "悬挂点、骨架线搜索容差，骨架线与对偶线匹配表设置";
            // 
            // panelEx7
            // 
            this.panelEx7.Controls.Add(this.richTextBox4);
            this.panelEx7.Location = new System.Drawing.Point(10, 9);
            this.panelEx7.Name = "panelEx7";
            this.panelEx7.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx7.Size = new System.Drawing.Size(230, 34);
            this.panelEx7.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx7.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx7.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx7.Style.BorderColor.Color = System.Drawing.Color.Gray;
            this.panelEx7.Style.BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.panelEx7.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx7.Style.GradientAngle = 90;
            this.panelEx7.TabIndex = 0;
            this.panelEx7.Text = "panelEx2";
            // 
            // richTextBox4
            // 
            this.richTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.richTextBox4.Location = new System.Drawing.Point(1, 1);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox4.Size = new System.Drawing.Size(228, 32);
            this.richTextBox4.TabIndex = 0;
            this.richTextBox4.Text = "线检查容差，对应表设置";
            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.tabControlPanel2;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "线标准化检查 &X";
            // 
            // tabControlPanel3
            // 
            this.tabControlPanel3.Controls.Add(this.panelEx5);
            this.tabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel3.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel3.Name = "tabControlPanel3";
            this.tabControlPanel3.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel3.Size = new System.Drawing.Size(554, 247);
            this.tabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel3.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel3.Style.GradientAngle = 90;
            this.tabControlPanel3.TabIndex = 3;
            this.tabControlPanel3.TabItem = this.tabItem3;
            // 
            // panelEx5
            // 
            this.panelEx5.Controls.Add(this.label3);
            this.panelEx5.Controls.Add(this.panelEx6);
            this.panelEx5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx5.Location = new System.Drawing.Point(1, 1);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(552, 59);
            this.panelEx5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.panelEx5.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx5.Style.GradientAngle = 90;
            this.panelEx5.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(10, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(319, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "高程要素与其对应注记搜索容差设置，级别分类对应表设置";
            // 
            // panelEx6
            // 
            this.panelEx6.Controls.Add(this.richTextBox6);
            this.panelEx6.Location = new System.Drawing.Point(10, 9);
            this.panelEx6.Name = "panelEx6";
            this.panelEx6.Padding = new System.Windows.Forms.Padding(1);
            this.panelEx6.Size = new System.Drawing.Size(230, 34);
            this.panelEx6.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx6.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelEx6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx6.Style.BorderColor.Color = System.Drawing.Color.Gray;
            this.panelEx6.Style.BorderDashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.panelEx6.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx6.Style.GradientAngle = 90;
            this.panelEx6.TabIndex = 0;
            this.panelEx6.Text = "panelEx2";
            // 
            // richTextBox6
            // 
            this.richTextBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.richTextBox6.Location = new System.Drawing.Point(1, 1);
            this.richTextBox6.Name = "richTextBox6";
            this.richTextBox6.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox6.Size = new System.Drawing.Size(228, 32);
            this.richTextBox6.TabIndex = 0;
            this.richTextBox6.Text = "高程检查容差，对应表设置";
            // 
            // tabItem3
            // 
            this.tabItem3.AttachedControl = this.tabControlPanel3;
            this.tabItem3.Name = "tabItem3";
            this.tabItem3.Text = "高程要素检查  &G";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(562, 310);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelEx1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(281, 142);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据检查设置";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl2)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabControlPanel5.ResumeLayout(false);
            this.tabControlPanel4.ResumeLayout(false);
            this.tabControlPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            this.panelEx3.ResumeLayout(false);
            this.tabControlPanel2.ResumeLayout(false);
            this.panelEx4.ResumeLayout(false);
            this.panelEx4.PerformLayout();
            this.panelEx7.ResumeLayout(false);
            this.tabControlPanel3.ResumeLayout(false);
            this.panelEx5.ResumeLayout(false);
            this.panelEx5.PerformLayout();
            this.panelEx6.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        private void buttonX2_Click(object sender, EventArgs e)
        {
            #region 获取文件
            openFileDialog1.Title = "选择数据标准文件库";
            //openFileDialog1.RestoreDirectory = true;
            
            openFileDialog1.Filter = "(*.mdb)|*.MDB";
            openFileDialog1.ShowDialog();

            textBoxX5.Text = openFileDialog1.FileName; 
            #endregion 

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.tabControl2.SelectedTab = this.tabItem4;
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pOracleConnSign = comboBoxEx2.Text;

            //验证连接的正确性
            #region 验证数据库连接的正确性
            if (TestConnSign(pOracleConnSign))
            {

                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\RES\\Pic\\GeoDataChecker.验证成功.png");
            }
            else
            {

                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\RES\\Pic\\GeoDataChecker.验证失败.png");
            }
            pictureBox1.Visible = true; 
            #endregion
        }

        //使用oracle默认的scott用户进行连接测试  密码tiger
        /// <summary>
        /// 使用Oracle默认用户测试连接是否成功
        /// </summary>
        /// <param name="Sign"></param>
        /// <returns>连接是否成功，返回true或者false</returns>
        #region 使用Oracle默认用户测试连接是否成功
        private bool TestConnSign(string Sign)
        {
            string ConnectionString = "Data Source=" + Sign + ";User Id=scott;Password=tiger;";

            OracleConnection conn = new OracleConnection(ConnectionString);

            try
            {
                conn.Open();

                //if (conn.State)
                //{
                conn.Close();
                conn = null;
                return true;
                //}
            }
            catch (Exception e)
            {
                //string pErr= e.ToString();
                string pErr = e.Message;

                if (pErr == "ORA-12541: TNS: 无监听程序\n")
                {
                    return false;
                }
                else if (pErr == "ORA-28000: the account is locked\n")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        } 
        #endregion




		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        //[STAThread]
        //static void Main() 
        //{
        //    Application.Run(new Form1());
        //}

		
	}
}
