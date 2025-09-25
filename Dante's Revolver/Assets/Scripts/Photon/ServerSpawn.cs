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

        bool shouldSpawnDead =
            DisconectManager.intentionallyLeftPlayers.ContainsKey(roomName) &&
            DisconectManager.intentionallyLeftPlayers[roomName].Contains(actorId);

        GameObject prefabToSpawn = shouldSpawnDead ? deadPlayerPrefab : playerPrefab;

        GameObject player = PhotonNetwork.Instantiate(prefabToSpawn.name, new Vector3(0, 1, 0), Quaternion.identity);
        playerList.Add(player);
        playerIdList.Add(player.GetPhotonView().ViewID);

        if (player.GetComponentInChildren<Camera>() != null && player.GetComponent<PhotonView>().IsMine)
            player.GetComponentInChildren<Camera>().enabled = true;

        // Limpa o estado se spawnou morto
        if (shouldSpawnDead)
        {
            DisconectManager.intentionallyLeftPlayers[roomName].Remove(actorId);
        }
    }
}
