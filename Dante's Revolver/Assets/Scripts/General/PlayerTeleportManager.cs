using UnityEngine;
using Photon.Pun;

public class PlayerTeleportManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private string playerTag = "Player";

    [Header("Debug")]
    [SerializeField] private int playersInArea = 0;
    [SerializeField] private bool firstPlayerEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag(playerTag))
        {
            PhotonView playerPhotonView = other.GetComponent<PhotonView>();
            if (playerPhotonView == null || !playerPhotonView.IsMine) return;

            playersInArea++;

            if (playersInArea <= 1 && !firstPlayerEntered)
            {
                firstPlayerEntered = true;
            }
            else if (playersInArea >= 2)
            {
                TeleportPlayer(other.gameObject);
            }
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        photonView.RPC("RPC_TeleportPlayer", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    private void RPC_TeleportPlayer(int playerViewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(playerViewID);
        if (targetPhotonView != null)
        {
            GameObject player = targetPhotonView.gameObject;
            player.transform.position = teleportDestination.position;
            player.transform.rotation = teleportDestination.rotation;

            Debug.Log($"Jogador {player.name} teleportado para {teleportDestination.name}");
        }
    }
}