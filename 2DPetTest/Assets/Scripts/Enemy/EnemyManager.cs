using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyManager : MonoBehaviour, IService
    {
        [SerializeField] private List<Enemy> Enemies;
        [SerializeField] private int _numberOfEnemiesTotal;
        public List<Enemy> _enemies => Enemies;
        public int NumberOfEnemiesTotal => _numberOfEnemiesTotal;

        public int _numberOfEnemiesRemaining => Enemies.Count;

        public void Init()
        {
            // Enemies = new List<Enemy>();
        }
        public void RegisterEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);

            _numberOfEnemiesTotal++;
        }

        public void UnregisterEnemy(Enemy enemyKilled)
        {
            /* if (Enemies.Contains(enemyKilled))
            {
            } */
            int enemiesRemainingNotification = _numberOfEnemiesRemaining - 1;

            //EnemyKillEvent evt = Events.EnemyKillEvent;
            //evt.Enemy = enemyKilled.gameObject;
            //evt.RemainingEnemyCount = enemiesRemainingNotification;
            //EventManager.Broadcast(evt);

            Enemies.Remove(enemyKilled);

        }
    }
}
