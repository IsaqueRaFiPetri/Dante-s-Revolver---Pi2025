using UnityEngine;
using UnityEngine.Events;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] UnityEvent OnStep;
    public float radius = 15.0F;
    public float power = 80.0F;
    private void OnCollisionEnter(Collision collision)
    {
        print("hit");
        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            print("collide");
            Knockback();
            OnStep.Invoke();
        }
    }
    void Knockback()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rigidBody = hit.GetComponent<Rigidbody>();
            print("explosion");
            if (rigidBody != null)
                rigidBody.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }
}
