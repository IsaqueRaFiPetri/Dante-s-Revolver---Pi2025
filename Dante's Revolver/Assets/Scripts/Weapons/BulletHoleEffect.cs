using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BulletHoleEffect : MonoBehaviour
{
    [SerializeField] UnityEvent OnFinishTimer;
    [SerializeField] UnityEvent OnBulletHit;
    public void Start()
    {
        StartCoroutine(BulletHoleTimer());
    }
    public void Destroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    IEnumerator BulletHoleTimer()
    {
        OnBulletHit.Invoke();
        yield return new WaitForSeconds(4);
        OnFinishTimer.Invoke();
    }
}
