using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;

namespace GeoDataChecker
{
    /// <summary>
    /// 数据检查错误处理父类
    /// 检查出的各类错误处理实现类从此类继承
    /// </summary>
    public class GOGISErrorTreator
    {
        protected OleDbConnection _con;

        public GOGISErrorTreator()
        {

        }
        protected string _LogPath;

        public string LogPath
        {
            get { return _LogPath; }
            set { _LogPath = value; }
        }

        public virtual void LogErr(object sender,ErrorEventArgs e)
        {


        }

        public virtual void BatchTreat()
        { 

        }

        /// <summary>
        /// 释放连接对象
        /// </summary>
        public virtual void Dispose()
        {
            if (this._con.State == System.Data.ConnectionState.Open)
	        {
                this._con.Close();
	        }
        }
    }
}
