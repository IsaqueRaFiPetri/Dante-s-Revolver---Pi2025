using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorObj : MonoBehaviour
{
    [SerializeField] bool isInitialElevator;
    [SerializeField] List<Rigidbody> _objInsideElevator;
    [SerializeField] UnityEvent OnStart, OnCloseElevatorDoor;
    private void OnBecameInvisible()
    {
        if (isInitialElevator)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        OnStart.Invoke();
    }
    public void CloseDoors()
    {
        OnCloseElevatorDoor.Invoke();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out PhotonView _pv))
        {
            _objInsideElevator.Add(_pv.gameObject.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerExit(Collider collision)
    {
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
