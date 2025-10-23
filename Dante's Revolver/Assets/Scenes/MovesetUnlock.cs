using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
public class MovesetUnlock : MonoBehaviour, IKillable
{
    [SerializeField] Moveset _movesetUnlocked;
    [SerializeField] Image _lifeBar;
    [SerializeField] float _life;
    [SerializeField] float _maxLife;
    [SerializeField] UnityEvent OnDestroyCrystal;
    private void Start()
    {
        _life = _maxLife;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        _life -= damage;
        _lifeBar.fillAmount = _life / _maxLife;
        if(_life <= 0)
        {
            OnDestroyCrystal.Invoke();
            gameObject.SetActive(false);
            _life = _maxLife;
        }
    }
}
