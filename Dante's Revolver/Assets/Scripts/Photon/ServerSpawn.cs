using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class ServerSpawn : MonoBehaviourPunCallbacks
{
    public List<GameObject> playerList;
    public List<int> playerIdList;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject deadPlayerPrefab;

    public static ServerSpawn instance;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);

        int actorId = PhotonNetwork.LocalPlayer.ActorNumber;
        string roomName = PhotonNetwork.CurrentRoom.Name;
        /*
        bool shouldSpawnDead =
            DisconectManager.intentionallyLeftPlayers.ContainsKey(roomName) &&
            DisconectManager.intentionallyLeftPlayers[roomName].Contains(actorId);
        */
        GameObject prefabToSpawn = playerPrefab;
        
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 50, 0), Quaternion.identity);
        print(player + "=============================================================================================");
        playerList.Add(player);
        playerIdList.Add(player.GetPhotonView().ViewID);

        if (player.GetComponentInChildren<Camera>() != null && player.GetComponent<PhotonView>().IsMine)
            player.GetComponentInChildren<Camera>().enabled = true;
    }
}
