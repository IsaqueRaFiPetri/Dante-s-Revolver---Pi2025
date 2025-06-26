using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusDogHead : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    private void FixedUpdate()
    {
        FindClosestPlayer();
    }
    private void LateUpdate()
    {
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

    }
}