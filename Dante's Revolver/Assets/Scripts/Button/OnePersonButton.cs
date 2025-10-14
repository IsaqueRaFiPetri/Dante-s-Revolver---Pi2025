using UnityEngine;
using UnityEngine.Events;

public class OnePersonButton : MonoBehaviour
{
    [SerializeField] UnityEvent OnPress, OnUnpress;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Rigidbody _rbObj))
        {
            OnPress.Invoke();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody _rbObj))
        {
            OnUnpress.Invoke();
        }
    }
}
