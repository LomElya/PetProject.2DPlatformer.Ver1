using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Platformer.Mechanics;
using System;


namespace Interactables
{
    public abstract class Interactable : KinematicObject
    {
        [Header("Анимация")]
        [SerializeField] private float _verticalBobFrequency = 0.1f;
        [SerializeField] private float _bobbingAmount = 0.03f;
        [Header("Звук")]
        [SerializeField] protected AudioClip _pickUpSound;
        public AudioSource _pickUpAudioSource { get; private set; }
        //public AudioClip PickUpSound => _pickUpSound;
        [Header("Эффекты")]
        [SerializeField] public GameObject VFX_Pickup;
        protected Collider2D _collider;
        private Vector2 _startPosition;
        protected EventBus _eventBus;
        private bool _wantsInteract = false;

        protected abstract void Interact(PlayerController playerController);
        protected abstract void outSideInteract();

        protected override void Start()
        {
            base.Start();
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _pickUpAudioSource = GetComponent<AudioSource>();
            _startPosition = transform.position;
        }
        protected override void Update()
        {

            if (IsGrounded)
            {
                _wantsInteract = true;
                ///Покачивание 
                //transform.position = _startPosition + Vector2.up * bobbingAnimation;
                velocity.y = 1.7f;
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!_wantsInteract)
                return;

            _collider = collider;
            PlayerController playerController = collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Interact(playerController);
                Dispose();
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            _collider = col;
            if (_collider.gameObject.tag.Equals("Player"))
            {
                //Debug.Log("Вышел");
                outSideInteract();
                Dispose();
            }
        }
        protected void Hide()
        {
            Destroy(gameObject);
        }
        protected void PlayPickupSound()
        {
            if (_pickUpSound)
            {
                //_pickUpAudioSource.PlayOneShot(_pickUpSound);
                AudioUtility.CreateSFX(_pickUpSound, transform.position, AudioUtility.AudioGroups.ItemPickup, 0f);
            }
        }
        protected void EffectPickup()
        {
            if (VFX_Pickup)
            {
                Instantiate(VFX_Pickup, transform.position, Quaternion.identity);
            }
        }

        private void Dispose()
        {
            _eventBus.Invoke(new DisposeInteractableSignal(this));
        }
    }



}
