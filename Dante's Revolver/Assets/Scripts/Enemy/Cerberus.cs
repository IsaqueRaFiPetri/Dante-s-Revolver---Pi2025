using UnityEngine;

public class Cerberus : BossController
{
    public enum Appearing
    {
        IsAppearing, IsNotAppearing
    }
    [SerializeField] Appearing appearing;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider positionArea;
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
        transform.position = new Vector3(GetRandomPointInCollider(positionArea).x, transform.position.y, GetRandomPointInCollider(positionArea).z);
    }

    private Vector3 GetRandomPointInCollider(Collider collider, float offset = 1f)
    {
        Bounds collBounds = collider.bounds;

        Vector3 minBounds = new Vector3(collBounds.min.x + offset, collBounds.min.y + offset, collBounds.min.z + offset);
        Vector3 maxBounds = new Vector3(collBounds.max.x - offset, collBounds.max.y - offset, collBounds.max.z - offset);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);
        float randomZ = Random.Range(minBounds.z, maxBounds.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
