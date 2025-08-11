using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IKillable
{
    void TakeDamage(int damage);
}

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    List<Transform> players;
    RaycastHit hit;
    Rigidbody body;
    Vector3 moveDirection;
    float moveSpeed;

    [SerializeField] Transform vision;
    [SerializeField] Stats enemyStats;
    [SerializeField] float range;
    [SerializeField] UnityEvent OnAttack, OnDamageTake;
    [SerializeField] float cooldown = 2f;

    ServerSpawn serverSpawn;

    float currentCooldown;
    float lifeValue;
    Transform player;

    [SerializeField] Animator anim;

    private void Start()
    {
        lifeValue = enemyStats.lifeValue;
        body = GetComponent<Rigidbody>();
        moveSpeed = enemyStats.moveSpeed;
    }
    protected void Walk()
    {
        if (player == null || Vector3.Distance(player.position, transform.position) > range)
        {
            FindClosestPlayer();
            anim.SetBool("IsChasing", false);
            return;
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
            anim.SetBool("IsChasing", true);
        }

        if (body.linearVelocity.magnitude > moveSpeed * 2)
        {
            body.linearVelocity = body.linearVelocity.normalized * moveSpeed * 2;
        }
        else
        {
            body.linearVelocity = Vector3.zero;
            anim.SetBool("IsChasing", false);
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


        if (distance <= 2f)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0f)
            {
                Attack();
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        lifeValue -= damage;
        OnDamageTake.Invoke();
        if (lifeValue <= 0)
        {
            Destroy(gameObject);
        }
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

    void Attack()
    {
        if (player == null) return;

        if (player.TryGetComponent<PhotonView>(out var pv))
        {
            pv.RPC("TakeDamage", RpcTarget.AllBuffered, (int)enemyStats.damage);
            OnAttack?.Invoke();
            currentCooldown = cooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }

}