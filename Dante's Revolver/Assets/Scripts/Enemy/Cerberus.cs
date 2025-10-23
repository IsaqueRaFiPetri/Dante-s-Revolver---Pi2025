using UnityEngine;
using UnityEngine.Events;

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
        SetIsAppearing();
    }
    public void InvertState()
    {
        ChangeBossPhase();
        lastMoveset = null;
        DetectPhase();
        DoAction();
    }
    void SetIsAppearing()
    {
        switch (appearing)
        {
            case Appearing.IsAppearing:

                SetRandomPos();
                break;
            case Appearing.IsNotAppearing:

                break;
        }
    }
    void SetRandomPos()
    {
        transform.position = _positions[Random.Range(0, _positions.Length)].position;
    }
}
