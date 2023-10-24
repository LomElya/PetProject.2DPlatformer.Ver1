using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Model
{
    [System.Serializable]
    public class PlatformerModel
    {
        
        public Cinemachine.CinemachineVirtualCamera virtualCamera;

        
        public PlayerController player;

        
        public Transform spawnPoint;

        
        public float jumpModifier = 1.5f;

        
        public float jumpDeceleration = 0.2f;
        

        //public LevelChanger level;

    }
}

