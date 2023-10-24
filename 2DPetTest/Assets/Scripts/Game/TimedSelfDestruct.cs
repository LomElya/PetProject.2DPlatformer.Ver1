using UnityEngine;


public class TimedSelfDestruct : MonoBehaviour
{
    public float LifeTime = 1f;

    private float _spawnTime;

    void Awake()
    {
        _spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time > _spawnTime + LifeTime)
        {
            Destroy(gameObject);
        }
    }
}
