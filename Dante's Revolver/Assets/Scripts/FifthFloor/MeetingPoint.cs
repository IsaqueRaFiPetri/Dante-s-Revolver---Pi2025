using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class MeetingPoint : MonoBehaviourPunCallbacks
{
    [Header("Meeting Point Settings")]
    public float checkRadius = 3.0f;
    public LayerMask playerLayer = 1; // Default layer
    public string requiredTag = "Player"; // Tag para identificar jogadores

    [Header("Visualization")]
    public bool showGizmos = true;
    public Color gizmoColor = Color.green;

    private HashSet<int> playersInZone = new HashSet<int>();

    public bool AreAllPlayersInZone()
    {
        if (PhotonNetwork.InRoom)
        {
            return playersInZone.Count >= PhotonNetwork.CurrentRoom.PlayerCount;
        }
        return false;
    }

    public int PlayersInZoneCount()
    {
        return playersInZone.Count;
    }

    public int TotalPlayersCount()
    {
        return PhotonNetwork.CurrentRoom?.PlayerCount ?? 0;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        CheckPlayersInZone();
    }

    private void CheckPlayersInZone()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius, playerLayer);
        HashSet<int> currentPlayers = new HashSet<int>();

        foreach (Collider collider in colliders)
        {
            if (string.IsNullOrEmpty(requiredTag) || collider.CompareTag(requiredTag))
            {
                PhotonView photonView = collider.GetComponent<PhotonView>();
                if (photonView != null && photonView.IsMine)
                {
                    currentPlayers.Add(photonView.OwnerActorNr);
                }
            }
        }

        // Update players in zone
        playersInZone = currentPlayers;

        // Sync with other clients
        photonView.RPC("UpdatePlayersInZone", RpcTarget.Others, playersInZone.Count);
    }

    [PunRPC]
    private void UpdatePlayersInZone(int count)
    {
        // Non-master clients receive updated count
        // Para simplificar, usamos apenas a contagem para UI
        playersInZone.Clear();
        // Em uma implementação mais complexa, você poderia sincronizar quais jogadores específicos
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, checkRadius);

        // Draw icon or text
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.3f);
        Gizmos.DrawSphere(transform.position, checkRadius * 0.1f);
    }
}