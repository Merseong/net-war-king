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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendData();
        }
        if (receivedData.Count > 0)
        {
            // do something with data
            ChatManager.inst.AppendLog(receivedData.Dequeue());
        }
    }

    public void CloseSocket()
    {
        try
        {
            mainSocket.Disconnect(false);
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

    protected abstract void DataReceived(IAsyncResult ar);
    protected abstract void SendData();
}
