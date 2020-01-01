using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoDragableObject : MonoBehaviour
{
    [Header("Dragable Object Base")]
    public float xSize;
    public float ySize;
    public Vector2 ldPos
    { // left down world position of icon
        get
        {
            Vector2 ld = new Vector2();
            ld.x = transform.position.x - xSize / 2;
            ld.y = transform.position.y - ySize / 2;
            return ld;
        }
    }
    public Vector2 ruPos
    { // right up world position of icon
        get
        {
            Vector2 ru = new Vector2();
            ru.x = transform.position.x + xSize / 2;
            ru.y = transform.position.y + ySize / 2;
            return ru;
        }
    }

    public virtual void OnMouseDrag()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0;
        transform.position = mousePoint;
    }
    public virtual void OnMouseUp()
    {

    }
    public bool CheckInside(Vector2 pos)
    {
        if (ldPos.x < pos.x && ldPos.y < pos.y && 
            pos.x < ruPos.x && pos.y < ruPos.y)
            return true;
        else
            return false;
    }

    #region Gizmos
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(xSize, ySize, 1));
    }
    #endregion
}
