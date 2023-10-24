using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о том что игрок начал диалог
    /// </summary>
    public class PlayerStartDialogSignal
    {
        public readonly TextAsset Text;

        public PlayerStartDialogSignal(TextAsset text)
        {
            Text = text;
        }
    }
}

