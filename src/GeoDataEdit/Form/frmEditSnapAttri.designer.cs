namespace GeoDataEdit
{
    partial class frmEditSnapAttri
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ();
            }
            base.Dispose ( disposing );
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent ()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditSnapAttri));
            this.gb_SnapType = new System.Windows.Forms.GroupBox();
            this.chb_SnapType_Center = new System.Windows.Forms.CheckBox();
            this.chb_SnapType_Boundry = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txt_SnapRadius = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chb_SnapType_Intersect = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chb_SnapType_Node = new System.Windows.Forms.CheckBox();
            this.txt_CacheRadius = new System.Windows.Forms.TextBox();
            this.chb_SnapType_Mid = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chb_SnapType_Port = new System.Windows.Forms.CheckBox();
            this.btnCannel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.gb_SnapType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl2
            // 
            //this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            //this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            //this.ultraTabPageControl2.Size = new System.Drawing.Size(196, 74);
            // 
            // gb_SnapType
            // 
            this.gb_SnapType.Controls.Add(this.label5);
            this.gb_SnapType.Controls.Add(this.chb_SnapType_Center);
            this.gb_SnapType.Controls.Add(this.chb_SnapType_Boundry);
            this.gb_SnapType.Controls.Add(this.pictureBox1);
            this.gb_SnapType.Controls.Add(this.txt_SnapRadius);
            this.gb_SnapType.Controls.Add(this.label1);
            this.gb_SnapType.Controls.Add(this.label4);
            this.gb_SnapType.Controls.Add(this.chb_SnapType_Intersect);
            this.gb_SnapType.Controls.Add(this.label3);
            this.gb_SnapType.Controls.Add(this.chb_SnapType_Node);
            this.gb_SnapType.Controls.Add(this.txt_CacheRadius);
            this.gb_SnapType.Controls.Add(this.chb_SnapType_Mid);
            this.gb_SnapType.Controls.Add(this.label2);
            this.gb_SnapType.Controls.Add(this.chb_SnapType_Port);
            this.gb_SnapType.Location = new System.Drawing.Point(6, 6);
            this.gb_SnapType.Name = "gb_SnapType";
            this.gb_SnapType.Size = new System.Drawing.Size(380, 177);
            this.gb_SnapType.TabIndex = 1;
            this.gb_SnapType.TabStop = false;
            this.gb_SnapType.Text = "设置捕捉方式";
            // 
            // chb_SnapType_Center
            // 
            this.chb_SnapType_Center.AutoSize = true;
            this.chb_SnapType_Center.Location = new System.Drawing.Point(248, 54);
            this.chb_SnapType_Center.Name = "chb_SnapType_Center";
            this.chb_SnapType_Center.Size = new System.Drawing.Size(108, 16);
            this.chb_SnapType_Center.TabIndex = 88;
            this.chb_SnapType_Center.Text = "几何中心点捕捉";
            this.chb_SnapType_Center.UseVisualStyleBackColor = true;
            // 
            // chb_SnapType_Boundry
            // 
            this.chb_SnapType_Boundry.AutoSize = true;
            this.chb_SnapType_Boundry.Location = new System.Drawing.Point(248, 20);
            this.chb_SnapType_Boundry.Name = "chb_SnapType_Boundry";
            this.chb_SnapType_Boundry.Size = new System.Drawing.Size(84, 16);
            this.chb_SnapType_Boundry.TabIndex = 87;
            this.chb_SnapType_Boundry.Text = "边上点捕捉";
            this.chb_SnapType_Boundry.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(25, 87);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(36, 36);
            this.pictureBox1.TabIndex = 84;
            this.pictureBox1.TabStop = false;
            // 
            // txt_SnapRadius
            // 
            this.txt_SnapRadius.Location = new System.Drawing.Point(65, 142);
            this.txt_SnapRadius.Name = "txt_SnapRadius";
            this.txt_SnapRadius.Size = new System.Drawing.Size(80, 21);
            this.txt_SnapRadius.TabIndex = 83;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 82;
            this.label1.Text = "捕捉半径：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(343, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 81;
            this.label4.Text = "像素";
            // 
            // chb_SnapType_Intersect
            // 
            this.chb_SnapType_Intersect.AutoSize = true;
            this.chb_SnapType_Intersect.Location = new System.Drawing.Point(132, 54);
            this.chb_SnapType_Intersect.Name = "chb_SnapType_Intersect";
            this.chb_SnapType_Intersect.Size = new System.Drawing.Size(72, 16);
            this.chb_SnapType_Intersect.TabIndex = 4;
            this.chb_SnapType_Intersect.Text = "交点捕捉";
            this.chb_SnapType_Intersect.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 80;
            this.label3.Text = "像素";
            // 
            // chb_SnapType_Node
            // 
            this.chb_SnapType_Node.AutoSize = true;
            this.chb_SnapType_Node.Location = new System.Drawing.Point(132, 20);
            this.chb_SnapType_Node.Name = "chb_SnapType_Node";
            this.chb_SnapType_Node.Size = new System.Drawing.Size(72, 16);
            this.chb_SnapType_Node.TabIndex = 3;
            this.chb_SnapType_Node.Text = "结点捕捉";
            this.chb_SnapType_Node.UseVisualStyleBackColor = true;
            // 
            // txt_CacheRadius
            // 
            this.txt_CacheRadius.Location = new System.Drawing.Point(260, 142);
            this.txt_CacheRadius.Name = "txt_CacheRadius";
            this.txt_CacheRadius.Size = new System.Drawing.Size(80, 21);
            this.txt_CacheRadius.TabIndex = 79;
            // 
            // chb_SnapType_Mid
            // 
            this.chb_SnapType_Mid.AutoSize = true;
            this.chb_SnapType_Mid.Location = new System.Drawing.Point(16, 54);
            this.chb_SnapType_Mid.Name = "chb_SnapType_Mid";
            this.chb_SnapType_Mid.Size = new System.Drawing.Size(72, 16);
            this.chb_SnapType_Mid.TabIndex = 2;
            this.chb_SnapType_Mid.Text = "中点捕捉";
            this.chb_SnapType_Mid.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 78;
            this.label2.Text = "缓冲半径：";
            // 
            // chb_SnapType_Port
            // 
            this.chb_SnapType_Port.AutoSize = true;
            this.chb_SnapType_Port.Location = new System.Drawing.Point(16, 20);
            this.chb_SnapType_Port.Name = "chb_SnapType_Port";
            this.chb_SnapType_Port.Size = new System.Drawing.Size(72, 16);
            this.chb_SnapType_Port.TabIndex = 1;
            this.chb_SnapType_Port.Text = "端点捕捉";
            this.chb_SnapType_Port.UseVisualStyleBackColor = true;
            // 
            // btnCannel
            // 
            this.btnCannel.Location = new System.Drawing.Point(311, 189);
            this.btnCannel.Name = "btnCannel";
            this.btnCannel.Size = new System.Drawing.Size(75, 23);
            this.btnCannel.TabIndex = 99;
            this.btnCannel.Text = "取消";
            this.btnCannel.UseVisualStyleBackColor = true;
            this.btnCannel.Click += new System.EventHandler(this.btnCannel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(231, 189);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 98;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(67, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(289, 41);
            this.label5.TabIndex = 100;
            this.label5.Text = "请用户根据需要选择捕捉的类型。请尽量选择较少的捕捉方式和合适的捕捉半径，以加快捕捉的效率。";
            // 
            // frmEditSnapAttri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 224);
            this.Controls.Add(this.btnCannel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gb_SnapType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditSnapAttri";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "捕捉属性编辑";
            this.Load += new System.EventHandler(this.frmEditSnapAttri_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmEditSnapAttri_KeyDown);
            this.gb_SnapType.ResumeLayout(false);
            this.gb_SnapType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gb_SnapType;
        private System.Windows.Forms.CheckBox chb_SnapType_Intersect;
        private System.Windows.Forms.CheckBox chb_SnapType_Node;
        private System.Windows.Forms.CheckBox chb_SnapType_Mid;
        private System.Windows.Forms.CheckBox chb_SnapType_Port;
        private System.Windows.Forms.TextBox txt_CacheRadius;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txt_SnapRadius;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chb_SnapType_Center;
        private System.Windows.Forms.CheckBox chb_SnapType_Boundry;
        private System.Windows.Forms.Button btnCannel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label5;
    }
}