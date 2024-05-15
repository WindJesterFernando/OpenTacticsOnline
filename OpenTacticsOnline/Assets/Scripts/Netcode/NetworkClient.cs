using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;
using System.Text;
using System.Net.NetworkInformation;

public class NetworkClient : MonoBehaviour
{
    private NetworkDriver networkDriver;
    private NetworkConnection networkConnection;
    private NetworkPipeline reliableAndInOrderPipeline;
    private NetworkPipeline nonReliableNotInOrderedPipeline;
    private const ushort NetworkPort = 9001;
    private string IPAddress;

    private void Start()
    {
        if (NetworkClientProcessing.GetNetworkedClient() == null)
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && ni.Name == "Wi-Fi")
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            IPAddress = ip.Address.ToString();
                        }
                    }
                }
            }

            DontDestroyOnLoad(this.gameObject);
            NetworkClientProcessing.SetNetworkedClient(this);
            Connect();
        }
        else
        {
            Debug.Log(
                "Singleton-ish architecture violation detected, investigate where NetworkClient.cs Start() is being called.  Are you creating a second instance of the NetworkClient game object or has NetworkClient.cs been attached to more than one game object?");
            Destroy(this.gameObject);
        }
    }

    public void OnDestroy()
    {
        networkConnection.Disconnect(networkDriver);
        networkConnection = default(NetworkConnection);
        networkDriver.Dispose();
    }

    private void Update()
    {
        networkDriver.ScheduleUpdate().Complete();

        #region Check for client to server connection

        if (!networkConnection.IsCreated)
        {
            Debug.Log("Client is unable to connect to server");
            return;
        }

        #endregion

        #region Manage Network Events

        NetworkEvent.Type networkEventType;
        DataStreamReader streamReader;
        NetworkPipeline pipelineUsedToSendEvent;

        while (PopNetworkEventAndCheckForData(out networkEventType, out streamReader, out pipelineUsedToSendEvent))
        {
            TransportPipeline pipelineUsed = TransportPipeline.NotIdentified;
            if (pipelineUsedToSendEvent == reliableAndInOrderPipeline)
                pipelineUsed = TransportPipeline.ReliableAndInOrder;
            else if (pipelineUsedToSendEvent == nonReliableNotInOrderedPipeline)
                pipelineUsed = TransportPipeline.FireAndForget;

            switch (networkEventType)
            {
                case NetworkEvent.Type.Connect:
                    NetworkClientProcessing.ConnectionEvent();
                    break;
                case NetworkEvent.Type.Data:
                    int sizeOfDataBuffer = streamReader.ReadInt();
                    NativeArray<byte> buffer = new NativeArray<byte>(sizeOfDataBuffer, Allocator.Persistent);
                    streamReader.ReadBytes(buffer);
                    byte[] byteBuffer = buffer.ToArray();
                    string msg = Encoding.Unicode.GetString(byteBuffer);
                    NetworkClientProcessing.ReceivedMessageFromServer(msg, pipelineUsed);
                    buffer.Dispose();
                    break;
                case NetworkEvent.Type.Disconnect:
                    NetworkClientProcessing.DisconnectionEvent();
                    networkConnection = default(NetworkConnection);
                    break;
            }
        }

        #endregion
    }

    private bool PopNetworkEventAndCheckForData(out NetworkEvent.Type networkEventType, out DataStreamReader streamReader, out NetworkPipeline pipelineUsedToSendEvent)
    {
        networkEventType = networkConnection.PopEvent(networkDriver, out streamReader, out pipelineUsedToSendEvent);

        if (networkEventType == NetworkEvent.Type.Empty)
            return false;
        return true;
    }

    public void SendMessageToServer(string msg, TransportPipeline pipeline)
    {
        NetworkPipeline networkPipeline = reliableAndInOrderPipeline;
        if (pipeline == TransportPipeline.FireAndForget)
            networkPipeline = nonReliableNotInOrderedPipeline;

        byte[] msgAsByteArray = Encoding.Unicode.GetBytes(msg);
        NativeArray<byte> buffer = new NativeArray<byte>(msgAsByteArray, Allocator.Persistent);

        DataStreamWriter streamWriter;
        networkDriver.BeginSend(networkPipeline, networkConnection, out streamWriter);
        streamWriter.WriteInt(buffer.Length);
        streamWriter.WriteBytes(buffer);
        networkDriver.EndSend(streamWriter);

        buffer.Dispose();
    }

    public void Connect()
    {
        networkDriver = NetworkDriver.Create();
        reliableAndInOrderPipeline = networkDriver.CreatePipeline(typeof(FragmentationPipelineStage), typeof(ReliableSequencedPipelineStage));
        nonReliableNotInOrderedPipeline = networkDriver.CreatePipeline(typeof(FragmentationPipelineStage));
        networkConnection = default(NetworkConnection);
        NetworkEndpoint endpoint = NetworkEndpoint.Parse(IPAddress, NetworkPort, NetworkFamily.Ipv4);;
        networkConnection = networkDriver.Connect(endpoint);
    }

    public bool IsConnected()
    {
        return networkConnection.IsCreated;
    }

    public void Disconnect()
    {
        networkConnection.Disconnect(networkDriver);
        networkConnection = default(NetworkConnection);
    }
}

public enum TransportPipeline
{
    NotIdentified,
    ReliableAndInOrder,
    FireAndForget
}