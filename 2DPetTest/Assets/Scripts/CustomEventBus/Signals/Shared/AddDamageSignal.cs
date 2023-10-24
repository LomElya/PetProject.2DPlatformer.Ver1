using UnityEngine;
using UnityEngine.UI;

namespace CustomEventBus.Signals
{
    public class AddDamageSignal
    {
        public readonly float Damage;
        public readonly Vector2 UnitPosition;
        public AddDamageSignal(float damage, Vector2 unitPosition)
        {
            Damage = damage;
            UnitPosition = unitPosition;
        }
    }
}
