using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

namespace Enemies.AI
{
    public class EnemyMove : MonoBehaviour
    {
        public enum AIState
        {
            Patrol,
            Follow,
            Attack,
        }
        
        [SerializeField][Range(0f, 1f)] private float AttackStopDistanceRatio = 0.1f;
        [Header("Эффекты")]
        [SerializeField] private ParticleSystem[] _randomHitSparks;
        [SerializeField] private ParticleSystem[] _onDetectVfx;
        [Header("Звук")]
        [SerializeField] private AudioClip _onDetectSound;
        [SerializeField] private AudioClip _movementSound;
        [SerializeField] private AudioSource _audioSource;

        [Header("Разное")]
        public MinMaxFloat PitchDistortionMovementSpeed;
        private Enemy _enemy;
        private Animator _animator;
        public AIState _aiState { get; private set; }

        private const string _triggerTextAlerted = "Alerted";
        private const string _animTextMoveSpeed = "MoveSpeed";
        private const string _triggerTextOnDamage = "OnDamage";

        private EventBus _eventBus;

        public void Init(EventBus eventBus, Enemy enemy, Animator animator)
        {
            _eventBus = eventBus;
            _enemy = enemy;
            _animator = animator;

            _aiState = AIState.Patrol;

            _audioSource.clip = _movementSound;
            _audioSource.Play();

            _eventBus.Subscribe<EnemyAttackSignal>(OnAttack);
        }

        private void Update()
        {
            UpdateAiState();
            UpdateCurrentAiState();

            float moveSpeed = _enemy.MaxSpeed;

            _animator.SetFloat(_animTextMoveSpeed, moveSpeed);

            _audioSource.pitch = Mathf.Lerp(PitchDistortionMovementSpeed.Min, PitchDistortionMovementSpeed.Max,
                            moveSpeed / _enemy.MaxSpeed);
        }

        private void UpdateAiState()
        {
            switch (_aiState)
            {
                case AIState.Follow:
                    /// Атаковать, если видит врага и враг находится в радиусе атаки
                    if (_enemy.IsSeeingTarget && _enemy.IsTargetInAttackRange)
                    {
                        _aiState = AIState.Attack;
                        _enemy.SetNavDestination(transform.position);
                    }

                    break;
                case AIState.Attack:
                    /// Преследовать врага, если он не находится в радиусе атаки
                    if (!_enemy.IsTargetInAttackRange)
                    {
                        _aiState = AIState.Follow;
                    }

                    break;
            }
        }

        private void UpdateCurrentAiState()
        {
            switch (_aiState)
            {
                case AIState.Patrol:
                    _enemy.UpdatePathDestination();
                    _enemy.SetNavDestination(_enemy.GetDestinationOnPath());
                    break;

                case AIState.Follow:
                    _enemy.SetNavDestination(_enemy.KnownDetectedTarget.transform.position);
                    //_enemy.OrientTowards(_enemy.KnownDetectedTarget.transform.position);
                    _enemy.OrientWeaponsTowards(_enemy.KnownDetectedTarget.transform.position);
                    break;

                case AIState.Attack:
                    if (Vector2.Distance(_enemy.KnownDetectedTarget.transform.position,
                            _enemy.DetectionModule.DetectionSourcePoint.position)
                        >= (AttackStopDistanceRatio * _enemy.DetectionModule.AttackRange))
                    {
                        _enemy.SetNavDestination(_enemy.KnownDetectedTarget.transform.position);
                    }
                    else
                    {
                        _enemy.SetNavDestination(transform.position);
                    }
                    //_enemy.OrientTowards(_enemy.KnownDetectedTarget.transform.position);
                    _enemy.TryAttack(_enemy.KnownDetectedTarget.transform.position, _enemy.KnownDetectedTarget);
                    break;
            }
        }

        public void OnDetectTarget()
        {
            if (_aiState == AIState.Patrol)
            {
                _aiState = AIState.Follow;
            }

            for (int i = 0; i < _onDetectVfx.Length; i++)
            {
                _onDetectVfx[i].Play();
            }

            if (_onDetectSound)
            {
                AudioUtility.CreateSFX(_onDetectSound, transform.position, AudioUtility.AudioGroups.EnemyDetection, 1f);
            }

            _animator.SetBool(_triggerTextAlerted, true);
        }

        public void OnLostTarget()
        {
            if (_aiState == AIState.Follow || _aiState == AIState.Attack)
            {
                _aiState = AIState.Patrol;
            }

            for (int i = 0; i < _onDetectVfx.Length; i++)
            {
                _onDetectVfx[i].Stop();
            }

            _animator.SetBool(_triggerTextAlerted, false);
        }

        public void OnDamaged()
        {
            if (_randomHitSparks.Length > 0)
            {
                int random = Random.Range(0, _randomHitSparks.Length - 1);
                _randomHitSparks[random].Play();
            }
        }

        private void OnAttack(EnemyAttackSignal signal)
        {
            Enemy enemy = signal.Enemy;

            if (enemy == _enemy && _animator != null)
                _animator.SetTrigger(_triggerTextOnDamage);
        }
        
        private void OnDestroy()
        {
            /* _eventBus.Unsubscribe<OnAlertSignal>(OnDetected);
            _eventBus.Unsubscribe<OnLostSignal>(OnLost); */
            _eventBus.Unsubscribe<EnemyAttackSignal>(OnAttack);
        }
    }
}
