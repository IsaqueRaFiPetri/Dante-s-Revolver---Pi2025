using UnityEngine;

public class Cerberus : BossController
{
    public enum Appearing
    {
        IsAppearing, IsNotAppearing
    }
    [SerializeField] Appearing appearing;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Transform[] _positions;
    public void DetectPhase()
    {
        switch (bossPhase)
        {
            case BossPhase.OnMoveset:
                appearing = Appearing.IsNotAppearing;
                break;
            case BossPhase.OnSleeping:
                appearing = Appearing.IsAppearing;
                break;
        }
        SetIsAppearing();
    }
    void SetIsAppearing()
    {
        switch (appearing)
        {
            case Appearing.IsAppearing:
                meshRenderer.enabled = true;
                SetRandomPos();
                break;
            case Appearing.IsNotAppearing:
                meshRenderer.enabled = false;
                break;
        }
    }
    void SetRandomPos()
    {
        transform.position = _positions[Random.Range(0, _positions.Length)].position;
    }
}
