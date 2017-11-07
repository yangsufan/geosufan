/*******************************************************************/
/*                                                                 */
/*      Copyright (C) 2007 SafeNet, Inc.                           */
/*                      All Rights Reserved                        */
/*                                                                 */
/*******************************************************************/

/*C#.NET******************************************************************
* FILENAME    : LeaseDate.cs
* DESCRIPTION : 
*             Simple demonstration of Sentinel Keys 1.2.0 library calls.
*             
*             This is a fixed lifetime program. It expires on a fixed
*             date whether used or not. This program assumes that an AES
*             feature has been programmed into the key by toolkit and the
*             lease expiration date has been specified in it. A periodic 
*             query is made to verify that hardware key is present and 
*             response from the key is valid otherwise the application 
*             is terminated. 
*C#.NET*/

using System;
using System.Windows.Forms;

namespace GeoDatabaseManager
{

    struct ResultKey_Item
    {
        long  ResultKey_Id;      // 配置项标识ID NOT NULL Range[2000-8000]
        //char * ResultKey_Info;  // 配置项处理过程描述信息 NOT NULL	
    };
    //ResultKey_Item  ResultKey_Items[]=
    //{
    //    { 201,   " 一个或多个输入参数错误。请验证输入的值。" },
    //    { 202,   " 发生下列任何情况：软件密钥格式错误。可以在您的模板头文件中找到相应的软件密钥。该软件密钥用于其它的加密狗。" },
    //    { 203,   " 无法找到许可证ID对应的许可证。可以在您的模板头文件中找到此许可证ID。无效许可证句柄。许可证超时。" },
    //    { 204,   " 指定的特征项ID错误。" },
    //    { 205,   " 无法找到所需要的加密狗。确保发送的是正确的开发商ID。" },
    //    { 206,   " 加密狗为空－－不包含请求的许可证。" },
    //    { 207,   " 缓冲区的大小无法容纳期望获取的数据。请分配足够的内存或限制数据以匹配缓冲区的大小。" },
    //    { 208,   " 发生下列任何情况：用于验证的公钥不正确。签名内容被篡改。" },
    //    { 209,   " 无法与硬件狗通讯，原因可能是未正确安装或加载加密狗驱动程序。" },
    //    { 210,   " 您试图对特征项执行非法操作。例如，您可能尝试读取AES或ECC特征项或者写只读特征项。" },
    //    { 211,   " 无法与硬件狗通讯。请验证硬件狗是否正确连接。您可以在拔出硬件狗后，重新插入硬件狗。" },
    //    { 212,   " 无法与加密狗通讯。请确认：加密狗驱动程序已安装并正在运行。客户端和加密狗服务器使用同样的协议。加密狗正确连接。 不存在与网络相关的故障（例如，	网络拥塞或崩溃）。" },
    //    { 213,   " 您试图递减已经为零值的计数器。" },
    //    { 214,   " 加密狗存储区损坏。" },
    //    { 215,   " 该特征项不支持您尝试使用的某些功能。要了解各个特征项的功能，请参阅主题特征项属性。" },
    //    { 216,   " 加密狗驱动程序当前处于忙碌状态。请重试。" },
    //    { 217,   " 客户端库的版本较低。需要升级客户端库的版本。" },
    //    { 218,   " 操作超时。请再次尝试该操作。" },
    //    { 219,   " 数字签名或被加密的数据包无效。" },
    //    { 220,   " 特征项未激活。" },
    //    { 221,   " 特征项无法使用此API函数。可能在创建特征项时，没有选择相应的特征项属性。例如，在添加AES特征项时没有选择加密属性的情况下调用SFNTEncrypt。" },
    //    { 222,   " 加密狗已被拔出。请重新插入并再次调用函数。" },
    //    { 223,   " 对于非RTC硬件狗，系统时钟已被破坏或时间闸计数器已达到零值。对于RTC硬件狗，RTC时间已被篡改。" },
    //    { 224,   " 在API函数中指定了一个无效命令。" },
    //    { 225,   " 不能完成请求。可用资源（如内存）不足。" },
    //    { 226,   " 没有找到指定的加密狗。" },
    //    { 227,   " 应用程序/特征项的使用次数已失效。 " },
    //    { 228,   " 查询数据超过112个字节。请发送更短的查询。" },
    //    { 301,   " 应用程序尝试联系的加密狗服务器尚未运行。" },
    //    { 302,   " 指定的加密狗服务器名称不存在。如果您正在使用IP/IPX协议，请尝试使用IP/IPX地址代替系统主机名称。" },
    //    { 303,   " 客户端不能解析来自加密狗服务器的信息。" },
    //    { 304,   " 硬限制或用户数限制已用尽。" },
    //    { 305,   " 调用了不支持的API函数。" },
    //    { 306,   " 遇到内部错误。" },
    //    { 307,   " 未安装网络协议。" },
    //    { 308,   " 加密狗服务器不能解析客户端信息。" },
    //    { 309,   " 在套接字操作中遇到错误（例如套接字初始化失败）。" },
    //    { 310,   " 子网中没有加密狗服务器返回响应。本错误码由SFNTEnumServer API函数返回。" }
    //};
    public class GeoOneKey
    {
        public static SentinelKey oSentinelKeys = new SentinelKey(); //Object of SentinelKey class
        public static SentinelKeysLicense oSentinelKeysLicense = new SentinelKeysLicense(); //Object of SentinelKeysLicense class
        //public static SentinelKey.FeatureInfo featureinfo = new SentinelKey.FeatureInfo(); //Object of SentinelKey.FeatureInfo class

            
        public int addkey()
        {
            uint devID = 0; //Developer ID
            uint status;
            uint statusnet;
            //这里分单机狗、网络狗进行不同设置 SP_STANDALONE_MODE 、SP_SERVER_MODE
            uint flag = SentinelKey.SP_STANDALONE_MODE;
            uint nflag = SentinelKey.SP_SERVER_MODE; 
            byte[] queryResp; //Response returned by query API
            uint qrySize;

            devID = (uint)SentinelKeysLicense.DEVELOPERID;
            status = oSentinelKeys.SFNTGetLicense(devID, oSentinelKeysLicense.SOFTWARE_KEY, SentinelKeysLicense.LICENSEID, flag);
            if (status != 0)                
            {
                statusnet = oSentinelKeys.SFNTGetLicense(devID, oSentinelKeysLicense.SOFTWARE_KEY, SentinelKeysLicense.LICENSEID, nflag);
                if (statusnet != 0)
                {
                    MessageBox.Show("插入GeoOne加密狗！错误代码：" + status.ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Console.WriteLine("    GetLicense failed! Return code is " + Convert.ToString(status) + ".");
                    //Console.WriteLine("    Press Enter to Exit...");
                    //Console.Read();
                    return -1;
                }
            }


            Console.WriteLine();

            //qrySize indicates the length of query 
            qrySize = SentinelKeysLicense.SP_LEN_OF_QR;

            byte[] queryArr = new Byte[qrySize];
            queryResp = new byte[qrySize];
            int cnt = 0;
            Random r=new Random();
            cnt = r.Next( 1, 500);
           
            for (int k = 0; k < qrySize; k++)
                queryArr[k] = oSentinelKeysLicense.QUERY_SP_AESALGO_1_SP_CHECK_DEMO[cnt, k];

            //下面可以用于判断圣天狗是否过期，或者圣天狗是否在本机或网络中，可根据情况在主要方法中调用            
            status = oSentinelKeys.SFNTQueryFeature(SentinelKeysLicense.SP_AESALGO_1, SentinelKey.SP_CHECK_DEMO, queryArr, qrySize, queryResp, qrySize);
            if (status != 0)
            {
                MessageBox.Show("插入GeoOne加密狗！错误代码：" + status.ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                oSentinelKeys.SFNTReleaseLicense();               
                return -1;
            }

            /* Now check whether the response returned is a valid one, 
            * by comparing with the response table given in SentinelKeysLicense.cs. */
            for (int q = 0; q < qrySize; q++)
            {
                if (queryResp[q] != oSentinelKeysLicense.RESPONSE_SP_AESALGO_1_SP_CHECK_DEMO[cnt, q])
                {
                    MessageBox.Show("插入正确的GeoOne加密狗！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Console.WriteLine("    Invalid Query Response!.");
                    oSentinelKeys.SFNTReleaseLicense();
                    Console.WriteLine("    Press Enter to Exit.....");
                    Console.Read();
                    return -1;
                }
            }
            //ReleaseLicense called before terminating the program        
            oSentinelKeys.SFNTReleaseLicense();              
            return 0;
        }
    }
}
