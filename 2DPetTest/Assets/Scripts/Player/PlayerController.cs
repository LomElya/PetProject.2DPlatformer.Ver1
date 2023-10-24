using UnityEngine;
using Platformer.Model;
using CustomEventBus;

namespace Platformer.Mechanics
{
    public class PlayerController : KinematicObject
    {
        [SerializeField] private float _footstepFrequency = 1f;
        [SerializeField] private Camera _mainCamera;

        [Header("Звук")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _footstepSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _landedSound;
        [SerializeField] private AudioClip _gotDamageSound;

        [Header("Разное")]
        [SerializeField] private PlayerInputHandler _inputHandler;
        [SerializeField] private Collider2D collider2d;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform _endDrop;
        private Player _player;
        private bool stopJump;
        private bool jump;
        private Vector2 move;

        private bool faceRight = true;
        private Vector3 _mousePosition;

        private float _footstepDistanceCounter;
        public JumpState jumpState = JumpState.Grounded;
        public Bounds Bounds => collider2d.bounds;
        public Transform EndDrop => _endDrop;

        ///Модель поведения юнитов
        [SerializeField] private float jumpModifier = 1.5f;


        [SerializeField] private float jumpDeceleration = 0.2f;
        ///

        private EventBus _eventBus;

        /// Init в Player
        public void Init(Player player)
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _player = player;

            PersonagesManager personagesManager = FindObjectOfType<PersonagesManager>();
            if (personagesManager != null)
                personagesManager.SetPlayer(_player);

            //GameInput.Key.LoadSettings();
            transform.position = _player.mainInformation.initialValue;

            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            HandleCharacterMovement();

            UpdateJumpState();
            base.Update();
        }
        private void HandleCharacterMovement()
        {
            move.x = _inputHandler.GetMoveInput();
            _mousePosition = _mainCamera.WorldToScreenPoint(transform.position);

            if (jumpState == JumpState.Grounded && _inputHandler.GetJumpInputDown())
                jumpState = JumpState.PrepareToJump;
            else if (_inputHandler.GetJumpInputDown())
            {
                stopJump = true;
            }
        }

        private void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;

                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        jumpState = JumpState.InFlight;
                    }
                    break;

                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        jumpState = JumpState.Landed;
                    }
                    break;

                case JumpState.Landed:
                    if (_landedSound != null)
                        _audioSource.PlayOneShot(_landedSound);

                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (IsGrounded)
            {
                if (jump && IsGrounded)
                {
                    if (_jumpSound != null)
                        _audioSource.PlayOneShot(_jumpSound);

                    velocity.y = _player.JumpTakeOffSpeed * jumpModifier; // jumpModifier в модели
                    jump = false;
                }
                else if (stopJump)
                {
                    stopJump = false;
                    if (velocity.y > 0)
                    {
                        velocity.y = velocity.y * jumpDeceleration; //jumpDeceleration в модели
                    }
                }
                animator.SetBool("onGround", IsGrounded);
                animator.SetFloat("moveX", Mathf.Abs(velocity.x) / _player.MaxSpeedOnGround);

                targetVelocity = move * _player.MaxSpeedOnGround;

                if (_footstepDistanceCounter >= 0.4f)
                {
                    _footstepDistanceCounter = 0f;

                    _audioSource?.PlayOneShot(_footstepSound);
                }
                _footstepDistanceCounter += targetVelocity.magnitude * Time.deltaTime;
            }
            else
            {
                targetVelocity = move * _player.MaxSpeedInAir;
            }
            /// Поворот по нажатию клавишь 
            /*  if (faceRight == false && move.x > 0.01f)
                 Flip();
             else if (faceRight == true && move.x < -0.01f)
                 Flip(); */
            /// Поворот следом за мышью
            if (Input.mousePosition.x < _mousePosition.x && faceRight == true)
                Flip();
            if (Input.mousePosition.x > _mousePosition.x && faceRight == false)
                Flip();
        }

        private void Flip()
        {
            faceRight = !faceRight;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }
        public void onHit()
        {
            if (_gotDamageSound != null)
                _audioSource.PlayOneShot(_gotDamageSound);
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}
