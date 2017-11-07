using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Analyst3D;
namespace jiangxinoamaluniversity
{
    public partial class frmPlayer : DevComponents.DotNetBar.Office2007Form
    {

        public IFeature m_feature;
        public IGlobeControl m_GlobeControl;//定义一个MapControl 全局变量
        public IScene m_Scene;
        public frmPlayer(IGlobeControl pGlobeControl)
        {
            
            InitializeComponent();
            m_GlobeControl = pGlobeControl;
            m_Scene = pGlobeControl.Globe as IScene;
           
        }

        private void ResizeOriginal()
        { 
            int intWidth = axWindowsMediaPlayer1.currentMedia.imageSourceWidth; 
            int intHeight = axWindowsMediaPlayer1.currentMedia.imageSourceHeight;
            axWindowsMediaPlayer1.Width = intWidth + 2;
            axWindowsMediaPlayer1.Height = intHeight + 2;
        }

        public void InitData()
        {
               string fieldname = m_feature.get_Value(3).ToString();
             String picPath =null;
             //picPath =@+ m_feature.get_Value(3).ToString()+"\"+ m_feature.get_Value(4).ToString();灵活的用法
             //axWindowsMediaPlayer1.URL = picPath;
             //ResizeOriginal();
             //axWindowsMediaPlayer1.ShowAboutBox();

             //axWindowsMediaPlayer1.Ctlcontrols.play();
          switch (fieldname)
            {
              case"16":
                    picPath = string.Concat(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "\\Data\\南丰服务区G.wmv");
            axWindowsMediaPlayer1.URL = picPath;
            ResizeOriginal();
            axWindowsMediaPlayer1.ShowAboutBox();

            axWindowsMediaPlayer1.Ctlcontrols.play();
            break;
              default:
            picPath = string.Concat(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "\\Data\\上游围堰-1.wmv");
            axWindowsMediaPlayer1.URL = picPath;
            ResizeOriginal();
            axWindowsMediaPlayer1.ShowAboutBox();

            axWindowsMediaPlayer1.Ctlcontrols.play();
            break;
            }
 
        }

  

        private void axWindowsMediaPlayer1_KeyUpEvent(object sender, AxWMPLib._WMPOCXEvents_KeyUpEvent e)
        {
            axWindowsMediaPlayer1.settings.balance = 0;//声道（-1代表左声道 ，1代表右声道，0代表均衡）
            axWindowsMediaPlayer1.settings.volume = 100;//声量（0—100）
        }

        private void axWindowsMediaPlayer1_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            axWindowsMediaPlayer1.settings.balance = 0;
            axWindowsMediaPlayer1.settings.volume = 20;
        }

        //private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        //{
        //    axWindowsMediaPlayer1.Ctlcontrols.play();
           

        //}
   

        private void frmPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
          m_Scene.ClearSelection();
          axWindowsMediaPlayer1.currentPlaylist.clear();
          axWindowsMediaPlayer1.Update();
          axWindowsMediaPlayer1.Dispose();
            Dispose();
        }

  
    }
}
