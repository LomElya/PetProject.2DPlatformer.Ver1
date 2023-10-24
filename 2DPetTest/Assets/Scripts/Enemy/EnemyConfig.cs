using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 1)]
    public class EnemyConfig : ScriptableObject
    {
        
        [SerializeField] private List<EnemyData> _enemiesData;
        public float GetDamage(int id)
        {
            var _enemy =
                _enemiesData.FirstOrDefault(x =>
                     x.ID == id);

            return _enemy.Damage;
        }
        public float GetMaxHealth(int id)
        {
            var _enemy =
                _enemiesData.FirstOrDefault(x =>
                     x.ID == id);

            return _enemy.MaxHealth;
        }
        public int GetArmor(int id)
        {
            var _enemy =
                _enemiesData.FirstOrDefault(x =>
                     x.ID == id);

            return _enemy.Armor;
        }
        public float GetSpeed(int id)
        {
            var _enemy =
                _enemiesData.FirstOrDefault(x =>
                     x.ID == id);

            return _enemy.MaxSpeed;
        }
        public Animator GetAnimator(int id)
        {
            var _enemy =
                _enemiesData.FirstOrDefault(x =>
                     x.ID == id);

            return _enemy.EnemyAnimator;
        }
        public Sprite GetSprite(int id)
        {
            var _enemy =
                _enemiesData.FirstOrDefault(x =>
                     x.ID == id);

            return _enemy.EnemySprite;
        }
        public Enemy GetEnemyPrefab(int id)
        {
            var _enemy =
                _enemiesData.FirstOrDefault(x =>
                     x.ID == id);

            return _enemy.EnemyPrefab;
        }

    }
}

