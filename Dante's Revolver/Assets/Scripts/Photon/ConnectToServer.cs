using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Events;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] UnityEvent OnLobbyLoaded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        if (PhotonNetwork.InLobby)
        {
            print("IsAlreadyConnected");
        }
    }

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
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
