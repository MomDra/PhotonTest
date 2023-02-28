using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    private PlayerFollow playerFollow;

    private void Awake()
    {
        playerFollow = FindObjectOfType<PlayerFollow>();
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, transform.rotation);

        playerFollow.SetCameratarget(player.transform);
    }
}
