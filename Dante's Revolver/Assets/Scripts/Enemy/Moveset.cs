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

    private void OnEnable()
    {
        transform.position = _startPos;
    }
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
    public void SetMoveAnimation(bool _isStartScene)
    {
        print("start_");
        if (_isStartScene)
        {
            transform.DOMove(_endPos, _animationTime).SetEase(Ease.InQuad).onComplete = SetActive;
        }
        else
        {
            transform.DOMove(_startPos, _animationTime).SetEase(Ease.InQuad).onComplete = SetActive;
        }
    }
    void SetActive()
    {
        _childGameObject.SetActive(!_childGameObject.activeSelf);
    }
    public void Clear()
    {

    }
}
