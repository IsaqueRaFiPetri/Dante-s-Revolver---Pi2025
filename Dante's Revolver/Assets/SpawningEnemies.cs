using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SpawningEnemies : MonoBehaviour
{
    public EnemyController enemyPrefab;
    [SerializeField] float timeToStartSpawn;
    [SerializeField] UnityEvent OnStartSpawn;
    [SerializeField] UnityEvent StartSpawn;


    private void Start()
    {
        StartCoroutine(StartSpawning());
    }
    private void OnEnable()
    {
        StartCoroutine(StartSpawning());
    }
    public void Spawn()
    {
        OnStartSpawn.Invoke();
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(timeToStartSpawn);
        StartSpawn.Invoke();
    }
}
