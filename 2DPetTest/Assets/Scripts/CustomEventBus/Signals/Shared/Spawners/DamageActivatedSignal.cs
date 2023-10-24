using UnityEngine;
using UnityEngine.UI;

namespace CustomEventBus.Signals
{
    public class DamageActivatedSignal
    {
        public readonly Text Text;
        public readonly Camera Camera;
        public readonly Vector2 UnitPosition;
        public DamageActivatedSignal(Text text, Camera camera, Vector2 unitPosition)
        {
            Text = text;
            Camera = camera;
            UnitPosition = unitPosition;
        }
    }
}
