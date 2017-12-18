
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
    string _loadPath;
    string _savePath;

    [SerializeField] private string _fileName = "TestTable";
    private const string EXTENSION = ".csv";

    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _loadBtn;
    [SerializeField] private Text _display;

    private CSVTable _table;

    // Bind Component
    void Awake()
    {
        _loadPath = Application.dataPath + "/Load/";
        _savePath = Application.dataPath + "/Save/";

        _saveBtn.onClick.AddListener(Save);
        _loadBtn.onClick.AddListener(Load);
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

        _table = null;
        _display.text = "Save.";
    }

    private void Load()
    {
        if (!Directory.Exists(_loadPath))
        {
            Debug.LogError("The file not be found in this path. path:" + _loadPath);
            return;
        }

        string fullFileName = _loadPath + _fileName + EXTENSION;
        StreamReader sr;
        sr = File.OpenText(fullFileName);
        string content = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();

        _table = CSVTable.CreateTable(_fileName, content);

        Debug.Log(_table.ToString());
        _display.text = _table.GetContent();

        // Test
        _display.text += "\n" + "1001的年龄: " + _table["1001"]["年龄"];
        _display.text += "\n" + "1002的年龄: " + _table["1002"]["性别"];
        _display.text += "\n" + "1003的年龄: " + _table["1002"]["姓名"];
        _table["1004"]["年龄"] = "10000";
        _display.text += "\n" + "1004新的年龄: " + _table["1004"]["年龄"];
        _display.text += "\n" + "1001的数据: " + _table["1001"].ToString();
    }
}
