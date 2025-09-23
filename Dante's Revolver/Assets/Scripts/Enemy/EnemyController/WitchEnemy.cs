using UnityEngine;
using System.Collections;
using Photon.Pun;

public class WitchEnemy : EnemyController, ILauncher
{
    [SerializeField] string enemyToSpawn;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float timeToSpawn;
    [SerializeField] GameObject attackSpawn;
    [SerializeField] Transform attackPoint;
    bool canContinue = true;

    void Awake()
    {
        StartCoroutine(SpawnMinions());
    }
    void Update()
    {
        Walk();
    }

    IEnumerator SpawnMinions()
    {
        while (canContinue)
        {
            yield return new WaitForSeconds(timeToSpawn);

            int amountToSpawn = Random.Range(1, spawnPoints.Length + 1);

            Transform[] shuffledPoints = (Transform[])spawnPoints.Clone();
            for (int i = 0; i < shuffledPoints.Length; i++)
            {
                int rnd = Random.Range(i, shuffledPoints.Length);
                Transform temp = shuffledPoints[i];
                shuffledPoints[i] = shuffledPoints[rnd];
                shuffledPoints[rnd] = temp;
            }

            for (int i = 0; i < amountToSpawn; i++)
            {
                PhotonNetwork.Instantiate(enemyToSpawn, shuffledPoints[i].position, Quaternion.identity);
            }
        }
    }

    public override void Attack()
    {
        Shoot(attackSpawn);
    }

    public void Shoot(GameObject projectilPrefab)
    {
        attackSpawn = PhotonNetwork.Instantiate(projectilPrefab.name, attackPoint.position, Quaternion.identity);
        attackSpawn.GetComponent<Rigidbody>().AddForce(transform.up * 5, ForceMode.Impulse);
        attackSpawn.GetComponent<Rigidbody>().AddForce(transform.forward * 35, ForceMode.Impulse);
    }
}
