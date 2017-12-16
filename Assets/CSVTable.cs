
// ------------------------------ //
// Product Name : CSV_RW
// Company Name : MOESTONE
// Author  Name : Eazey Wang
// Editor  Data : 2017/12/16
// ------------------------------ //

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVTable
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
    /// 获取表中所有数据对象的主键
    /// </summary>
    public List<string> DataMajorKeys { get { return _dataMajorKeys; } }
    private List<string> _dataMajorKeys;

    /// <summary>
    /// 存储表中所有数据对象
    /// </summary>
    private Dictionary<string, CSVDataObject> _dataObjDic;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="tableName"> 表名 </param>
    public CSVTable(string tableName)
    {
        _name = tableName;

        // init 
        _atrributeKeys = new List<string>();
        _dataMajorKeys = new List<string>();
        _dataObjDic = new Dictionary<string, CSVDataObject>();
    }

    /// <summary>
    /// 提供类似于键值对的访问方式便捷获取和设置数据对象
    /// </summary>
    /// <param name="key"> 数据对象主键 </param>
    /// <returns></returns>
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
        if (!_dataObjDic.ContainsKey(dataMajorKey))
        {
            _dataObjDic.Add(dataMajorKey, value);
            _dataMajorKeys.Add(dataMajorKey);
        }
        else
        {
            _dataObjDic[dataMajorKey] = value;
        }
    }

    /// <summary>
    /// 通过数据对象主键获取数据对象
    /// </summary>
    /// <param name="dataMajorKey"> 数据对象主键 </param>
    /// <returns> 数据对象 </returns>
    private CSVDataObject GetDataObject(string dataMajorKey)
    {
        if (_dataObjDic.ContainsKey(dataMajorKey))
        {
            return _dataObjDic[dataMajorKey];
        }
        else
        {
            Debug.LogError("The table not include data of this key.");
            return null;
        }
    } 
}
