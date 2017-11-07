using ESRI.ArcGIS.Controls;
namespace GeoDataManagerFrame
{
    partial class FrmEagleEye
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
            pAxMapCtrl.OnMapReplaced -= new IMapControlEvents2_Ax_OnMapReplacedEventHandler(pAxMapCtrl_OnMapReplaced);
            pAxMapCtrl.OnExtentUpdated -= new IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(pAxMapCtrl_OnExtentUpdated);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEagleEye));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.p1 = new System.Windows.Forms.PictureBox();
            this.p4 = new System.Windows.Forms.PictureBox();
            this.p3 = new System.Windows.Forms.PictureBox();
            this.p2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2)).BeginInit();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(0, 0);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(213, 188);
            this.axMapControl1.TabIndex = 0;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            // 
            // p1
            // 
            this.p1.BackColor = System.Drawing.Color.Red;
            this.p1.Location = new System.Drawing.Point(55, 77);
            this.p1.Name = "p1";
            this.p1.Size = new System.Drawing.Size(10, 3);
            this.p1.TabIndex = 1;
            this.p1.TabStop = false;
            // 
            // p4
            // 
            this.p4.BackColor = System.Drawing.Color.Red;
            this.p4.Location = new System.Drawing.Point(65, 79);
            this.p4.Name = "p4";
            this.p4.Size = new System.Drawing.Size(3, 10);
            this.p4.TabIndex = 1;
            this.p4.TabStop = false;
            // 
            // p3
            // 
            this.p3.BackColor = System.Drawing.Color.Red;
            this.p3.Location = new System.Drawing.Point(67, 77);
            this.p3.Name = "p3";
            this.p3.Size = new System.Drawing.Size(10, 3);
            this.p3.TabIndex = 1;
            this.p3.TabStop = false;
            // 
            // p2
            // 
            this.p2.BackColor = System.Drawing.Color.Red;
            this.p2.Location = new System.Drawing.Point(65, 67);
            this.p2.Name = "p2";
            this.p2.Size = new System.Drawing.Size(3, 10);
            this.p2.TabIndex = 1;
            this.p2.TabStop = false;
            // 
            // FrmEagleEye
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 188);
            this.Controls.Add(this.p2);
            this.Controls.Add(this.p4);
            this.Controls.Add(this.p3);
            this.Controls.Add(this.p1);
            this.Controls.Add(this.axMapControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEagleEye";
            this.Opacity = 0.7;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "鹰眼";
            this.Load += new System.EventHandler(this.FrmEagleEye_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEagleEye_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
		//add by xisheng 20110801
        private System.Windows.Forms.PictureBox p1;
        private System.Windows.Forms.PictureBox p4;
        private System.Windows.Forms.PictureBox p3;
        private System.Windows.Forms.PictureBox p2;
    }
}