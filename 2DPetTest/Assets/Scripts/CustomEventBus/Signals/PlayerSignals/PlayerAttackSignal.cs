using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о том что Игрок атакует
    /// </summary>
    public class PlayerAttackSignal
    {
        public readonly Animator Animator;
        public readonly float DamageValue;
        public PlayerAttackSignal(Animator animator, float damageValue)
        {
            Animator = animator;
            DamageValue = damageValue;
        }
    }
}

