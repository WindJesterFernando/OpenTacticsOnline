using System;
using UnityEngine;

public static class NetworkClientProcessing
{
    private static NetworkClient networkClient;
    public delegate void ReceivedMessage(Message message);

    private static ReceivedMessage onMessageReceived;

    #region Send and Receive Data Functions
    public static void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from pipeline = " + pipeline);
        Message messageGot = new Message(msg);
        onMessageReceived?.Invoke(messageGot);
    }

    public static void SendMessageToServer(MessageBuilder builder, TransportPipeline pipeline = TransportPipeline.ReliableAndInOrder)
    {
        networkClient.SendMessageToServer(builder.GetMessage(), pipeline);
    }
    #endregion

    #region Connection Related Functions and Events
    public static void ConnectionEvent()
    {
        Debug.Log("Network Connection Event!");
    }
    public static void DisconnectionEvent()
    {
        Debug.Log("Network Disconnection Event!");
    }
    public static bool IsConnectedToServer()
    {
        if (networkClient == null) 
            return false;
        return networkClient.IsConnected();
    }
    public static void ConnectToServer()
    {
        networkClient.Connect();
    }
    public static void DisconnectFromServer()
    {
        networkClient.Disconnect();
    }

    #endregion

    #region Setup

    public static void SetNetworkedClient(NetworkClient networkClient)
    {
        NetworkClientProcessing.networkClient = networkClient;
    }
    public static NetworkClient GetNetworkedClient()
    {
        return networkClient;
    }

    public static void SetMessageReceiver(ReceivedMessage receiver)
    {
        if (onMessageReceived != null)
            throw new Exception("Message receiver is already set!");

        onMessageReceived = receiver;
    }
    public static void ClearMessageReceiver()
    {
        onMessageReceived = null;
    }

    #endregion
}
