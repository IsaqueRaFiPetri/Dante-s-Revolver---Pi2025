using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Cage : DamageInteraction
{
    bool canFall;
    [SerializeField] UnityEvent OnExitTrigger;
    [SerializeField] Rigidbody _rb;
    [SerializeField] bool _canKill;
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
            if (_canKill)
            {
                print(_iKillable.GetHashCode());
                DoDamage(_iKillable);
            }
        }
    }
    public void Fall()
    {
        _rb.DOMoveY(_finalPos.y, .25f).SetEase(Ease.InQuart).onComplete = GetUp;
        _canKill = true;
    }
    void GetUp()
    {
        _rb.DOMoveY(_startPos.y, 4f).SetEase(Ease.Linear);
        _canKill = false;
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
