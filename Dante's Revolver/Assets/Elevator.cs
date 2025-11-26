using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Elevator : MonoBehaviour, IKillable
{
    [SerializeField] UnityEvent OnClick;
    [SerializeField] Transform[] _elevatorPositions;
    [SerializeField] ElevatorObj _elevatorObj;
    [SerializeField] Vector2 _teleportPos;
    private void Start()
    {
        _elevatorObj = GetComponentInParent<ElevatorObj>();

        _teleportPos.x = _elevatorPositions[1].position.x - _elevatorPositions[0].position.x;
        _teleportPos.y = _elevatorPositions[1].position.z - _elevatorPositions[0].position.z;
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
            _elevatorObj.GetPlayers()[i].MovePosition(new Vector3 (_elevatorObj.GetPlayers()[i].transform.position.x + _teleportPos.x, _elevatorObj.GetPlayers()[i].transform.position.y, _elevatorObj.GetPlayers()[i].transform.position.z + _teleportPos.y));
            _elevatorObj.GetPlayers()[i].GetComponent<Collider>().enabled = true;
        }
    }
}
