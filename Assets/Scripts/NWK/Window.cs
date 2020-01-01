using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoDragableObject
{
    public Dictionary<Vector2Int, FileSystemBase> fileGrid = new Dictionary<Vector2Int, FileSystemBase>();

    [Header("Folder Window Basics")]
    public FileSystemBase matchObject;
    public Vector2 windowOffset;

    public bool CheckDraggablePosition(Vector2 pos)
    {
        if (CheckInside(pos) && pos.y > ruPos.y - windowOffset.y)
            return true;
        else
            return false;
    }

    #region Gizmos
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(xSize - 2 * windowOffset.x, ySize - 2 * windowOffset.y, 1));
    }
    #endregion
}
