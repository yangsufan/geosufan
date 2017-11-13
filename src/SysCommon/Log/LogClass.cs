using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SysCommon.Log
{
    //本地日志
    public class SysLocalLog
    {
        private StreamWriter _StreamWriter;

        //输出路径
        private string _strSavePath;
        public string SavePath
        {
            get
            {
                return _strSavePath;
            }
            set
            {
                _strSavePath = value;
            }
        }

        //构造函数
        public SysLocalLog()
        {
        }

        public SysLocalLog(string strSavePath)
        {
            _strSavePath = strSavePath;
        }

        public void CreateLogFile(string strFileName)
        {
            if (_strSavePath == string.Empty) return;
            if (!Directory.Exists(_strSavePath))
            {
                Directory.CreateDirectory(_strSavePath);
            }

            if (!File.Exists(_strSavePath + @"\" + strFileName))
            {
                FileStream pFileStream = File.Create(_strSavePath + @"\" + strFileName);
                pFileStream.Close();
            }

            _StreamWriter = new StreamWriter(_strSavePath + @"\" + strFileName);
        }

        // 写入本地日志(带参数的构造函数与CreateLogFile函数先使用)
        public void WriteLocalLog(string content)
        {
            if (_StreamWriter == null) return;
            _StreamWriter.Write(_StreamWriter.NewLine);
            _StreamWriter.Write(content);
            _StreamWriter.Write(_StreamWriter.NewLine);
        }

        /// <summary>
        /// 写入本地日志(单独使用)
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="filepath">文件路径</param>
        /// <returns>写入本地日志是否成功</returns>
        public void WriteLocalLog(string content, string filepath)
        {
            if (!File.Exists(filepath))
            {
                _strSavePath = filepath.Substring(0, filepath.LastIndexOf("\\"));
                string fileName = filepath.Substring(filepath.LastIndexOf("\\"));
                CreateLogFile(fileName);
            }

            if (_StreamWriter == null)
            {
                _StreamWriter = new StreamWriter(filepath);
            }

            WriteLocalLog(content);
        }

        public void LogClose()
        {
            if (_StreamWriter != null)
            {
                _StreamWriter.Close();
                _StreamWriter = null;
            }
        }
    }

    //数据库日志
    public class SysDBLog
    {
    }
}
