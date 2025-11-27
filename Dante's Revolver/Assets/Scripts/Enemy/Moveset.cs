using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Moveset : MonoBehaviour
{
    [SerializeField] bool isInfinite;
    [SerializeField] UnityEvent OnStartMoveset;
    [SerializeField] UnityEvent OnFinishMoveset;
    [SerializeField] float movesetDuration;
    [SerializeField] GameObject _childGameObject;
    [SerializeField] Vector3 _startPos, _endPos;
    [SerializeField] float _animationTime;
    private void Start()
    {
        if (isInfinite)
        {
            movesetDuration = Mathf.Infinity;
        }
    }
    public UnityEvent GetOnFinish()
    {
        return OnFinishMoveset;
    }
    public UnityEvent GetOnStart()
    {
        return OnStartMoveset;
    }
    public float MovesetDuration()
    {
        return movesetDuration;
    }
    public void Started()
    {
        print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        transform.localPosition += _startPos;
        _childGameObject.SetActive(true);
        transform.DOLocalMove(_endPos, _animationTime);
    }
    public void SetMoveAnimation()
    {
        transform.DOLocalMove(_startPos, _animationTime).onComplete = SetActive;
    }
    void SetActive()
    {
        _childGameObject.SetActive(false);
    }
    public void Clear()
    {

    }
}
