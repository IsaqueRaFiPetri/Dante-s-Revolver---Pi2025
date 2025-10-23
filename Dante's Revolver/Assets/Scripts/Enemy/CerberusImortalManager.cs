using UnityEngine;

public class CerberusImortalManager : MonoBehaviour
{
    [SerializeField] bool isDiving;
    [SerializeField] Animator _diveAnimation;
    [SerializeField] string _boolName;

    public void SetIsDiving(bool _isDiving)
    {
        isDiving = _isDiving;
        SetAnimator();
    }
    void SetAnimator()
    {
        _diveAnimation.SetBool(_boolName, isDiving);
    }
}
