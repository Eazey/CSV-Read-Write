##**【先说点废话】**
哈哈哈哈好久没发文章不知道大家有没有想我，这一大段时间鬼知道我经历了什么，弄比赛、备战考研、各种求职各种做简历、弄毕业设计、租房子。。。等等等，做了一大堆都没做太好哈哈哈，不过好在找到了心仪的工作，之后会继续保持更新，把一些技术分享给大家，一起进步一起学习。以后如果有时间也特别想分享一下我这段日子的经历，十分的难能可贵。好了话不多说，接下来进入正题吧！

##**【需要了解的知识基础】**
###**什么是CSV文件？**

> 逗号分隔值（Comma-Separated Values，CSV，有时也称为字符分隔值，因为分隔字符也可以不是逗号），其文件以纯文本形式存储表格数据（数字和文本）。纯文本意味着该文件是一个字符序列，不含必须像二进制数字那样被解读的数据。—— [百度百科](https://baike.baidu.com/item/CSV/10739?fr=aladdin)

简单来说，CSV 文件可以直接通过文件内容来获取表格数据。那逗号分割值是什么意思呢？比如我们需要学生的数据表，包含学生的学号，姓名，性别等级这些数据，通常我们会用 Excel 来记录，它的表现形式类似于下表：
|学号| 姓名 | 性别 |
|:-----  | :-----| :-----|
| 1001 | 张三 |boy|
| 1002 | 李四 |girl|
| 1003 | 王五 |none|

（请不要问我none是什么性别(●ˇ∀ˇ●)）

那 CSV 文件的形式又是什么样子呢？我们在 Excel 中做一个如上表格，然后另存为 CSV 格式文件，用 NotePad++ 打开，会发现他是如下样式

```
学号,姓名,性别
1001,张三,boy
1002,李四,girl
1003,王五,none
```
可以看到文件中所有的值都主要以逗号分隔开，每一行数据携带换行符。（当然，逗号不一定是唯一的分割方式，但是是比较常用。这里以“,”这个字符分割为例）
###**为什么要使用CSV文件？**
目前总结出来的优点就有以下三点，但是在认识优点的同时也要认识到这种方式所存在的不足，权衡利弊后进行选择。

优点：
1. 结构简单，易于理解；
2. 解析文本和还原文本的方式较为简洁高效；
3. 可以轻松转换为 Excel 的 .xls 文件，亦可以利用 Excel 以表格的方式进行查阅。相比 .xls 文件，其本身由于只存储文本而不包含表格中的公式等其他附带信息，在相同的文件内容下 CSV 文件可以具有更小的文件体积。

缺点：
1. 相比于二进制文件，由于是纯文本存储，体积会比较大；
2. 虽然由于数据格式参差不齐，具备基本的安全性，但破解的风险依旧很高。

###**使用CSV文件要注意什么？**
1. 可以在 Excel 中创建保存为 CSV 文件，但是后续对 CSV 文件操作最好用 Notepad++ 等文本编辑器来打开，最好使用 Notepad++，详细原因看了之后的几点就懂啦；
2. 使用 Notepad++ 打开 CSV 文件后，需要将其转码为 UTF-8 格式，这样才能保证文件中的中文被正确显示，而 Excel 存储的文件均不是 UTF-8 编码格式的；
3. 在 Notepad++ 中打开 CSV 文件后，会发现多了一个空行，这是 Excel 的存储所导致的。我个人习惯把这个空行删掉，以便于我程序中计算文件中的真实行数。


##**【又到了紧张刺激的分析环节了】**
没有勇气看代码？ 可能分析你都没有勇气看完。。。
###表的结构分析
在准备环节中已经对 CSV 文件有了初步的介绍，简短的总结下 CSV 文件的结构：
1. 第一行是表中数据对象包含的所有属性的属性名称，即键名；
2. 从第二行开始，每一行都是一个独立的数据对象，包含了所有属性的属性值；
3. 值与值之间使用逗号分隔。
###数据模型分析
>主键：用于唯一标识一个对象的键值，比如学号可以唯一找到一条学生数据，假设这个学生的学号是12345，那么“学号”就是主键的名称，而“12345”就是主键值，一般简称为主键。——by 亦泽
####数据对象类：
1. 数据对象类应提供所有属性值的读取功能，而除主键之外还应提供所有的属性值的写入功能，以便对数据对象的读写操作。
2. 我们需要通过属性名称来获取对应的属性值。基于这样的键值关系，我们需要使用 Dictionary（即字典）类型来存储所有的属性名称和对应的属值；
3. 构造对象的三个参数：
(1) 由于主键值为只读模式，所以需要在构造数据对象时来设置主键值，并提供一个属性供外部访问其主键；
(2) 由于所有的键值（除主键）可以修改，而键名不可以修改或者增加删除，所以所有的键值对儿（除主键）也需要在构造对象时初始化；
(3) 由于第二个参数中不包含主键的键名，但是要保证数据对象的标签（用所有的键名有序构造）是完整的，所以第三个参数要传入所有的键名组成的数组来构造标签。
4. 为了使数据的读写更简单高效，可改写 `this` 关键来构建与 Dictionary 类类似的读写方式，其应隐式包含 GetValue 和 SetValue 两个方法；
5. 重写 `ToString()` 方法，提供对象的数据内容便于测试；
6. 继承 IEnumerable 接口，实现遍历数据对象中所有键值对的方法。
####数据表类：
1. 数据表类应该提供所有数据对象的读取功能，所有的数据对象应该都是只读的，但是以数据对象为单位添加或者删除。
2. 可以通过每条数据的主键来访问到具体的数据对象，从而访问该数据对象的其他属性值；
3. 与数据对象类相似的，可使用改写 `this` 关键字来提供更简单高效的读写模式；
4. 数据表对象应该具有但不限于增、删、改、查四个功能；
5. 数据表对象应具有获取标签的方法，即在构造数据表对象时记录数据表所有的键名，并提供获取签名的方法。该方法的算法应与数据对象签名的算法保持一致；
6. 数据表对象应具有将抽象数据结构转换为文本字符串的方法共外部使用，方便外部直接获取数据对象的所对应的文本值而无须考虑其中的算法；
7. 数据表类应提供静态方法通过表名和文件内容用于构造数据表对象，方法内实现将文本内容解析为数据表对象的算法并返回；
8. 基于上述的需求，构造数据表对象时应该提供两个参数：表名和键名数组；
9. 重写 `ToString()` 方法，提供数据表的字符串形式用于测试；
10. 继承 IEnumerable 接口，实现遍历数据表中所有数据对象的方法。
###读、写算法分析
好了，写了一大推，有点累，但是还没完，我们还要分析一下主要算法是如何实现的，其他方法就不分析了主要是一些逻辑校验之类的，到时候直接看代码就好啦。废话不多说，继续继续~

我们再来重新温习一下所涉及到的文件格式，一般策划给的都是基于 Excel 的类似于下面这种格式**（从这里开始的表需要记住，在代码中我会使用这个表来进行测试）**
| 编号 | 姓名 | 年龄 | 性别 |
|:-----|:-----|:-----||
| 1001 | 张三 | 20| 男|
| 1002 | 李四 |20|女|
| 1003 | 王五 |12| 不详|
|1004|ABC|100|male|

当我们改存为 CSV 文件时，就会变成这个样子（别忘了之前讲过的几点注意事项！）

```
编号,姓名,年龄,性别
1001,张三,20,男
1002,李四,40,女
1003,王五,12,不详
1004,ABC,100,male
```
####文本字符串转化为数据表对象
首先我们观察下我们的文本内容，一共有五行，第一行是所有的键名，第二行开始每一行都是一个数据对象，对应着键名都有四个属性。那么我们可以先把他们以行为单位进行拆分，得到一个关于行的数组，第一行单独用作键名数组，从第二行开始，每一行构造一个数据对象，然后存储在数据表对象当中。
####数据表对象转化为文本字符串
相当于反过来思考，首先键名行是比较特殊的，单独拿出来写；从第二行开始，我们可以循环去将所有数据对象的所有属性值拼接出来写入。
####文件的读写
可以使用 System.IO 中的方法来操作。我这里采用的时 Stream 的方式来操作，因为文本中含有中文，所以使用 Stream 搭配 File 类中静态方法的方式时较为简答的。


##**【代码来啦！】**
###数据对象类 CSVDataObject

```csharp

// ------------------------------ //
// Product Name : CSV_Read&Write
// Company Name : MOESTONE
// Author  Name : Eazey Wang
// Create  Data : 2017/12/16
// ------------------------------ //

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CSVDataObject : IEnumerable
{
    /// <summary>
    /// 此值作为数据对象的唯一标识，只能通过此属性获取到唯一标识
    /// 无法通过 '数据对象[主键名]' 的方式来获取
    /// </summary>
    public string ID { get { return _major; } }
    private readonly string _major;

    /// <summary>
    /// 一条数据应包含的所有的键名
    /// </summary>
    public string[] AllKeys { get { return _allKeys; } }
    private readonly string[] _allKeys;

    private Dictionary<string, string> _atrributesDic;

    /// <summary>
    /// 初始化，获取唯一标识与除主键之外所有属性的键与值
    /// </summary>
    /// <param name="major"> 唯一标识，主键 </param>
    /// <param name="atrributeDic"> 除主键值外的所有属性键值字典 </param>
    public CSVDataObject(string major, Dictionary<string, string> atrributeDic, string[] allKeys)
    {
        _major = major;
        _atrributesDic = atrributeDic;
        _allKeys = allKeys;
    }

    /// <summary>
    /// 获取数据对象的签名，用于比较是否与数据表的签名一致
    /// </summary>
    /// <returns> 数据对象的签名 </returns>
    public string GetFormat()
    {
        string format = string.Empty;
        foreach (string key in _allKeys)
        {
            format += (key + "-");
        }
        return format;
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
```

###数据表类 CSVTable

```csharp

// ------------------------------ //
// Product Name : CSV_Read&Write
// Company Name : MOESTONE
// Author  Name : Eazey Wang
// Create  Data : 2017/12/16
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
    /// 获取数据表对象的签名，用于比较是否与数据对象的签名一致
    /// </summary>
    /// <returns> 数据表对象的签名 </returns>
    public string GetFormat()
    {
        string format = string.Empty;
        foreach (string key in _atrributeKeys)
        {
            format += (key + "-");
        }
        return format;
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
        if (dataMajorKey != value.ID)
        {
            Debug.LogError("所设对象的主键值与给定主键值不同！设置失败！");
            return;
        }

        if (value.GetFormat() != GetFormat())
        {
            Debug.LogError("所设对象的的签名与表的签名不同！设置失败！");
            return;
        }

        if (_dataObjDic.ContainsKey(dataMajorKey))
        {
            Debug.LogError("表中已经存在主键为 '" + dataMajorKey + "' 的对象！设置失败！");
            return;
        }

        _dataObjDic.Add(dataMajorKey, value);
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
            Debug.LogWarning("The table is empty, fuction named 'GetContent()' will just retrun key's list.");
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
            Dictionary<string, string> tempAttributeDic = new Dictionary<string, string>();
            for (int j = 1; j < values.Length; j++)
            {
                string key = keys[j].Trim();
                string value = values[j].Trim();
			    tempAttributeDic.Add(key, value);
            }
            CSVDataObject dataObj = new CSVDataObject(major, tempAttributeDic, keys);
            table[dataObj.ID] = dataObj;
        }

        return table;
    }
}

```
###数据管理类 DataManager

```csharp

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
```

###效果截图
在场景中添加测试的按钮和文本框，与脚本关联。将数据文件放在代码中所给的路径下（可以构建存储的目录）点击 Load 按钮即可出线代码中测试函数的输出效果。
![点击 Load 后加载出来的效果](http://img.blog.csdn.net/20171219184210067?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvZWF6ZXlfd2o=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)


##**【完整项目地址】**
github：[CSV 文件的读写示例工程 —— By 亦泽](https://github.com/Eazey/CSV-Read-Write)
PS：如果你觉得对你有很大帮助的话麻烦给我的项目点个星，这样可以让更多的开发者看到哦~