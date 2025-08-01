using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

public class ServerSpawn : MonoBehaviourPunCallbacks
{
    public List<GameObject> playerList;
    public List<int> playerIdList;
    [SerializeField] GameObject playerPrefab;

    public static ServerSpawn instance;
    public void Awake()
    {
        instance = this;
    }
    IEnumerator Start()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        playerList.Add(PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity));
        playerIdList.Add(playerList[playerList.Count - 1].GetPhotonView().ViewID);
        playerList[playerList.Count - 1].GetComponentInChildren<Camera>().enabled = true;
    }
}
