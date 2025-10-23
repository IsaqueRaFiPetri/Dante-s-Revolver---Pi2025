using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class RotationValueChange : MonoBehaviour, IKillable
{
    [SerializeField] GameObject objToRotate;
    [SerializeField] float rotationValue;
    [SerializeField] UnityEvent OnClick;
    public GameObject GetGameObject()
    {
        return objToRotate;
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        OnClick.Invoke();
    }

    public void Rotation()
    {
        objToRotate.transform.Rotate(0, rotationValue, 0);
    }
}
