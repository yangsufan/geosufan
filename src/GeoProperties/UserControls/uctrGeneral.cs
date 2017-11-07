using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ESRI.ArcGIS.Carto;

namespace GeoProperties.UserControls
{
    public partial class uctrGeneral : UserControl
    {
        private ILayer m_pLayer;
        //private XmlDocument m_pXmlDoc;
        private bool m_layerVisible;
        private ILayer m_pCurrentLayer;
        private double m_iMax;
        private double m_iMin;
        /// <summary>
        /// ZQ  20111119 add
        /// </summary>
        string m_LayerDescription = "";
        public uctrGeneral(ILayer pLayer)
        {
            m_pLayer = pLayer;
            m_pCurrentLayer = pLayer;
            //m_pXmlDoc = xmlDoc;
            m_layerVisible = m_pLayer.Visible;
            m_iMax = m_pCurrentLayer.MaximumScale;
            m_iMin = m_pCurrentLayer.MinimumScale;

            InitializeComponent();
            InitializeComBox();
        }

        private void InitializeComBox()
        {
            object[] scaleItems = new object[14];
            scaleItems[0] = "<空>";
            scaleItems[1] = "1:500";
            scaleItems[2] = "1:1000";
            scaleItems[3] = "1:10000";
            scaleItems[4] = "1:50000";
            scaleItems[5] = "1:100000";
            scaleItems[6] = "1:250000";
            scaleItems[7] = "1:500000";
            scaleItems[8] = "1:750000";
            scaleItems[9] = "1:1000000";
            scaleItems[10] = "1:2500000";
            scaleItems[11] = "1:5000000";
            scaleItems[12] = "1:7500000";
            scaleItems[13] = "1:10000000";
            cboMaxScale.Items.AddRange(scaleItems);
            cboMinScale.Items.AddRange(scaleItems);

            cboMaxScale.SelectedItem = cboMaxScale.Items[GetSelectedIndex(m_iMax)];

            cboMinScale.SelectedItem = cboMinScale.Items[GetSelectedIndex(m_iMin)];
        }

        //设置全局变量保存图层NodeKey
        string m_nodeKey = "";
        private void uctrGeneral_Load_1(object sender, EventArgs e)
        {
           //xisheng 20111117 屏蔽掉判断是不是影像，不明白之前为什么要这样处理*******************//
            //if (frmLayerProperties.m_featureTrue)
            //{
              txtLayerName.Text = m_pLayer.Name;
            //}
            //else
            //{
            //    txtLayerName.Text = "影像数据";
            //}
              //xisheng 20111117 屏蔽掉判断是不是影像，不明白之前为什么要这样处理end*************//
            
            //获得图层NodeKey xisheng 20111117****************************************************
              ILayerGeneralProperties pLayerGenPro = m_pLayer as ILayerGeneralProperties;
              txtDescription.Text = pLayerGenPro.LayerDescription;
              if (pLayerGenPro.LayerDescription != "")
              {
                  XmlDocument xmldoc = new XmlDocument();
                  xmldoc.LoadXml(pLayerGenPro.LayerDescription);
                  XmlNode xmlNode = xmldoc.SelectSingleNode(@"//Layer");
                  m_nodeKey = (xmlNode as XmlElement).GetAttribute("NodeKey");
                  //获得图层NodeKey xisheng 20111117**********************************************end

                  m_LayerDescription = "图层名称：" + (xmlNode as XmlElement).GetAttribute("NodeText") + "；\r\n";
                  m_LayerDescription += "要素集名称：" + (xmlNode as XmlElement).GetAttribute("FeatureDatasetName") + "；\r\n";
                  switch ((xmlNode as XmlElement).GetAttribute("DataType"))
                  {
                      case "FC":
                          m_LayerDescription += "要素集类型：FeatureClass；\r\n";
                          break;
                      case "FD":
                          m_LayerDescription += "要素集类型：FeatureDataset；\r\n";
                          break;
                      case "RC":
                          m_LayerDescription += "要素集类型：RasterClasss；\r\n";
                          break;
                      case "RD":
                          m_LayerDescription += "要素集类型：RasterDataset；\r\n";
                          break;
                  }
                  m_LayerDescription += "要素类名称：" + (xmlNode as XmlElement).GetAttribute("Code") + "；\r\n";
                  m_LayerDescription += "要素类类型：" + (xmlNode as XmlElement).GetAttribute("FeatureType") + "；";
              }
            List<string> list=new List<string>();
            chkLayerVisible.Checked = m_layerVisible;
            if (m_pCurrentLayer.MaximumScale == 0 && m_pCurrentLayer.MinimumScale == 0)
            {
                rbtnAllScale.Checked = true;
                cboMaxScale.Enabled = false;
                cboMinScale.Enabled = false;

                //等于上次的比例尺 xisheng 20111117
                if (Plugin.Mod.m_Dic.ContainsKey(m_nodeKey))
                {
                    list = Plugin.Mod.m_Dic[m_nodeKey];
                    cboMaxScale.Text = list[1];
                    cboMinScale.Text = list[0];
                }
                //等于上次的比例尺 xisheng 20111117 end
            }
            else
            {
                rbtnRangeScale.Checked = true;
                cboMaxScale.Enabled = true;
                cboMinScale.Enabled = true;

                //shduan add 20110720
                cboMaxScale.Text = m_pCurrentLayer.MaximumScale.ToString();
                cboMinScale.Text = m_pCurrentLayer.MinimumScale.ToString();
           }
            txtDescription.Text = m_LayerDescription;
            ILayerEffects pLyrEffects = m_pCurrentLayer as ILayerEffects;
            this.inPutTrans.Value = pLyrEffects.Transparency;
        }

        private void chkLayerVisible_CheckedChanged(object sender, EventArgs e)
        {
            m_layerVisible = chkLayerVisible.Checked;
        }

        public void SaveChangeResult()
        {
            InterposeLayerVisible(m_layerVisible);
            InterposeLayerDescription();
            InterposeLayerScale();
            //if(frmLayerProperties.m_featureTrue) xisheng 20111117 屏蔽掉图层名修改
            m_pLayer.Name = txtLayerName.Text;

            //定义透明度
            ILayerEffects pLyrEffects = m_pCurrentLayer as ILayerEffects;
            pLyrEffects.Transparency = (short)this.inPutTrans.Value;
            //xisheng 20110729 透明度设置
             frmLayerProperties.m_shorttra= pLyrEffects.Transparency;

        }

        private void InterposeLayerVisible(bool visible)
        {
            if (visible == true)
            {
                m_pLayer.Visible = true;
            }
            else
            {
                m_pLayer.Visible = false;
            }
        }

        private void InterposeLayerDescription()
        {
            //string strDescription = txtDescription.Text;
            //ILayerGeneralProperties pLayerGenPro = m_pLayer as ILayerGeneralProperties;
            //pLayerGenPro.LayerDescription = strDescription;
        }

        private void InterposeLayerScale()
        {
            if (rbtnAllScale.Checked == true)
            {
                m_pCurrentLayer.MaximumScale = 0;
                m_pCurrentLayer.MinimumScale = 0;

                //保存上次的比例尺设置 xisheng 20111117******************
                if (Plugin.Mod.m_Dic.ContainsKey(m_nodeKey))
                {
                    Plugin.Mod.m_Dic.Remove(m_nodeKey);
                }
                List<string> list = new List<string>();
                list.Add(cboMinScale.Text);
                list.Add(cboMaxScale.Text);
                Plugin.Mod.m_Dic.Add(m_nodeKey, list);
                //*****************************************************end
            }
            else
            {
                // shduan 20110720
                double maxScale = GetScale(cboMaxScale.Text);
                double minScale = GetScale(cboMinScale.Text);
                m_pCurrentLayer.MaximumScale = maxScale;
                m_pCurrentLayer.MinimumScale = minScale;
            }
        }

        private double GetScale(object value)
        {
            double scale = 0;
            string strScale = (string)value;
            if (strScale.Contains(":"))
            {
                strScale = strScale.Substring(strScale.IndexOf(":") + 1);
            }
            try
            {
                scale=Convert.ToDouble(strScale);
            }
            catch(Exception err)
            {
            }

            //switch ((string)value)
            //{
            //    case "1:500":
            //        scale = 500;
            //        break;
            //    case "1:1000":
            //        scale = 1000;
            //        break;
            //    case "1:10000":
            //        scale = 10000;
            //        break;
            //    case "1:50000":
            //        scale = 50000;
            //        break;
            //    case "1:100000":
            //        scale = 100000;
            //        break;
            //    case "1:250000":
            //        scale = 250000;
            //        break;
            //    case "1:500000":
            //        scale = 500000;
            //        break;
            //    case "1:750000":
            //        scale = 750000;
            //        break;
            //    case "1:1000000":
            //        scale = 1000000;
            //        break;
            //    case "1:2500000":
            //        scale = 2500000;
            //        break;
            //    case "1:5000000":
            //        scale = 5000000;
            //        break;
            //    case "1:7500000":
            //        scale = 7500000;
            //        break;
            //    case "1:10000000":
            //        scale = 10000000;
            //        break;
            //    default:
            //        scale = 0;
            //        break;
            //}
            return scale;
        }

        private int GetSelectedIndex(double scale)
        {
            int index = 0;
            switch (Convert.ToInt32(scale))
            {
                case 500:
                    index = 1;
                    break;
                case 1000:
                    index = 2;
                    break;
                case 10000:
                    index = 3;
                    break;
                case 50000:
                    index = 4;
                    break;
                case 100000:
                    index = 5;
                    break;
                case 250000:
                    index = 6;
                    break;
                case 500000:
                    index = 7;
                    break;
                case 750000:
                    index = 8;
                    break;
                case 1000000:
                    index = 9;
                    break;
                case 2500000:
                    index = 10;
                    break;
                case 5000000:
                    index = 11;
                    break;
                case 7500000:
                    index = 12;
                    break;
                case 10000000:
                    index = 13;
                    break;
                default:
                    index = 0;
                    break;
            }
            return index;
        }

        private void rbtnRangeScale_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbtnRangeScale.Checked == true)
            {
                cboMaxScale.Enabled = true;
                cboMinScale.Enabled = true;
            }
            else
            {
                cboMaxScale.Enabled = false;
                cboMinScale.Enabled = false;
            }
        }

        private void cboMinScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            //m_iMax = cboMinScale.SelectedIndex;
            cboMinScale.SelectedItem = cboMinScale.Items[cboMinScale.SelectedIndex];
        }

        private void cboMaxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboMaxScale.SelectedItem = cboMaxScale.Items[cboMaxScale.SelectedIndex];
        }

        private void inPutTrans_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar=='.')  e.Handled=true;
           
        }

       //当滚动条值变化时 
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            inPutTrans.Value = Convert.ToDecimal(trackBar.Value);
        }

        //值变事件更改 xisheng 20111117 *************************************
        private int iValueLast = 0;
        private void inPutTrans_ValueChanged(object sender, EventArgs e)
        {

            try
            {

                int iValue = Convert.ToInt32(inPutTrans.Value) > 100 ? 100 : Convert.ToInt32(inPutTrans.Value);
                iValueLast = iValue;//记录上次  20110802 xisheng

                if (iValue < 0)
                {
                    inPutTrans.Value = Convert.ToDecimal(1);
                    iValue = 1;
                }
                trackBar.Value = iValue;
                ILayerEffects pLyrEffects = m_pCurrentLayer as ILayerEffects;
                pLyrEffects.Transparency = (short)this.inPutTrans.Value;
            }
            catch
            {
                // (ee.Message,"提示");
                inPutTrans.Value = Convert.ToDecimal(iValueLast);//20110802 xisheng
            }

            //屏蔽设置透明度而一直刷新 现为点击应用再刷新 xisheng 20111117
           // frmLayerProperties.m.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, m_pCurrentLayer, null);
        }
        //值变事件更改 xisheng 20111117 *************************************end 

        //当滚动条滚动时
        private void trackBar_Scroll(object sender, EventArgs e)
        {
            
        }

        //当按键弹起时触发值变事件　xisheng 20111117 
        private void inPutTrans_KeyUp(object sender, KeyEventArgs e)
        {
            inPutTrans_ValueChanged(sender, e);
        }

    }
}
