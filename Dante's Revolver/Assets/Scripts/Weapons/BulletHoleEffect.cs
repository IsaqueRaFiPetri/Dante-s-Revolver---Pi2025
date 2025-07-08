using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BulletHoleEffect : MonoBehaviour
{
    [SerializeField] UnityEvent OnFinishTimer;
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
        yield return new WaitForSeconds(4);
        OnFinishTimer.Invoke();
    }
}
