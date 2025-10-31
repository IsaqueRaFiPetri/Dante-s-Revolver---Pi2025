using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [Header("Scene and Player Settings")]
    [SerializeField] private string loadingSceneName = "Loading";
    [SerializeField] private string playerPrefabName = "PlayerTeste";
    [SerializeField] private Transform[] spawnPoints;

    [Header("Timer Reference")]

    private bool playerSpawned = false;
    private bool timerStarted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (PhotonNetwork.InRoom)
            SpawnPlayer();
    }

    private void Start()
    {
        if (PhotonNetwork.InRoom && !playerSpawned)
            SpawnPlayer();
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerSpawned)
            return;

        if (spawnPoints == null || spawnPoints.Length == 0)
            return;

        int index = PhotonNetwork.LocalPlayer.ActorNumber % spawnPoints.Length;
        Transform point = spawnPoints[index];

        if (point == null)
            return;

        PhotonNetwork.Instantiate(playerPrefabName, point.position, point.rotation);
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        playerSpawned = true;
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        else
            SceneManager.LoadScene(loadingSceneName);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(loadingSceneName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(loadingSceneName);
    }

    public string GetRoomName()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null)
            return PhotonNetwork.CurrentRoom.Name;
        return string.Empty;
    }
}
