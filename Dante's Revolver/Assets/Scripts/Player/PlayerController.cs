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
    [SerializeField] UnityEvent OnDeath, OnTakeDamage;
    [SerializeField] GameObject Grave;

    private void Awake()
    {

    }
    private void Start()
    {
        maxLife = playerStats.lifeValue;
        lifeBarText = lifeBarSprite.GetComponentInChildren<TMP_Text>();
        currentLife = maxLife;
        shieldBarText = shieldBarSprite.GetComponentInChildren<TMP_Text>();
        SetStatsBar(shieldBarSprite, shieldBarText, maxShield, currentShield);
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        Debug.Log("inimigo atacando");

        OnTakeDamage.Invoke();
        
        if(currentShield > 0)
        {
            if(damage > currentShield)
            {
                int remainDamage;
                remainDamage = damage -= (int)currentShield;
                currentLife -= damage;
                currentShield = 0;
            }
            else
            {
                currentShield -= damage;
            }
            SetStatsBar(lifeBarSprite, lifeBarText, maxLife, currentLife);
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
            GameOver.instance.gameObject.GetComponent<PhotonView>().RPC("DetectGameOver", RpcTarget.AllBuffered);
            GameOver.instance.gameObject.GetComponent<PhotonView>().RPC("SetIsOneDead", RpcTarget.AllBuffered, true);
            PhotonNetwork.Instantiate(Grave.name, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity);
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
