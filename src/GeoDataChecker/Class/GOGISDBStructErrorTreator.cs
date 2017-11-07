using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;

namespace GeoDataChecker
{
    public class GOGISDBStructErrorTreator:GOGISErrorTreator
    {

        public GOGISDBStructErrorTreator()
        {
            ///创建日志，从模板复制mdb文件到bin\..\Log文件夹下
            string DesLogPaht = Application.StartupPath + "\\..\\Log\\Log" + System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString() + System.DateTime.Today.Day.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".mdb";

            ///复制文件
            File.Copy(Application.StartupPath + "\\..\\Template\\Log.mdb", DesLogPaht);
           

            ///记录当前日志路径
            this._LogPath = DesLogPaht;

            ///创建数据库连接
            _con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this._LogPath);
            _con.Open();
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void LogErr(object sender,ErrorEventArgs e)
        {
            string pSql = "insert into Errorlog(ErrorName,ErrorDescription,ErrorFeatureClassName) values('" + e .ErrorName+"','"+e.ErrDescription+"','"+e.FeatureClassName+"'"+ ")";
            OleDbCommand SqlCommand = new OleDbCommand(pSql, this._con);
            SqlCommand.ExecuteNonQuery();
        }
    }
}
