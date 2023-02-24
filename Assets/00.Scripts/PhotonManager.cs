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

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);

        connectToMaster.onClick.AddListener(ConnectToMaster);
        disconnectToServer.onClick.AddListener(DisconnectToServer);
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
}
