using UnityEngine;
using UnityEngine.Events;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] UnityEvent OnStep;
    private void OnCollisionEnter(Collision collision)
    {
        print("hit");
        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            print("collide hit");
            rb.AddForce(transform.forward * 10, ForceMode.Impulse);
            rb.AddForce(transform.up * 20, ForceMode.Impulse);

            OnStep.Invoke();
        }
    }
}
