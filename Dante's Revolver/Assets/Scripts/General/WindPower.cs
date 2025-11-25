using UnityEditor;
using UnityEngine;

public class WindPower : MonoBehaviour
{
    [SerializeField] float windForce;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody _rb))
        {
            _rb.AddForce(transform.forward * windForce, ForceMode.Acceleration);
        }
    }
}
