using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 임시로 Unit만듬
public class Unit
{

}

public class BattleBoard : MonoBehaviour
{
    private Dictionary<Vector2Int, Unit> unitGrid = new Dictionary<Vector2Int, Unit>();

    const int gridMax = 5; // 5*5 board, 0 ~ 4

    public bool SetUnit(Vector2Int pos, Unit u)
    {
        if (CheckUnit(pos))
            return false;

        unitGrid[pos] = u;
        return true;
    }

    public bool ResetUnit(Vector2Int pos)
    {
        if (!CheckUnit(pos))
            return false;

        unitGrid.Remove(pos);
        return true;
    }

    public bool CheckUnit(Vector2Int pos)
    {
        return unitGrid.ContainsKey(pos);
    }

    public Unit GetUnit(Vector2Int pos)
    {
        if (CheckUnit(pos))
            return unitGrid[pos];
        else
            return null;
    }

    public Vector2Int GetNearGrid(Vector2 pos)
    {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        return new Vector2Int(x, y);
    }

    public Vector2Int GetOppositeGrid(Vector2Int pos)
    {
        return new Vector2Int(pos.x, gridMax - pos.y);
    }
}
