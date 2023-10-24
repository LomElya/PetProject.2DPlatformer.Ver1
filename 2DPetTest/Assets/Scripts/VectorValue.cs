using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject
{
    //Начальные данные
    [SerializeField] private const float _startingMaxHealth = 30f;
    [SerializeField] private const float _startingHealth = _startingMaxHealth;
    //[SerializeField] private const Vector3 startingInitialValue = new Vector3(0,0,0);
    [SerializeField] private const int _startingCoin = 100;
    public float startingMaxHealth => _startingMaxHealth;
    public float startingHealth => _startingHealth;
    public int startingCoin => _startingCoin;
   

    //Текущие данные
    public float maxHealth;
    public float currentHealth;
    public Vector3 initialValue;
    public int currentCoin;
    
}


