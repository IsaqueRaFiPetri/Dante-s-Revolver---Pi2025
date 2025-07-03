using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public interface ILauncher
{
    public void Shoot(GameObject projectilPrefab);
}
public class CerberusDogHead : EnemyController, ILauncher
{
    [SerializeField] Transform playerTransform, vision;
    [SerializeField] GameObject projectil;
    [SerializeField] WeaponStats weaponsStats;
    GameObject lastProjectil;
    bool hasFoundPlayer;

    private void Start()
    {
        StartCoroutine(Shooting());
    }
    private void FixedUpdate()
    {
        FindClosestPlayer();
    }
    private void LateUpdate()
    {
        if (!hasFoundPlayer)
        {
            return;
        }
         transform.LookAt(playerTransform.transform.position);
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

    public void Shoot(GameObject projectilPrefab)
    {
        lastProjectil = PhotonNetwork.Instantiate(projectilPrefab.name, vision.position, Quaternion.identity);
        lastProjectil.GetComponent<Rigidbody>().AddForce(transform.up * 5, ForceMode.Impulse);
        lastProjectil.GetComponent<Rigidbody>().AddForce(transform.forward * 35, ForceMode.Impulse);
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