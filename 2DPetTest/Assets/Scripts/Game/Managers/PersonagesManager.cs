using System.Collections.Generic;
using UnityEngine;

public class PersonagesManager : MonoBehaviour, IService
{
    [SerializeField] private List<Personage> _personages = new List<Personage>();
    [SerializeField] private Player _player;
    public void SetPlayer(Player player) => _player = player;

    public List<Personage> Personages => _personages;
    public Player Player => _player;

    public void Init()
    {

    }
}
