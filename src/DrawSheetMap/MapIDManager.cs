
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;

namespace DrawSheetMap
{
    /// <summary>
    ///用于图号解析，用于图幅号转换和判断
    ///图幅名称:标准分幅的图幅名.500的如84-80-Ⅱ-Ⅱ-Ⅱ,2k的如84-80-Ⅱ
    ///图幅ID:与图幅名称对应的适于手工在程序中输入的符号串,如84-80-Ⅱ-Ⅱ-Ⅱ对应的图幅ID是84-80-2-2-2
    /// </summary>
    public class MapIDManager
    {
        public MapIDManager()
        {
            m_IDtoName = new List<string>() { "Ⅰ", "Ⅱ", "Ⅲ", "Ⅳ" };
            m_strConst = "ⅠⅡⅢⅣ";
        }
        private string m_strConst=string.Empty;
        private List<string> m_IDtoName;
        /// <summary>
        /// 根据图幅号获取地图比例尺
        /// </summary>
        /// <param name="strMapID">图幅号字符串，如84-80-2-2-2、84-80-2等</param>
        /// <returns></returns>
        public int GetScaleByMapID(string strMapID)
        {
            return 0;
        }
        /// <summary>
        /// 根据图幅名称获取比例尺
        /// </summary>
        /// <param name="strMapName">图幅名称</param>
        /// <returns></returns>
        public int GetScaleByMapName(string strMapName)
        {
            return 0;
        }
        /// <summary>
        ///  根据图幅号获取图幅名称,如输入84-80-2-2-2则返回84-80-Ⅱ-Ⅱ-Ⅱ,1万比例尺直接返回输入图幅号
        /// </summary>
        /// <param name="strMapId">图幅ID</param>
        /// <returns></returns>
        public string GetMapNammeString(string strMapId)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据地图名称获取地图ID
        /// </summary>
        /// <param name="strMapName"></param>
        /// <returns></returns>
        public string GetMapID(string strMapName)
        {
            return string.Empty;
        }
        /// <summary>
        /// 获取500图幅号所在的2000图幅号的名称
        /// </summary>
        /// <param name="str500MapID"></param>
        /// <returns></returns>
        public string Get2KMapIDBy500ID(string str500MapID)
        {
            return string.Empty;
        }
        /// <summary>
        /// 获取2000图幅号包含的500图幅号链表
        /// </summary>
        /// <param name="str2KMapID"></param>
        /// <param name="strArray500ID"></param>
        /// <returns></returns>
        public bool Get500MapIDArray(string str2KMapID, ref List<string> strArray500ID)
        {
            return false;
        }
        public void ComputerSubSheetOrigin(ref int XOrigin, ref int YOrigin, int iSubSheet, int iScale)
        {

        }
        /// <summary>
        /// 根据图幅ID(如86-67-1-1-3,106-084-1-1-2等)获取比例尺数据(500,2000)的图阔范围
        /// </summary>
        /// <param name="strSheetNo"></param>
        /// <param name="ipSpatial"></param>
        /// <returns></returns>
        public IGeometry GetGeometryByBigMapSheet(string strSheetNo, ISpatialReference ipSpatial = null)
        {
            return null;
        }
        public IGeometry GetGeometryByBigMapSheetFor500(string strSheetNo, ISpatialReference ipSpatial = null)
        {
            return null;
        }
        public IPolygon GetPolygonByBigMapSheet(string strSheetNo, ISpatialReference ipSpatial = null)
        {
            return null;
        }
        /// <summary>
        /// 判断500图幅号是否正确：格式如78-84-1-2-3或106-064-1-2-3，暂时不判断107-065-1-2-3类似不规范的命名
        /// </summary>
        /// <param name="strMapID500"></param>
        /// <returns></returns>
        public bool MapID500IsCorrect(string strMapID500)
        {
            return false;
        }
        /// <summary>
        /// 判断1000图幅号是否正确：格式如78-84-1-1或106-064-1-1，暂时不判断078-065-1-1类似不规范的命名
        /// </summary>
        /// <param name="strMapID1K"></param>
        /// <returns></returns>
        public bool MapID1KIsCorrect(string strMapID1K)
        {
            return false;
        }
        /// <summary>
        /// 判断2000图幅号是否正确：格式如78-84-1或106-064-1，暂时不判断078-065-1类似不规范的命名
        /// </summary>
        /// <param name="strMapID2K"></param>
        /// <returns></returns>
        public bool MapID2KIsCorrect(string strMapID2K)
        {
            return false;
        }
        /// <summary>
        /// 判断1万图幅号是否正确:各式如H48g001053或I48g096053
        /// </summary>
        /// <param name="strMapID10K"></param>
        /// <returns></returns>
        public bool MapID10KIsCorrect(string strMapID10K)
        {
            return false;
        }
        /// <summary>
        /// 检查500的图幅名称是否正确
        /// </summary>
        /// <param name="strMapName500"></param>
        /// <returns></returns>
        private bool MapName500IsCorrect(string strMapName500)
        {
            return false;
        }
        /// <summary>
        /// 检查2K的图幅名称是否正确
        /// </summary>
        /// <param name="strMapName2K"></param>
        /// <returns></returns>
        private bool MapName2KIsCorrect(string strMapName2K)
        {
            return false;
        }
        /// <summary>
        /// 检查10K的图幅名称是否正确
        /// </summary>
        /// <param name="strMapName10K"></param>
        /// <returns></returns>
        private bool MapName10KIsCorrect(string strMapName10K)
        {
            return false;
        }
        /// <summary>
        /// 根据500的图幅ID获取图幅名称
        /// </summary>
        /// <param name="strMapID500"></param>
        /// <returns></returns>
        private string Get500MapName(string strMapID500)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据2K的图幅ID获取图幅名称
        /// </summary>
        /// <param name="strMapID2K"></param>
        /// <returns></returns>
        private string Get2KMapName(string strMapID2K)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据10K的图幅ID获取图幅名称,目前不需要转换
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private string Get10KMapName(string strMapID10K)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据500的图幅名称获取图幅ID
        /// </summary>
        /// <param name="strMapName500"></param>
        /// <returns></returns>
        private string Get500MapIDByName(string strMapName500)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据2K的图幅名称获取图幅ID
        /// </summary>
        /// <param name="strMapName2K"></param>
        /// <returns></returns>
        private string Get2KMapIDByName(string strMapName2K)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据10K的图幅名称获取图幅ID
        /// </summary>
        /// <param name="strMapName10K"></param>
        /// <returns></returns>
        private string Get10KMapIDByName(string strMapName10K)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据图幅名称获取图幅ID,用于500和2K图幅号,替换标志图幅位置的阿拉伯数字为"Ⅰ"等希腊数字
        /// </summary>
        /// <param name="strMapName"></param>
        /// <returns></returns>
        private string GetMapIDByName(string strMapName)
        {
            return string.Empty;
        }
        /// <summary>
        /// 根据图幅ID获取图幅名称,用于500和2K图幅号,替换"Ⅰ"等希腊数字为阿拉伯数字
        /// </summary>
        /// <param name="strMapID"></param>
        /// <returns></returns>
        private string GetMapName(string strMapID)
        {
            return string.Empty;
        }
    }
}
