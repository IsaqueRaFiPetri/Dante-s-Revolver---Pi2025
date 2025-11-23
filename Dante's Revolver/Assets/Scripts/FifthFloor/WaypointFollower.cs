using UnityEngine;
using Photon.Pun;
using System.Collections;
using TMPro;

public class WaypointFollower : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Movement Settings")]
    public float movementSpeed = 3.0f;
    public float rotationSpeed = 2.0f;
    public float waitTimeAtWaypoint = 1.0f;

    [Header("Start Conditions")]
    public MeetingPoint startMeetingPoint;
    public bool waitForAllPlayers = true;

    [Header("References")]
    public WaypointSystem waypointSystem;
    public TextMeshPro statusText; // Optional: para mostrar status

    // Network synchronized variables
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    private bool isWaiting = false;
    private bool canStart = false;
    private int playersReady = 0;
    private int totalPlayers = 0;

    // Local variables
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    // Public properties para acesso externo
    public bool CanStart => canStart;
    public bool IsMoving => isMoving;
    public int PlayersReady => playersReady;
    public int TotalPlayers => totalPlayers;

    private void Start()
    {
        // Only the master client controls the movement
        if (!PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.MasterClient);
        }

        // Initialize network sync
        networkPosition = transform.position;
        networkRotation = transform.rotation;

        // Start checking for players if master client
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(CheckStartConditions());
        }

        UpdateStatusText();
    }

    private void Update()
    {
        if (waypointSystem == null || waypointSystem.waypoints.Count == 0) return;

        // Only master client controls the movement logic
        if (PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            MasterUpdate();
        }
        else
        {
            // Non-master clients smoothly interpolate to network position
            SmoothNetworkMovement();
        }

        UpdateStatusText();
    }

    private void MasterUpdate()
    {
        if (!isMoving || !canStart) return;

        if (isWaiting) return;

        // Verifica se o waypoint atual é válido
        if (currentWaypointIndex >= waypointSystem.waypoints.Count || waypointSystem.waypoints[currentWaypointIndex] == null)
        {
            currentWaypointIndex = waypointSystem.GetNextWaypointIndex(currentWaypointIndex);
            return;
        }

        Vector3 targetPosition = waypointSystem.GetWaypointPosition(currentWaypointIndex);

        // Move towards current waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            movementSpeed * Time.deltaTime
        );

        // Rotate towards waypoint
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Check if reached waypoint
        if (waypointSystem.HasReachedWaypoint(transform.position, currentWaypointIndex))
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtWaypoint);

        // Move to next waypoint
        currentWaypointIndex = waypointSystem.GetNextWaypointIndex(currentWaypointIndex);
        isWaiting = false;
    }

    private IEnumerator CheckStartConditions()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds

            if (waitForAllPlayers && startMeetingPoint != null)
            {
                bool allPlayersReady = startMeetingPoint.AreAllPlayersInZone();
                playersReady = startMeetingPoint.PlayersInZoneCount();
                totalPlayers = startMeetingPoint.TotalPlayersCount();

                if (allPlayersReady && !canStart)
                {
                    canStart = true;
                    isMoving = true;
                    photonView.RPC("OnAllPlayersReady", RpcTarget.All);
                }
            }
            else
            {
                // If not waiting for players, can start immediately
                if (!canStart)
                {
                    canStart = true;
                    isMoving = true;
                }
            }
        }
    }

    [PunRPC]
    private void OnAllPlayersReady()
    {
        // This can be used for visual/audio effects when all players are ready
        Debug.Log("Todos os jogadores estão prontos! Iniciando movimento...");

        // Play sound effect, show message, etc.
        if (statusText != null)
        {
            statusText.color = Color.green;
        }
    }

    private void SmoothNetworkMovement()
    {
        transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdateStatusText()
    {
        if (statusText == null) return;

        if (!canStart)
        {
            statusText.text = $"Aguardando jogadores... {playersReady}/{totalPlayers}";
            statusText.color = Color.yellow;
        }
        else if (isMoving)
        {
            statusText.text = "Movendo...";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "Parado";
            statusText.color = Color.red;
        }
    }

    // Photon Network Synchronization
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We are the master client - send data to others
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(currentWaypointIndex);
            stream.SendNext(isMoving);
            stream.SendNext(isWaiting);
            stream.SendNext(canStart);
            stream.SendNext(playersReady);
            stream.SendNext(totalPlayers);
        }
        else
        {
            // We are a remote client - receive data from master
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            currentWaypointIndex = (int)stream.ReceiveNext();
            isMoving = (bool)stream.ReceiveNext();
            isWaiting = (bool)stream.ReceiveNext();
            canStart = (bool)stream.ReceiveNext();
            playersReady = (int)stream.ReceiveNext();
            totalPlayers = (int)stream.ReceiveNext();
        }
    }

    // Public methods to control movement
    [PunRPC]
    public void SetMovement(bool move)
    {
        isMoving = move;
    }

    [PunRPC]
    public void ForceStart()
    {
        // Força o início mesmo sem todos os jogadores (para debug ou casos especiais)
        canStart = true;
        isMoving = true;
    }

    [PunRPC]
    public void SetWaypointIndex(int index)
    {
        if (waypointSystem != null && index >= 0 && index < waypointSystem.waypoints.Count)
        {
            currentWaypointIndex = index;
        }
    }

    [PunRPC]
    public void ResetToStart()
    {
        currentWaypointIndex = 0;
        if (waypointSystem != null && waypointSystem.waypoints.Count > 0)
        {
            transform.position = waypointSystem.GetWaypointPosition(0);
        }
    }

    // Call these methods from other scripts to control the follower
    public void StartMovement()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetMovement(true);
        }
        else
        {
            photonView.RPC("SetMovement", RpcTarget.MasterClient, true);
        }
    }

    public void StopMovement()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetMovement(false);
        }
        else
        {
            photonView.RPC("SetMovement", RpcTarget.MasterClient, false);
        }
    }

    // Override do OnPlayerEnteredRoom para atualizar contagens
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateStatusText();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdateStatusText();
    }
}