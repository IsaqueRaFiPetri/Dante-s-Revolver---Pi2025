using Photon.Pun;
using UnityEngine;

public class PhantomMode : MonoBehaviour
{
    [SerializeField] GameObject playerBody;
    public GameObject SetBody(GameObject _playerBody)
    {
        return playerBody = _playerBody;
    }
    Vector3 SetBodyPos(Transform _playerObj , Vector3 _pos)
    {
        return _playerObj.position = _pos;
    }
    public void isInReviveArea(Vector3 _pos)
    {
        print("revive");
        SetBodyPos(playerBody.transform, _pos);
        playerBody.SetActive(true);
        playerBody.GetPhotonView().enabled = true;
        Destroy(GetComponentInParent<GameObject>());
    }
}
