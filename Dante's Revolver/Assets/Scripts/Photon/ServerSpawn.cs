using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;

public class ServerSpawn : MonoBehaviour
{
    public List<GameObject> playerList;
    [SerializeField] GameObject playerPrefab;
    private void Start()
    {
        if(PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
        {
            if(PlayerMovementAdvanced.LocalPlayerInstance == null)
            {
                playerList.Add(PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity));
                playerList[playerList.Count - 1].GetComponentInChildren<Camera>().enabled = true;
            }
        }
    }
}
