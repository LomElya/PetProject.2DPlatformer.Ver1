using UnityEngine;
using UnityEngine.UI;

namespace CustomEventBus.Signals
{
    public class DamageRemoveSignal
    {
        public readonly Text Text;
        public DamageRemoveSignal(Text text)
        {
            Text = text;
        }
    }
}
