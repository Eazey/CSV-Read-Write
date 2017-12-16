
// ------------------------------ //
// Product Name : CSV_RW
// Company Name : MOESTONE
// Author  Name : Eazey Wang
// Editor  Data : 2017/12/16
// ------------------------------ //

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CSVDataObject
{
    const string majorKeyName = "ID";

    private Dictionary<string, string> _atrributes;

    public string this[string key]
    {
        get { return GetValue(key); }
        set { AddKey(key, value); }
    }

    private void AddKey(string key, string value)
    {
        if (!_atrributes.ContainsKey(key))
            _atrributes.Add(key, value);
        else
            _atrributes[key] = value;
    }

    private string GetValue(string key)
    {
        if(_atrributes.ContainsKey(key))
        {
            return _atrributes[key];
        }
        else
        {
            Debug.LogError("The data not include value of this key.");
            return string.Empty;
        }
    }
}