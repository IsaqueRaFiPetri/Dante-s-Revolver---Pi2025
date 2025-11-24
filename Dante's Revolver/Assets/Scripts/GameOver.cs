using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameOver : MonoBehaviourPunCallbacks
{
    public static GameOver instance;
    [SerializeField] bool _isOneDead;
    [SerializeField] UnityEvent OnGameOver;
    private void Awake()
    {
        instance = this;
    }
    [PunRPC]
    public void DetectGameOver()
    {
        if (_isOneDead)
        {
            OnGameOver.Invoke();
            print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==================================================");
        }
        else
        {
            print("NAAAADAAAAAAAAAAAA");
        }
    }
    [PunRPC]
    public bool SetIsOneDead(bool _isDead)
    {
        return _isOneDead = _isDead;
    }

}
