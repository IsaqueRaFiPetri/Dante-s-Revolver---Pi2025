using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjRotation : MonoBehaviour
{
    [SerializeField] Vector3 objRotation;
    [SerializeField] bool randomRotation;

    private void Start()
    {
        if (randomRotation)
        {
            StartCoroutine(RandomizeValues());
        }
    }
    private void FixedUpdate()
    {
        transform.Rotate(objRotation);

    }
    IEnumerator RandomizeValues()
    {
        yield return new WaitForSeconds(5);
        objRotation.x = Random.Range(3, -3);
        objRotation.y = Random.Range(3, -3);
        objRotation.z = Random.Range(3, -3);
        StartCoroutine(RandomizeValues());
    }
}
