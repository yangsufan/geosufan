using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Fan.Common
{
    public static class XmlOperation
    {
        public static double GetDouble(XmlNode xmlnode, double defaultValue, string attriName, bool IsAllowZero)
        {
            if (xmlnode == null) return defaultValue;
            if (xmlnode.Attributes[attriName] == null) return defaultValue;
            if (xmlnode.Attributes[attriName].Value == "0")
            {
                if (IsAllowZero) return 0;
                else return defaultValue;
            }

            try
            {
                double value = Convert.ToDouble(xmlnode.Attributes[attriName].Value);
                return value;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public static string GetString(XmlNode xmlnode, string defaultValue, string attriName, bool IsAllowEmpty)
        {
            if (xmlnode == null) return defaultValue;
            if (xmlnode.Attributes[attriName] == null) return defaultValue;
            if (xmlnode.Attributes[attriName].Value == "")
            {
                if (IsAllowEmpty) return "";
                else return defaultValue;
            }

            return xmlnode.Attributes[attriName].Value;
        }

        public static int GetInteger(XmlNode xmlnode, int defaultValue, string attriName, bool IsAllowZero)
        {
            if (xmlnode == null) return defaultValue;
            if (xmlnode.Attributes[attriName] == null) return defaultValue;
            if (xmlnode.Attributes[attriName].Value == "0")
            {
                if (IsAllowZero) return 0;
                else return defaultValue;
            }

            try
            {
                int value = Convert.ToInt32(xmlnode.Attributes[attriName].Value);
                return value;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }

        public static short GetShort(XmlNode xmlnode, int defaultValue, string attriName, bool IsAllowZero)
        {
            if (xmlnode == null) return (short)defaultValue;
            if (xmlnode.Attributes[attriName] == null) return (short)defaultValue;
            if (xmlnode.Attributes[attriName].Value == "0")
            {
                if (IsAllowZero) return 0;
                else return (short)defaultValue;
            }

            try
            {
                int value = Convert.ToInt32(xmlnode.Attributes[attriName].Value);
                return (short)value;
            }
            catch (Exception ex)
            {
                return (short)defaultValue;
            }
        }

        public static bool GetBoolean(XmlNode xmlnode, bool defaultValue, string attriName)
        {
            if (xmlnode == null) return defaultValue;
            if (xmlnode.Attributes[attriName] == null) return defaultValue;

            if (xmlnode.Attributes[attriName].Value.ToLower() == "true" ||
                xmlnode.Attributes[attriName].Value == "1")
                return true;
            else
                return false;
        }

        public static int GetEnumValue(XmlNode xmlnode, int defaultValue, string attriName, Type enumType)
        {
            if (xmlnode == null) return defaultValue;
            if (xmlnode.Attributes[attriName] == null) return defaultValue;

            try
            {
                int value = (int)System.Enum.Parse(enumType, xmlnode.Attributes[attriName].Value, false);
                return value;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }
    }

    public enum enumKeyConstant { LEFT = 100, RIGHT = 102, DOWN = 98, UP = 104 };
}
