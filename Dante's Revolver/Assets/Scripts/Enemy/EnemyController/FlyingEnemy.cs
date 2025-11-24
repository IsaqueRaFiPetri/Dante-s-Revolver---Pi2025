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

    private void FixedUpdate()
    {
        FindClosestPlayer();
        Walk();
    }

    public void Shoot(GameObject projectilPrefab)
    {
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
    IEnumerator Shooting()
    {
        if (hasFoundPlayer)
        {
            Shoot(projectil);
        }
        yield return new WaitForSeconds(weaponsStats.shootCooldown);

        StartCoroutine(Shooting());
    }
}
