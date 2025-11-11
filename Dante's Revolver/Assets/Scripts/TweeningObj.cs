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
    [SerializeField] float _duration;
    public void ApplyTweenTo(Transform _objTransform)
    {
        switch (_tweenType)
        {
            case TweenType.Position:
                _objTransform.DOMove(_finalVector, _duration);
                break;
            case TweenType.Scale:
                _objTransform.DOScale(_finalVector, _duration);
                break;
            case TweenType.Rotation:
                _objTransform.DOLocalRotate(_finalVector, _duration);
                break;
        }
    }
}
