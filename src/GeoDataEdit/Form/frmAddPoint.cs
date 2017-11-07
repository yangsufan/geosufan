using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;


using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using GeoDataCenterFunLib;




namespace GeoDataEdit
{
    public partial class frmAddPoint : DevComponents.DotNetBar.Office2007Form
    {
        public frmAddPoint(IFeature pt, IActiveView pAv, IFeatureLayer pFL)
        {
            InitializeComponent();
            pPointFeature = pt;
            pPoint = pPointFeature.Shape as IPoint;

            txtPX.Text = pPoint.X.ToString();
            txtPY.Text = pPoint.Y.ToString();
            pExtent = pAv.FullExtent;
            pointFL = pFL;
            pActView = pAv;
        }

        //private string shpPath = "";
        //private IWorkspace pWorkspace = null;
        //private IFeatureClass pFeaClass=null;
        private IFeatureLayer pointFL = null;
        private IFeature pPointFeature = null;
        private IEnvelope pExtent = null;
        private ESRI.ArcGIS.Geometry.IPoint pPoint = null;
        private IActiveView pActView = null;
        
     

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (pPointFeature == null)
                return;
            if (Convert.ToDouble(txtPX.Text) < pExtent.XMin || Convert.ToDouble(txtPX.Text) > pExtent.XMax || Convert.ToDouble(txtPY.Text) < pExtent.YMin || Convert.ToDouble(txtPY.Text) > pExtent.YMax)
                return;
            
            
            try
            {
                pPoint.X = Convert.ToDouble(txtPX.Text);
                pPoint.Y = Convert.ToDouble(txtPY.Text);
                pPointFeature.Shape = pPoint;
                pPointFeature.Store();
                (pointFL as IFeatureSelection).Clear();
                (pointFL as IFeatureSelection).Add(pPointFeature);
                pActView.Refresh();
               
            }
            catch (Exception ex)
            {
            

            }

            this.Close();
            Application.DoEvents();
           
            
        }
      

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddPoint_Load(object sender, EventArgs e)
        {

        }

      





    }
}