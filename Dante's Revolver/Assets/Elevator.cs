using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Elevator : MonoBehaviour, IKillable
{
    [SerializeField] UnityEvent OnClick;
    [SerializeField] Transform[] _elevatorPositions;
    [SerializeField] ElevatorObj _elevatorObj;
    private void Start()
    {
        _elevatorObj = GetComponentInParent<ElevatorObj>();
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        OnClick.Invoke();
    }
    public void SetPlayerPos()
    {
        print("teleported");
        for (int i = 0; i < _elevatorObj.GetPlayers().Count; i++)
        {
            _elevatorObj.GetPlayers()[i].GetComponent<Collider>().enabled = false;
            _elevatorObj.GetPlayers()[i].MovePosition(new Vector3(_elevatorPositions[1].position.x, _elevatorPositions[1].position.y, _elevatorPositions[1].position.z));
            _elevatorObj.GetPlayers()[i].GetComponent<Collider>().enabled = true;
        }
    }
}
