using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class OnGameStart : MonoBehaviour
{
    [SerializeField] UnityEvent isOnLobby;
    private void Awake()
    {
        //Application.targetFrameRate = 90;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (PhotonNetwork.InLobby)
        {
            print("isConnected");
            isOnLobby.Invoke();
        }
    }
}
