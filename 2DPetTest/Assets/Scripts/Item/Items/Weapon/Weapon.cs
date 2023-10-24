using UnityEngine;
using CustomEventBus.Signals;

namespace Items.Weapons
{
    public enum WeaponType
    {
        Undefined = 0,
        Sword = 1,
        Knife = 2,
        Bow = 3,
    }
    public enum RangeAttackType
    {
        Undefined = 0,
        Melee = 1,
        Range = 2,
    }
    public enum WeaponAttackType
    {
        Manual,
        Automatic,
        Charge,
    }
    public abstract class Weapon : Item
    {
        [Header("Параметры:")]
        [SerializeField] private float _damage;
        [SerializeField] private float _delayBetweenAttack = 0.5f;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRange;
        [Header("Остальное:")]
        [SerializeField] private LayerMask _enemyLayers;
        [SerializeField] private WeaponAttackType _attackType;
        [SerializeField] private WeaponType _typeWeapon;
        [SerializeField] private RangeAttackType _rangeTypeAttack;
        private float _lastTimeAttack = Mathf.NegativeInfinity;

        public WeaponType TypeWeapon => _typeWeapon;
        public RangeAttackType RangeTypeAttack => _rangeTypeAttack;
        public float Damage => _damage;
        public LayerMask EnemyLayers => _enemyLayers;
        public Transform AttackPoint => _attackPoint;
        public float AttackRange => _attackRange;
        public bool IsWeaponActive { get; private set; }
        private bool _wantsToAttack = false;
        protected override string _animUseParameter => "isAttack";

        public override bool HandleUseInputs(bool inputDown, bool inputHeld, bool inputUp)
        {
            _wantsToAttack = inputDown || inputHeld;

            switch (_attackType)
            {
                case WeaponAttackType.Manual:
                    if (inputDown)
                    {
                        return TryUse();
                    }
                    return false;

                case WeaponAttackType.Automatic:
                    if (inputHeld)
                    {
                        return TryUse();
                    }
                    return false;

                default:
                    return false;
            }
        }
        public override bool TryUse()
        {
            if (_delayBetweenAttack + _lastTimeAttack <= Time.time)
            {
                HandleUse();
                return true;
            }
            return false;
        }
        public override void HandleUse()
        {
            Attack(this);
            _lastTimeAttack = Time.time;

            base.HandleUse();

            _eventBus.Invoke(new OnAttackSignal(this, _enemyLayers, _attackPoint, _attackRange));
        }


        public void OnDrawGizmosSelected()
        {
            if (_attackPoint == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
        }
    }
}

