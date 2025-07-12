using UnityEngine;
using System.Collections;

public class WitchEnemy : EnemyController
{
    [SerializeField] GameObject minionsObj;
    [SerializeField] Transform[] spawnPoints;
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
            yield return new WaitForSeconds(5f);

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
                Instantiate(minionsObj, shuffledPoints[i].position, minionsObj.transform.rotation);
            }
        }
    }
}
