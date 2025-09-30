using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateTwoPeople : MonoBehaviour
{
    [SerializeField] List<PhotonView> _pvList;
    [SerializeField] UnityEvent OnClick, OnUnclick, OnAllPlayersClicking;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PhotonView _pv))
        {
            _pvList.Add(_pv);
            OnClick.Invoke();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PhotonView _pv))
        {
            _pvList.Remove(_pv);
            OnUnclick.Invoke();
        }
    }
    public void DetectPlayers()
    {
        if(_pvList.Count >= 2)
        {
            OnAllPlayersClicking.Invoke();
        }
    }
}
