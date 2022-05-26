using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public class Server : MonoBehaviour
{
    private const int MAX_CONNECTION = 10;
    private int port = 5805;
    private int hostID;
    private int reliableChannel;
    private bool isStarted = false;
    private byte error;
    List<int> connectionIDs = new List<int>();
    Dictionary<int, string> dataTableUser;
    public void StartServer()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(cc, MAX_CONNECTION);
        hostID = NetworkTransport.AddHost(topology, port);
        isStarted = true;
        
    }
    public void ShutDownServer()
    {
        if (!isStarted) return;
        NetworkTransport.RemoveHost(hostID);
        NetworkTransport.Shutdown();
        isStarted = false;
        dataTableUser = new Dictionary<int, string>();
    }

    void Update()
    {
        if (!isStarted) return;
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out
        channelId, recBuffer, bufferSize, out dataSize, out error);
        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.ConnectEvent:
                    connectionIDs.Add(connectionId);
                    SendMessageToAll($"{GetUserName(connectionId)} {connectionId} has connected.");
                    Debug.Log($"{GetUserName(connectionId)} {connectionId} has connected.");
                    break;
                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    SendMessageToAll($"Player {connectionId}: {message}");

                    dataTableUser.Add(connectionId, SetUserName(message));

                    Debug.Log($"Player {connectionId}: {message}");
                    break;
                case NetworkEventType.DisconnectEvent:
                    connectionIDs.Remove(connectionId);
                    SendMessageToAll($"Player {connectionId} has disconnected.");
                    Debug.Log($"Player {connectionId} has disconnected.");
                    break;
                case NetworkEventType.BroadcastEvent:
                    break;
            }
            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
            bufferSize, out dataSize, out error);
        }
    }
    public void SendMessageToAll(string message)
    {
        for (int i = 0; i < connectionIDs.Count; i++)
        {
            SendMessage(message, connectionIDs[i]);
        }
    }
    public void SendMessage(string message, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, message.Length * sizeof(char), out error);
        if ((NetworkError)error != NetworkError.Ok) Debug.Log((NetworkError)error);
    }
    public string GetUserName(int connectionId)
    {
        string userName = "Player";

        if (dataTableUser.Count>0)
        {
            userName = dataTableUser[connectionId].ToString();
        }

        return userName;
    }

    public string SetUserName(string message)
    {
        

        if (message != "")
        {
            string[] dataArray = message.Split(',');

            for (int i = 0; i < dataArray.Length; i++)
            {
                string[] dataItem = dataArray[i].Split(':');

                for (int x = 0; x < dataItem.Length; x++)
                {
                    if (dataItem[x].Replace('"', ' ').Trim() == "nameUser")
                    {
                        userName = dataItem[x + 1];
                        return userName;
                    }
                }
            }
        }

        return userName;
    }

}

class UserData
{
    public int ConnectionId { get; set; }
    public string NameUser { get; set; }
    public UserData()
    {
    }
    public UserData(int connectionId, string nameUser)
    {
        ConnectionId = connectionId;
        NameUser = nameUser;
    }
}

class ChangeUserData
{
    List<UserData> listData;
    UserData userData;
    public int ConnectionId { get; set; }
    public string NameUser { get; set; }
    public ChangeUserData(int connectionId, string nameUser)
    {
        listData = new List<UserData>();
        ConnectionId = connectionId;
        NameUser = nameUser;
    }

    public void SetData(int connectionId, string nameUser)
    {
        listData.Add(new UserData(connectionId, nameUser));
    }

    public void RemoveData (int index) 
    { 

    }

    public List<UserData> GetData()
    {
        return listData;
    }

    public UserData this[int index]
    {
        get { return listData[index]; }
    }
}

