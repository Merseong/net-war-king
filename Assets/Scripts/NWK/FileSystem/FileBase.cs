using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileBase : FileSystemBase
{
    private void Start()
    {
        // for test
        (fileWindow as FolderWindow).AddToGrid(this as FileSystemBase);
        fileWindow = null;
    }
}
