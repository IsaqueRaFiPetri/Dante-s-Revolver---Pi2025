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

    ServerSpawn serverSpawn;

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
        if (player == null || Vector3.Distance(player.position, transform.position) > range)
        {
            FindClosestPlayer();
        }

        moveDirection = Vector3.zero;
        float distance = Vector3.Distance(player.position, transform.position);
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (distance < range)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            body.AddForce(direction * moveSpeed, ForceMode.VelocityChange);
        }

        if (body.linearVelocity.magnitude > moveSpeed * 2)
        {
            body.linearVelocity = body.linearVelocity.normalized * moveSpeed * 2;
        }
        else
        {
            body.linearVelocity = Vector3.zero;
        }

        if (Physics.SphereCast(vision.position, 10.5f, vision.forward, out hit, range))
        {
            Debug.DrawRay(vision.position, vision.forward * hit.distance, Color.red);
            if (hit.collider.GetComponent<IPlayable>() != null)
            {
                player = hit.collider.transform;
            }
        }

        transform.LookAt(player);
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

    void FindClosestPlayer()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject p in allPlayers)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                player = p.transform;
            }
        }

    }
}