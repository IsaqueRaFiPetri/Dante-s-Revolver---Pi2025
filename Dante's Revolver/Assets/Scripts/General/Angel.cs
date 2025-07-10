using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class Angel : MonoBehaviourPunCallbacks
{
    List<Transform> players;
    Transform player;


    private void Start()
    {
        FollowPlayer();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        RotateTowardsPlayer();

    }
    void FollowPlayer()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject p in allPlayers)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                player = p.transform;
            }
            
        }
    }
    void RotateTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.fixedDeltaTime);
        }
    }
}
