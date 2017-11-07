using System;
using System.Collections.Generic;
using System.Text;

namespace SysCommon.Authorize
{
    /// <summary>
    /// ×Ô¶¨ÒåComboBoxItem
    /// </summary>
    public class ComboBoxItem
    {
        private string _text;
        private object _value;
        private object _tag;

        public string Text
        {
            set { _text = value; }
            get { return _text; }
        }

        public object Value
        {
            set { _value = value; }
            get { return _value; }
        }

        public object Tag
        {
            set { _tag = value; }
            get { return _tag; }
        }

        public ComboBoxItem(string text, object value)
        {
            _text = text;
            _value = value;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
