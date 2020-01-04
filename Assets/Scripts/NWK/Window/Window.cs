using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoreable
{
    List<FileSystemBase> GetFiles();
    bool AddToGrid(FileSystemBase file);
}

public interface IInteractive
{

}

public abstract class Window : MonoDragableObject
{
    [Header("Window Basics")]
    public FileSystemBase matchObject;
    public Vector2 windowOffset;

    public virtual bool CheckDraggablePosition(Vector2 pos)
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
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x - 2 * windowOffset.x, size.y - 2 * windowOffset.y, 1));
    }
    #endregion
}
