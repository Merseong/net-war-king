using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocketClient : SocketBehavior
{
    public void StartClient(string address, int port)
    {
        try
        {
            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            mainSocket.Connect(address, port);

            AsyncObject ao = new AsyncObject(4096)
            {
                WorkingSocket = mainSocket
            };
            mainSocket.BeginReceive(ao.Buffer, 0, ao.BufferSize, 0, DataReceived, ao);

            AppendData("Client connected to Server");
            AppendAction(() =>
            {
                ChatManager.inst.ipPortText.text = address;
            });
        }
        catch (Exception e)
        {
            AppendData("Client connection failed");
            AppendData(e.ToString());
        }
    }

    protected override void SendData()
    {
        if (!mainSocket.IsBound)
        {
            AppendData("Server not connected");
            return;
        }

        string text = ChatManager.inst.inputFieldText.text.Trim();
        ChatManager.inst.inputFieldText.text = "";

        IPEndPoint ip = (IPEndPoint)mainSocket.LocalEndPoint;
        string addr = ip.Address.ToString();

        byte[] bDts = Encoding.UTF8.GetBytes(addr + '\x01' + text);

        mainSocket.Send(bDts);

        AppendData("You: " + text);
    }

    protected override void DataReceived(IAsyncResult ar)
    {
        AsyncObject obj = (AsyncObject)ar.AsyncState;

        int received = obj.WorkingSocket.EndReceive(ar);
        if (received <= 0)
        {
            obj.WorkingSocket.Close();
            return;
        }

        string text = Encoding.UTF8.GetString(obj.Buffer);
        string[] tokens = text.Split('\x01');
        if (text.Length > 0)
            AppendData(tokens[0] + ": " + tokens[1].TrimEnd(text[text.Length - 1]));

        obj.ClearBuffer();

        obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
    }
}