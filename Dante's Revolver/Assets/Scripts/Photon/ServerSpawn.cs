using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class ServerSpawn : MonoBehaviourPunCallbacks
{
    public List<GameObject> playerList;
    public List<int> playerIdList;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject deadPlayerPrefab;

    public static ServerSpawn instance;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator Start()
    {
        //yield return new WaitUntil(() => PhotonNetwork.InRoom);

        while (!PhotonNetwork.InRoom)
        {
            yield return null;
        }
        int actorId = PhotonNetwork.LocalPlayer.ActorNumber;
        string roomName = PhotonNetwork.CurrentRoom.Name;
        bool shouldSpawnDead =
            DisconectManager.intentionallyLeftPlayers.ContainsKey(roomName) &&
            DisconectManager.intentionallyLeftPlayers[roomName].Contains(actorId);
        GameObject prefabToSpawn = playerPrefab;
        
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, _spawnPoints[playerList.Count].position, Quaternion.identity, 0);
        print(player + "=============================================================================================");
        playerList.Add(player);
        playerIdList.Add(player.GetPhotonView().ViewID);

        if (player.GetComponentInChildren<Camera>() != null && player.GetComponent<PhotonView>().IsMine)
            player.GetComponentInChildren<Camera>().enabled = true;
    }
}
