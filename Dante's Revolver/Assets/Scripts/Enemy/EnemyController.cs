using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    void TakeDamage(int damage);
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
    [SerializeField] Stats enemyStats;
    [SerializeField] ParticleSystem bloodParticle;
    [SerializeField] float range;

    float lifeValue;
    Transform player;

    private void Start()
    {
        lifeValue = enemyStats.lifeValue;
        body = GetComponent<Rigidbody>();
        moveSpeed = enemyStats.moveSpeed;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        moveDirection = Vector3.zero;
        float distance = Vector3.Distance(player.position, transform.position);
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (distance < 15)
        {
            body.linearVelocity = transform.forward * moveSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.1f);
            Debug.Log("following");
        }
        else if (distance >= 15 && distance < 40)
        {
            body.linearVelocity = transform.forward * (moveSpeed * 5);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.1f);
            Debug.Log("trying to escape");
        }
        else
        {
            body.linearVelocity = Vector3.zero;
            Debug.Log("bye");
        }

        if (Physics.Raycast(vision.position, vision.forward, out hit, range))
        {
            if (hit.collider.GetComponent<IPlayable>() != null && hit.distance < 35)
            {
                player = hit.collider.transform;
            }
        }
    }

    public void BloodParticle(Vector3 hitPosition)
    {
        bloodParticle.transform.position = hitPosition;
        bloodParticle.Play();
    }

    [PunRPC]
    public void TakeDamage(int damage) 
    {
        lifeValue -= damage;
        BloodParticle(transform.position);

        if (lifeValue <= 0)
        {
            BloodParticle(transform.position);
            StartCoroutine(Diying());
        }
    }

    IEnumerator Diying()
    {
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    IEnumerator FindClose()
    {
        List<float> playersTransform = new List<float>();
        foreach (Transform player in players)
        {
            playersTransform.Add(Vector3.Distance(player.position, transform.position));
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FindClose());
    }
}