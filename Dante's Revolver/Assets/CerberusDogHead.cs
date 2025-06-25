using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusDogHead : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform playerTransform;
    [SerializeField] List<GameObject> allPlayers;
    private void Start()
    {
        StartCoroutine(GetPlayers());
        StartCoroutine(DetectClosePlayer());
    }
    private void FixedUpdate()
    {
         transform.LookAt(playerTransform.transform.position);
    }
    IEnumerator GetPlayers()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < ServerSpawn.instance.playerList.Count; i++)
        {
            allPlayers.Add(ServerSpawn.instance.playerList[i]);
        }
    }
    IEnumerator DetectClosePlayer()
    {
        yield return new WaitForSeconds(1);
        photonView.RPC("FindClosestPlayer", RpcTarget.AllBuffered);
        StartCoroutine(DetectClosePlayer());
    }
    [PunRPC] void FindClosestPlayer()
    {
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

    }
}