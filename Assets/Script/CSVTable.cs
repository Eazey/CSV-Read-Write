
// ------------------------------ //
// Product Name : CSV_Read&Write
// Company Name : MOESTONE
// Author  Name : Eazey Wang
// Editor  Data : 2017/12/16
// ------------------------------ //

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVTable : IEnumerable
{
    /// <summary>
    /// 获取表名
    /// </summary>
    public string Name { get { return _name; } }
    private string _name;

    /// <summary>
    /// 获取表中的所有属性键
    /// </summary>
    public List<string> AtrributeKeys { get { return _atrributeKeys; } }
    private List<string> _atrributeKeys;

    /// <summary>
    /// 存储表中所有数据对象
    /// </summary>
    private Dictionary<string, CSVDataObject> _dataObjDic;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="tableName"> 表名 </param>
    public CSVTable(string tableName, string[] attributeKeys)
    {
        _name = tableName;

        // init 
        _atrributeKeys = new List<string>(attributeKeys);
        _dataObjDic = new Dictionary<string, CSVDataObject>();
    }

    /// <summary>
    /// 提供类似于键值对的访问方式便捷获取和设置数据对象
    /// </summary>
    /// <param name="key"> 数据对象主键 </param>
    /// <returns> 数据对象 </returns>
    public CSVDataObject this[string dataMajorKey]
    {
        get { return GetDataObject(dataMajorKey); }
        set { AddDataObject(dataMajorKey, value); }
    }

    /// <summary>
    /// 添加数据对象, 并将数据对象主键添加到主键集合中
    /// </summary>
    /// <param name="dataMajorKey"> 数据对象主键 </param>
    /// <param name="value"> 数据对象 </param>
    private void AddDataObject(string dataMajorKey, CSVDataObject value)
    {
        if(dataMajorKey != value.ID)
        {
            Debug.LogError("所设对象的主键值与给定主键值不同！设置失败！");
            return;
        }

        if (!_dataObjDic.ContainsKey(dataMajorKey))
            _dataObjDic.Add(dataMajorKey, value);
        else
            _dataObjDic[dataMajorKey] = value;
    }

    /// <summary>
    /// 通过数据对象主键获取数据对象
    /// </summary>
    /// <param name="dataMajorKey"> 数据对象主键 </param>
    /// <returns> 数据对象 </returns>
    private CSVDataObject GetDataObject(string dataMajorKey)
    {
        CSVDataObject data = null;

        if (_dataObjDic.ContainsKey(dataMajorKey))
            data = _dataObjDic[dataMajorKey];
        else
            Debug.LogError("The table not include data of this key.");

        return data;
    }

    /// <summary>
    /// 根据数据对象主键删除对应数据对象
    /// </summary>
    /// <param name="dataMajorKey"> 数据对象主键 </param>
    public void DeleteDataObject(string dataMajorKey)
    {
        if (_dataObjDic.ContainsKey(dataMajorKey))
            _dataObjDic.Remove(dataMajorKey);
        else
            Debug.LogError("The table not include the key.");     
    }

    /// <summary>
    /// 删除所有所有数据对象
    /// </summary>
    public void DeleteAllDataObject()
    {
        _dataObjDic.Clear();
    }

    /// <summary>
    /// 获取数据表对象的文本内容
    /// </summary>
    /// <returns> 数据表文本内容 </returns>
    public string GetContent()
    {
        string content = string.Empty;

        foreach(string key in _atrributeKeys)
        {
            content += (key + ",").Trim();
        }
        content = content.Remove(content.Length - 1);

        if (_dataObjDic.Count == 0)
        {
            Debug.LogWarning("The table is empty, fuction named 'GetContent()' will retrun key's list.");
            return content;
        }

        foreach (CSVDataObject data in _dataObjDic.Values)
        {
            content += "\n" + data.ID + ",";
            foreach (KeyValuePair<string,string> item in data)
            {
                content += (item.Value + ",").Trim();
            }
            content = content.Remove(content.Length - 1);
        }

        return content;
    }

    /// <summary>
    /// 通过数据表名字和数据表文本内容构造一个数据表对象
    /// </summary>
    /// <param name="tableName"> 数据表名字 </param>
    /// <param name="tableContent"> 数据表文本内容 </param>
    /// <returns> 数据表对象 </returns>
    public static CSVTable CreateTable(string tableName, string tableContent)
    {
        string content = tableContent.Replace("\r", "");
        string[] lines = content.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError("The csv file is not csv table format.");
            return null;
        }

        string keyLine = lines[0];
        string[] keys = keyLine.Split(',');
        CSVTable table = new CSVTable(tableName, keys);   

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            string major = values[0].Trim();   
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            for (int j = 1; j < values.Length; j++)
            {
                string key = keys[j].Trim();
                string value = values[j].Trim();
                tempDic.Add(key, value);
            }
            CSVDataObject dataObj = new CSVDataObject(major, tempDic);
            table[dataObj.ID] = dataObj;
        }

        return table;
    }

    /// <summary>
    /// 迭代表中所有数据对象
    /// </summary>
    /// <returns> 数据对象 </returns>
    public IEnumerator GetEnumerator()
    {
        if (_dataObjDic == null)
        {
            Debug.LogWarning("The table is empty.");
            yield break;
        }

        foreach (var data in _dataObjDic.Values)
        {
            yield return data;
        }
    }

    /// <summary>
    /// 获得数据表内容
    /// </summary>
    /// <returns> 数据表内容 </returns>
    public override string ToString()
    {
        string content = string.Empty;

        foreach(var data in _dataObjDic.Values)
        {
            content += data.ToString() + "\n";
        }

        return content;
    }
}
