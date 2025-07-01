using System.Collections;
using UnityEngine;

public interface IWeakeable
{
    public GameObject HeadshotParticle();
}
public class WeakPoint : MonoBehaviour, IWeakeable
{
    [SerializeField] ParticleSystem weakPointParticle;
    public GameObject HeadshotParticle()
    {
        return weakPointParticle.gameObject;
    }
}
