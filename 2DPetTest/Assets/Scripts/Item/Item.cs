using UnityEngine;
using CustomEventBus;
using Items.Weapons;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        [Header("Разное:")]
        [SerializeField] protected GameObject _itemRoot;
        [SerializeField] private ItemData _itemData;

        [Header("Звук")]
        [SerializeField] protected AudioClip _useSound;
        [SerializeField] protected AudioClip _changeItemSound;
        [SerializeField] protected AudioSource _useAudioSource;

        [Header("Анимация")]
        [SerializeField] protected Animator _animatorItem;
        public int _amountCurrent = 1;

        public GameObject Owner { get; set; }
        public GameObject SourcePrefab { get; set; }
        protected abstract string _animUseParameter { get; }
        private bool _wantsToUse = false;

        public GameObject ItemRoot => _itemRoot;
        public ItemData ItemData => _itemData;

        protected EventBus _eventBus;

        public abstract string GetDescription();

        private void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
        }
        
        public void ShowItem(bool show)
        {
            ItemRoot.SetActive(show);

            /// Звук смены предмета
            if (show && _changeItemSound)
            {
                _useAudioSource.PlayOneShot(_changeItemSound);
            }

            _wantsToUse = show;
        }

        public virtual bool HandleUseInputs(bool inputDown, bool inputHeld, bool inputUp)
        {
            return false;
        }

        public virtual bool TryUse()
        {
            HandleUse();
            return true;
        }

        public virtual void HandleUse()
        {
            /// Активировать звук использования, если она есть
            if (_useSound)
            {
                _useAudioSource.PlayOneShot(_useSound);
            }

            /// Активировать анимацию использования, если она есть
            if (_animatorItem)
            {
                _animatorItem.SetTrigger(_animUseParameter);
            }

            _eventBus = ServiceLocator.Current.Get<EventBus>();
        }

        public virtual bool DoStack()
        {
            if (_itemData.MaximumAmountInSlot <= 1)
            {
                return false;
            }
            return true;
        }

        public virtual void DestroyItem()
        {
            Destroy(gameObject);
        }

        public void Attack(Weapon weapon)
        {
            Owner = weapon.gameObject;
        }

        public void Use(Item item)
        {
            Owner = item.gameObject;
        }
    }
}

