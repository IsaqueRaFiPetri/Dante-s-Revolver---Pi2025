using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallrunSpeed;
    [Header("Stats")]
    public float lifeValue;
    public float shieldValue;
    public float staminaValue;
    public float damage;
}
