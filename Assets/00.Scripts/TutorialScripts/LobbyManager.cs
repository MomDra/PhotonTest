using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField createRoomInputField;
    [SerializeField]
    private TMP_InputField joinRoomInputField;

    [SerializeField]
    private Button createRoomButton;
    [SerializeField]
    private Button joinRoomButton;

    [SerializeField]
    private int maxPlayers;

    private void Awake()
    {
        createRoomButton.onClick.AddListener(CreateRoom);
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayers;

        PhotonNetwork.CreateRoom(createRoomInputField.text, roomOptions);
    }

    private void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Demo");
    }
}
