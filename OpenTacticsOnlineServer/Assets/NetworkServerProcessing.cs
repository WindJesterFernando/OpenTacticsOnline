using Unity.Networking.Transport;
using UnityEngine;

public static class NetworkServerProcessing
{
    #region Send and Receive Data Functions
    public static void ReceivedMessageFromClient(string msg, int clientConnectionID, TransportPipeline pipeline)
    {
        Debug.Log("Network msg received =  " + msg + ", from connection id = " + clientConnectionID + ", from pipeline = " + pipeline);

        Message messageGot = new Message(msg);
        
        if (messageGot.signifier == NetworkSignifier.C_Disconnect)
        {
            networkServer.DisconnectClient(clientConnectionID);

        }
        else
        {
            serverLogic.MessageGot(clientConnectionID, messageGot);
        }


    }
    // public static void SendMessageToClient(string msg, int clientConnectionID, TransportPipeline pipeline = TransportPipeline.ReliableAndInOrder)
    // {
    //     networkServer.SendMessageToClient(msg, clientConnectionID, pipeline);
    // }

    public static void SendMessageToClient(MessageBuilder builder, int clientConnectionID,
        TransportPipeline pipeline = TransportPipeline.ReliableAndInOrder)
    {
        networkServer.SendMessageToClient(builder.GetMessage(), clientConnectionID, pipeline);
    }

#endregion

    #region Connection Events

    public static void ConnectionEvent(int clientConnectionID)
    {
        Debug.Log("Client connection, ID == " + clientConnectionID);
        serverLogic.ClientConnected(clientConnectionID);
    }
    public static void DisconnectionEvent(int clientConnectionID)
    {
        Debug.Log("Client disconnection, ID == " + clientConnectionID);
        serverLogic.ClientDisconnected(clientConnectionID);
    }

    #endregion

    #region Setup
    static NetworkServer networkServer;
    static ServerLogic serverLogic;

    public static void SetNetworkServer(NetworkServer NetworkServer)
    {
        networkServer = NetworkServer;
    }
    public static NetworkServer GetNetworkServer()
    {
        return networkServer;
    }
    public static void SetGameLogic(ServerLogic serverLogic)
    {
        NetworkServerProcessing.serverLogic = serverLogic;
    }

    #endregion
}

