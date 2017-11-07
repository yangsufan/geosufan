using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace GeoDataCenterFunLib
{
    public class LogFile
    {
        public System.Windows.Forms.RichTextBox m_tipRichBox;
        public string m_strInitXmlPath="";
        public LogFile(System.Windows.Forms.RichTextBox tipRichBox,string strLogFile)
        {
            m_tipRichBox = tipRichBox;
            m_strInitXmlPath = strLogFile;
            if (m_strInitXmlPath == null)
            {
                m_strInitXmlPath = Application.StartupPath + "\\..\\Log\\Log.txt";
            }
            if (m_strInitXmlPath.Equals(""))
            {
                m_strInitXmlPath = Application.StartupPath + "\\..\\Log\\Log.txt";
            }
            //判断文件是否存在  不存在就创建
            if (!File.Exists(m_strInitXmlPath))
            {
                FileStream pFileStream = File.Create(m_strInitXmlPath);
                pFileStream.Close();
            }
         
        }
        //写日志
        public void Writelog(string logstr)
        {
            FileStream fs = new FileStream(m_strInitXmlPath, FileMode.OpenOrCreate,FileAccess.ReadWrite);
            StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
            List<string> list = new List<string>();
            string strread;
            while ((strread = reader.ReadLine()) != null)
                list.Add(strread);
            reader.Close();

            string strTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.ToLongTimeString();
            if(m_tipRichBox!=null)
            {
                m_tipRichBox.AppendText(strTime + "/" + logstr + "\r\n");
            }
            for (int i = 0; i < list.Count; i++)
                strread += list[i] + "\r\n";
            strread = DateTime.Now.ToString() + "/" + logstr + "\r\n"+strread;
            StreamWriter sw = new StreamWriter(m_strInitXmlPath, false, Encoding.GetEncoding("gb2312"));
            sw.Write(strread); 
             sw.Close();
            fs.Close();
            sw = null;
            fs = null;
        }

        //查询日志
        public List<string> SeachLog(DateTime dt1, DateTime dt2)
        {
            List<string> returnlist = new List<string>();
            try
            {
                string[] array = new string[2];
                FileStream fs = new FileStream(m_strInitXmlPath, FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
                List<string> list = new List<string>();
                string strread;
                while ((strread = reader.ReadLine()) != null)
                    list.Add(strread);
                reader.Close();
                for (int i = 0; i < list.Count; i++)
                {
                    array = list[i].Split('/');
                    //array[0] = list[i].Substring(0, list[i].LastIndexOf("/"));
                    //array[1] = list[i].Substring(list[i].LastIndexOf("/") + 1);
                    DateTime dt = Convert.ToDateTime(array[0]);
                    if (DateTime.Compare(dt, dt1) >= 0 && DateTime.Compare(dt2, dt) >= 0)
                        returnlist.Add(array[0] + "/" + array[1]);
                }
            }
            catch { }
            return returnlist;


        }
        //重载一个无参的查询日志
        public List<string> SeachLog()
        {
            List<string> list = new List<string>();
            try
            {
                string[] array = new string[2];
                FileStream fs = new FileStream(m_strInitXmlPath, FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
                string strread;
                while ((strread = reader.ReadLine()) != null)
                    list.Add(strread);
                reader.Close();
            }
            catch { }
            return list;
        }

        //public List<string> SeachLog()
        //{
        //    List<string> returnlist = new List<string>();
        //    FileStream fs = new FileStream(m_strInitXmlPath, FileMode.OpenOrCreate, FileAccess.Read);
        //    StreamReader reader = new StreamReader(fs);
        //    List<string> list = new List<string>();
        //    string strread;
        //    while ((strread = reader.ReadLine()) != null)
        //        list.Add(strread);
        //    reader.Close();
        //    return list;

        //}

        //清空日志
        public void ClearLog(ListView lv)
        {
            try
            {
                lv.Items.Clear();
                FileStream fs = new FileStream(m_strInitXmlPath, FileMode.Truncate, FileAccess.ReadWrite);
                fs.Close();
                fs = null;
            }
            catch
            {
                MessageBox.Show("日志文件不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //导出日志
        public void ExportLog(string path)
        { 
            if (File.Exists(m_strInitXmlPath))
            {
                File.Copy(m_strInitXmlPath,path, true);
                MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("日志文件未生成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }
    }
}
