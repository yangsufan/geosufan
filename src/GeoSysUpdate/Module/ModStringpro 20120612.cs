using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                for (int i = 0; i < StrKey.Length;i++ )
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
                StrSQL = CString ;
            }
            return StrSQL;
        }




    }
}
