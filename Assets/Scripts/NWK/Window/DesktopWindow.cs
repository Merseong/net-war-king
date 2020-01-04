using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopWindow : FolderWindow
{
    public override bool CheckDraggablePosition(Vector2 pos)
    {
        return false; // non draggable window
    }
}
