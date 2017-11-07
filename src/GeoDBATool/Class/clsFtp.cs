using System.Net;
using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GeoDBATool
{
    public class FTP_Class
    {
        private  string ftpServerIP;//ftp服务器ip
        private string ftpUserID;//用户id
        private string ftpPassword;//密码


        private FtpWebRequest reqFTP;

        public FTP_Class()
        {

        }

        public FTP_Class(string IP,string ID,string PassWord)
        {
            this.ftpServerIP = IP;
            this.ftpUserID = ID;
            this.ftpPassword = PassWord;
        }

        #region 测试连接
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="ErrorStr">返回执行状态；“Succeed”为成功</param>
        public void Connecttest(string ftpServerIP, string ftpUserID, string ftpPassword, out string ErrorStr)
        {

            try
            {
                ErrorStr = "";
                // 根据uri创建FtpWebRequest对象
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                Connect("ftp://" + ftpServerIP + "/");
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.EnableSsl = false;///////////////是否使用加密（true为使用加密）代理模式下无法使用加密
                reqFTP.Timeout = 10000;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                string line = reader.ReadLine();
                ErrorStr = "Succeed";
            }
            catch (Exception ex)
            {
                ErrorStr = string.Format("因{0},连接失败", ex.Message);
            }
        }
        #endregion

        #region 连接
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="path"></param>
        private void Connect(string path)//连接ftp
        {
            // 根据uri创建FtpWebRequest对象
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // ftp用户名和密码
            reqFTP.KeepAlive = false;
            reqFTP.Timeout = 10000;
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
        }
        #endregion

        #region ftp设置登录信息
        /// <summary>
        /// ftp登录信息
        /// </summary>
        /// <param name="ftpServerIP">ftpServerIP</param>
        /// <param name="ftpUserID">ftpUserID</param>
        /// <param name="ftpPassword">ftpPassword</param>
        public void FtpUpDown(string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUserID = ftpUserID;
            this.ftpPassword = ftpPassword;
        }
        #endregion

        #region 获取文件列表
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="path"></param>
        /// <param name="WRMethods"></param>
        /// <returns></returns>
        private string[] GetFileList(string path, string WRMethods,out string err )//上面的代码示例了如何从ftp服务器上获得文件列表
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            bool read = false;
            try
            {
                err="";
                Connect(path);
                reqFTP.Method = WRMethods;
                reqFTP.Timeout = 10000;
                reqFTP.KeepAlive = false;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                    read = true;
                }
                // to remove the trailing '\n'
                if (!read)
                {
                    err = "Succeed";
                    return null;
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                err = "Succeed";
                return result.ToString().Split('\n');
                
            }
            catch (Exception ex)
            {
                err=ex.Message;
                downloadFiles = null;
                return downloadFiles;
            }
            finally
            
            {
                if (null != result)
                    result = null;
            }
        }
        public string[] GetFileList(string path,out string error)//上面的代码示例了如何从ftp服务器上获得文件列表
        {
            return GetFileList("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectory,out error);
        }
        public string[] GetFileList(out string error)//上面的代码示例了如何从ftp服务器上获得文件列表
        {
            return GetFileList("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectory,out error);
        }
        #endregion

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        public bool Upload(string filename, string path, string savename,out string errorinfo) //上面的代码实现了从ftp服务器上载文件的功能
        {
            //FrmProcessBar frmbar = new FrmProcessBar();
            //frmbar.Show();
            path = path.Replace("\\", "/");
            FileInfo fileInf = new FileInfo(filename);

            savename = savename.Replace("+", "-");// 处理非法字符
            savename = savename.Replace("#", "-");//
            string uri;
            if (string.IsNullOrEmpty(path))
                uri = "ftp://" + ftpServerIP + "/" + savename;
            else
                uri = "ftp://" + ftpServerIP + "/" + path + "/" + savename;
            Connect(uri);//连接    

            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            reqFTP.Timeout = 10000;
            // 指定执行什么命令
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength =  fileInf.Length; 
            // 缓冲大小设置为kb 
            long procMax = fileInf.Length / 2048;
            //进度条最大值
            //frmbar.SetFrmProcessBarMax(procMax);
            //string task = "正在上传文件：" + savename;
            //frmbar.SetFrmProcessBarText(task);
            int value = 0;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // 打开一个文件流(System.IO.FileStream) 去读上传的文件
            
            try
            {
                FileStream fs = fileInf.OpenRead();
                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();
                // 每次读文件流的kb
                contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (contentLen != 0)
                {
                    value++;
                    //frmbar.SetFrmProcessBarValue(value);
                    Application.DoEvents();
                    // 把内容从file stream 写入upload stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流
                strm.Close();
                fs.Close();
                errorinfo = "Succeed";
                return true;
            }
            catch (Exception ex)
            {
                errorinfo = string.Format("因{0},无法完成上传", ex.Message);
                return false;
            }
            finally
            {
                //frmbar.Dispose();
                //frmbar.Close();
            }
            
        }
        #endregion

        #region 续传文件
        /// <summary>
        /// 续传文件
        /// </summary>
        /// <param name="filename"></param>
        public bool Upload(string filename, long size, string path, out string errorinfo) //上面的代码实现了从ftp服务器上载文件的功能
        {
            FrmProcessBar frmbar = new FrmProcessBar();
            frmbar.Show();
            path = path.Replace("\\", "/");
            FileInfo fileInf = new FileInfo(filename);
            //string uri = "ftp://" + path + "/" + fileInf.Name;
            string uri = "ftp://" + ftpServerIP + "/" + path + filename;
            Connect(uri);//连接         
            // 默认为true，连接不会被关闭
            // 在一个命令之后被执行
            reqFTP.KeepAlive = false;
            reqFTP.Timeout = 10000;
            // 指定执行什么命令         
            reqFTP.Method = WebRequestMethods.Ftp.AppendFile;
            // 上传文件时通知服务器文件的大小
            reqFTP.ContentLength = fileInf.Length;
            // 缓冲大小设置为kb 
            long procMax = (fileInf.Length -size)/ 2048;
            frmbar.SetFrmProcessBarMax(procMax);
            frmbar.SetFrmProcessBarText("正在续传文件：" + filename);
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // 打开一个文件流(System.IO.FileStream) 去读上传的文件
            FileStream fs = fileInf.OpenRead();
            int value = 0;
            try
            {
                
                StreamReader dsad = new StreamReader(fs);
                fs.Seek(size, SeekOrigin.Begin);
                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();
                // 每次读文件流的kb
                contentLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入upload stream 
                    value++;
                    frmbar.SetFrmProcessBarValue(value);
                    Application.DoEvents();
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                    
                }
                // 关闭两个流
                strm.Close();
                fs.Close();
                errorinfo = "Succeed";
                return true;
            }
            catch (Exception ex)
            {
                errorinfo = string.Format("因{0},无法完成上传", ex.Message);
                return false;
            }
            finally
            {
                if (null != fileInf)
                    fileInf = null;
                frmbar.Close();
                frmbar.Dispose();
            }
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="ftpfilepath">相对根目录的文件完整路径</param>
        /// <param name="filePath">本地保存文件的完整路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool Download(string ftpfilepath, string filePath, string fileName, out string errorinfo)////上面的代码实现了从ftp服务器下载文件的功能
        {
            Exception ex = null;
            errorinfo="";
            FrmProcessBar ProcBar = new FrmProcessBar();
            ProcBar.Show();
            try
            {
                filePath = filePath.Replace("我的电脑\\", "");
                String onlyFileName = Path.GetFileName(fileName);
                string newFileName = filePath;
                if (File.Exists(newFileName))
                {
                    errorinfo = string.Format("本地文件{0}已存在,无法下载", newFileName);
                    return false;
                }
                ftpfilepath = ftpfilepath.Replace("\\", "/");
                string url = "ftp://" + ftpServerIP + "/" + ftpfilepath;
                Connect(url);//连接 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Timeout = 10000;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;

                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                FileStream outputStream = new FileStream(newFileName, FileMode.Create);
                long ProcBarValue = 0;
                //////
                long all = GetFileSize(ftpfilepath, out ex);
                {
                    if (null != ex) return false;
                }
                ProcBar.SetFrmProcessBarMax(all / 2048);
                //////
                while (readCount > 0)
                {
                    ProcBarValue++;
                    ProcBar.SetFrmProcessBarText("正在下载文件：" + fileName);
                    ProcBar.SetFrmProcessBarValue(ProcBarValue);
                    Application.DoEvents();
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                errorinfo = "Succeed";
                return true;

            }
            catch (Exception exx)
            {
                errorinfo = string.Format("因{0},无法下载", exx.Message);
                return false;
            }
            finally
            {
                ProcBar.Dispose();
                ProcBar.Close();
            }
           
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">相对根目录下的文件完整路径</param>
        public void DeleteFileName(string fileName,out string error)
        {
            try
            {
                error="";
                // FileInfo fileInf = new FileInfo(fileName);
                string uri = "ftp://" + ftpServerIP + "/" + fileName;
                Connect(uri);//连接         
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                reqFTP.Timeout = 10000;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                response.Close();
                error="Succeed";
            }
            catch (Exception ex)
            {
                error=ex.Message;
            }
        }
        #endregion

        #region 在ftp上创建目录
        /// <summary>
        /// 在ftp上创建目录
        /// </summary>
        /// <param name="dirName">相对根目录下的完整文件夹路径</param>
        public void MakeDir(string dirName,out string error)
        {
            error = "";
            try
            {
                string uri = "ftp://" + ftpServerIP + "/" + dirName;
                Connect(uri);//连接      
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.Timeout = 10000;
                reqFTP.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                error = "Succeed";
            }
            catch (Exception ex)
            {
                error = string.Format("因{0},无法下载", ex.Message);
            }
        }
        #endregion

        #region 删除ftp上目录
        /// <summary>
        /// 删除ftp上目录
        /// </summary>
        /// <param name="dirName">相对根目录下的完整完整文件夹路径</param>
        public void delDir(string dirName, out string Error)
        {
            try
            {
                string uri = "ftp://" + ftpServerIP + "/" + dirName;
                Connect(uri);//连接      
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                reqFTP.Timeout = 10000;
                reqFTP.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                Error = "Succeed";
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
                Error = string.Format("因{0},无法下载", ex.Message);
            }
        }
        #endregion

        #region 获得ftp上文件大小
        /// <summary>
        /// 获得ftp上文件大小
        /// </summary>
        /// <param name="filename">相对根目录下的完整文件路径</param>
        /// <returns></returns>
        public long GetFileSize(string filename,out Exception ex)
        {
            long fileSize = 0;
            ex = null;
            filename = filename.Replace("\\", "/");
            try
            {
                // FileInfo fileInf = new FileInfo(filename);
                //string uri1 = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                // string uri = filename;
                string uri = "ftp://" + ftpServerIP + "/"+filename;
                Connect(uri);//连接      
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.Timeout = 10000;
                reqFTP.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                fileSize = response.ContentLength;
                response.Close();
            }
            catch (Exception newex)
            {
                ex = newex;
                return -1;
            }
            return fileSize;
        }
        #endregion

        #region ftp上文件改名
        /// <summary>
        /// ftp上文件改名
        /// </summary>
        /// <param name="currentFilename">相对根目录下的完整文件名</param>
        /// <param name="newFilename">新的文件名</param>
        public void Rename(string currentFilename, string newFilename,out string error)
        {
            try
            {
                error="";
                //FileInfo fileInf = new FileInfo(currentFilename);
                string uri = "ftp://" + ftpServerIP + "/" + currentFilename;
                Connect(uri);//连接
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.KeepAlive = false;
                reqFTP.Timeout = 10000;
                reqFTP.RenameTo = newFilename;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                //Stream ftpStream = response.GetResponseStream();
                //ftpStream.Close();
                response.Close();
                error="Succeed";
            }
            catch (Exception ex)
            {
                error=ex.Message;
            }
        }
        #endregion

        #region 获得文件明晰
        /// <summary>
        /// 获得文件明晰
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList(out string error)
        {
            return GetFileList("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectoryDetails,out error);
        }
        /// <summary>
        /// 获得文件明晰
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetFilesDetailList(string path,out string error)
        {
            path = path.Replace("\\", "/");
            return GetFileList("ftp://" + path, WebRequestMethods.Ftp.ListDirectoryDetails,out error);
        }
        #endregion

        #region 获得文件夹（不包含子目录）
        /// <summary>
        /// 获得指定的路径下的文件夹,Path根目录下路径
        /// </summary>
        /// <returns></returns>
        public string[] GetFloder(string Path,out string error)
        {
            string[] FileDetial;
            string[] Floder=null;//用于返回
            bool Isnull = false;
            StringBuilder result = new StringBuilder();
            try
            {
                error = "";
                if ("" == Path || null == Path)
                    FileDetial = GetFileList("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectoryDetails,out error);
                else
                    FileDetial = GetFileList("ftp://" + ftpServerIP + "/" + Path + "/", WebRequestMethods.Ftp.ListDirectoryDetails, out error);
                if (null != FileDetial || 0 > FileDetial.Length)
                {
                    foreach (string FileStr in FileDetial)
                    {
                        string File = FileStr.Trim();
                        string newfileinfo = File.Substring(0, 3);
                        if ("drw" == newfileinfo)
                        {
                            int strl = File.Length;
                            int indexm = File.LastIndexOf(':');
                            int start = indexm + 4;
                            int end = strl - start;
                            string floder = File.Substring(start, end);
                            if ("." == floder || ".." == floder)
                                continue;
                            else
                            {
                                result.Append(floder);
                                result.Append("\n");
                                Isnull = true;
                            }
                        }


                    }

                    if (Isnull)
                    {
                        result.Remove(result.ToString().LastIndexOf('\n'), 1);
                        Floder = result.ToString().Split('\n');
                    }


                }
                error = "Succeed";
            }
                
            catch (Exception ex)
            {
                error=ex.Message;
                Isnull = false;
            }

            if (true == Isnull)
                return Floder;
            else
                return null;
            
        }

        // *----------------------------------------------------------------------------------
        // *开 发 者：陈亚飞 
        // *开发时间：20110616
        // *功能函数：获得FTP的指定文件夹下面的所有目录以及子目录
        // *参    数：指定的文件夹目录，true（是否查找指定文件夹目录下的文件夹），异常
        // *------------------------------------------------------------------------------------
        public List<string> GetSubDirectory(string path, bool beGetSubDir, out Exception pError)
        {
            pError = null;
            string[] FileDetial;
            string error = "";
            List<string> LstDir = new List<string>();
            try
            {
                error = "";
                if (beGetSubDir)
                {
                    if ("" == path || null == path)
                        FileDetial = GetFileList("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectoryDetails, out error);
                    else
                        FileDetial = GetFileList("ftp://" + ftpServerIP + "/" + path + "/", WebRequestMethods.Ftp.ListDirectoryDetails, out error);
                    if (error != "Succeed")
                    {
                        pError = new Exception("获取文件目录列表失败");
                        return null;
                    }
                    if (null != FileDetial || 0 > FileDetial.Length)
                    {
                        foreach (string FileStr in FileDetial)
                        {
                            string File = FileStr.Trim();
                            string newfileinfo = File.Substring(0, 3);
                            if ("drw" == newfileinfo)
                            {
                                int strl = File.Length;
                                int indexm = File.LastIndexOf(':');
                                int start = indexm + 4;
                                int end = strl - start;
                                string floder = File.Substring(start, end);
                                if ("." == floder || ".." == floder)
                                    continue;
                                else
                                {
                                    if (path != "")
                                    {
                                        if (!LstDir.Contains(path + "/" + floder))
                                        {
                                            LstDir.Add(path + "/" + floder);
                                        }
                                    }
                                    else
                                    {
                                        if (!LstDir.Contains(floder))
                                        {
                                            LstDir.Add(floder);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (LstDir != null)
                {
                    if (LstDir.Count == 0)
                    {
                        beGetSubDir = false;
                    }
                    else
                    {
                        beGetSubDir = true;
                        int pCount = LstDir.Count;
                        for (int j = 0; j < pCount; j++)
                        {
                            string pSubDir = LstDir[j];
                            List<string> tempSubDirLst = new List<string>();
                            tempSubDirLst = GetSubDirectory(pSubDir, beGetSubDir, out pError);
                            if (pError != null)
                            {
                                pError = new Exception("获取文件目录列表失败");
                                return null;
                            }
                            if (tempSubDirLst != null)
                            {
                                if (tempSubDirLst.Count > 0)
                                {
                                    LstDir.AddRange(tempSubDirLst);
                                }
                            }
                        }
                    }
                }
               
                return LstDir;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
            
        }

        #endregion

        public void Close()
        {
            if (null != this.reqFTP)
            {
                reqFTP.Abort();
                reqFTP = null;
            }
        }
 
    }
}
