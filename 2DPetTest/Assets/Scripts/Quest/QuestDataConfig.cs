using System.Collections.Generic;
using UnityEngine;

namespace Examples.PlatformerExample.Scripts.Quest
{
    [CreateAssetMenu(fileName = "QuestDataConfig", menuName = "ScriptableObjects/QuestDataConfig", order = 1)]
    public class QuestDataConfig : ScriptableObject
    {
        [SerializeField] private List<QuestData> _questData;

        public List<QuestData> QuestData => _questData;
    }
}

