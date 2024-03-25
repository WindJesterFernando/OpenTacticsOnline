using UnityEngine;

public static class NetworkClientProcessing
{

    #region Send and Receive Data Functions
    public static void ReceivedMessageFromServer(string msg, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from pipeline = " + pipeline);

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        // if (signifier == ServerToClientSignifiers.asd)
        // {

        // }
        // else if (signifier == ServerToClientSignifiers.asd)
        // {

        // }

        //gameLogic.DoSomething();

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
    static MainSystemBooter mainSystemBooter;

    public static void SetNetworkedClient(NetworkClient NetworkClient)
    {
        networkClient = NetworkClient;
    }
    public static NetworkClient GetNetworkedClient()
    {
        return networkClient;
    }
    public static void SetGameLogic(MainSystemBooter mainSystemBooter)
    {
        NetworkClientProcessing.mainSystemBooter = mainSystemBooter;  
    }

    #endregion

}

#region Protocol Signifiers
public static class ClientToServerSignifiers
{
    public const int asd = 1;
}

public static class ServerToClientSignifiers
{
    public const int asd = 1;
}

#endregion

