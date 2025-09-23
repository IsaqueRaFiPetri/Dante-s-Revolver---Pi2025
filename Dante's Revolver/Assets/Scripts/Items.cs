using UnityEngine;

public interface ICollectable
{
    public abstract void Collect();
}
public abstract class Items : MonoBehaviour, ICollectable
{
    public abstract void Collect();
}
