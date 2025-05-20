using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegSpawnCommand : BaseCommand
{
    GoldenLeg goldenLeg;
    int row, col;
    int[] trow = {0,0,1,-1 };
    int[] tcol = {-1,1,0,0 };
    public LegSpawnCommand(GoldenLeg goldenLeg, int row, int col) : base(goldenLeg)
    {
        this.goldenLeg = goldenLeg;
        this.row = row;
        this.col = col;
    }

    public override void Do()
    {
        base.Do();

        int count = goldenLeg.legkids;

        if(count < goldenLeg.maxlegkids)
        {
            int i = 10;
            while(true)
            {
                int idx = Random.Range(0, 4);
                int nrow = row + trow[idx];
                int ncol = col + tcol[idx];
                if (GameApp.MapManager.GetBlockType(nrow, ncol) == BlockType.floor)
                {
                    GameApp.EnemyManager.CreatEnemy(EnemyType.Leg, nrow, ncol);
                    break;
                }
                i--;
                if (i <= 0)
                {
                    break;
                }
            }
        }
    }

    public override bool Update(float dt)
    {
        return true;
    }
}
