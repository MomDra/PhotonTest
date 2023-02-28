using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager3 : MonoBehaviourPunCallbacks
{

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.InLobby);
        Debug.Log(PhotonNetwork.InRoom);
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.InLobby);
        Debug.Log(PhotonNetwork.InRoom);
        PhotonNetwork.Instantiate("Player", Vector3.up, Quaternion.identity);
    }
}
