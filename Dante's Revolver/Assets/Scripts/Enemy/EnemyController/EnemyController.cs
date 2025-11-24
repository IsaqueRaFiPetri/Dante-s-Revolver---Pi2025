using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IKillable
{
    void TakeDamage(int damage);

    GameObject GetGameObject();
}

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    List<Transform> players;
    RaycastHit hit;
    [SerializeField] Rigidbody body;
    Vector3 moveDirection;
    float moveSpeed = 15;
    float _distance;

    [SerializeField] Transform vision;
    [SerializeField] protected Stats enemyStats;
    public float range;
    [SerializeField] protected UnityEvent OnAttack, OnDamageTake, OnDeath;
    [SerializeField] float cooldown = 2f;

    float currentCooldown;
    float lifeValue;
    Transform player;

    public Animator anim;

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
            return;
        }

        anim.SetBool("IsChasing", true);        

        float distance = Vector3.Distance(player.position, transform.position);
        _distance = distance;
        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        if (distance < range)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
                    
            body.linearVelocity = direction * moveSpeed;

            Quaternion targetRot = Quaternion.LookRotation(direction);
            body.MoveRotation(Quaternion.Lerp(body.rotation, targetRot, Time.deltaTime * 5f));

        }

        else if(distance > range)
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
    protected float GetDistance()
    {
        return _distance;
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        lifeValue -= damage;
        OnDamageTake.Invoke();
        if (lifeValue <= 0)
        {
            OnDeath.Invoke();
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

    public virtual void Attack()
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
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}