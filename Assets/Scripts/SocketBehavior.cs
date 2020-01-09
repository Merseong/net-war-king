using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class AsyncObject
{
    public byte[] Buffer;
    public Socket WorkingSocket;
    public readonly int BufferSize;
    public AsyncObject(int bufferSize)
    {
        BufferSize = bufferSize;
        Buffer = new byte[BufferSize];
    }

    public void ClearBuffer()
    {
        Array.Clear(Buffer, 0, BufferSize);
    }
}

public abstract class SocketBehavior : MonoBehaviour
{
    protected Socket mainSocket;
    protected Queue<string> receivedData = new Queue<string>();
    protected Queue<Action> receivedAction = new Queue<Action>();

    private void Update()
    {
        if (receivedData.Count > 0)
        {
            // do something with data
            ChatManager.inst.AppendLog(receivedData.Dequeue());
        }
        if (receivedAction.Count > 0)
        {
            receivedAction.Dequeue()();
        }
    }

    public virtual void CloseSocket()
    {
        try
        {
            //Debug.Log(mainSocket.Connected + " " + mainSocket.IsBound);
            if (mainSocket.Connected)
            {
                mainSocket.Shutdown(SocketShutdown.Both);
            }
            //mainSocket.Disconnect(false);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            mainSocket.Close();
        }
    }

    protected void AppendData(string data)
    {
        receivedData.Enqueue(data);
    }

    public void AppendAction(Action action)
    {
        receivedAction.Enqueue(action);
    }

    protected abstract void DataReceived(IAsyncResult ar);
    public abstract void SendData(string str);
}
