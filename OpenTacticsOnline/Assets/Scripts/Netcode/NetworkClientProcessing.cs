using System;
using UnityEngine;

public static class NetworkClientProcessing
{
    #region Send and Receive Data Functions
    public static void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from pipeline = " + pipeline);
        Message messageGot = new Message(msg);
        OnMessageReceived?.Invoke(messageGot);
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
    static NetworkClient networkClient;

    public static event Action<Message> OnMessageReceived;
    
    public static void SetNetworkedClient(NetworkClient networkClient)
    {
        NetworkClientProcessing.networkClient = networkClient;
    }
    public static NetworkClient GetNetworkedClient()
    {
        return networkClient;
    }
    #endregion

}
