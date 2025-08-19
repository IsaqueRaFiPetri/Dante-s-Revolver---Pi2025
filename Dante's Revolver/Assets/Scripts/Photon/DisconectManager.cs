using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class DisconectManager : MonoBehaviour
{
    [SerializeField] UnityEvent OnPause, OnUnpause;
    bool isPaused;

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
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneName);
    }
    public void LeaveRoom(string sceneName)
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(sceneName);
    }
    public void ReturnLobby(string sceneName)
    {
        PhotonNetwork.JoinLobby();
        SceneManager.LoadScene(sceneName);
    }

    public void DisconnectAndQuit()
    {
        Application.Quit();
    }
}
