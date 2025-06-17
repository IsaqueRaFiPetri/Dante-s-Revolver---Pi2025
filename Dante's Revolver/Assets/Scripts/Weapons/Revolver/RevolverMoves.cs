using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RevolverMoves : MonoBehaviour
{
    public IEnumerator Rebound(Transform bulletHolder)
    {
        transform.DOLocalRotate(new Vector3(0, 90, -25f), 0.25f);
        transform.DOLocalMoveZ(.3f, 0.25f);
        yield return new WaitForSeconds(.1f);
        bulletHolder.DOLocalRotate(new Vector3(0, 0, bulletHolder.eulerAngles.z + 60f), 0.25f);
        transform.DOLocalRotate(new Vector3(0, 90, 0f), 0.25f);
        transform.DOLocalMoveZ(.55f, 0.25f);
    }
    public void SetTransform(Transform transformObj, Vector3 rotation, float time)
    {
        transformObj.DOLocalRotate(rotation, time);
    }
    public IEnumerator Taunting(float interval)
    {
        transform.DOLocalRotate(new Vector3(0, 90, 270), 0.25f);
        transform.DOLocalRotate(new Vector3(0, 90, -270), 0.25f);
        yield return new WaitForSeconds(interval);
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.25f);
    }
    public IEnumerator InfiniteTaunting(float interval, float time)
    {
        transform.DOLocalRotate(new Vector3(0, 90, 270), time);
        transform.DOLocalRotate(new Vector3(0, 90, -270), time);
        yield return new WaitForSeconds(interval);
        transform.DOLocalRotate(new Vector3(0, 90, 0), time);
        yield return new WaitForSeconds(interval);
        StartCoroutine(InfiniteTaunting(interval, time));
    }
}
