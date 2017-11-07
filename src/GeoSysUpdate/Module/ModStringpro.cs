using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
//ZQ   20110820    针对字符串进行处理
namespace GeoSysUpdate
{
    public class ModStringpro
    {
        ///// <summary>
        ///// 判断字符是否只含中文
        ///// </summary>
        ///// <param name="CString"></param>
        ///// <returns></returns>
        // public static bool IsChina(string CString)
        //{
        //    bool BoolValue = false;
        //    for (int i = 0; i < CString.Length; i++)
        //    {
        //        if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
        //        {
        //            BoolValue = false;
        //            break;
        //        }
        //        else
        //        {
        //            return BoolValue = true;
        //        }
        //    }
        //    return BoolValue;
        //}
        /// <summary>
        /// 判断字符是否只含数字和英文
        /// </summary>
        /// <param name="Cstring"></param>
        /// <returns></returns>
        public static bool IsMathEng(string Cstring)
        {
            bool BoolValue = false;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-z0-9]+$");
            if (regex.IsMatch(Cstring.Trim()))
            {
                BoolValue = true;
            }
            else
            {
                BoolValue = false;
            }
            return BoolValue;
        }
        /// <summary>
        /// 判断字符是否含中文
        /// </summary>
        /// <param name="CString"></param>
        /// <returns></returns>
        public static bool IsChina(string CString)
        {
            bool BoolValue = false;
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    BoolValue = false;

                }
                else
                {
                    return BoolValue = true;
                }
            }
            return BoolValue;
        }

        //public static bool
        /// <summary>
        /// 获取只含英文和数字的全文检索的ＳＱＬ语句
        /// </summary>
        /// <param name="CString"></param>
        /// <returns></returns>
        public static string GetSQLMathEng(string CString)
        {
            string StrSQL = "";
            if (CString.Contains(" "))
            {
                string[] StrKey = CString.Split(new char[] { ' ' });
                for (int i = 0; i < StrKey.Length; i++)
                {
                    if (StrKey[i].ToString() != "")
                    {
                        if (i == 0)
                        {
                            StrSQL = "%" + StrKey[i].ToString() + "%";
                        }
                        else
                        {
                            StrSQL = StrSQL + " AND %" + StrKey[i].ToString() + "%";
                        }
                    }

                }
            }
            else
            {
                StrSQL = "%" + CString + "%";
            }
            return StrSQL;

        }
        /// <summary>
        /// 获取只含中文字段的ＳＱＬ语句
        /// </summary>
        /// <param name="CString"></param>
        /// <returns></returns>
        public static string GetSQLChina(string CString)
        {
            string StrSQL = "";
            if (CString.Contains(" "))
            {
                string[] StrKey = CString.Split(new char[] { ' ' });
                for (int i = 0; i < StrKey.Length; i++)
                {
                    if (StrKey[i].ToString() != "")
                    {
                        if (i == 0)
                        {
                            StrSQL = StrKey[i].ToString();
                        }
                        else
                        {
                            StrSQL = StrSQL + " AND " + StrKey[i].ToString();
                        }
                    }
                }
            }
            else
            {
                StrSQL = CString;
            }
            return StrSQL;
        }
        //******************************文件夹操作********************************//
        /// <summary>          
        /// Copy文件夹          
        /// </summary>          
        /// <param name="sPath">源文件夹路径</param>          
        /// <param name="dPath">目的文件夹路径</param>          
        /// <returns>完成状态：success-完成；其他-报错</returns>          
        public static string CopyFolder(string sPath, string dPath, SysCommon.CProgress vProgress)
        {
            string flag = "success";
            try
            {
                // 创建目的文件夹                  
                if (!Directory.Exists(dPath))
                {
                    Directory.CreateDirectory
                   (dPath);
                }
                // 拷贝文件                  
                DirectoryInfo sDir = new DirectoryInfo(sPath);
                FileInfo[] fileArray = sDir.GetFiles();
                vProgress.ProgresssValue = 0;
                vProgress.MaxValue = fileArray.Length;
                vProgress.ProgresssValue = 0;
                vProgress.Step = 1;
                vProgress.ShowProgress();
                foreach (FileInfo file in fileArray)
                {
                    vProgress.SetProgress("正在导出：" + file.Name + "......");
                    vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
                    if (vProgress.UserAskCancel)
                    {
                        vProgress.Close();
                        break;
                    }
                    file.CopyTo(dPath + "\\" + file.Name, true);
                }
                // 循环子文件夹                  
                DirectoryInfo dDir = new DirectoryInfo(dPath);
                DirectoryInfo[] subDirArray = sDir.GetDirectories();
                foreach (DirectoryInfo subDir in subDirArray)
                {
                    CopyFolder(subDir.FullName, dPath + "//" + subDir.Name, vProgress);
                }
            }
            catch (Exception ex)
            {
                flag = ex.ToString();
            }
            return flag;
        }
        //判断当前节点下子节点是否存在相同的节点
        public static bool IsSameTreeNode(TreeNode rootNode, string strName, string strPath)
        {
            bool bIsSame = false;
            if (rootNode == null) { return bIsSame = true; }
            if (rootNode.Nodes.Count == 0) { return bIsSame; }
            for (int i = 0; i < rootNode.Nodes.Count; i++)
            {
                if (rootNode.Nodes[i].Text == strName)
                {
                    return bIsSame = true;
                }
                if ((rootNode.Nodes[i].Tag as Dictionary<string, string>)["Path"].ToString().Trim() == strPath)
                {
                    return bIsSame = true;
                }
            }
            return bIsSame;
        }
        /// <summary>
        /// 数据拷贝
        /// </summary>
        /// <param name="Src"></param>
        /// <param name="Dst"></param>
        public static bool copyDirectory(string Src, string Dst)
        {
            try
            {
                if (File.Exists(Src))
                    File.Copy(Src, Dst, true);
            }
            catch { return false; }
            return true;
        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcdir"></param>
        /// <param name="desdir"></param>
        public static void CopyDirectory(string srcdir, string desdir,SysCommon.CProgress vProgress)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            vProgress.ProgresssValue = 0;
            vProgress.MaxValue = filenames.Length;
            vProgress.ProgresssValue = 0;
            vProgress.Step = 1;
            vProgress.ShowProgress();
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {

                //    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                //    if (!Directory.Exists(currentdir))
                //    {
                //        Directory.CreateDirectory(currentdir);
                //    }

                //    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }


                    File.Copy(file, srcfileName);
                }
            }//foreach
        }//function end


        /// <summary>
        /// 清空指定的文件夹，删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            if (!Directory.Exists(dir))
            {
                if (File.Exists(dir))
                {
                    FileInfo fi = new FileInfo(dir);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    try
                    {
                        File.Delete(dir);//直接删除其中的文件
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                return;
            }
            try
            {
                Directory.Delete(dir, true);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
        /// <summary>
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteSubFolder(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                return;
            }
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    try
                    {
                        File.Delete(d);//直接删除其中的文件 
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    try
                    {
                        Directory.Delete(d);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
        }
    }
}
