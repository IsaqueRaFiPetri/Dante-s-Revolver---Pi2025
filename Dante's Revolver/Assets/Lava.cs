using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] float _inpuseForce;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Rigidbody _rb))
        {
            _rb.AddForce(transform.up * _inpuseForce, ForceMode.Impulse);
        }
    }
}
