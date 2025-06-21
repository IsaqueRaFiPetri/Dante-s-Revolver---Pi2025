using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IKillable
{
    [PunRPC]
    public void TakeDamage(int damage);
}

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviourPunCallbacks, IKillable
{
    List<Transform> players;
    RaycastHit hit;
    Rigidbody body;
    Vector3 moveDirection;
    float moveSpeed;

    [SerializeField] Transform vision;
    [SerializeField]Stats enemyStats;
    [SerializeField] ParticleSystem bloodParticle;

    float lifeValue;

    [SerializeField]float range;

    private void Start()
    {
        lifeValue = enemyStats.lifeValue;
        body = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {

        if (players == null) return;

        /*moveDirection = Vector3.zero;

        float direction = Vector3.Distance(player.position, this.transform.position);

        Vector3 dir = player.position - this.transform.position;
        dir.y = 0;

        /if (direction < 15)
        {
            body.linearVelocity = transform.forward * moveSpeed;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 0.1f);

            print("following");
        }
        else if (direction >= 15 && direction < 40)
        {
            body.linearVelocity = transform.forward * (moveSpeed * 5);

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 0.1f);

            print("trying to escape");
        }
        else
        {
            body.linearVelocity = transform.forward * moveSpeed * 0;
            print("bye");
        }

        if (hit.collider.GetComponent<IPlayable>() != null)
        {
            if (hit.distance >= 35)
                return;

            player = hit.collider.transform;            
        }*/
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
            BloodParticle(transform.position);
            StartCoroutine(Diying());
        }
    }

    IEnumerator FindClose()
    {
        List<float> playersTransform = new List<float>();
        foreach(Transform player in players)
        {
            playersTransform.Add(Vector3.Distance(player.position, transform.position));
        }
        yield return new WaitForSeconds(.2f);
        StartCoroutine(FindClose());
    }
    IEnumerator Diying()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}