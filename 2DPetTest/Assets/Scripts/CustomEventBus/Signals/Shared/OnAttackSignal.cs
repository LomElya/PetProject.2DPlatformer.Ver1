using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items.Weapons;

namespace CustomEventBus.Signals
{
    public class OnAttackSignal
    {
        public readonly Weapon Weapon;
        public readonly LayerMask EnemyLayers;
        public readonly Transform AttackPoint;
        public readonly float AttackRange;
        public OnAttackSignal(Weapon weapon)
        {
            Weapon = weapon;
        }
        public OnAttackSignal(Weapon weapon, LayerMask enemyLayers)
        {
            Weapon = weapon;
            EnemyLayers = enemyLayers;
        }
        public OnAttackSignal(Weapon weapon, LayerMask enemyLayers, Transform attackPoint, float attackRange)
        {
            Weapon = weapon;
            EnemyLayers = enemyLayers;
            AttackPoint = attackPoint;
            AttackRange = attackRange;
        }
    }
}
