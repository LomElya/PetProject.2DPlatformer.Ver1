using UnityEngine;
using CustomEventBus.Signals;
using Items.Weapons;

namespace Items.Flasks
{
    public enum FlaskType
    {
        Undefined = 0,
        Heal = 1,
        Posion = 2
    }
    public abstract class Flask : Item
    {
        [Header("Параметры")]
        [SerializeField] private FlaskType _typeFlask;
        [SerializeField] private float _value;
        private PlayersItemManager _playersItemManager;
        public float Value => _value;
        public FlaskType TypeFlask => _typeFlask;
        protected override string _animUseParameter => "isUse";

        public override bool HandleUseInputs(bool inputDown, bool inputHeld, bool inputUp)
        {
            if (inputDown)
            {
                return TryUse();
            }
            return false;
        }
        public override bool TryUse()
        {
            HandleUse();
            return true;
        }
        public override void HandleUse()
        {
            base.HandleUse();
            Use(this);

            AudioUtility.CreateSFX(_useSound, transform.position, AudioUtility.AudioGroups.ItemPickup, 0f);
            
            _eventBus.Invoke(new OnUseHealthSignal(this, _value));

            _playersItemManager = FindObjectOfType<PlayersItemManager>();
            int currentIndex = _playersItemManager.ActiveItemIndex;
            _eventBus.Invoke(new OutOfUseSignal(this, currentIndex));
            //Destroy(this.gameObject);
        }
    }
}

