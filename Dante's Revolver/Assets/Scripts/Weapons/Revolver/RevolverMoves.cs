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
    public void ResetTranform(Transform transform)
    {
        transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f);
    }
    public IEnumerator Taunting(Transform bulletHolder)
    {
        transform.DOLocalRotate(new Vector3(0, 90, 270), 0.25f);
        transform.DOLocalRotate(new Vector3(0, 90, -270), 0.25f);
        bulletHolder.DOLocalRotate(new Vector3(0, 0, 0), 0.25f);
        yield return new WaitForSeconds(.1f);
        transform.DOLocalRotate(new Vector3(0, 90, 0), 0.25f);
    }
}
