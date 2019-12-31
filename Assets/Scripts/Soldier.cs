using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    /*
     * 병사
     * 주변의 병사 유닛에게 자신의 초기공격력과 초기체력를 부여
     * 자신과 가장 가까운 적 유닛을 공격한다.
     */

    public override void BuffAction()
    {
        Queue<Unit> units = GetBuffTargets();
        while (units.Count > 0)
        {
            if (units.Dequeue() is Soldier u)
            {
                u.attackPoint += attackPointStat;
                u.healthPoint += healthPointStat;
                BeforeDead += () =>
                {
                    u.attackPoint -= attackPointStat;
                    u.healthPoint -= healthPointStat;
                };
            }
        }
    }

    protected override Queue<Unit> GetAttackTargets(UnitBoard b)
    {
        Queue<Unit> targets = new Queue<Unit>();
        // 임시
        for (int i = 0; i < UnitBoard.gridMax; ++i)
        {
            bool isDone = false;
            for (int j = UnitBoard.gridMax - 1; j > -1; --j)
            {
                if (b.CheckUnit(new Vector2Int(UnitBoard.gridMax - i - 1, j)))
                {
                    targets.Enqueue(b.GetUnit(new Vector2Int(UnitBoard.gridMax - i - 1, j)));
                    isDone = true;
                    break;
                }
            }
            if (isDone) break;
        }
        
        return targets;
    }
}
