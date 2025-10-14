using UnityEngine;
using UnityEngine.Events;

public enum ButtonType
{
    OneTimePress, AlwaysPress
}
public class OnePersonButton : MonoBehaviour
{
    [SerializeField] ButtonType _buttonType;
    [SerializeField] UnityEvent OnPress, OnUnpress;
    bool _alreadyPressed;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Rigidbody _rbObj))
        {
            if (!_alreadyPressed)
            {
                OnPress.Invoke();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody _rbObj))
        {
            if (!_alreadyPressed)
            {
                OnUnpress.Invoke();
            }
        }
    }
    public void Pressing()
    {
        switch (_buttonType)
        {
            case ButtonType.OneTimePress:
                _alreadyPressed = true;
                break;
            case ButtonType.AlwaysPress:
                break;
        }
    }
}
