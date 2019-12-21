using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBoard : MonoBehaviour
{
    private Dictionary<Vector2Int, Unit> unitGrid = new Dictionary<Vector2Int, Unit>();

    const int gridMax = 5; // 5*5 board, 0 ~ 4
    private Vector2 ldPos;
    private Vector2 ruPos;

    private void Start()
    {
        ResetPos();
    }

    public void ResetPos()
    {
        Vector3 offset = new Vector3(0.5f, 0.5f);
        ldPos = transform.position - offset;
        ruPos = transform.position + offset + gridMax * Vector3.one;
    }

    public bool IsInBoard(Vector2 globalPos)
    {
        return (ldPos.x < globalPos.x && ldPos.y < globalPos.y && ruPos.x > globalPos.x && ruPos.y > globalPos.y);
    }

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
