using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CustomEventBus.Signals
{
     /// <summary>
    /// Сигнал о том что игрок вошел в зону к NPC
    /// </summary>
    public class PlayerEnteredNPCZoneSignal
    {
       public readonly TextAsset Text;
       public readonly string TextEventButton;
       public readonly bool PlayerInZone;
        public PlayerEnteredNPCZoneSignal(TextAsset text, string textEventButton)
        {
            Text = text;
            TextEventButton = textEventButton;
        }
        
    }
}

