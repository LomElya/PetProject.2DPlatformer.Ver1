using UnityEngine;

public class Personage : MonoBehaviour
{
    [Tooltip("Номер \"команды\". Персонажи одной команды не атакуют друг друга")]
    [SerializeField] private int _affiliation;

    [Tooltip("Точка куда будут атаковать другие персонажи")]
    [SerializeField] private Transform _aimPoint;
    public int Affiliation => _affiliation;
    public Transform AimPoint => _aimPoint;
    private PersonagesManager _personagesManager;

    private void Start()
    {
        _personagesManager = GameObject.FindObjectOfType<PersonagesManager>();

        /// Регистрация персонажа
        if (!_personagesManager.Personages.Contains(this))
        {
            _personagesManager.Personages.Add(this);
        }
    }

    private void OnDestroy()
    {
        /// Удаление персонажа
        if (_personagesManager)
        {
            _personagesManager.Personages.Remove(this);
        }
    }

}
