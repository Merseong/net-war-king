using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketServer : SocketBehavior
{
    private List<Socket> connectedClients = new List<Socket>();

    public override void CloseSocket()
    {
        while (connectedClients.Count > 0)
        {
            connectedClients[0].Shutdown(SocketShutdown.Both);
            connectedClients[0].Close();
            connectedClients.RemoveAt(0);
        }
        base.CloseSocket();
    }

    public void StartServer(int port)
    {
        try
        {
            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, port);
            mainSocket.Bind(serverEP);

            mainSocket.Listen(10);

            mainSocket.BeginAccept(AcceptCallback, null);

            AppendData("Server Ready");
            AppendAction(() => 
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ChatManager.inst.ipPortText.text = ip.ToString();
                        break;
                    }
                }
            });
        }
        catch (Exception e)
        {
            AppendData("Server Broken");
            AppendData(e.ToString());
        }
    }

    void AcceptCallback(IAsyncResult ar)
    {
        Socket client = mainSocket.EndAccept(ar);

        mainSocket.BeginAccept(AcceptCallback, null);

        AsyncObject obj = new AsyncObject(4096);
        obj.WorkingSocket = client;
        connectedClients.Add(client);

        client.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);

        AppendData("new Client connected");
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
        {
            AppendData(tokens[0] + ": " + tokens[1].TrimEnd(text[text.Length - 1]));
            ChatManager.inst.OnReceived(tokens[1]);
        }

        for (int i = connectedClients.Count - 1; i >= 0; i--)
        {
            Socket socket = connectedClients[i];
            if (socket.Handle != obj.WorkingSocket.Handle)
            {
                try { socket.Send(obj.Buffer); }
                catch
                {
                    try { socket.Dispose(); } catch { }
                    connectedClients.RemoveAt(i);
                    AppendData("Client " + i + " disconnected");
                }
            }
        }

        obj.ClearBuffer();

        obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
    }

    public override void SendData(string str)
    {
        if (!mainSocket.IsBound)
        {
            Debug.LogError("Server not connected");
            return;
        }

        string text = str.Trim();

        IPEndPoint ip = (IPEndPoint)mainSocket.LocalEndPoint;
        string addr = ip.Address.ToString();

        byte[] bDts = Encoding.UTF8.GetBytes(addr + '\x01' + text);

        for (int i = connectedClients.Count - 1; i >= 0; i--)
        {
            Socket socket = connectedClients[i];
            try { socket.Send(bDts); }
            catch
            {
                try { socket.Dispose(); } catch { }
                connectedClients.RemoveAt(i);
            }
        }

        AppendData("You: " + text);
    }
}
