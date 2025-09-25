using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Elevator : MonoBehaviour, IKillable
{
    [SerializeField] GameObject _elevatorObj;
    [SerializeField] UnityEvent OnClick;
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void TakeDamage(int damage)
    {
        OnClick.Invoke();
    }
    public void SetPos(float _setPos)
    {
        transform.DOMoveZ(_setPos, .25f);
    }
}
