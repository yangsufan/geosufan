using System;
using System.Data;
using System.Collections;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;



namespace GeoPageLayout
{
    public class Maps : IMaps, IDisposable
    {
      
        private ArrayList m_array = null;

        #region class constructor
        public Maps()
        {
            m_array = new ArrayList();
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (m_array != null)
            {
                m_array.Clear();
                m_array = null;
            }
        }

        #endregion

        #region IMaps Members

        public void RemoveAt(int Index)
        {
            if (Index > m_array.Count || Index < 0)
                throw new Exception("Maps::RemoveAt:\r\nIndex is out of range!");

            m_array.RemoveAt(Index);
        }

        public void Reset()
        {
            m_array.Clear();
        }


        public int Count
        {
            get
            {
                return m_array.Count;
            }
        }


        public IMap get_Item(int Index)
        {
            if (Index > m_array.Count || Index < 0)
                throw new Exception("Maps::get_Item:\r\nIndex is out of range!");

            return m_array[Index] as IMap;
        }

 
        public void Remove(IMap Map)
        {
            m_array.Remove(Map);
        }


        public IMap Create()
        {
            IMap newMap = new MapClass();
            m_array.Add(newMap);

            return newMap;
        }


        public void Add(IMap Map)
        {
            if (Map == null)
                throw new Exception("Maps::Add:\r\nNew Map is mot initialized!");

            m_array.Add(Map);
        }

        #endregion
    }
}
