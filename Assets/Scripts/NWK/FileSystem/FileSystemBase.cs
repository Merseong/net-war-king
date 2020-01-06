using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystemBase : MonoDragableObject
{
    [Header("File Basics")]
    public string fileName;
    public string fileRoute;
    public Vector2Int filePosition;
    public Window fileWindow;
    public Window filePropertyWindow;
    public FolderWindow fileParentWindow;

    public override void OnMouseDrag()
    {
        if (fileParentWindow != null)
        {
            fileParentWindow.RemoveFromGrid(this);
        }
        base.OnMouseDrag();
    }

    public override void OnMouseUp(Window w)
    {
        base.OnMouseUp(w);
        w.Interact(this);
    }
}
