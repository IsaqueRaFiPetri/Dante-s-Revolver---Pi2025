using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public int weaponDamage;
    public float maxDistance;
    public int ammoValue;
    public int ammoTotal;
}