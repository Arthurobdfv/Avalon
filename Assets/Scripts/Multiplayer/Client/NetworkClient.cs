using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetworkClient : MonoBehaviour
{

    TcpClient client;
    Coroutine ConnectCoroutine;
    Coroutine ReceiveCoroutine;
    NetworkStream stream;
    // Start is called before the first frame update
    void Start()
    {
        client = new TcpClient();
        ConnectCoroutine = StartCoroutine(Connect());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (client.Connected && ReceiveCoroutine == null)
        {
            ReceiveCoroutine = StartCoroutine(Receive());
        }
    }

    private IEnumerator Receive()
    {
        while (client.Connected)
        {
            var buffer = new byte[MultiplayerConstants.MAX_PACKET_SIZE];
            
            int bytesRead = 0;
            var read = stream.ReadAsync(buffer, 0, buffer.Length);
            while (!read.IsCompleted)
            {
                yield return null;
            }
            if (read.IsFaulted)
            {
                Debug.LogError("Failed to read from server: " + read.Exception);
                yield break;
            }
            bytesRead = read.Result;
            if(bytesRead > 0)
            {
                OnDataReceived?.Invoke(buffer);
            }
            else
            {
                Debug.Log("Received 0 byte stream from server!");
                client.Close();
            }
        }
    }

    IEnumerator Connect()
    {
        var connect = client.ConnectAsync("localhost", MultiplayerConstants.SERVER_PORT);
        while (!connect.IsCompleted)
        {
            yield return new WaitForFixedUpdate();
        }
        if (connect.IsFaulted)
        {
            Debug.LogError("Failed to connect to server: " + connect.Exception);
            yield break;
        }
        stream = client.GetStream();
    }

    public void Send(byte[] data)
    {
        stream.Write(data);
    }

    public delegate void DataReceived(byte[] data);
    public static event DataReceived OnDataReceived;
}
