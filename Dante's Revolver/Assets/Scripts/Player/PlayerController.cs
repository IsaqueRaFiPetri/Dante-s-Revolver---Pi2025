using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public interface IRegenerable
{
    public void RegenLife(float regenValue, bool isLife);
}
public class PlayerController : MonoBehaviourPunCallbacks, IKillable, IRegenerable
{
    public PhotonView playerPhotonView;
    [SerializeField] Stats playerStats;
    [Space(20)]
    [Header("Life_Stats")]
    [SerializeField] float currentLife;
    [SerializeField] float maxLife;
    [SerializeField] float currentShield;
    [SerializeField] float maxShield;
    [Space(5)]
    [Header("Layout")]
    [SerializeField] Image lifeBarSprite;
    [SerializeField] Image shieldBarSprite;
    TMP_Text lifeBarText;
    TMP_Text shieldBarText;
    [SerializeField] UnityEvent OnDeath;
    [SerializeField] GameObject Grave;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        maxLife = playerStats.lifeValue;
        lifeBarText = lifeBarSprite.GetComponentInChildren<TMP_Text>();
        currentLife = maxLife;

    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        Debug.Log("inimigo atacando");
        
        if(currentShield > 0)
        {
            currentShield -= damage;
            SetStatsBar(shieldBarSprite, shieldBarText, maxShield, currentShield);
        }
        else
        {
            currentLife -= damage;
            SetStatsBar(lifeBarSprite, lifeBarText, maxLife, currentLife);
        }

        if (currentLife <= 0 && photonView.IsMine)
        {
            OnDeath.Invoke();
            PhotonNetwork.Instantiate(Grave.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
    }
    public void RegenLife(float regenValue, bool isLife)
    {
        if (isLife)
        {
            currentLife += regenValue;
            if (currentLife >= maxLife)
            {
                currentLife = maxLife;
            }
            SetStatsBar(lifeBarSprite, lifeBarText, maxLife, currentLife);
        }
        else
        {
            currentShield += regenValue;
            if (currentShield >= maxShield)
            {
                currentShield = maxShield;
            }
            SetStatsBar(shieldBarSprite, shieldBarText, maxShield, currentShield);
        }
    }
    public void SetStatsBar(Image statsBarImage,TMP_Text statsBarText, float maxStats, float currentStats)
    {
        statsBarText.text = currentStats + "/" + maxStats;
        statsBarImage.fillAmount = currentStats / maxStats;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
