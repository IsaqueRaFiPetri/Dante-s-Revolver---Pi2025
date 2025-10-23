using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class RotationValueChange : MonoBehaviour
{
    [SerializeField] GameObject objToRotate;
    [SerializeField] float rotationValue;
    
    public void Rotation()
    {
        objToRotate.transform.Rotate(0, rotationValue, 0);
    }
}
