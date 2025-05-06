using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 读取csv表格数据
/// </summary>
public class ConfigData
{
    private Dictionary<int, Dictionary<string, string>> datas;//<id, data>

    public string fileName;//配置表文件名称

    public ConfigData(string fileName)
    {
        this.fileName = fileName;
        this.datas = new Dictionary<int, Dictionary<string, string>>();
    }

    public TextAsset LoadFile()
    {
        return Resources.Load<TextAsset>($"config/{fileName}");
    }

    //读取
    public void Load(string txt)
    {
        string[] dataArr = txt.Split("\r\n");
        string[] titleArr = dataArr[0].Trim().Split(',');//去掉首尾空格然后分割出每行字典中的key

        for (int i = 1; i < dataArr.Length; i++)
        {
            string[] tempArr = dataArr[i].Trim().Split(',');
            Dictionary<string, string> tempData = new Dictionary<string, string>();
            for (int j = 0; j < tempArr.Length; j++)
            {
                tempData.Add(titleArr[j], tempArr[j]);
            }
            datas.Add(int.Parse(tempData["Id"]), tempData);
        }
    }

    public Dictionary<string, string> GetDataById(int id)
    {
        if (datas.ContainsKey(id))
        {
            return datas[id];
        }
        return null;
    }

    public Dictionary<int, Dictionary<string, string>> GetLines()
    {
        return datas;
    }
}
