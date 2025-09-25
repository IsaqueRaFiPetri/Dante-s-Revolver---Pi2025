using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Elevator : MonoBehaviour, IKillable
{
    [SerializeField] UnityEvent OnClick;
    [SerializeField] Transform[] _elevatorPositions;
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        OnClick.Invoke();
    }
    public void SetPos(float _setPos)
    {
        transform.DOLocalMoveZ(_setPos, .25f);
    }
}
