using UnityEngine;

namespace CustomEventBus.Signals
{
    public class DieSignal
    {
        public readonly GameObject Chatacter;

        public DieSignal(GameObject сhatacter)
        {
            Chatacter = сhatacter;
        }
    }
}

