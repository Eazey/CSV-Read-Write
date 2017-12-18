
// ------------------------------ //
// Product Name : CSV_Read&Write
// Company Name : MOESTONE
// Author  Name : Eazey Wang
// Editor  Data : 2017/12/18
// ------------------------------ //

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class DataManager : MonoBehaviour
{
    string _readPath = Application.dataPath + "/Read/";
    string _savePath = Application.dataPath + "/Save/";

    [SerializeField] private string _fileName = "TestTable";
    private const string EXTENSION = ".csv";

    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _readBtn;


    private CSVTable _table;

    // Bind Component
    void Awake()
    {
        _saveBtn.onClick.AddListener(Save);
        _readBtn.onClick.AddListener(Read);
    }

    private void Save()
    {
        if (_table == null)
        {
            Debug.LogError("The table is null.");
            return;
        }
        string tableContent = _table.GetContent();

        if(!Directory.Exists(_savePath))
        {
            Debug.Log("未找到路径, 已自动创建");
            Directory.CreateDirectory(_savePath);
        }
        string fullFileName = _savePath + _fileName + EXTENSION;

        StreamWriter sw;
        sw = File.CreateText(fullFileName);
        sw.Write(tableContent);
        sw.Close();
        sw.Dispose();
    }

    private void Read()
    {
        if (!Directory.Exists(_readPath))
        {
            Debug.LogError("The file not be found in this path. path:" + _readPath);
            return;
        }

        string fullFileName = _readPath + _fileName + EXTENSION;
        StreamReader sr;
        sr = File.OpenText(fullFileName);
        string content = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();

        _table = CSVTable.InitTable(_fileName, content);

        Debug.Log(_table.ToString());
    }
}
