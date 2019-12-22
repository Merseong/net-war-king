using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Unit Data")]
    public int attackPointStat;
    public int healthPointStat;
    public List<Vector2Int> buffAreas;

    [Header("Unit Current Status")]
    public BattleBoard board;
    public Vector2Int position;
    public int attackPoint;
    public int healthPoint;
    public int stackPoint;

    public Action BeforeAttack;
    public Func<int, int> BeforeDamaged;
    public Action BeforeDead;

    public Action AfterAttack;
    public Action AfterDamaged;

    private void Awake()
    {
        BeforeAttack = new Action(() => { });
        BeforeDamaged = new Func<int, int>((int damage) => { return damage; });
        BeforeDead = new Action(() => { });

        AfterAttack = new Action(() => { });
        AfterDamaged = new Action(() => { });
    }

    private void ResetStatus()
    {
        attackPoint = attackPointStat;
        healthPoint = healthPointStat;
    }

    protected abstract Queue<Unit> GetAttackTargets();
    protected Queue<Unit> GetBuffTargets()
    {
        Queue<Unit> units = new Queue<Unit>();
        foreach (Vector2Int v in buffAreas)
        {
            if (board.CheckUnit(position + v))
            {
                units.Enqueue(board.GetUnit(position + v));
            }
        }
        return units;
    }
    public virtual void AttackAction()
    {
        Queue<Unit> targets = GetAttackTargets();
        BeforeAttack();
        while (targets.Count > 0)
        {
            targets.Dequeue().DamagedAction(attackPoint);
        }
        AfterAttack();
    }
    public abstract void BuffAction();
    public virtual void DamagedAction(int damage)
    {
        damage = BeforeDamaged(damage);
        healthPoint -= damage;
        AfterDamaged();
        if (healthPoint < 0)
            DeadAction();
    }

    public void DeadAction()
    {
        BeforeDead();
        stackPoint--;
        if (stackPoint < 0)
            Destroy(gameObject);
    }
}
