
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


public class CSVDataObject : IEnumerable
{
    /// <summary>
    /// 此值作为数据对象的唯一标识
    /// </summary>
    public string ID { get { return _major; } }
    private readonly string _major;

    private Dictionary<string, string> _atrributesDic;

    /// <summary>
    /// 初始化，获取唯一标识与所有属性的键与值
    /// </summary>
    /// <param name="major"> 唯一标识，主键 </param>
    /// <param name="atrributeDic"> 所有属性键值字典 </param>
    public CSVDataObject(string major, Dictionary<string, string> atrributeDic)
    {
        _major = major;
        _atrributesDic = atrributeDic;
    }

    public string this[string key]
    {
        get { return GetValue(key); }
        set { SetKey(key, value); }
    }

    private void SetKey(string key, string value)
    {
        if (_atrributesDic.ContainsKey(key))
            _atrributesDic[key] = value;
        else
            Debug.LogError("The data not include the key.");
    }

    private string GetValue(string key)
    {
        string value = string.Empty;

        if (_atrributesDic.ContainsKey(key))
            value = _atrributesDic[key];
        else
            Debug.LogError("The data not include value of this key.");

        return value;
    }

    public override string ToString()
    {
        string content = string.Empty;

        if (_atrributesDic != null)
        {
            foreach (var item in _atrributesDic)
            {
                content += (item.Key + ": " + item.Value + ".  ");
            }
        }
        return content;
    }

    public IEnumerator GetEnumerator()
    {
        foreach (var item in _atrributesDic)
        {
            yield return item;
        }
    }
}