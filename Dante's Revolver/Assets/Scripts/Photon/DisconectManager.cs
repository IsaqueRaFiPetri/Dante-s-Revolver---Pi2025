using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DisconectManager : MonoBehaviourPunCallbacks
{
    public static DisconectManager instance;

    // Agora é um dicionário: cada sala tem sua lista de jogadores que devem voltar mortos
    public static Dictionary<string, HashSet<int>> intentionallyLeftPlayers
        = new Dictionary<string, HashSet<int>>();

    [SerializeField] UnityEvent OnPause, OnUnpause;
    bool isPaused;

    private void Awake()
    {
        instance = this;
    }

    void SetCursor()
    {
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = isPaused;
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPause.Invoke();
            SetPaused(true);
            SetCursor();
        }
        if (context.canceled)
        {
            OnUnpause.Invoke();
            SetPaused(false);
            SetCursor();
        }
    }

    bool SetPaused(bool _paused)
    {
        return isPaused = _paused;
    }

    public void Disconnect(string sceneName)
    {
        RegisterPlayerAsDeadInRoom();
        photonView.RPC("KickAllPlayersToMenu", RpcTarget.AllBuffered, sceneName);
    }

    public void LeaveRoom(string sceneName)
    {
        RegisterPlayerAsDeadInRoom();
        photonView.RPC("KickAllPlayersToLobby", RpcTarget.AllBuffered, sceneName);
    }

    public void DisconnectAndQuit()
    {
        RegisterPlayerAsDeadInRoom();
        photonView.RPC("KickAllPlayersToMenu", RpcTarget.AllBuffered, "menu");
        Application.Quit();
    }

    private void RegisterPlayerAsDeadInRoom()
    {
        if (PhotonNetwork.LocalPlayer != null && PhotonNetwork.CurrentRoom != null)
        {
            string roomName = PhotonNetwork.CurrentRoom.Name;

            if (!intentionallyLeftPlayers.ContainsKey(roomName))
                intentionallyLeftPlayers[roomName] = new HashSet<int>();

            intentionallyLeftPlayers[roomName].Add(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
    [PunRPC]
    public void KickAllPlayersToMenu(string sceneName)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneName);
    }
    [PunRPC]
    public void KickAllPlayersToLobby(string sceneName)
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(sceneName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected: " + cause);
    }
}
