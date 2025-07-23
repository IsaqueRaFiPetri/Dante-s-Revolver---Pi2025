using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
public enum BossPhase
{
    OnMoveset, OnSleeping
}
public class BossController : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    [SerializeField] List<Moveset> movesets;
    [Space(5)]
    [SerializeField] BossPhase bossPhase;
    [Space(5)]
    [SerializeField] Moveset lastMoveset;
    [Space(20)]
    [SerializeField] Image bossLifeBar;
    [SerializeField] float lifeValue;
    [SerializeField] float maxLifeValue;
    public void DoAction()
    {
        switch (bossPhase)
        {
            case BossPhase.OnMoveset:
                DoMoveset();
                break;
            case BossPhase.OnSleeping:
                StartCoroutine(Sleeping());
                break;
        }
    }
    void DoMoveset()
    {
        lastMoveset = movesets[Random.Range(0, movesets.Count)];
        StartCoroutine(DoingMoveset(lastMoveset.MovesetDuration()));
    }
    IEnumerator DoingMoveset(float duration)
    {
        lastMoveset.GetOnStart().Invoke();
        yield return new WaitForSeconds(duration);
        lastMoveset.GetOnFinish().Invoke();
        ChangeBossPhase();
        lastMoveset = null;
    }
    IEnumerator Sleeping()
    {
        yield return new WaitForSeconds(5);
        ChangeBossPhase();
        DoAction();
    }
    void ChangeBossPhase()
    {
        switch (bossPhase)
        {
            case BossPhase.OnMoveset:
                bossPhase = BossPhase.OnSleeping;
                break;
            case BossPhase.OnSleeping:
                bossPhase = BossPhase.OnMoveset;
                break;
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        lifeValue -= damage;
        bossLifeBar.fillAmount = UpdateLifeBar();
        //OnDamageTake.Invoke();
        if (lifeValue <= 0)
        {
            Destroy(gameObject);
        }
    }

    float UpdateLifeBar()
    {
        return bossLifeBar.fillAmount = lifeValue / maxLifeValue;
    }
}
 