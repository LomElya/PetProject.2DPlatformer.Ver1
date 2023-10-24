using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using System.Linq;
using Enemies;

public class DetectionModule : MonoBehaviour
{
    [Tooltip("Точка откуда исходил луч для обнаружения врагов")]
    [SerializeField] private Transform _detectionSourcePoint;

    [Tooltip("Максимальная дистанция обнаружения")]
    [SerializeField] private float _detectionRange = 2f;

    [Tooltip("Максимальная дальность атаки")]
    [SerializeField] private float _attackRange = 1f;

    [Tooltip("Время потери цели")]
    [SerializeField] private float _knownTargetTimeout = 4f;
    [Tooltip("Игнорирование коллайдера")]
    [SerializeField] private LayerMask _ignorLayer;
    private PersonagesManager _personagesManager;

    public GameObject KnownDetectedTarget { get; private set; }
    public bool IsTargetInAttackRange { get; private set; }
    public bool IsSeeingTarget { get; private set; }
    public bool HadKnownTarget { get; private set; }

    public Transform DetectionSourcePoint => _detectionSourcePoint;
    public float DetectionRange => _detectionRange;
    public float AttackRange => _attackRange;

    protected float TimeLastSeenTarget = Mathf.NegativeInfinity;

    private Enemy _enemy;
    private EventBus _eventBus;

    public void Init(EventBus eventBus, Enemy enemy)
    {
        _personagesManager = GameObject.FindObjectOfType<PersonagesManager>();

        _eventBus = eventBus;
        _enemy = enemy;
    }
    public void HandleTargetDetection(Personage actor, Collider2D[] selfCollider)
    {
        /// Обработка обнаружения цели
        if (KnownDetectedTarget && !IsSeeingTarget && (Time.time - TimeLastSeenTarget) > _knownTargetTimeout)
        {
            KnownDetectedTarget = null;
        }

        /// Найти ближайщего видимого врага
        float sqrDetectionRange = _detectionRange * _detectionRange;
        IsSeeingTarget = false;
        float closestSqrDistance = Mathf.Infinity;
        foreach (Personage otherPersonage in _personagesManager.Personages)
        {
            if (otherPersonage.Affiliation != actor.Affiliation)
            {
                float sqrDistance = (otherPersonage.transform.position - _detectionSourcePoint.position).sqrMagnitude;
                if (sqrDistance < sqrDetectionRange && sqrDistance < closestSqrDistance)
                {
                    /// Проверка на наличие препятствия
                    RaycastHit2D[] hits = Physics2D.RaycastAll(_detectionSourcePoint.position,
                        (otherPersonage.AimPoint.position - _detectionSourcePoint.position).normalized, _detectionRange, -1);

                    RaycastHit2D closestValidHit = new RaycastHit2D();
                    closestValidHit.distance = Mathf.Infinity;
                    bool foundValidHit = false;
                    foreach (var hit in hits)
                    {
                        int layerIndex = Mathf.RoundToInt(Mathf.Log(_ignorLayer.value, 2));

                        if (!selfCollider.Contains(hit.collider) && hit.distance < closestValidHit.distance && hit.collider.gameObject.layer != layerIndex)
                        {
                            Debug.DrawLine(_detectionSourcePoint.position, hit.collider.transform.position, Color.red);
                            closestValidHit = hit;
                            foundValidHit = true;
                        }
                    }

                    if (foundValidHit)
                    {
                        Personage hitPersonage = closestValidHit.collider.GetComponentInParent<Personage>();
                        if (hitPersonage == otherPersonage)
                        {
                            Debug.DrawLine(_detectionSourcePoint.position, closestValidHit.collider.transform.position, Color.green);
                            IsSeeingTarget = true;
                            closestSqrDistance = sqrDistance;

                            TimeLastSeenTarget = Time.time;
                            KnownDetectedTarget = otherPersonage.AimPoint.gameObject;
                        }
                    }

                }
            }
        }

        IsTargetInAttackRange = KnownDetectedTarget != null && Vector2.Distance(transform.position, KnownDetectedTarget.transform.position)
        <= _attackRange;

        /// Эвенты обнаружения
        if (!HadKnownTarget && KnownDetectedTarget != null)
        {
            OnDetect();
        }

        if (HadKnownTarget && KnownDetectedTarget == null)
        {
            OnLostTarget();
        }

        /// Помнить, если уже знали цель(для следующего кадра)
        HadKnownTarget = KnownDetectedTarget != null;
    }
    private void OnDetect() => _eventBus.Invoke(new EnemyDetectSignal(_enemy));
    private void OnLostTarget() => _eventBus.Invoke(new EnemyLostTargetSignal(_enemy));

    public void OnDamaged(GameObject damageSource)
    {
        TimeLastSeenTarget = Time.time;
        KnownDetectedTarget = damageSource;
    }

}
