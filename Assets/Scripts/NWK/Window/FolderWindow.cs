using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderWindow : Window, IStoreable
{
    protected Dictionary<Vector2Int, FileSystemBase> fileGrid = new Dictionary<Vector2Int, FileSystemBase>();
    [Header("Folder Window Settings")]
    public Vector2Int maxFileGrid;

    private Vector2 fileOffset
    {
        // 1.03, 1.47은 테스트용 파일 크기 상수
        get
        {
            Vector2 windowSize = ruPos - ldPos - windowOffset * 2;
            return new Vector2((windowSize.x - maxFileGrid.x * 1.03f) / (maxFileGrid.x - 1),
                (windowSize.y - maxFileGrid.y * 1.47f) / (maxFileGrid.y - 1));
        }
    }
    private Vector2 windowLu
    {
        get
        {
            return new Vector2(ldPos.x + windowOffset.x - fileOffset.x / 2, ruPos.y - windowOffset.y + fileOffset.y / 2);
        }
    }

    public Vector2Int PositionToGrid(Vector2 pos, Vector2 size)
    {
        Vector2 offsets = size + fileOffset;
        Vector2 localLd = windowLu;
        localLd.y -= offsets.y;
        Vector2 localRu = windowLu;
        localRu.x += offsets.x;

        //Debug.Log(localLd + ", " + localRu);
        for (int y = 0; y < maxFileGrid.y; ++y)
        {
            for (int x = 0; x < maxFileGrid.x; ++x)
            {
                //Debug.Log(pos + ", " + (localLd + new Vector2(offsets.x * x, 0)) + ", " + (localRu + new Vector2(offsets.x * x, 0)));
                if (CheckInside(pos, localLd + new Vector2(offsets.x * x, 0), localRu + new Vector2(offsets.x * x, 0)))
                {
                    return new Vector2Int(x, y);
                }
            }
            localLd.y -= offsets.y;
            localRu.y -= offsets.y;
        }
        return new Vector2Int(-1, -1);
    }

    // 나중에 파일들은 다 크기를 고정해야할듯
    public Vector2 GridToCenterPos(Vector2Int grid, Vector2 size)
    {
        Vector2 offsets = size + fileOffset;
        Vector2 output = windowLu;
        output.x += offsets.x / 2 + offsets.x * grid.x;
        output.y -= offsets.y / 2 + offsets.y * grid.y;
        return output;
    }

    #region IStoreable
    public List<FileSystemBase> GetFiles()
    {
        return new List<FileSystemBase>(fileGrid.Values);
    }
    public bool AddToGrid(FileSystemBase file)
    {
        Vector2Int grid = PositionToGrid(file.transform.position, file.size);
        //Debug.Log(grid);

        if (grid.x < 0)
        {
            //file.transform.parent.GetComponent<IStoreable>()
            return false;
        }

        file.transform.parent = transform;
        file.transform.position = GridToCenterPos(grid, file.size);
        file.filePosition = grid;
        fileGrid.Add(grid, file);

        return true;
    }
    #endregion

    #region Gizmos
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Vector2 offsets = new Vector2(1.03f, 1.47f) + fileOffset;
        Gizmos.color = Color.red;
        for (int i = 0; i < maxFileGrid.x + 1; ++i)
        {
            Gizmos.DrawLine(windowLu + new Vector2(offsets.x, 0) * i, windowLu + new Vector2(offsets.x * i, -offsets.y * maxFileGrid.y));
        }
        for (int i = 0; i < maxFileGrid.y + 1; ++i)
        {
            Gizmos.DrawLine(windowLu + new Vector2(0, -offsets.y) * i, windowLu + new Vector2(offsets.x * maxFileGrid.x, -offsets.y * i));
        }
    }
    #endregion
}
