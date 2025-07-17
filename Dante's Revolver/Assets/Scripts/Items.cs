using UnityEngine;

public interface ICollectable
{
    public void Collect();
}
public class Items : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        
    }
}
