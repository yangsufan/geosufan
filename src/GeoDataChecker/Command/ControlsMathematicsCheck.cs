using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;


namespace GeoDataChecker
{
    /// <summary>
    /// 数学基础正确性检查
    /// </summary>
    public class ControlsMathematicsCheck:Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsMathematicsCheck()
        {
            base._Name = "GeoDataChecker.ControlsMathematicsCheck";
            base._Caption = "数学基础正确性检查";
            base._Tooltip = "检查坐标系是否符合标准规定";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数学基础正确性检查";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                if (_AppHk.MapControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                return true;
            }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        } 
              
        public override void OnClick()
        {
            Exception eError = null;

            //FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.数学基础正确性);
            //mFrmMathematicsCheck.ShowDialog();

          

            FrmMathCheck pFrmMathCheck = new FrmMathCheck();
            if(pFrmMathCheck.ShowDialog()==DialogResult.OK)
            {
                #region 错误日志保存
                //错误日志连接信息
                string logPath = TopologyCheckClass.GeoLogPath+"Log"+ System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString() + System.DateTime.Today.Day.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".xls";
                if (File.Exists(logPath))
                {
                    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "日志文件\n'" + logPath + "'\n已经存在,是否替换?"))
                    {
                        File.Delete(logPath);
                    }
                    else
                    {
                        return;
                    }
                }
                //生成日志信息EXCEL格式
                SysCommon.DataBase.SysDataBase pSysLog = new SysCommon.DataBase.SysDataBase();
                pSysLog.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + logPath + "; Extended Properties=Excel 8.0;", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "日志信息表连接失败！");
                    return;
                }
                string strCreateTableSQL = @" CREATE TABLE ";
                strCreateTableSQL += @" 错误日志 ";
                strCreateTableSQL += @" ( ";
                strCreateTableSQL += @" 检查功能名 VARCHAR, ";
                strCreateTableSQL += @" 错误类型 VARCHAR, ";
                strCreateTableSQL += @" 错误描述 VARCHAR, ";
                strCreateTableSQL += @" 数据图层1 VARCHAR, ";
                strCreateTableSQL += @" 数据OID1 VARCHAR, ";
                strCreateTableSQL += @" 数据图层2 VARCHAR, ";
                strCreateTableSQL += @" 数据OID2 VARCHAR, ";
                strCreateTableSQL += @" 定位点X VARCHAR , ";
                strCreateTableSQL += @" 定位点Y VARCHAR , ";
                strCreateTableSQL += @" 检查时间 VARCHAR ,";
                strCreateTableSQL += @" 数据文件路径 VARCHAR ";
                strCreateTableSQL += @" ) ";

                pSysLog.UpdateTable(strCreateTableSQL, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "插入表头出错！");
                    pSysLog.CloseDbConnection();
                    return;
                }

                DataCheckClass dataCheckCls = new DataCheckClass(_AppHk);
                //将日志表连接信息和表名保存起来
                dataCheckCls.ErrDbCon = pSysLog.DbConn;
                dataCheckCls.ErrTableName = "错误日志";
                #endregion

                #region 获得要检查的IFeatureClass的集合
                //将Mapcontrol上所有的图层名保存起来
                List<IFeatureClass> LstFeaClass = new List<IFeatureClass>();
                for (int i = 0; i < _AppHk.MapControl.LayerCount; i++)
                {
                    ILayer player = _AppHk.MapControl.get_Layer(i);
                    if (player is IGroupLayer)
                    {
                       
                        ICompositeLayer pComLayer = player as ICompositeLayer;
                        for (int j = 0; j < pComLayer.Count; j++)
                        {
                            ILayer mLayer = pComLayer.get_Layer(j);
                            IFeatureLayer mfeatureLayer = mLayer as IFeatureLayer;
                            if (mfeatureLayer == null) continue;
                            IFeatureClass pfeaCls = mfeatureLayer.FeatureClass;
                            if (!LstFeaClass.Contains(pfeaCls))
                            {
                                LstFeaClass.Add(pfeaCls);
                            }
                        }
                    }
                    else
                    {
                        IFeatureLayer pfeatureLayer = player as IFeatureLayer;
                        if (pfeatureLayer == null) continue;
                        IFeatureClass mFeaCls = pfeatureLayer.FeatureClass;
                        if (!LstFeaClass.Contains(mFeaCls))
                        {
                            LstFeaClass.Add(mFeaCls);
                        }
                    }
                }
                #endregion

                if (_AppHk.DataTree == null) return;
                _AppHk.DataTree.Nodes.Clear();
                //创建处理树图
                IntialTree(_AppHk.DataTree);
                //设置树节点颜色
                setNodeColor(_AppHk.DataTree);
                _AppHk.DataTree.Tag = false;

                string prjStr = pFrmMathCheck.PRJFNAME;
                if(prjStr=="")
                {
                    return;
                }
                try
                {
                    ISpatialReferenceFactory spatialRefFac = new SpatialReferenceEnvironmentClass();
                    ISpatialReference standardSpatialRef = spatialRefFac.CreateESRISpatialReferenceFromPRJFile(prjStr);

                    
                    for (int i = 0; i < LstFeaClass.Count; i++)
                    {
                        IFeatureClass pFeatureClass = LstFeaClass[i];
                        string pFeaClsNameStr = "";//图层名
                        pFeaClsNameStr = (pFeatureClass as IDataset).Name.Trim();

                        //创建树图节点(以图层名作为根结点)
                        DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                        pNode = (DevComponents.AdvTree.Node)CreateAdvTreeNode(_AppHk.DataTree.Nodes, pFeaClsNameStr, pFeaClsNameStr, _AppHk.DataTree.ImageList.Images[6], true);//图层名节点
                        //显示进度条
                        ShowProgressBar(true);

                        int tempValue = 0;
                        ChangeProgressBar((_AppHk as Plugin.Application.IAppFormRef).ProgressBar, 0, 1, tempValue);

                        dataCheckCls.MathematicsCheck(pFeatureClass, standardSpatialRef, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数学基础性正确检查失败。" + eError.Message);
                            pSysLog.CloseDbConnection();
                            return;
                        }

                        tempValue += 1;//进度条的值加1
                        ChangeProgressBar((_AppHk as Plugin.Application.IAppFormRef).ProgressBar, -1, -1, tempValue);

                        //改变树图运行状态
                        ChangeTreeSelectNode(pNode, "完成图层"+pFeaClsNameStr+"的数据基础性正确检查", false);
                    }
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据检查完成!");
                    pSysLog.CloseDbConnection();
                    //隐藏进度条
                    ShowProgressBar(false);
                }
                catch(Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示",ex.Message);
                    pSysLog.CloseDbConnection();
                    return;
                }
                
            }
        }


       public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }



        #region 处理树图相关函数
        //创建处理树图
        private void IntialTree(DevComponents.AdvTree.AdvTree aTree)
        {
            DevComponents.AdvTree.ColumnHeader aColumnHeader;
            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "FCName";
            aColumnHeader.Text = "图层名";
            aColumnHeader.Width.Relative = 50;
            aTree.Columns.Add(aColumnHeader);

            aColumnHeader = new DevComponents.AdvTree.ColumnHeader();
            aColumnHeader.Name = "NodeRes";
            aColumnHeader.Text = "结果";
            aColumnHeader.Width.Relative = 45;
            aTree.Columns.Add(aColumnHeader);
        }
        //设置选中树节点颜色
        private void setNodeColor(DevComponents.AdvTree.AdvTree aTree)
        {
            DevComponents.DotNetBar.ElementStyle elementStyle = new DevComponents.DotNetBar.ElementStyle();
            elementStyle.BackColor = Color.FromArgb(255, 244, 213);
            elementStyle.BackColor2 = Color.FromArgb(255, 216, 105);
            elementStyle.BackColorGradientAngle = 90;
            elementStyle.Border = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderBottomWidth = 1;
            elementStyle.BorderColor = Color.DarkGray;
            elementStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderLeftWidth = 1;
            elementStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderRightWidth = 1;
            elementStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            elementStyle.BorderTopWidth = 1;
            elementStyle.BorderWidth = 1;
            elementStyle.CornerDiameter = 4;
            elementStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            aTree.NodeStyleSelected = elementStyle;
            aTree.DragDropEnabled = false;
        }
        //创建树图节点
        private DevComponents.AdvTree.Node CreateAdvTreeNode(DevComponents.AdvTree.NodeCollection nodeCol, string strText, string strName, Image pImage, bool bExpand)
        {

            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
            node.Text = strText;
            node.Image = pImage;
            if (strName != null)
            {
                node.Name = strName;
            }

            if (bExpand == true)
            {
                node.Expand();
            }
            //添加树图列节点
            DevComponents.AdvTree.Cell aCell = new DevComponents.AdvTree.Cell();
            aCell.Images.Image = null;
            node.Cells.Add(aCell);
            nodeCol.Add(node);
            return node;
        }

        //添加树图节点列
        private DevComponents.AdvTree.Cell CreateAdvTreeCell(DevComponents.AdvTree.Node aNode, string strText, Image pImage)
        {
            DevComponents.AdvTree.Cell aCell = new DevComponents.AdvTree.Cell(strText);
            aCell.Images.Image = pImage;
            aNode.Cells.Add(aCell);

            return aCell;
        }

        //为数据处理树图节点添加处理结果状态
        private void ChangeTreeSelectNode(DevComponents.AdvTree.Node aNode, string strRes, bool bClear)
        {
            if (aNode == null)
            {
                _AppHk.DataTree.SelectedNode = null;
                _AppHk.DataTree.Refresh();
                return;
            }

            _AppHk.DataTree.SelectedNode = aNode;
            if (bClear)
            {
                _AppHk.DataTree.SelectedNode.Nodes.Clear();
            }
            _AppHk.DataTree.SelectedNode.Cells[1].Text = strRes;
            _AppHk.DataTree.Refresh();
        }
        #endregion

        #region 进度条显示
        //控制进度条显示
        private void ShowProgressBar(bool bVisible)
        {
            if (bVisible == true)
            {
                (_AppHk as Plugin.Application.IAppFormRef).ProgressBar.Visible = true;
            }
            else
            {
                (_AppHk as Plugin.Application.IAppFormRef).ProgressBar.Visible = false;
            }
        }
        //修改进度条
        private void ChangeProgressBar(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
        {
            if (min != -1)
            {
                pProgressBar.Minimum = min;
            }
            if (max != -1)
            {
                pProgressBar.Maximum = max;
            }
            pProgressBar.Value = value;
            pProgressBar.Refresh();
        }


        //改变状态栏提示内容
        private void ShowStatusTips(string strText)
        {
            (_AppHk as Plugin.Application.IAppFormRef).OperatorTips = strText;
        }
        #endregion
    }
}
