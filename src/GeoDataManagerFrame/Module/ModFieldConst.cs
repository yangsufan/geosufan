using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataManagerFrame
{
    class ModFieldConst
    {
        public static string g_KCBL = "KCBL";   //扣除比例
        public static string g_KCTBBH1 = "KCTBBH1";    //线状地物属性：扣除图斑编号1
        public static string g_KCTBBH2 = "KCTBBH2";    //扣除图斑编号2
        public static string g_KCTBDWDM1 = "KCTBDWDM1";  //扣除图斑单位代码1
        public static string g_KCTBDWDM2 = "KCTBDWDM2";  //扣除图斑单位代码2
        public static string g_DLBM = "DLBM"; //地类编码
        public static string g_DLMC = "DLMC";//地类名称
        public static string g_XZQHDM = "XZQHDM";//行政区划代码
        public static string g_XZQHMC = "XZQHMC";//行政区划名称 
        public static string g_TBBH = "TBBH";//图斑编号
        public static string g_MJ = "MJ";//面积----建议为变化图斑增加一个属性(面积)
        public static string g_PC = "PC";//批次
        public static string g_PCBH = "PCBH";//批次编号
        public static string g_SGTJPFWH = "SGTJPFWH";//市林业局批复文号
        public static string g_SGTTPFWH = "SGTTPFWH";//省林业厅批复文号
        public static string g_GWYPFWH = "GWYPFWH";//国务院批复文号
        public static string g_TDYTFQDM = "TDYTFQDM";//森林用途分区代码
        public static string g_TDYTFQBH = "TDYTFQBH";//森林用途分区编号
        public static string g_TBMJ = "TBMJ";//图斑面积
        public static string g_XZDWMJ = "XZDWMJ";//线状地物面积
        public static string g_LXDWMJ = "LXDWMJ";//零星地物面积
        public static string g_TBDLMJ = "TBDLMJ";//图斑地类面积
        public static string g_ZB = "ZB";//占比

        public static string g_YSDM = "YSDM";
        public static string g_DLDM = "DLDM";         //地类代码   在地类字典中使用
        //public static string g_DLBM = "DLBM";         //地类编码   在图斑面、线状地物、零星地中使用

        //public static string g_DLMC = "DLMC";     //added by chulili 2007-9-6
        public static string g_XH = "XH";             //序号
        public static string g_QSXZ = "QSXZ";
        public static string g_ZLMC = "ZLDWMC";
        public static string g_SZTF = "SZTFH";
        //public static string g_TBBH = "TBBH";
        public static string g_PDJB = "GDPDJ"; //"PDJB";
        public static string g_TKXS = "TKXS";         //在新的数据表中称 扣除地类系数
        public static string g_JSMJ = "JSMJ";
        public static string g_PCMJ = "TBMJ";
        public static string g_XZMJ = "XZDWMJ";       //线状地物面积   图斑和线通用
        public static string g_LXMJ = "LXDLMJ";
        public static string g_TKMJ = "TKMJ";
        public static string g_TBJMJ = "TBDLMJ";

        public static string g_DLJB = "DLJB";
        public static string g_SCBZ = "SCBZ";         //删除标志
        public static string g_XZJB = "XZJB";

        public static string g_KCXS = "KCBL";         //扣除比例  线状地物专用
        public static string g_ChangDu = "CD";
        public static string g_KuanDu = "KD";

        public static string g_KZMJ = "KZMJ";         //勘丈面积  零星地类专用

        public static string g_ZZRQ = "ZZRQ";
        public static string g_QSRQ = "QSRQ";
        public static string g_KCDLBM = "KCDLBM";     //扣除地类编码  //added by chulili 2008-3-27

        public static string g_oID = "OID";
        public static string g_Soid = "SOID";
        public static string g_Soid1 = "SOID1";
        public static string g_Soid2 = "SOID2";

        public static string g_QSDM = "QSDWDM";      //权属单位代码
        public static string g_QSDWMC = "QSDWMC";     //权属单位名称
        public static string g_ZLDM = "ZLDWDM";       //坐落单位代码
        public static string g_ZLDWMC = "ZLDWMC";     //坐落单位名称
        public static string g_BSM = "BSM";           //唯一的标识码   added by chulili 2008-3-6
        public static string g_FHDM = "FHDM";         //符号代码

        public static string g_QSXZtable = "DIC_QSXZ";        //权属性质表
        public static string g_DLZDtable = "DIC_DLZD";        //地类字典表
        public static string g_XZQBtable = "行政区字典表";    //行政区表
        public static string g_BGJLtable = "TDLY_CUR_BGJL";   //变更记录表

        public static string g_BaseStaticTable = "TDLY_CUR_JCTJB";//基础统计表
        public static string g_BaseCompuTable = "TDLY_CUR_JCJSB"; //基础计算表
        public static string g_VillageAreaTable = "TDLY_CUR_CMJB";        //村面积表
        public static string g_LandAdjustTable = "TZ_K";          //带K地类面积调整表

        public static string g_BGBASICTABLE = "TDLY_CUR_BGJCJSB";          //变更基础计算表
        public static string g_BGBASICTABLE_M = "TDLY_CUR_BGJCJSB_M";          //变更基础计算表(平方米)

        public static string g_QSDM1 = "QSDWDM1";         //线状地物的左权属单位代码
        public static string g_QSDM2 = "QSDWDM2";          //右
        public static string g_KCTBZLDM1 = "KCTBDWDM1";    //'线状地物的左图斑座落单位代码
        public static string g_KCTBZLDM2 = "KCTBDWDM2";    //右

        public static string g_GDLX = "GDLX";
        public static string g_XZQM = "NAME";
        public static string g_XZBM = "CODE";
        public static string g_SFZL = "SFZL";

        public static string g_XZQDM = "XZQDM";//added by chulili 20110921 行政区划层行政区代码属性
        public static string m_Fields = "Field_01,Field_011,Field_012,Field_013,Field_02,Field_021,Field_022,Field_023,Field_03,Field_031,Field_032,Field_033,"
        + "Field_04,Field_041,Field_042,Field_043,Field_20,Field_201,Field_202,Field_203,Field_204,Field_205,"
        + "Field_10,Field_101,Field_102,Field_104,Field_105,Field_106,Field_107,Field_11,Field_111,Field_112,"
        + "Field_113,Field_114,Field_115,Field_116,Field_117,Field_118,Field_119,Field_12,Field_122,"
        + "Field_123,Field_124,Field_125,Field_126,Field_127";

        public static string m_FieldsName_Access = "Field_01 double,Field_011 double,Field_012 double,Field_013 double,Field_02 double,Field_021 double,"
            + "Field_022 double,Field_023 double,Field_03 double,Field_031 double,Field_032 double,Field_033 double,Field_04 double,"
            + "Field_041 double,Field_042 double,Field_043 double,Field_20 double,Field_201 double,Field_202 double,Field_203 double,"
            + "Field_204 double,Field_205 double,Field_10 double,Field_101 double,Field_102 double,Field_104 double,Field_105 double,"
            + "Field_106 double,Field_107 double,Field_11 double,Field_111 double,Field_112 double,Field_113 double,Field_114 double,"
            + "Field_115 double,Field_116 double,Field_117 double,Field_118 double,Field_119 double,Field_12 double,Field_122 double,"
            + "Field_123 double,Field_124 double,Field_125 double,Field_126 double,Field_127 double";
        public static string m_InitFieldsValue = "Field_01=0,Field_011=0,Field_012=0,Field_013=0,Field_02=0,Field_021=0,Field_022=0,Field_023=0,Field_03=0,Field_031=0,Field_032=0,Field_033=0,"
                + "Field_04=0,Field_041=0,Field_042=0,Field_043=0,Field_20=0,Field_201=0,Field_202=0,Field_203=0,Field_204=0,Field_205=0,"
                + "Field_10=0,Field_101=0,Field_102=0,Field_104=0,Field_105=0,Field_106=0,Field_107=0,Field_11=0,Field_111=0,Field_112=0,"
                + "Field_113=0,Field_114=0,Field_115=0,Field_116=0,Field_117=0,Field_118=0,Field_119=0,Field_12=0,Field_122=0,"
                + "Field_123=0,Field_124=0,Field_125=0,Field_126=0,Field_127=0";
        public static string m_sum01 = "Field_011+Field_012+Field_013";
        public static string m_sum02 = "Field_021+Field_022+Field_023";
        public static string m_sum03 = "Field_031+Field_032+Field_033";
        public static string m_sum04 = "Field_041+Field_042+Field_043";
        public static string m_sum20 = "Field_201+Field_202+Field_203+Field_204+Field_205";
        public static string m_sum10 = "Field_101+Field_102+Field_104+Field_105+Field_106+Field_107";
        public static string m_sum11 = "Field_111+Field_112+Field_113+Field_114+Field_115+Field_116+Field_117+Field_118+Field_119";
        public static string m_sum12 = "Field_122+Field_123+Field_124+Field_125+Field_126+Field_127";
        public static string m_sumsum = "Field_01+field_02+field_03+Field_04+field_20+field_10+field_11+field_12";

    }
}
