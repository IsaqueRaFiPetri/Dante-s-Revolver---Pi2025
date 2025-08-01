using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Events;
public enum BossPhase
{
    OnMoveset, OnSleeping
}
public class BossController : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    [SerializeField] protected List<Moveset> movesets;
    [Space(5)]
    [SerializeField] protected BossPhase bossPhase;
    [Space(5)]
    [SerializeField] protected Moveset lastMoveset;
    [Space(20)]
    [SerializeField] protected Image bossLifeBar;
    [SerializeField] protected float lifeValue;
    [SerializeField] protected float maxLifeValue;
    [Space(20)]
    [SerializeField] protected UnityEvent OnChangePhase;

    private void Start()
    {
        DoAction();
    }
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
        print("StartedMoveset: " + lastMoveset);
        StartCoroutine(DoingMoveset(lastMoveset.MovesetDuration()));
    }
    IEnumerator DoingMoveset(float duration)
    {
        lastMoveset.GetOnStart().Invoke();
        yield return new WaitForSeconds(duration);
        lastMoveset.GetOnFinish().Invoke();
        print("EndedMoveset: " + lastMoveset);
        lastMoveset = null;
        print("MovesetClear");
        ChangeBossPhase();
        DoAction();
    }
    IEnumerator Sleeping()
    {
        print("StartedSleeping");
        yield return new WaitForSeconds(5);
        print("EndedSleeping");
        ChangeBossPhase();
        DoAction();
    }
    void ChangeBossPhase()
    {
        print("ChangingBossPhase");
        switch (bossPhase)
        {
            case BossPhase.OnMoveset:
                bossPhase = BossPhase.OnSleeping;
                break;
            case BossPhase.OnSleeping:
                bossPhase = BossPhase.OnMoveset;
                break;
        }
        print("BossPhase: " + bossPhase);
        OnChangePhase.Invoke();
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        lifeValue -= damage;
        bossLifeBar.fillAmount = UpdateLifeBar();
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
 