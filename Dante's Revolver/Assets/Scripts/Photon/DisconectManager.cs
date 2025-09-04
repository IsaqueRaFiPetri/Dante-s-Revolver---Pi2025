using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DisconectManager : MonoBehaviourPunCallbacks
{
    public static DisconectManager instance;
    public static HashSet<int> intentionallyLeftPlayers = new HashSet<int>();

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
        if (PhotonNetwork.LocalPlayer != null)
            intentionallyLeftPlayers.Add(PhotonNetwork.LocalPlayer.ActorNumber);

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneName);
    }

    public void LeaveRoom(string sceneName)
    {
        if (PhotonNetwork.LocalPlayer != null)
            intentionallyLeftPlayers.Add(PhotonNetwork.LocalPlayer.ActorNumber);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(sceneName);
    }

    public void DisconnectAndQuit()
    {
        if (PhotonNetwork.LocalPlayer != null)
            intentionallyLeftPlayers.Add(PhotonNetwork.LocalPlayer.ActorNumber);

        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected: " + cause);
    }
}
