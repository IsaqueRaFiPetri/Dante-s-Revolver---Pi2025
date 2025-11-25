using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class Floor1ElevatorLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] bool _onePlayerEntered;
    [SerializeField] UnityEvent OnAllPlayersEntered;
    private void Start()
    {
        photonView.RPC("AddPlayer", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void AddPlayer()
    {
        if (!_onePlayerEntered)
        {
            _onePlayerEntered = true;
        }
        else
        {
            OnAllPlayersEntered.Invoke();
        }
    }
}
