using UnityEngine;

public class FinishAnimation : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void DesactiveAnimator()
    {
        Destroy(_animator);
    }
}
