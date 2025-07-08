using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerController : MonoBehaviourPunCallbacks, IKillable
{
    [SerializeField] Stats playerStats;
    [SerializeField] Image lifeBarSprite;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] UnityEvent OnDeath;

    [SerializeField]float currentLife;
    [SerializeField]float maxLife;

    private void Start()
    {
        maxLife = playerStats.lifeValue;
        currentLife = maxLife;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        Debug.Log("inimigo atacando");
        
        currentLife -= damage;
        LifeBar();

        if (currentLife <= 0)
        {
            OnDeath.Invoke();
            DisconectManager.instance.Disconnect("Menu");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LifeBar()
    {
        lifeText.text = currentLife + "/" + maxLife;
        lifeBarSprite.fillAmount = currentLife / maxLife;
    }
}
