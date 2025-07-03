using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Explosives : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject explosivePrefab;
    public void SpawnExplosive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PhotonNetwork.Instantiate(explosivePrefab.name, transform.position, Quaternion.identity);
        }
    }
}
