using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystemManager : MonoBehaviour
{
    public static FileSystemManager inst;

    private void Awake()
    {
        if (inst != null) Destroy(inst);
        inst = this;
        DontDestroyOnLoad(this);
    }

    public List<Window> windowStack = new List<Window>(); // 0이 가장 위에있는것.

    private MonoDragableObject dragging = null;
    private bool isPressed = false;
    private Vector3 initPressedPos;
    private GameObject selectCube;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnMouseDrag();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
    }

    private void OnMouseDrag()
    {
        //Debug.Log("Drag Start, dragging = " + dragging);
        if (dragging != null)
        {
            dragging.OnMouseDrag();
        }
        else if (!isPressed)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initPressedPos = mousePoint;
            initPressedPos.z = 1;
            for (int i = 0; i < windowStack.Count; ++i)
            {
                if (windowStack[i].CheckInside(mousePoint))
                {
                    if (windowStack[i].CheckDraggablePosition(mousePoint))
                    {
                        dragging = windowStack[i] as MonoDragableObject;
                    }
                    else if (windowStack[i] is IStoreable stored)
                    {
                        foreach (FileSystemBase f in stored.GetFiles())
                        {
                            if (f.CheckInside(mousePoint))
                            {
                                dragging = f as MonoDragableObject;
                                break;
                            }
                        }
                    }
                }
            }
        }
        else // for fun ^^
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePoint.z = 2;
            if (selectCube == null)
                selectCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            selectCube.transform.position = Vector3.Lerp(initPressedPos, mousePoint, 0.5f);
            selectCube.transform.localScale = mousePoint - initPressedPos;
        }
        isPressed = true;
    }

    private void OnMouseUp()
    {
        Debug.Log("Drag End");
        isPressed = false;
        if (selectCube != null)
        {
            Destroy(selectCube);
            selectCube = null;
        }
        if (dragging != null)
        {
            Window interactWindow = null;
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initPressedPos = mousePoint;
            initPressedPos.z = 1;
            for (int i = 0; i < windowStack.Count; ++i)
            {
                if (windowStack[i].CheckInside(mousePoint))
                {
                    interactWindow = windowStack[i];
                    break;
                }
            }
            dragging.OnMouseUp(interactWindow);
            dragging = null;
        }
    }

    public void AddWindowOnTop(Window w)
    {

    }

    private void WindowToTop(Window w)
    {
        if (!windowStack.Contains(w))
        {
            Debug.LogError("No such window " + w);
            return;
        }

        windowStack.Remove(w);
        windowStack.Insert(0, w);
    }
}
