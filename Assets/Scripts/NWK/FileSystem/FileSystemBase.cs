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
}
