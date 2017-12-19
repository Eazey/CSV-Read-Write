
// ------------------------------ //
// Product Name : CSV_Read&Write
// Company Name : MOESTONE
// Author  Name : Eazey Wang
// Create  Data : 2017/12/18
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

    /// <summary>
    /// 加载文件
    /// </summary>
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

        // 添加测试
        Test();
    }

    /// <summary>
    /// 存储文件
    /// </summary>
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

    /// <summary>
    /// 测试方法
    /// </summary>
    private void Test()
    {
        // 显示所有数据（以调试格式显示)
        Debug.Log(_table.ToString());

        // 显示所有数据（以存储格式显示）
        _display.text = _table.GetContent();

        // 拿到某一数据
        _display.text += "\n" + "1001的年龄: " + _table["1001"]["年龄"];
        // 拿到数据对象
        _display.text += "\n" + "1002的数据: " + _table["1002"].ToString();
        // 修改某一数据
        _table["1003"]["年龄"] = "10000";
        _display.text += "\n" + "1003新的年龄: " + _table["1003"]["年龄"];

        // 添加一条数据
        CSVDataObject data = new CSVDataObject("1005",
            new Dictionary<string, string>()
            {
                { "姓名","hahaha" },
                { "年龄","250" },
                { "性别","随便吧" },
            },
            new string[] { "编号", "姓名", "年龄", "性别" });
        _table[data.ID] = data;
        _display.text += "\n" + "新添加的1005的数据: " + _table["1005"].ToString();

        // 删除数据
        _table.DeleteDataObject("1001");
        _table.DeleteDataObject("1002");
        _display.text += "\n" + "删了两个之后：" + "\n" + _table.GetContent();

        // 删除所有数据
        _table.DeleteAllDataObject();
        _display.text += "\n" + "还剩下:" + "\n" + _table.GetContent();
    }
}
