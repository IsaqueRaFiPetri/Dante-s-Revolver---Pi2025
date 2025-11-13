using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateTwoPeople : MonoBehaviour
{
    [SerializeField] List<PlayerMovementAdvanced> _pvList;
    [SerializeField] UnityEvent OnClick, OnUnclick, OnAllPlayersClicking;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerMovementAdvanced _pv))
        {
            _pvList.Add(_pv);
            OnClick.Invoke();
            print("V: IPlayable");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovementAdvanced _pv))
        {
            _pvList.Remove(_pv);
            OnUnclick.Invoke();
            print("X: IPlayable");
        }
    }
    public void DetectPlayers()
    {
        if(!PhotonNetwork.IsConnected)
        {
            OnAllPlayersClicking.Invoke();
        }
        if(_pvList.Count >= 2)
        {
            OnAllPlayersClicking.Invoke();
            print(_pvList.Count +  " -> PLAYERS INTERACTING");
        }
    }
}
