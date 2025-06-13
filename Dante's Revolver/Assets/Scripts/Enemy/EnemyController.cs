using Photon.Pun;
using UnityEngine;

public interface IKillable
{
    [PunRPC]
    public void TakeDamage(int damage);
}

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviourPunCallbacks, IKillable
{
    Transform player;
    Rigidbody body;
    Vector3 moveDirection;
    [SerializeField]Stats enemyStats;
    [SerializeField] ParticleSystem bloodParticle;

    float lifeValue;

    private void Start()
    {
        lifeValue = enemyStats.lifeValue;
        body = GetComponent<Rigidbody>();
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<IPlayable>() != null)
        {
            print("entered");
            player = collision.transform;
        }
    }
    void FixedUpdate()
    {
        if (player == null)
            return;

        moveDirection = Vector3.zero;

        float direction = Vector3.Distance(player.position, this.transform.position);

        Vector3 dir = player.position - this.transform.position;
        dir.y = 0;

        if (direction < 15)
        {
            body.linearVelocity = transform.forward * enemyStats.moveSpeed;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 0.1f);

            print("following");
        }
        else if (direction >= 15 && direction < 30)
        {
            body.linearVelocity = transform.forward * (enemyStats.moveSpeed * 5);

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 0.1f);

            print("trying to escape");
        }
        else
        {
            body.linearVelocity = transform.forward * enemyStats.moveSpeed * 0;
            print("bye");
        }
    }
    public void BloodParticle(Vector3 hitPosition)
    {
        bloodParticle.Play();
        bloodParticle.transform.position = hitPosition;
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        lifeValue -= damage;
        print(lifeValue);

        if(lifeValue <= 0)
        {
            PhotonNetwork.Destroy(gameObject);

        }
    }
}