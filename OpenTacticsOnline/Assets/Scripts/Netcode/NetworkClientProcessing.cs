using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class NetworkClientProcessing
{

    #region Send and Receive Data Functions
    public static void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from pipeline = " + pipeline);
        onMessageReceived.Invoke(msg);
        //
        // string[] csv = msg.Split(',');
        // int signifier = int.Parse(csv[0]);
        //
        //
        // if (signifier == 1)
        // {
        //     Camera.main.backgroundColor = new Color(Random.value, Random.value, Random.value, 1);
        // }
       
    }

    public static void SendMessageToServer(string msg, TransportPipeline pipeline)
    {
        networkClient.SendMessageToServer(msg, pipeline);
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

    public static Action<string> onMessageReceived;
    // static MainSystemBooter mainSystemBooter;

    public static void SetNetworkedClient(NetworkClient NetworkClient)
    {
        networkClient = NetworkClient;
    }
    public static NetworkClient GetNetworkedClient()
    {
        return networkClient;
    }
    // public static void SetMainSystemBooter(MainSystemBooter mainSystemBooter)
    // {
    //     
    //     NetworkClientProcessing.mainSystemBooter = mainSystemBooter;  
    // }

    #endregion

}
