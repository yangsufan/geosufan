using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

//针对数据库的读写操作
namespace GeoDataCenterFunLib
{
    public class GeoDataCenterDbFun
    {
        //------------------------------------------
        //功能：根据表达式获取access表中的内容
        //参数说明
        //      strCon      连接表达式
        //      strExp      查询表达式
        //返回：对应表格中的属性值
        //时间：2011-2-12
        //实现：
        //-------------------------------------------
        public string GetInfoFromMdbByExp(string strCon,string strExp)
        {
            string strValue = "";
            OleDbConnection mycon = new OleDbConnection(strCon);   //定义OleDbConnection对象实例并连接数据库
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();

                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();
                if (aReader.Read())
                {
                    strValue = aReader[0].ToString();
                }

                //关闭reader对象     
                aReader.Close();

                //关闭连接,这很重要     
                mycon.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return strValue;
        }

        //------------------------------------------
        //功能：根据表达式删除或者更新access表中的内容
        //参数说明
        //      strCon      连接表达式
        //      strExp      SQL表达式
        //作者：席胜
        //时间：2011-03-10
        //实现：
        //-------------------------------------------
        public void ExcuteSqlFromMdb(string strCon, string strExp)
        {
             
            OleDbConnection mycon = new OleDbConnection(strCon);   //定义OleDbConnection对象实例并连接数据库
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
           
                mycon.Open();
                aCommand.ExecuteNonQuery();
                //关闭连接,这很重要     
                mycon.Close();
            
            //catch (System.Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            
           
        }

        //------------------------------------------
        //功能：根据表达式产寻access表中的内容
        //参数说明
        //      strCon      连接表达式
        //      strExp      SQL表达式
        //作者：席胜
        //时间：2011-03-10
        //实现：
        //-------------------------------------------
        public int ExcuteSqlFromMdbEx(string strCon, string strExp)
        {

            OleDbConnection mycon = new OleDbConnection(strCon);   //定义OleDbConnection对象实例并连接数据库
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            int ii = -1;
            mycon.Open();
            OleDbDataReader aReader = aCommand.ExecuteReader();
            if (aReader.Read() == true)
            {
                ii = 1;
            }
            //关闭连接,这很重要     
            mycon.Close();

            return ii;
        }

        //------------------------------------------
        //功能：根据表达式得到Access表中的id
        //参数说明
        //      strCon      连接表达式
        //      strExp      SQL表达式
        //作者：席胜
        //时间：2011-03-10
        //实现：
        //-------------------------------------------
        public int GetIDFromMdb(string strCon, string strExp)
        {
            int i=0;
            OleDbConnection mycon = new OleDbConnection(strCon);   //定义OleDbConnection对象实例并连接数据库
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            { 
                mycon.Open();

                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();
                if (aReader.Read())
                {
                     i = Convert.ToInt32(aReader[0].ToString());
                }

                //关闭reader对象     
                aReader.Close();

                //关闭连接,这很重要     
                mycon.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return i;
            
           
        }

        //------------------------------------------
        //功能：根据表达式得到Access表中的满足的行数
        //参数说明
        //      strCon      连接表达式
        //      strExp      SQL表达式
        //作者：席胜
        //时间：2011-03-10
        //实现：
        //-------------------------------------------
        public int GetCountFromMdb(string strCon, string strExp)
        {
            int i=0 ;
            OleDbConnection mycon = new OleDbConnection(strCon);   //定义OleDbConnection对象实例并连接数据库
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();
                i=Convert.ToInt32(aCommand.ExecuteScalar());
                //关闭连接,这很重要     
                mycon.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return i;
        }
        
        //------------------------------------------
        //功能：根据传递的值更新地图入库信息表
        //作者：席胜
        //时间：2011-03-10
        //实现：
        //-------------------------------------------
        public void UpdateMdbInfoTable(string strBusness, string strYear, string strType, string strArea, string strScale)
        {
            string Layers="";
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "";
            strExp= string.Format("select 图层代码 from 数据编码表 where 业务大类代码='{0}' and 年度='{1}' and 行政代码='{2}' and 比例尺='{3}'",
                strBusness, strYear, strArea, strScale);
            DataTable dt1= GetDataTableFromMdb(strCon, strExp);
            strExp = "select * from 标准专题信息表";
            DataTable dt2 = GetDataTableFromMdb(strCon, strExp);
            for (int i = 0; i < dt2.Rows.Count; i++)//第一层循环标准图层
            {
                Layers = "";
                for (int j = 0; j < dt1.Rows.Count; j++)//第二层循环循环判断入库图层代码中有没有是关键图层的
                {
                    
                    if (dt2.Rows[i]["关键图层"].ToString().Equals("")||dt1.Rows[j][0].Equals(dt2.Rows[i]["关键图层"]))//若是关键图层
                    {
                        for (int k = 0; k < dt1.Rows.Count;k++)//第三层循环入库图层中包含在该专题图层组成中的
                        {
                             if(GetExists(dt1.Rows[k][0].ToString(),dt2.Rows[i]["图层组成"].ToString()))
                                Layers += dt1.Rows[k][0].ToString()+"/";
                        }
                        j =  dt1.Rows.Count;//终止第二层循环
                    }
                }
                if (Layers != "")
                {
                    Layers = Layers.Substring(0, Layers.LastIndexOf("/"));
                }
                    //判断地图入库信息表 中是否存在该记录 存在就更新 不存在就填写
                    strExp = string.Format("select * from 地图入库信息表 where 专题类型='{0}' and 年度='{1}' and 行政代码='{2}' and 比例尺='{3}'",dt2.Rows[i]["专题类型"].ToString(), strYear,strArea,strScale);
                    int iReturn = ExcuteSqlFromMdbEx(strCon, strExp);
                    if (iReturn == -1)
                    {
                        string strBuffer = "select 行政名称 from 数据单元表 where 行政代码 ='" + strArea + "'";
                        string strXzName = GetInfoFromMdbByExp(strCon, strBuffer);

                        strExp = string.Format("insert into 地图入库信息表(专题类型,年度,行政代码,行政名称,比例尺,图层组成) values('{0}','{1}','{2}','{3}','{4}','{5}')", dt2.Rows[i]["专题类型"].ToString(), strYear, strArea, strXzName, strScale, Layers);
                    }
                    else
                    {
                        strExp = "update 地图入库信息表 set 图层组成='" + Layers + "' where 专题类型='" + dt2.Rows[i]["专题类型"].ToString() + "' And 行政代码 ='" + strArea + "' And 年度='" +
                   strYear + "'And  比例尺='" + strScale + "'";
                    }
                
                  ExcuteSqlFromMdb(strCon, strExp);//执行更新
            }
             
           

        }
        //检测图层组成中是否包含图层
        private bool GetExists(string layer, string layers)
        {
            bool exist = false;
            if (layers.Contains("/"))
            {
                string[] arrlayer = layers.Split('/');
                for (int ii = 0; ii < arrlayer.Length; ii++)
                {
                    if (arrlayer[ii].Trim() == layer)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            else
            {
                if (layers.Trim() == layer)
                    exist = true;
            }
            return exist;
        }

        public List<string> GetDataReaderFromMdb(string strCon, string strExp)
        {  
            List<string> readlist = new List<string>();
            try
            {
                OleDbConnection conn = new OleDbConnection(strCon);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(strExp, conn);
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    readlist.Add(reader[0].ToString());
                }

                reader.Close();
                conn.Close();
            }
            catch { }
            return readlist;
        }
        public DataTable GetDataTableFromMdb(string strCon, string strExp)
        {
            OleDbConnection conn = new OleDbConnection(strCon);
            conn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter(strExp, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

    }
}
