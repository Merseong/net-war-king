using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoDragableObject : MonoBehaviour
{
    [Header("Dragable Object Base")]
    public bool useMouseOffset;
    public Vector2 size;
    public Vector2 ldPos
    { // left down world position of icon
        get
        {
            Vector2 ld = new Vector2();
            ld.x = transform.position.x - size.x / 2;
            ld.y = transform.position.y - size.y / 2;
            return ld;
        }
    }
    public Vector2 ruPos
    { // right up world position of icon
        get
        {
            Vector2 ru = new Vector2();
            ru.x = transform.position.x + size.x / 2;
            ru.y = transform.position.y + size.y / 2;
            return ru;
        }
    }

    private bool isOffsetReady = false;
    private Vector3 offset;

    public virtual void OnMouseDrag()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0;
        if (useMouseOffset)
        {
            if (!isOffsetReady)
            {
                offset = transform.position - mousePoint;
                isOffsetReady = true;
            }
            mousePoint += offset;
        }
        transform.position = mousePoint;
    }
    public virtual void OnMouseUp(Window w)
    {
        isOffsetReady = false;
    }
    public bool CheckInside(Vector2 pos)
    {
        if (ldPos.x < pos.x && ldPos.y < pos.y && 
            pos.x < ruPos.x && pos.y < ruPos.y)
            return true;
        else
            return false;
    }

    public bool CheckInside(Vector2 pos, Vector2 ld, Vector2 ru)
    {
        if (ld.x < pos.x && ld.y < pos.y &&
            pos.x < ru.x && pos.y < ru.y)
            return true;
        else
            return false;
    }

    #region Gizmos
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 1));
    }
    #endregion
}
