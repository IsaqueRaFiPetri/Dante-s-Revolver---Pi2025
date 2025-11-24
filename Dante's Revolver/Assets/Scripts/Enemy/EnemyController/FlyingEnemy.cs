using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyingEnemy : EnemyController, ILauncher
{
    [SerializeField] Transform playerTransform, vision;
    [SerializeField] GameObject projectil;
    [SerializeField] WeaponStats weaponsStats;
    GameObject lastProjectil;
    bool hasFoundPlayer;

    private void Start()
    {
        // INICIAR a corrotina de disparo
        StartCoroutine(Shooting());
    }

    private void FixedUpdate()
    {
        FindClosestPlayer();
        Walk();
    }

    public void Shoot(GameObject projectilPrefab)
    {
        if (!hasFoundPlayer || projectilPrefab == null) return;

        lastProjectil = PhotonNetwork.Instantiate(projectilPrefab.name, vision.position, Quaternion.identity);
        lastProjectil.GetComponent<Rigidbody>().AddForce(transform.up * 5, ForceMode.Impulse);
        lastProjectil.GetComponent<Rigidbody>().AddForce(transform.forward * 35, ForceMode.Impulse);
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
                playerTransform = p.transform;
            }
        }
        if (playerTransform != null)
        {
            hasFoundPlayer = true;
        }
    }

    // CORROTINA CORRIGIDA - estava faltando iniciar
    IEnumerator Shooting()
    {
        while (true) // Loop infinito para disparar continuamente
        {
            if (hasFoundPlayer)
            {
                Shoot(projectil);
                // Chamar animação de ataque se existir
                if (anim != null)
                {
                    anim.SetTrigger("Attack");
                }
            }
            yield return new WaitForSeconds(weaponsStats.shootCooldown);
        }
    }

    // SOBRESCREVER o método Walk para adicionar comportamentos específicos do inimigo voador
    protected new void Walk()
    {
        if (playerTransform == null)
        {
            // Parar animação se não há jogador
            if (anim != null)
            {
                anim.SetBool("IsChasing", false);
            }
            return;
        }

        // Chamar o Walk da classe base
        base.Walk();

        // Comportamentos adicionais específicos para FlyingEnemy
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Ativar animação de perseguição
        if (anim != null)
        {
            anim.SetBool("IsChasing", distance <= range && distance > 2f);
        }
    }
}