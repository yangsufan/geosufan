namespace GeoDBATool
{
    partial class frmSDEManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSDEManager));
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPwd = new System.Windows.Forms.Label();
            this.lblInstance = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGrid = new MyControls.Control.DataGrid();
            this.toolBar = new MyControls.Control.ToolBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnRefresh = new MyControls.Control.Button();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtInstance = new System.Windows.Forms.TextBox();
            this.btnStart = new MyControls.Control.Button();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.btnDelAll = new MyControls.Control.Button();
            this.btnStop = new MyControls.Control.Button();
            this.btnDelSel = new MyControls.Control.Button();
            this.btnPause = new MyControls.Control.Button();
            this.btnResume = new MyControls.Control.Button();
            this.btnView = new MyControls.Control.Button();
            this.btnStopView = new MyControls.Control.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnect = new MyControls.Control.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.gbStatus.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServer.Location = new System.Drawing.Point(84, 28);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(196, 23);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "127.0.0.1";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(12, 28);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(70, 14);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "服务器名:";
            this.lblServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(12, 60);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(70, 14);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "用 户 名:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new System.Drawing.Point(12, 96);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(70, 14);
            this.lblPwd.TabIndex = 0;
            this.lblPwd.Text = "密    码:";
            this.lblPwd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInstance
            // 
            this.lblInstance.AutoSize = true;
            this.lblInstance.Location = new System.Drawing.Point(12, 128);
            this.lblInstance.Name = "lblInstance";
            this.lblInstance.Size = new System.Drawing.Size(70, 14);
            this.lblInstance.TabIndex = 0;
            this.lblInstance.Text = "实 例 名:";
            this.lblInstance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUser
            // 
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.Location = new System.Drawing.Point(84, 60);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(196, 23);
            this.txtUser.TabIndex = 2;
            this.txtUser.Text = "dlguser";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.dataGrid);
            this.panel2.Controls.Add(this.toolBar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 208);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(782, 295);
            this.panel2.TabIndex = 6;
            // 
            // dataGrid
            // 
            this.dataGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataGrid.CaptionFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGrid.DataMember = "";
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid.Location = new System.Drawing.Point(0, 26);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ParentRowsVisible = false;
            this.dataGrid.Size = new System.Drawing.Size(778, 265);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.CurrentRowIndexChanged += new System.EventHandler(this.dataGrid_CurrentRowIndexChanged);
            // 
            // toolBar
            // 
            this.toolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolBar.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolBar.ForeColor = System.Drawing.Color.Black;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(778, 26);
            this.toolBar.TabIndex = 1;
            this.toolBar.ButtonClick += new MyControls.Control.ButtonClickEventHandler(this.toolBar_ButtonClick);
            this.toolBar.SelectedIndexChanged += new System.EventHandler(this.toolBar_SelectedIndexChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.SystemColors.Control;
            this.btnRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRefresh.BackgroundImage")));
            this.btnRefresh.Location = new System.Drawing.Point(558, 28);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 32);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新状态(&F)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtPwd
            // 
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPwd.Location = new System.Drawing.Point(84, 92);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(196, 23);
            this.txtPwd.TabIndex = 3;
            this.txtPwd.Text = "11111";
            // 
            // txtInstance
            // 
            this.txtInstance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInstance.Location = new System.Drawing.Point(84, 128);
            this.txtInstance.Name = "txtInstance";
            this.txtInstance.Size = new System.Drawing.Size(196, 23);
            this.txtInstance.TabIndex = 4;
            this.txtInstance.Text = "5151";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnStart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStart.BackgroundImage")));
            this.btnStart.Location = new System.Drawing.Point(24, 28);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 32);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "启动(&S)";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // gbStatus
            // 
            this.gbStatus.BackColor = System.Drawing.Color.Transparent;
            this.gbStatus.Controls.Add(this.btnDelAll);
            this.gbStatus.Controls.Add(this.btnStart);
            this.gbStatus.Controls.Add(this.btnStop);
            this.gbStatus.Controls.Add(this.btnDelSel);
            this.gbStatus.Controls.Add(this.btnPause);
            this.gbStatus.Controls.Add(this.btnResume);
            this.gbStatus.Controls.Add(this.btnView);
            this.gbStatus.Controls.Add(this.btnStopView);
            this.gbStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbStatus.Enabled = false;
            this.gbStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gbStatus.ForeColor = System.Drawing.Color.Black;
            this.gbStatus.Location = new System.Drawing.Point(296, 0);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(256, 208);
            this.gbStatus.TabIndex = 1;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "服务器状态";
            // 
            // btnDelAll
            // 
            this.btnDelAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelAll.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelAll.BackgroundImage")));
            this.btnDelAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelAll.Location = new System.Drawing.Point(140, 110);
            this.btnDelAll.Name = "btnDelAll";
            this.btnDelAll.Size = new System.Drawing.Size(100, 32);
            this.btnDelAll.TabIndex = 4;
            this.btnDelAll.Text = "删除所有连接(&d)";
            this.btnDelAll.Click += new System.EventHandler(this.btnDelAll_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.SystemColors.Control;
            this.btnStop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStop.BackgroundImage")));
            this.btnStop.Location = new System.Drawing.Point(140, 28);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 32);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止(&T)";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnDelSel
            // 
            this.btnDelSel.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelSel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelSel.BackgroundImage")));
            this.btnDelSel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelSel.Location = new System.Drawing.Point(24, 110);
            this.btnDelSel.Name = "btnDelSel";
            this.btnDelSel.Size = new System.Drawing.Size(100, 32);
            this.btnDelSel.TabIndex = 3;
            this.btnDelSel.Text = "删除选中连接(&k)";
            this.btnDelSel.Click += new System.EventHandler(this.btnDelSel_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.SystemColors.Control;
            this.btnPause.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPause.BackgroundImage")));
            this.btnPause.Location = new System.Drawing.Point(24, 72);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(100, 32);
            this.btnPause.TabIndex = 2;
            this.btnPause.Text = "暂停(&P)";
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnResume
            // 
            this.btnResume.BackColor = System.Drawing.SystemColors.Control;
            this.btnResume.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnResume.BackgroundImage")));
            this.btnResume.Location = new System.Drawing.Point(140, 72);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(100, 32);
            this.btnResume.TabIndex = 2;
            this.btnResume.Text = "恢复(&R)";
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnView
            // 
            this.btnView.BackColor = System.Drawing.SystemColors.Control;
            this.btnView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnView.BackgroundImage")));
            this.btnView.Location = new System.Drawing.Point(24, 164);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(100, 32);
            this.btnView.TabIndex = 2;
            this.btnView.Text = "开始监视(&V)";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnStopView
            // 
            this.btnStopView.BackColor = System.Drawing.SystemColors.Control;
            this.btnStopView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStopView.BackgroundImage")));
            this.btnStopView.Location = new System.Drawing.Point(140, 164);
            this.btnStopView.Name = "btnStopView";
            this.btnStopView.Size = new System.Drawing.Size(100, 32);
            this.btnStopView.TabIndex = 2;
            this.btnStopView.Text = "停止监视(&I)";
            this.btnStopView.Click += new System.EventHandler(this.btnStopView_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.gbStatus);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 208);
            this.panel1.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.lblUser);
            this.groupBox1.Controls.Add(this.lblPwd);
            this.groupBox1.Controls.Add(this.lblInstance);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.txtPwd);
            this.groupBox1.Controls.Add(this.txtInstance);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 208);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器连接";
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.SystemColors.Control;
            this.btnConnect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnConnect.BackgroundImage")));
            this.btnConnect.Location = new System.Drawing.Point(156, 164);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(124, 32);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "测试连接..(&C)";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // frmSDEManager
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
            this.ClientSize = new System.Drawing.Size(782, 503);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSDEManager";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ArcSDE 连接信息管理";
            this.Load += new System.EventHandler(this.frmSDEManager_Load);
            this.Closed += new System.EventHandler(this.frmSDEManager_Closed);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmSDEManager_Closing);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.gbStatus.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.Label lblInstance;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Panel panel2;
        private MyControls.Control.DataGrid dataGrid;
        private MyControls.Control.ToolBar toolBar;
        private System.Windows.Forms.Timer timer1;
        private MyControls.Control.Button btnRefresh;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtInstance;
        private MyControls.Control.Button btnStart;
        private System.Windows.Forms.GroupBox gbStatus;
        private MyControls.Control.Button btnStop;
        private MyControls.Control.Button btnPause;
        private MyControls.Control.Button btnResume;
        private MyControls.Control.Button btnView;
        private MyControls.Control.Button btnStopView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private MyControls.Control.Button btnConnect;
        private MyControls.Control.Button btnDelAll;
        private MyControls.Control.Button btnDelSel;
    }
}