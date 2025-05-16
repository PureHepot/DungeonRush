using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Evil,
    Heart,
    Leg,
    Hand
}


/// <summary>
/// 敌人的统一管理器，指令生成 指令执行
/// </summary>
public class EnemyManager
{
    public int enemyCount;
    public List<Enemy> enemies;
    private Dictionary<EnemyType, GameObject> enemyPrefabs;

    public EnemyManager()
    {
        enemies = new List<Enemy>();
        enemyPrefabs = new Dictionary<EnemyType, GameObject>();

        GameObject[] tempObj = Resources.LoadAll<GameObject>("Assets/Resources/Prefabs/Model/Enemys");

        foreach (GameObject obj in tempObj)
        {
            EnemyType type;
            if (Enum.TryParse(obj.ToString(), out type))
            {
                enemyPrefabs[type] = obj;
            }
        }
    }

    public void GetSceneEnemy()
    {
        enemies.Clear();
        GameObject[] tenemys = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject t in tenemys)
        {
            Enemy enemy = t.GetComponent<Enemy>();
            //enemy.Id = enemyCount++;
            enemies.Add(enemy);
        }
    }

    public Enemy GetEnemybyPos(int row, int col)
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.RowIndex == row && enemy.ColIndex == col)
            {
                return enemy;
            }
        }
        return null;
    }

    public void CreatEnemy(EnemyType type, int row, int col)
    {
        if (GameApp.MapManager.GetBlockType(row, col) != BlockType.enemy)
        {
            Enemy enemy = GameObject.Instantiate(enemyPrefabs[type], GameApp.MapManager.GetBlockPos(row,col), Quaternion.identity).GetComponent<Enemy>();
            AddEnmey(enemy);
        }
    }

    public void AddEnmey(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnmey(Enemy enemy)
    {
        enemies.Remove(enemy);
        GameObject.Destroy(enemy.gameObject,0.5f);
    }

    public void GenerateEnemyCommand()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.GenerateCommand();
            if (enemy.current != null)
                enemy.current.Do();
        }

        List<Enemy> temp = new List<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (enemy.CurHp <= 0)
            {
                temp.Add(enemy);
            }
        }
        foreach (var enemy in temp)
        {
            RemoveEnmey(enemy);
        }
    }

    public void Update(float dt)
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.current != null)
            {
                if (enemy.current.Update(dt))
                {
                    enemy.lastCommand = enemy.current;
                    enemy.current = null;
                }
            }
        }
    }
}
