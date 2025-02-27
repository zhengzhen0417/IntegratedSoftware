﻿using Global.Functions;
using Halcon.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIControl.HalconVision
{
    public partial class SetInputForm : Form
    {
        private int m_nObjectIndex = 0;
        private DataType m_dataType = DataType.INT;
        private string m_strDataName = string.Empty;
        private int m_nDataIndex = 0;

        public SetInputForm(int selectIndex)
        {
            InitializeComponent();

            m_nObjectIndex = selectIndex;
            SetUI(selectIndex);
        }

        private void SetUI(int selectIndex)
        {
            SetInputList(selectIndex);

            SetOutputList(selectIndex);

            //SetParameterList(selectIndex);
        }

        private void SetOutputList(int selectIndex)
        {
            listViewOutput.Items.Clear();
            int num = 1;
            for (int i = 0; i < selectIndex; i++)
            {
                string value = (i + 1).ToString() + "." + GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].GetProcessByIndex(i).ToolName();
                listViewOutput.Items.Add(value);
                num++;
            }
        }

        private void SetInputList(int selectIndex)
        {
            listViewInput.Items.Clear();
            int num = 1;
            IImageHalconObject imageHalconObject = GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].GetProcessByIndex(selectIndex);
            //整形
            List<string> names = imageHalconObject.GetDataManager.GetInputIntNames();
            for (int i = 0; i < names.Count; i++)
            {
                string value = num.ToString() + "." + names[i] + ":INT";
                listViewInput.Items.Add(value);
                num++;
            }
            //整形数组
            names = imageHalconObject.GetDataManager.GetInputIntArrayNames();
            for (int i = 0; i < names.Count; i++)
            {
                string value = num.ToString() + "." + names[i] + ":INT[]";
                listViewInput.Items.Add(value);
                num++;
            }
            //双精度
            names = imageHalconObject.GetDataManager.GetInputDoubleNames();
            for (int i = 0; i < names.Count; i++)
            {
                string value = num.ToString() + "." + names[i] + ":DOUBLE";
                listViewInput.Items.Add(value);
                num++;
            }
            //双精度数组
            names = imageHalconObject.GetDataManager.GetInputDoubleArrayNames();
            for (int i = 0; i < names.Count; i++)
            {
                string value = num.ToString() + "." + names[i] + ":DOUBLE[]";
                listViewInput.Items.Add(value);
                num++;
            }
        }

        private void SetParameterList(int selectIndex)
        {
            if (selectIndex != -1)
            {
                listViewParameter.Items.Clear();
                int num = 1;
                IImageHalconObject imageHalconObject = GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].GetProcessByIndex(selectIndex);
                List<string> names;
                switch (m_dataType)
                {
                    case DataType.INT:
                        //整形
                        names = imageHalconObject.GetDataManager.GetOutputIntNames();
                        for (int i = 0; i < names.Count; i++)
                        {
                            string value = num.ToString() + "." + names[i] + ":INT";
                            listViewParameter.Items.Add(value);
                            num++;
                        }
                        break;
                    case DataType.INTARRAY:
                        //整形数组
                        names = imageHalconObject.GetDataManager.GetOutputIntArrayNames();
                        for (int i = 0; i < names.Count; i++)
                        {
                            string value = num.ToString() + "." + names[i] + ":INT[]";
                            listViewParameter.Items.Add(value);
                            num++;
                        }
                        break;
                    case DataType.DOUBLE:
                        //双精度
                        names = imageHalconObject.GetDataManager.GetOutputDoubleNames();
                        for (int i = 0; i < names.Count; i++)
                        {
                            string value = num.ToString() + "." + names[i] + ":DOUBLE";
                            listViewParameter.Items.Add(value);
                            num++;
                        }
                        break;
                    case DataType.DOUBLEARRAY:
                        //双精度数组
                        names = imageHalconObject.GetDataManager.GetOutputDoubleArrayNames();
                        for (int i = 0; i < names.Count; i++)
                        {
                            string value = num.ToString() + "." + names[i] + ":DOUBLE[]";
                            listViewParameter.Items.Add(value);
                            num++;
                        }
                        break;
                    default:
                        break;
                }

                if (imageHalconObject.GetDataManager.GetDataBinding(m_nDataIndex) != null)
                {
                    for (int i = 0; i < num - 1; i++)
                    {
                        string key = (i + 1).ToString() + "." + sourceName;
                        switch (m_dataType)
                        {
                            case DataType.INT:
                                key += ":INT";
                                break;
                            case DataType.INTARRAY:
                                key += ":INT[]";
                                break;
                            case DataType.DOUBLE:
                                key += ":DOUBLE";
                                break;
                            case DataType.DOUBLEARRAY:
                                key += ":DOUBLE[]";
                                break;
                            default:
                                break;
                        }
                        if (listViewParameter.FindItemWithText(key) != null)
                        {
                            int index = listViewParameter.FindItemWithText(key).Index;
                            if (index != -1)
                            {
                                listViewParameter.Items[index].Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void listViewInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewInput.SelectedItems.Count != 0)
            {
                string[] splits = listViewInput.SelectedItems[0].Text.Split(':');
                switch (splits[1])
                {
                    case "INT":
                        m_dataType = DataType.INT;
                        break;
                    case "INT[]":
                        m_dataType = DataType.INTARRAY;
                        break;
                    case "DOUBLE":
                        m_dataType = DataType.DOUBLE;
                        break;
                    case "DOUBLE[]":
                        m_dataType = DataType.DOUBLEARRAY;
                        break;
                    default:
                        break;
                }
                m_strDataName = splits[0].Substring(2);
                m_nDataIndex = listViewInput.SelectedItems[0].Index;
                try
                {
                    IImageHalconObject imageHalconObject = GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].GetProcessByIndex(m_nObjectIndex);
                    if (imageHalconObject.GetDataManager.GetDataBinding(m_nDataIndex) != null)
                    {
                        DataBinding dataBinding = imageHalconObject.GetDataManager.GetDataBinding(m_nDataIndex);
                        sourceIndex = dataBinding.DataSourceIndex;
                        sourceName = dataBinding.DataSourceName;
                        listViewOutput.Items[sourceIndex].Selected = true;
                    }
                    SetParameterList(sourceIndex);
                }
                catch
                { }
            }
        }

        int sourceIndex = -1;
        private void listViewOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewOutput.SelectedItems.Count != 0)
            {
                sourceIndex = listViewOutput.SelectedItems[0].Index;
                SetParameterList(sourceIndex);
            }
        }

        string sourceName = "";
        private void listViewParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewParameter.SelectedItems.Count != 0)
            {
                string[] splits = listViewParameter.SelectedItems[0].Text.Split(':');
                sourceName = splits[0].Substring(2);
                GlobalObjectList.ImageListObject[GlobalObjectList.SelectedImageIndex].GetProcessByIndex(m_nObjectIndex).GetDataManager.SetDataBinding(m_nDataIndex, m_strDataName, m_dataType, sourceIndex, sourceName);
            }
        }
    }
}
