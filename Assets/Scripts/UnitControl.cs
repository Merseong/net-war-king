using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    private bool isDragged = false;

    public BattleBoard board;

    private void OnMouseDrag()
    {
        isDragged = true;
        var screenPoint = Input.mousePosition;
        screenPoint.z = -Camera.main.transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    private void OnMouseUp()
    {
        if (isDragged)
        {
            isDragged = false;
            if (board != null && board.IsInBoard(transform.position))
            {
                var snapPos = board.GetNearGrid(transform.localPosition);
                transform.localPosition = new Vector3(snapPos.x, snapPos.y);
            }
            else
            {
                board = null;
            }
        }
    }
}
