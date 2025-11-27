using UnityEngine;
using UnityEngine.Events;

public class AnimationEventSet : MonoBehaviour
{
    [SerializeField] UnityEvent _AnimationEvent;

    public void AnimationEvent()
    {
        _AnimationEvent.Invoke();
    }
}
