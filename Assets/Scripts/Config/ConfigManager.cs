using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理游戏中的所有配置表
/// </summary>
public class ConfigManager
{
    private Dictionary<string, ConfigData> loadList;//需要读取配置表

    private Dictionary<string, ConfigData> configs;//已经加载完成的配置表

    public ConfigManager()
    {
        loadList = new Dictionary<string, ConfigData>();
        configs = new Dictionary<string, ConfigData>();
    }

    //注册要加载的配置表
    public void Register(string file, ConfigData data)
    {
        loadList[file] = data;
    }

    //加载所有的配置表
    public void LoadAllConfigs()
    {
        foreach (var item in loadList)
        {
            TextAsset textAsset = item.Value.LoadFile();
            item.Value.Load(textAsset.text);
            configs.Add(item.Value.fileName, item.Value);
        }
        loadList.Clear();
    }

    public ConfigData GetConfigData(string fileName)
    {
        if (configs.ContainsKey(fileName))
        {
            return configs[fileName];
        }
        return null;
    }
}
