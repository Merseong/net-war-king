using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagement : MonoBehaviour
{
    public UnitBoard myBoard;
    public UnitBoard enemyBoard; // already fliped
    public List<Unit> attackerList = new List<Unit>();
    public Action beforeBattleAction;
    public Action afterBattleAction;

    private void Awake()
    {
        beforeBattleAction = new Action(() => { });
        afterBattleAction = new Action(() => { });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(TurnFlow());
        }
    }

    public IEnumerator TurnFlow()
    {
        yield return new WaitForSeconds(1);

        // 0. Reset Status
        for (int i = 0; i < UnitBoard.gridMax; ++i)
        {
            for (int j = UnitBoard.gridMax - 1; j > -1; --j)
            {
                if (myBoard.CheckUnit(new Vector2Int(i, j)))
                {
                    myBoard.GetUnit(new Vector2Int(i, j)).ResetStatus();
                }
                if (enemyBoard.CheckUnit(new Vector2Int(UnitBoard.gridMax - i - 1, j)))
                {
                    enemyBoard.GetUnit(new Vector2Int(UnitBoard.gridMax - i - 1, j)).ResetStatus();
                }
            }
        }

        Debug.Log("Done 0");
        yield return new WaitForSeconds(1);
        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        // 1. Apply buff from init to end
        for (int i = 0; i < UnitBoard.gridMax; ++i)
        {
            for (int j = UnitBoard.gridMax - 1; j > -1; --j)
            {
                if (myBoard.CheckUnit(new Vector2Int(i, j)))
                {
                    myBoard.GetUnit(new Vector2Int(i, j)).BuffAction();
                }
                if (enemyBoard.CheckUnit(new Vector2Int(UnitBoard.gridMax - i - 1, j)))
                {
                    enemyBoard.GetUnit(new Vector2Int(UnitBoard.gridMax - i - 1, j)).BuffAction();
                }
            }
        }

        Debug.Log("Done 1");
        yield return new WaitForSeconds(1);
        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        // 2. Check and compare sum of attack and health, put in to list by less sum
        attackerList.AddRange(myBoard.GetUnitList());
        attackerList.AddRange(enemyBoard.GetUnitList());
        attackerList.Sort((Unit x, Unit y) =>
        {
            if (x.healthPoint + x.attackPoint > y.healthPoint + y.attackPoint) return 1;
            else if (x.healthPoint + x.attackPoint < y.healthPoint + y.attackPoint) return -1;
            else // same sum
            {
                if (x.board == y.board) return 0;
                else if (x.board.isAttacker) return 1;
                else if (y.board.isAttacker) return -1;
                else return 0;
            }
        });

        beforeBattleAction();
        Debug.Log("Done 2");
        yield return new WaitForSeconds(1);
        while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

        while (myBoard.unitCount > 0 && enemyBoard.unitCount > 0)
        {
            // 3. Start Battle
            while (attackerList.Count > 0)
            {
                Unit u = attackerList[0];
                attackerList.RemoveAt(0);

                u.AttackAction(u.board != enemyBoard ? enemyBoard : myBoard);
            }
            Debug.Log("Done Battle");
            yield return new WaitForSeconds(1);
            while (!Input.GetKeyDown(KeyCode.Space)) yield return null;

            // 4. Refresh attacker list
            attackerList.AddRange(myBoard.GetUnitList());
            attackerList.AddRange(enemyBoard.GetUnitList());
            attackerList.Sort((Unit x, Unit y) =>
            {
                if (x.healthPoint + x.attackPoint > y.healthPoint + y.attackPoint) return 1;
                else if (x.healthPoint + x.attackPoint < y.healthPoint + y.attackPoint) return -1;
                else // same sum
                {
                    if (x.board == y.board) return 0;
                    else if (x.board.isAttacker) return 1;
                    else if (y.board.isAttacker) return -1;
                    else return 0;
                }
            });
            Debug.Log("Done Battle Turn");
            yield return new WaitForSeconds(1);
            while (!Input.GetKeyDown(KeyCode.Space)) yield return null;
        }

        afterBattleAction();
    }
}
