using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] UnityEvent OnLobbyLoaded;

    public void LoadingScreen()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void DisconnectFromLobby()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        OnLobbyLoaded.Invoke();
    }
}
