using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PlayerController : MonoBehaviourPunCallbacks, IKillable
{
    [SerializeField] Stats playerStats;
    [SerializeField] Image lifeBarSprite;
    [SerializeField] TextMeshProUGUI lifeText;

    int lifeValue;
    int maxLife;

    private void Start()
    {
        maxLife = playerStats.lifeValue;
        lifeValue = maxLife;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        lifeValue -= damage;

        if (lifeValue <= 0)
        {
            gameObject.SetActive(false);
            LifeBar();
        }
    }

    public void LifeBar()
    {
        lifeText.text = lifeValue + "/100";
        lifeBarSprite.fillAmount = (float)lifeValue / (float)maxLife;
    }
}
