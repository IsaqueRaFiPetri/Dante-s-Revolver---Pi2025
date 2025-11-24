using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameOver : MonoBehaviour
{
    public static GameOver instance;
    [SerializeField] UnityEvent OnGameOver;
    [SerializeField] List<GameObject> _phantonsInGame;
    private void Awake()
    {
        instance = this;
    }
    void DetectGameOver()
    {
        if (_phantonsInGame.Count >= 2)
        {
            OnGameOver.Invoke();
            print("GameOver========================");
        }
        else
        {
            print("Phantoms: " + _phantonsInGame.Count);
        }
    }
    public void AddToDeathList(GameObject _phantom)
    {
        _phantonsInGame.Add(_phantom);
        DetectGameOver();
    }
    public void RemoveFromDeathList(GameObject _phantom)
    {
        _phantonsInGame.Remove(_phantom);
    }

}
