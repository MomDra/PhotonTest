using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager2 : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    [SerializeField]
    private TMP_InputField nickNameInputField;
    [SerializeField]
    private Button connectButton;

    [Header("LobbyPanel")]
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private TMP_InputField roomInputField;
    [SerializeField]
    private TextMeshProUGUI welcomeText;
    [SerializeField]
    private TextMeshProUGUI lobbyInfoText;
    [SerializeField]
    private Button[] cellBtns;
    [SerializeField]
    private Button createRoomButton;
    [SerializeField]
    private Button quickStartButton;
    [SerializeField]
    private Button disconnectButton;

    [Header("RoomPanel")]
    [SerializeField]
    private GameObject roomPanel;
    [SerializeField]
    private TextMeshProUGUI playerNickNamesText;
    [SerializeField]
    private TextMeshProUGUI roomInfoText;
    [SerializeField]
    private TextMeshProUGUI[] chatTexts;
    [SerializeField]
    private TMP_InputField chatInputField;
    [SerializeField]
    private Button leaveRoomButton;
    [SerializeField]
    private Button sendButton;
    private int chatCount;

    [Header("ETC")]
    [SerializeField]
    private TextMeshProUGUI statusText;
    [SerializeField]
    private PhotonView pv;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        lobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";

        connectButton.onClick.AddListener(Connect);
        createRoomButton.onClick.AddListener(CreateRoom);
        quickStartButton.onClick.AddListener(JoinRandomRoom);
        disconnectButton.onClick.AddListener(Disconnect);
        leaveRoomButton.onClick.AddListener(LeaveRoom);
        sendButton.onClick.AddListener(Send);

        for (int i = 0; i < cellBtns.Length; ++i)
        {
            int index = i;
            cellBtns[i].onClick.AddListener(() => OnCellClicked(index));
        }
    }

    private void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();
        lobbyInfoText.text = $"{PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms} 로비 / {PhotonNetwork.CountOfPlayers} 접속";
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined in lobby");

        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = nickNameInputField.text;
        welcomeText.text = $"{PhotonNetwork.LocalPlayer.NickName} 님 환영합니다";
        myList.Clear();
    }

    private void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected to master");

        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
    }

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomInputField.text == "" ? "Room" + Random.Range(0, 100) : roomInputField.text, new RoomOptions { MaxPlayers = 4 });
    }

    private void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void OnCellClicked(int index)
    {
        PhotonNetwork.JoinRoom(myList[index].Name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; ++i)
        {
            Debug.Log($"{i}: {roomList[i].Name}, {roomList[i].RemovedFromList}");

            bool isContain = myList.Contains(roomList[i]);

            if (!roomList[i].RemovedFromList)
            {
                if (!isContain)
                {
                    myList.Add(roomList[i]);
                }
                else
                {
                    myList[myList.IndexOf(roomList[i])] = roomList[i];
                }
            }
            else
            {
                myList.Remove(roomList[i]);
            }
        }

        RommListRenewal();
    }

    private void RommListRenewal()
    {
        for (int i = 0; i < cellBtns.Length; ++i)
        {
            bool isActive = i < myList.Count;

            cellBtns[i].interactable = isActive ? true : false;
            cellBtns[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = isActive ? myList[i].Name : "";
            cellBtns[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = isActive ? $"{myList[i].PlayerCount} / {myList[i].MaxPlayers}" : "";
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined in room");

        roomPanel.SetActive(true);
        RoomRenewal();
        ClearChat();
    }

    private void ClearChat()
    {
        chatCount = 0;
        chatInputField.text = "";
        foreach (TextMeshProUGUI textUGUI in chatTexts)
        {
            textUGUI.text = "";
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Creating room is failed. code: {returnCode}, message: {message}");
        Debug.Log("Retry creating room");
        roomInfoText.text = "";
        CreateRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Joining random room is failed. code: {returnCode}, message: {message}");
        Debug.Log("Try creatingroom");
        roomInfoText.text = "";
        CreateRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} entered in room");
        RoomRenewal();
        ChatRPC($"<color=yellow>{newPlayer.NickName} has entered in room");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} left room");
        RoomRenewal();
        ChatRPC($"<color=yellow>{otherPlayer.NickName} has left out room");
    }

    private void RoomRenewal()
    {
        playerNickNamesText.text = "";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerNickNamesText.text += player.NickName + ", ";
        }

        roomInfoText.text = $"{PhotonNetwork.CurrentRoom.Name} / {PhotonNetwork.CurrentRoom.PlayerCount}명 / {PhotonNetwork.CurrentRoom.MaxPlayers} 최대";
    }

    private void Send()
    {
        pv.RPC("ChatRPC", RpcTarget.All, $"{PhotonNetwork.NickName} : {chatInputField.text}");
    }

    [PunRPC]
    private void ChatRPC(string msg)
    {
        if (!isChatFull())
        {
            chatTexts[chatCount].text = msg;
            ++chatCount;
        }
        else
        {
            for (int i = 1; i < chatTexts.Length; ++i)
            {
                chatTexts[i - 1].text = chatTexts[i].text;
            }
            chatTexts[chatTexts.Length - 1].text = msg;
        }
    }

    private bool isChatFull()
    {
        return chatCount == chatTexts.Length;
    }
}
