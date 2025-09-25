using DG.Tweening;
using UnityEngine;

public class Elevator : MonoBehaviour, IKillable
{
    float _initialPos;
    [SerializeField] float _clikedPos;
    [SerializeField] GameObject _elevatorObj;
    [SerializeField] bool isInitialElevator;

    private void Start()
    {
        _initialPos = transform.position.z;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public void TakeDamage(int damage)
    {
        SetPos(_clikedPos);
    }
    void SetPos(float _setPos)
    {
        transform.DOMoveZ(_setPos, .25f);
    }
}
