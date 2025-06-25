using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusDogHead : MonoBehaviour
{
    [SerializeField] GameObject playerTransform;
    [SerializeField] ServerSpawn serverSpawn;
    [SerializeField] List<GameObject> players;
    private void Start()
    {
        serverSpawn = FindFirstObjectByType<ServerSpawn>();
        StartCoroutine(GetPlayers());
        StartCoroutine(DetectClosePlayer());
    }
    private void FixedUpdate()
    {
         //transform.LookAt(playerTransform.transform.position);
    }
    IEnumerator GetPlayers()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < serverSpawn.playerList.Count; i++)
        {
            players.Add(serverSpawn.playerList[i]);
        }
    }
    IEnumerator DetectClosePlayer()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < players.Count; i++)
        {
            print(Vector3.Distance(players[i].transform.position, transform.position) + players[i].name);
        }
        StartCoroutine(DetectClosePlayer());
    }
}