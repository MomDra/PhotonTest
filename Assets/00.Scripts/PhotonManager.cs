using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI statusText;

    [SerializeField]
    private TMP_InputField roomInput;
    [SerializeField]
    private TMP_InputField nickNameInput;

    [SerializeField]
    private Button connectToMaster;
    [SerializeField]
    private Button disconnectToServer;
    [SerializeField]
    private Button joinLobbyButton;
    [SerializeField]
    private Button createRoomButton;
    [SerializeField]
    private Button joinRoomButton;
    [SerializeField]
    private Button joinOrCreateRoomButton;
    [SerializeField]
    private Button joinRandomRoomButton;
    [SerializeField]
    private Button leaveRoomButton;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);

        connectToMaster.onClick.AddListener(ConnectToMaster);
        disconnectToServer.onClick.AddListener(DisconnectToServer);
        joinLobbyButton.onClick.AddListener(JoinLobby);
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
        joinOrCreateRoomButton.onClick.AddListener(JoinOrCreateRoom);
        joinRandomRoomButton.onClick.AddListener(JoinRandomRoom);
        leaveRoomButton.onClick.AddListener(LeaveRoom);
    }

    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
    }

    private void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master!");
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
    }

    private void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    private void DisconnectToServer()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected to server? maybe?");
    }

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInput.text);
    }

    private void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    }

    private void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Making room is completed");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("JoinRoom is completed");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Making room is failed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Joing room is failed");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join random room is failed");
    }
}
