using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
public enum BossPhase
{
    Start, Medium, Final, Sleeping
}
public class BossController : MonoBehaviour
{
    [System.Serializable] struct BossMovesets
    {
        public List<GameObject> moveSets;
        public UnityEvent OnMoveset;
    }
    [SerializeField] List<BossMovesets> bossMovesets;
    [Space(5)]
    [SerializeField] BossPhase bossPhase;
    [Space(5)]
    [SerializeField] GameObject lastMoveset;
    [Space(2.5f)]
    [Header("==============")]
    [Space(2.5f)]
    [SerializeField] float movesetCooldown;
    [SerializeField] float movesetChangeCooldown;

    private void Start()
    {
        StartCoroutine(MovesetSelector());
    }
    public void DoMoveset()
    {
        lastMoveset = bossMovesets[(int)bossPhase].moveSets[Random.Range(0, bossMovesets[(int)bossPhase].moveSets.Count)];
    }
    IEnumerator MovesetSelector()
    {
        yield return new WaitForSeconds(movesetChangeCooldown);
        DoMoveset();
        StartCoroutine(MovesetSelector());
    }
}
 