using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoSysUpdate
{
    public partial class  frmWindowsPlayer: DevComponents.DotNetBar.Office2007Form
    {
        public frmWindowsPlayer()
        {
            InitializeComponent();
        }
        public frmWindowsPlayer(string Path)
        {
            InitializeComponent();
            if (Path == "")
            {
                MessageBox.Show("没有对应的多媒体文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if(!System.IO.File.Exists(Path))
            {
                MessageBox.Show("多媒体文件路径未找到", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Text += ":" + Path;
            this.axWindowsMediaPlayer1.URL = Path;
           
        }
       

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //设置为可以打开多个文件
            openFileDialog.Multiselect = true;
            //设置打开文件格式
            openFileDialog.Filter = "Mp3文件|*.mp3|Wav文件|*.wav|Wma文件|*.wma|Wmv文件|*.wmv|所有格式|*.*";
            //判断是否单击确定按钮
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //建立播放列表，名字为aa
                axWindowsMediaPlayer1.currentPlaylist = axWindowsMediaPlayer1.newPlaylist("aa", "");
                //遍历打开的集合
                foreach (string fn in openFileDialog.FileNames)
                {
                    //添加播放列表
                    axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(fn));
                }
            }
            //播放
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void 全屏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            axWindowsMediaPlayer1.fullScreen = true;
        }
    }
}
