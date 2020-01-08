﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public static ChatManager inst;

    private void Awake()
    {
        if (inst != null) Destroy(inst);
        inst = this;
    }

    public GameObject serverPrefab;
    public GameObject clientPrefab;

    public Text ipPortText;
    public Text ipInputText;
    public Text logText;
    public InputField inputFieldText;

    private GameObject currentSocketObject;
    private SocketBehavior currentSocketBehavior;

    public delegate void StringDelegate(string str);
    public StringDelegate OnReceived = new StringDelegate((str) => { });

    public void ServerAction()
    {
        if (currentSocketObject == null)
        {
            currentSocketObject = Instantiate(serverPrefab);
            SocketServer s = currentSocketObject.GetComponent<SocketServer>();
            s.StartServer(11111);
            currentSocketBehavior = s as SocketBehavior;
        }
    }

    public void ClientAction()
    {
        if (currentSocketObject == null)
        {
            currentSocketObject = Instantiate(clientPrefab);
            SocketClient c = currentSocketObject.GetComponent<SocketClient>();
            c.StartClient(ipInputText.text, 11111);
            currentSocketBehavior = c as SocketBehavior;
        }
    }

    public void DisconnectAction()
    {
        if (currentSocketObject != null)
        {
            currentSocketBehavior.CloseSocket();
            Destroy(currentSocketObject);
            currentSocketObject = null;
            AppendLog("Disconnected");
            ipPortText.text = "Not Connected";
        }
    }

    public void AppendLog(string str)
    {
        logText.text += "\n" + str;
    }

    public void SendData(string str)
    {
        if (currentSocketBehavior != null)
        {
            currentSocketBehavior.SendData(str);
        }
        else
        {
            Debug.LogError("Server not connected");
        }
    }

    public void AddOnReceived(string tag, Action action)
    {
        OnReceived += (str) =>
        {
            if (str == tag)
            {
                action();
            }
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendData(inputFieldText.text);
            inputFieldText.text = "";
        }
    }
}
