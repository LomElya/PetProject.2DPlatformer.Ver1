using UnityEngine;
using Enemies;

[System.Serializable]
public struct EnemyData
{
    [SerializeField] private int _id;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Sprite _enemySprite;
    [SerializeField] private Animator _enemyAnimator;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damage;
    [SerializeField] private int _armor;
    [SerializeField] private float _maxSpeed;
    public int ID => _id;
    public Enemy EnemyPrefab => _enemyPrefab;
    public Sprite EnemySprite => _enemySprite;
    public Animator EnemyAnimator => _enemyAnimator;
    public float MaxHealth => _maxHealth;
    public float Damage => _damage;
    public int Armor => _armor;
    public float MaxSpeed => _maxSpeed;
}

