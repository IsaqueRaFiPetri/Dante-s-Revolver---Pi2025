using DG.Tweening;
using UnityEngine;

public enum TweenType
{
    Position, Scale, Rotation
}
public class TweeningObj : MonoBehaviour
{
    [SerializeField] TweenType _tweenType;
    [SerializeField] Vector3 _finalVector;
    [SerializeField] Vector3 _initialVector;
    [SerializeField] float _duration;
    [SerializeField] bool _gatherVectorInStart;
    private void Start()
    {
        if (_gatherVectorInStart)
        {
            GatherInitialVector();
        }
    }
    public void GatherInitialVector()
    {
        switch (_tweenType)
        {
            case TweenType.Position:
                _initialVector = transform.position;
                break;
            case TweenType.Scale:
                _initialVector = transform.localScale;
                break;
            case TweenType.Rotation:
                _initialVector = transform.localEulerAngles;
                break;
        }
    }
    public void ApplyTweenTo()
    {
        switch (_tweenType)
        {
            case TweenType.Position:
                transform.DOMove(_finalVector, _duration);
                break;
            case TweenType.Scale:
                transform.DOScale(_finalVector, _duration);
                break;
            case TweenType.Rotation:
                transform.DOLocalRotate(_finalVector, _duration);
                break;
        }
    }
    public void ResetTween()
    {
        switch (_tweenType)
        {
            case TweenType.Position:
                transform.DOMove(_initialVector, _duration);
                break;
            case TweenType.Scale:
                transform.DOScale(_initialVector, _duration);
                break;
            case TweenType.Rotation:
                transform.DOLocalRotate(_initialVector, _duration);
                break;
        }
    }
}
