using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Cage : DamageInteraction
{
    bool canFall;
    [SerializeField] UnityEvent OnExitTrigger;
    Vector3 _startPos;
    [SerializeField] Vector3 _finalPos;
    private void Start()
    {
        _startPos = transform.position;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovementAdvanced _player))
        {
            DetectCanFall();
            if (canFall)
            {
                OnExitTrigger.Invoke();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out IKillable _iKillable))
        {
            print(_iKillable.GetHashCode());
            DoDamage(_iKillable);
        }
    }
    public void Fall()
    {
        transform.DOMoveY(_finalPos.y, .25f).SetEase(Ease.InQuart).onComplete = GetUp;
    }
    void GetUp()
    {
        transform.DOMoveY(_startPos.y, 4f).SetEase(Ease.Linear);
    }
    void DetectCanFall()
    {
        if(transform.position.y !=  _startPos.y)
        {
            canFall = false;
        }
        else
        {
            canFall = true;
        }
    }
}
