using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
public class Cerberus : BossController
{
    public enum Appearing
    {
        IsAppearing, IsNotAppearing
    }
    [SerializeField] Appearing appearing;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Transform[] _positions;
    [SerializeField] UnityEvent OnAppear, OnDisappear;
    [PunRPC]
    public void DetectPhase()
    {
        switch (bossPhase)
        {
            case BossPhase.OnMoveset:
                appearing = Appearing.IsNotAppearing;
                OnDisappear.Invoke();
                break;
            case BossPhase.OnSleeping:
                appearing = Appearing.IsAppearing;
                OnAppear.Invoke();
                break;
        }
        photonView.RPC("SetIsAppearing", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void InvertState()
    {
        photonView.RPC("ChangeBossPhase", RpcTarget.AllBuffered);
        lastMoveset = null;
        photonView.RPC("DetectPhase", RpcTarget.AllBuffered);
        photonView.RPC("DoAction", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void SetIsAppearing()
    {
        switch (appearing)
        {
            case Appearing.IsAppearing:
                photonView.RPC("SetRandomPos", RpcTarget.AllBuffered);
                break;
            case Appearing.IsNotAppearing:

                break;
        }
    }
    [PunRPC]
    public void SetRandomPos()
    {
        transform.position = _positions[Random.Range(0, _positions.Length)].position;
    }
}
