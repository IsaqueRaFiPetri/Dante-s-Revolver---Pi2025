using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;

public class InquisitorEnemy : EnemyController, ILauncher
{
    [SerializeField] string enemyToSpawn;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float timeToSpawn;
    [SerializeField] GameObject attackSpawn;
    [SerializeField] Transform attackPoint;
    [SerializeField] LineRenderer _lineRendererPrefab;
    [SerializeField] List<Material> _materials;
    [SerializeField] Transform _headTransform;
    [SerializeField] List<IPlayable> _players;
    bool canContinue = true;

    void Awake()
    {
        StartCoroutine(SpawnMinions());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IPlayable player))
        {
            _players.Add(player);
            VerifyPlayerQuantity();
            StartCoroutine(SpawnMinions());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IPlayable player))
        {
            _players.Remove(player);
            VerifyPlayerQuantity();
        }
    }
    void VerifyPlayerQuantity()
    {
        if(_players.Count <= 0)
        {
            canContinue = false;
        }
        else
        {
            canContinue = true;
        }
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
                GameObject _lastInstantiatedEnemy = PhotonNetwork.Instantiate(enemyToSpawn, shuffledPoints[i].position, Quaternion.identity);
                print(_lastInstantiatedEnemy);
                InquisitorFollower _lastInquitiorFollower = _lastInstantiatedEnemy.AddComponent<InquisitorFollower>();
                print(_lastInquitiorFollower);
                _lastInquitiorFollower._transformList[1] = _headTransform;
                _lastInquitiorFollower._transformList[0] = _lastInquitiorFollower.transform;
                _lastInquitiorFollower.SetLineRenderer(_lineRendererPrefab, _materials);

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
