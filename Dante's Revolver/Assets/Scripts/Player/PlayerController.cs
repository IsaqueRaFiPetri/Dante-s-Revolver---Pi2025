using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Rendering;
using System.Collections;

public class PlayerController : MonoBehaviourPunCallbacks, IKillable
{
    [SerializeField] Stats playerStats;
    [Space(20)]
    [Header("Life_Stats")]
    [SerializeField] float currentLife;
    [SerializeField] float maxLife;
    [Space(5)]
    [Header("Stamina_Stats")]
    [SerializeField] float currentStamina;
    [SerializeField] float maxStamina;
    [SerializeField] float staminaRegenCooldown;
    bool canMakeMove = true;
    [Space(5)]
    [Header("Layout")]
    [SerializeField] Image lifeBarSprite;
    TMP_Text lifeBarText;
    [SerializeField] Image staminaBarSprite;
    TMP_Text staminaBarText;
    [SerializeField] UnityEvent OnDeath;

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

        maxStamina = playerStats.staminaValue;

        staminaBarText = staminaBarSprite.GetComponentInChildren<TMP_Text>();
        currentStamina = maxStamina;
    }
    public void Action(float staminaDamage)
    {
        StopCoroutine(StaminaRegen());
        SetStatsBar(staminaBarSprite, staminaBarText, maxStamina, currentStamina);
        if (currentStamina <= 0)
        {
            currentStamina = 0;
            StartCoroutine(StaminaRegen());
            SetStatsBar(staminaBarSprite, staminaBarText, maxStamina, currentStamina);
            SetCanMove(false);
            return;
        }
        currentStamina -= staminaDamage;
        StartCoroutine(StaminaRegen());
    }
    public void RegenStamina(float increaseStamina)
    {
        SetCanMove(true);
        if(currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        currentStamina += increaseStamina;
    }
    public bool GetCanMove()
    {
        return canMakeMove;
    }
    bool SetCanMove(bool canMove)
    {
        return canMakeMove = canMove;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        Debug.Log("inimigo atacando");
        
        currentLife -= damage;
        SetStatsBar(lifeBarSprite, lifeBarText, maxLife, currentLife);

        if (currentLife <= 0)
        {
            OnDeath.Invoke();
            DisconectManager.instance.Disconnect("Menu");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    void SetStatsBar(Image statsBarImage,TMP_Text statsBarText, float maxStats, float currentStats)
    {
        statsBarText.text = currentStats + "/" + maxStats;
        statsBarImage.fillAmount = currentStats / maxStats;
    }

    IEnumerator StaminaRegen()
    {
        yield return new WaitForSeconds(10);
        currentStamina = maxStamina;
        SetStatsBar(staminaBarSprite, staminaBarText, maxStamina, currentStamina);

    }
}
