using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class RocketLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] UnityEvent OnStep;
    [SerializeField] ParticleSystem explosionParticle;
    public float radius = 15.0F;
    public float power = 80.0F;
    private void OnCollisionEnter(Collision collision)
    {
        print("hit");
        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            print("collide");
            Knockback();
            ExplosionParticle(transform.position);
            OnStep.Invoke();
            PhotonNetwork.Destroy(gameObject);
        }
    }
    void ExplosionParticle(Vector3 explosionPos)
    {
        PhotonNetwork.Instantiate(explosionParticle.name, explosionPos, Quaternion.identity);
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
