using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace GeoCustomExport
{
    class ListViewColumnSorter : System.Collections.IComparer
    {
        private int ColumnToSort;

        // SortOrder 是一个列举型别。
        private SortOrder OrderOfSort;
        private CaseInsensitiveComparer ObjectCompare;

        public ListViewColumnSorter()
        {
            // 将资料行初始化成 0。
            ColumnToSort = 0;

            // 将排序顺序初始化成 None 。
            OrderOfSort = SortOrder.None;

            // 初始化 CaseInsensitiveComparer 物件。
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int SortColumn
        {
            get
            {
                return ColumnToSort;
            }

            set
            {
                ColumnToSort = value;
            }
        }

        public SortOrder Order
        {
            get
            {
                return OrderOfSort;
            }

            set
            {
                OrderOfSort = value;
            }
        }

        #region IComparer 成员
        int IComparer.Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX;
            ListViewItem listviewY;

            // 将要被比较的对象转换成 ListViewItem 对象。
            listviewX = (ListViewItem)(x);
            listviewY = (ListViewItem)(y);

            // 比较两个项目。
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            // 将比较结果传回。
            if (OrderOfSort == SortOrder.Ascending)
            {
                // 递增排序被选取，传回比较操作的典型结果。
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // 递减排序被选取，传回比较操作之结果的负值。
                return (-compareResult);
            }
            else
            {
                // 如果两者相等则传回 0 。
                return 0;
            }
        }
        #endregion
    }
}
