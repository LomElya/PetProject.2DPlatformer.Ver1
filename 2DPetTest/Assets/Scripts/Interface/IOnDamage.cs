using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CustomEventBus;
using CustomEventBus.Signals;

namespace Interface 
{
    public interface IOnDamage 
    {
        public void onDamage(EnemyDamageSignal signal);
    }
}

