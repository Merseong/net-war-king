using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileBase : FileSystemBase
{
    private void Start()
    {
        // for test
        fileWindow.fileGrid.Add(filePosition, this);
        transform.parent = fileWindow.transform;
        fileWindow = null;
    }
}
