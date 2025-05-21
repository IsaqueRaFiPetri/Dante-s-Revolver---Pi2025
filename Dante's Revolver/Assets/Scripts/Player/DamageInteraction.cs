using UnityEngine;
public interface IDamaging
{
    public int GetDamagevalue();
    public void DoDamage();
}
public class DamageInteraction : MonoBehaviour, IDamaging
{
    public void DoDamage()
    {
        throw new System.NotImplementedException();
    }

    public int GetDamagevalue()
    {
        throw new System.NotImplementedException();
    }
}
