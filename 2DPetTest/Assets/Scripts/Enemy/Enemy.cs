using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using Items.Weapons;
using Items;
using Enemies.AI;


namespace Enemies
{
    public abstract class Enemy : KinematicObject
    {
        [Header("Информация")]
        [SerializeField] private int _id;
        [SerializeField] private float _pathReachingRadius = 2f;

        private int _armor;
        [SerializeField] private float _maxSpeed;
        private Sprite _sprireEnemy;
        public StatsBarEnemy _statsBarEnemy;
        [Header("Звук")]
        [SerializeField] private AudioClip _damageSound;
        [SerializeField] private AudioClip _dieSound;
        [SerializeField] private AudioSource _enemyAudioSource;
        [Header("Лут")]
        [SerializeField] private List<DropItem> _lootItem = new List<DropItem>();
        public List<DropItem> LootItem => _lootItem;

        [Header("Разное")]
        [SerializeField] private Animator _animatorEnemy;
        [SerializeField] private Health _enemyHealth;
        [SerializeField] private Weapon _enemyWeapon;
        [SerializeField] private EnemyMove _enemyMover;
        [SerializeField] private DetectionModule _detectionModule;
        [SerializeField] private Personage _personage;

        [Header("Прорисовка дальности")]
        [SerializeField] private Color _attackRangeColor = Color.red;
        [SerializeField] private Color _detectionRangeColor = Color.blue;
        public PatrolPath PatrolPath { get; set; }
        public EnemyManager _enemyManager { get; set; }
        private NavigationModule _navigationModules;
        private float oldSpeed;
        private int _pathDestinationNodeIndex;
        private Vector2 move;

        public Weapon EnemyWeapon => _enemyWeapon;
        public Health EnemyHealth => _enemyHealth;

        public int ID => _id;
        public float Armor => _armor;
        public float MaxSpeed => _maxSpeed;
        public Animator Animator => _animatorEnemy;
        public Sprite SprireEnemy => _sprireEnemy;
        public EnemyMove EnemyMover => _enemyMover;
        public DetectionModule DetectionModule => _detectionModule;

        public GameObject KnownDetectedTarget => _detectionModule.KnownDetectedTarget;
        public bool IsTargetInAttackRange => _detectionModule.IsTargetInAttackRange;
        public bool IsSeeingTarget => _detectionModule.IsSeeingTarget;
        public bool HadKnownTarget => _detectionModule.HadKnownTarget;

        private const string _triggerTextOnHit = "isHit";
        private const string _triggerTextOnDamage = "OnDamage";

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        private bool jump;
        private float _jumpTakeOffSpeed = 2f;

        protected EventBus _eventBus;

        [SerializeField] private Collider2D[] _selfCollider;
        protected abstract string GetDescription();
        protected override void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            _enemyManager = FindObjectOfType<EnemyManager>();
            _enemyManager.RegisterEnemy(this);
            _selfCollider = GetComponentsInChildren<Collider2D>();
            InitElement();

            oldSpeed = _maxSpeed;
        }
        protected override void Update()
        {
            //targetVelocity = move;

            UpdateJumpState();
            _detectionModule.HandleTargetDetection(_personage, _selfCollider);
            base.Update();
        }
        private void InitElement()
        {
            _enemyHealth.Init(gameObject);
            _enemyMover.Init(_eventBus, this, _animatorEnemy);
            _detectionModule.Init(_eventBus, this);
        }

        private bool IsPathValid()
        {
            return PatrolPath && PatrolPath._pathNodes.Count > 0;
        }

        public Vector3 GetDestinationOnPath()
        {
            if (IsPathValid())
            {
                return PatrolPath.GetPositionOfPathNode(_pathDestinationNodeIndex);
            }
            else
            {
                return transform.position;
            }
        }

        public void SetNavDestination(Vector2 destination)
        {
            //transform.position = Vector2.MoveTowards(transform.position, destination, _maxSpeed * Time.deltaTime);

            if (jumpState == JumpState.Grounded)
                jumpState = JumpState.PrepareToJump;
            else
            {
                stopJump = true;
            }
        }

        ///Временно тут
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
                    /*  if (_landedSound != null)
                         _audioSource.PlayOneShot(_landedSound); */

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
                    _animatorEnemy.SetTrigger("isJumping");
                    velocity.y = _jumpTakeOffSpeed * 1.5f; // jumpModifier в модели
                    jump = false;
                }
                else if (stopJump)
                {
                    stopJump = false;
                    if (velocity.y > 0)
                    {
                        velocity.y = velocity.y * 0.2f; //jumpDeceleration в модели
                    }
                }
                /*  _animatorEnemy.SetBool("onGround", IsGrounded);
                 _animatorEnemy.SetFloat("moveX", Mathf.Abs(velocity.x) / _maxSpeed); */

                //targetVelocity = move * _maxSpeed;  //Скорость на земле

                /*  if (_footstepDistanceCounter >= 0.4f)
                 {
                     _footstepDistanceCounter = 0f;

                     if (_footstepSound != null)
                         _audioSource.PlayOneShot(_footstepSound);
                 }
                 _footstepDistanceCounter += targetVelocity.magnitude * Time.deltaTime; */
            }
            /* else
            {
                //targetVelocity = move * _maxSpeed;  //Скорость в воздухе
            } */
        }
        ///

        public void OrientTowards(Vector3 lookPosition)
        {
            Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, Vector3.up).normalized;
            if (lookDirection.sqrMagnitude != 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
            }
        }
        public void OrientWeaponsTowards(Vector3 lookPosition)
        {
            if (_enemyWeapon != null)
            {
                //Направить оружие на игрока
                Vector3 weaponForward = (lookPosition - _enemyWeapon.ItemRoot.transform.position).normalized;
                _enemyWeapon.transform.forward = weaponForward;
            }
        }
        public bool TryAttack(Vector3 enemyPosition, GameObject hurt)
        {
            /* if (_aameFlowManager.GameIsEnding)
                return false; */

            OrientWeaponsTowards(enemyPosition);

            // Атаковать
            bool didFire = _enemyWeapon.HandleUseInputs(false, true, false);

            if (didFire)
            {
                ///Сигнал о атаке врага
                _eventBus.Invoke(new EnemyAttackSignal(this, hurt));
            }

            return didFire;
        }

        public void UpdatePathDestination(bool inverseOrder = false)
        {
            if (IsPathValid())
            {
                /// Если дошел до конца
                if ((transform.position - GetDestinationOnPath()).magnitude <= _pathReachingRadius)
                {
                    /// Увеличить индекс
                    _pathDestinationNodeIndex =
                        inverseOrder ? (_pathDestinationNodeIndex - 1) : (_pathDestinationNodeIndex + 1);
                    if (_pathDestinationNodeIndex < 0)
                    {
                        _pathDestinationNodeIndex += PatrolPath._pathNodes.Count;
                    }

                    if (_pathDestinationNodeIndex >= PatrolPath._pathNodes.Count)
                    {
                        _pathDestinationNodeIndex -= PatrolPath._pathNodes.Count;
                    }
                }
            }
        }

        public bool TryDropItem(DropItem LootItem)
        {
            if (LootItem._dropRate == 0 || LootItem._lootPregabItem == null)
                return false;
            else if (LootItem._dropRate == 1)
                return true;
            else
                return (Random.value <= LootItem._dropRate);
        }

        public void onHit(GameObject damageSource, float damage)
        {
            if (_damageSound)
            {
                AudioUtility.CreateSFX(_damageSound, transform.position, AudioUtility.AudioGroups.SlimeTakeHit, 0f);
            }

            if (_animatorEnemy)
                _animatorEnemy.SetTrigger(_triggerTextOnHit);

            _enemyMover.OnDamaged();

            _detectionModule.OnDamaged(damageSource);
            _eventBus.Invoke(new EnemyDamageSignal(this, damage, damageSource));
            _eventBus.Invoke(new AddDamageSignal(damage, transform.position));
            StartCoroutine(SlowTime());
        }

        public void Die()
        {
            if (_dieSound)
            {
                AudioUtility.CreateSFX(_dieSound, transform.position, AudioUtility.AudioGroups.SlimeDie, 0f);
            }
        }

        public void Hide()
        {
            Destroy(gameObject);
        }
        public void Hide(float hideDuration)
        {
            Destroy(gameObject, hideDuration);
        }

        private IEnumerator SlowTime()
        {
            _maxSpeed = 0.2f;
            yield return new WaitForSeconds(1f);
            _maxSpeed = oldSpeed;
        }


        private void OnDrawGizmosSelected()
        {
            if (_detectionModule != null)
            {
                // Дальность обнаружения
                Gizmos.color = _detectionRangeColor;
                Gizmos.DrawWireSphere(transform.position, _detectionModule.DetectionRange);

                // Дальность атаки
                Gizmos.color = _attackRangeColor;
                Gizmos.DrawWireSphere(transform.position, DetectionModule.AttackRange);

                Gizmos.color = Color.yellow;
                //
            }
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
