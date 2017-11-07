namespace GeoDBIntegration
{
    partial class DelGroup
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
            this.LstViewRole = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.btnConSel = new DevComponents.DotNetBar.ButtonX();
            this.btnDelSel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // LstViewRole
            // 
            // 
            // 
            // 
            this.LstViewRole.Border.Class = "ListViewBorder";
            this.LstViewRole.CheckBoxes = true;
            this.LstViewRole.Location = new System.Drawing.Point(3, 12);
            this.LstViewRole.Name = "LstViewRole";
            this.LstViewRole.Size = new System.Drawing.Size(233, 126);
            this.LstViewRole.TabIndex = 0;
            this.LstViewRole.UseCompatibleStateImageBehavior = false;
            this.LstViewRole.View = System.Windows.Forms.View.List;
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(240, 12);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(50, 23);
            this.btnSelAll.TabIndex = 1;
            this.btnSelAll.Text = "全选";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnConSel
            // 
            this.btnConSel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConSel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnConSel.Location = new System.Drawing.Point(242, 41);
            this.btnConSel.Name = "btnConSel";
            this.btnConSel.Size = new System.Drawing.Size(50, 23);
            this.btnConSel.TabIndex = 2;
            this.btnConSel.Text = "反选";
            this.btnConSel.Click += new System.EventHandler(this.btnConSel_Click);
            // 
            // btnDelSel
            // 
            this.btnDelSel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDelSel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDelSel.Location = new System.Drawing.Point(240, 115);
            this.btnDelSel.Name = "btnDelSel";
            this.btnDelSel.Size = new System.Drawing.Size(50, 23);
            this.btnDelSel.TabIndex = 3;
            this.btnDelSel.Text = "删除";
            this.btnDelSel.Click += new System.EventHandler(this.btnDelSel_Click);
            // 
            // DelGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 140);
            this.Controls.Add(this.btnDelSel);
            this.Controls.Add(this.btnConSel);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.LstViewRole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DelGroup";
            this.ShowIcon = false;
            this.Text = "删除角色";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx LstViewRole;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        private DevComponents.DotNetBar.ButtonX btnConSel;
        private DevComponents.DotNetBar.ButtonX btnDelSel;
    }
}