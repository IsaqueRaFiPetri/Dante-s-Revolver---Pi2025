using Photon.Pun;
using System.Collections;
using UnityEngine;

public class CerberusDogHead : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject projectil;
    bool canShoot = true;
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
        PhotonNetwork.Instantiate(projectilPrefab.name, transform.position, Quaternion.identity);
    }
    IEnumerator Shooting()
    {
        Shoot(projectil);
        yield return new WaitForSeconds(1);
        StartCoroutine(Shooting());
    }
}