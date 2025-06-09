using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public int weaponDamage;
    public float maxDistance;
    public float shootCooldown;
    public float reloadCooldown;
    public int ammoTotal;
}