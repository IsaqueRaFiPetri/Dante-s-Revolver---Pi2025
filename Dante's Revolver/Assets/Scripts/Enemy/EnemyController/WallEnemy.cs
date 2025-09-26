using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class WallEnemy : MonoBehaviour, IKillable
{
    [SerializeField] string text;
    [SerializeField] TextMeshProUGUI textHolder;
    [SerializeField] Stats enemyStats;

    float lifeValue;

    [SerializeField] protected UnityEvent OnDamageTake, OnDeath;

    private void Start()
    {
        lifeValue = enemyStats.lifeValue;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void TakeDamage(int damage)
    {
        lifeValue -= damage;
        OnDamageTake.Invoke();
        if (lifeValue <= 0)
        {
            OnDeath.Invoke();
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        textHolder.SetText(text);
    }
}
