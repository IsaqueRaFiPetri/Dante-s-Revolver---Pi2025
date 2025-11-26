using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorObj : MonoBehaviour
{
    [SerializeField] bool isInitialElevator;
    [SerializeField] List<Rigidbody> _objInsideElevator;
    [SerializeField] List<PlayerMovementAdvanced> _players;
    [SerializeField] UnityEvent OnStart, OnCloseElevatorDoor;
    private void OnEnable()
    {
        if (isInitialElevator)
        {
            gameObject.SetActive(true);
        }
    }
    private void OnBecameInvisible()
    {
        if (isInitialElevator)
        {
            gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        OnStart.Invoke();
    }
    public void CloseDoors()
    {
        if(_players.Count >= 2)
        {
            OnCloseElevatorDoor.Invoke();
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovementAdvanced _player))
        {
            _players.Add(_player);
        }
        if (collision.gameObject.TryGetComponent(out PhotonView _pv))
        {
            _objInsideElevator.Add(_pv.gameObject.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovementAdvanced _player))
        {
            _players.Remove(_player);
        }
        if (collision.gameObject.TryGetComponent(out PhotonView _pv))
        {
            _objInsideElevator.Remove(_pv.gameObject.GetComponent<Rigidbody>());
        }
    }
    public List<Rigidbody> GetPlayers()
    {
        return _objInsideElevator;
    }
}
