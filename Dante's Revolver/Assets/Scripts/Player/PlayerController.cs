using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public interface IRegenerable
{
    public void RegenLife(float regenValue);
}
public class PlayerController : MonoBehaviourPunCallbacks, IKillable, IRegenerable
{
    public PhotonView playerPhotonView;
    [SerializeField] Stats playerStats;
    [Space(20)]
    [Header("Life_Stats")]
    [SerializeField] float currentLife;
    [SerializeField] float maxLife;
    [Space(5)]
    [Header("Layout")]
    [SerializeField] Image lifeBarSprite;
    TMP_Text lifeBarText;
    [SerializeField] Image staminaBarSprite;
    TMP_Text staminaBarText;
    [SerializeField] UnityEvent OnDeath;
    [SerializeField] GameObject Grave;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            //Destroy(this);
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
        
        currentLife -= damage;
        SetStatsBar(lifeBarSprite, lifeBarText, maxLife, currentLife);

        if (currentLife <= 0 && photonView.IsMine)
        {
            OnDeath.Invoke();
            PhotonNetwork.Instantiate(Grave.name, transform.position, Quaternion.identity).GetComponentInChildren<PhantomMode>().SetBody(this.gameObject);
            photonView.enabled = false;
        }
    }
    public void RegenLife(float regenValue)
    {
        currentLife += regenValue;
        if(currentLife >= maxLife)
        {
            currentLife = maxLife;
        }
        SetStatsBar(lifeBarSprite, lifeBarText, maxLife, currentLife);
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
