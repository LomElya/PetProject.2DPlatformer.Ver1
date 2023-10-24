using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using UnityEngine.UI;
using CustomEventBus.Signals;
using CustomPool;
using DamageValue;

public class DamageUI : MonoBehaviour, IService
{


    [SerializeField] private Text _textPrefab;

    private Camera _Camera;
    private Transform _transform;
    private readonly Dictionary<string, CustomPool<Text>> _pools =
        new Dictionary<string, CustomPool<Text>>();
    ///Удалить

    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<AddDamageSignal>(AddText);
        _eventBus.Subscribe<DamageRemoveSignal>(RemoveText);
    }
    private void Start()
    {
        _Camera = Camera.main;
        _transform = transform;
    }
    public void AddText(AddDamageSignal signal)
    {
        float damage = signal.Damage;
        Vector2 unitPos = signal.UnitPosition;

        Text text = _textPrefab;

        var pool = GetPool(text);

        Text item = pool.Get();
        item.text = "-" + damage;
        item.transform.parent = _transform;

        _eventBus.Invoke(new DamageActivatedSignal(item, _Camera, unitPos));
    }
    public void RemoveText(DamageRemoveSignal signal)
    {
        var text = signal.Text;
        var pool = GetPool(text);
        pool.Release(text);
    }
    private CustomPool<Text> GetPool(Text _text)
    {
        var objectTypeStr = _text.GetType().ToString();
        CustomPool<Text> pool;

        // Создать новый пул, если такого нет
        if (!_pools.ContainsKey(objectTypeStr))
        {
            pool = new CustomPool<Text>(_text, 5);
            _pools.Add(objectTypeStr, pool);
        }
        // Если пул есть, возвращаем его
        else
        {
            pool = _pools[objectTypeStr];
        }


        return pool;
    }
    private void OnDestroy()
    {
        _eventBus.Unsubscribe<AddDamageSignal>(AddText);
        _eventBus.Unsubscribe<DamageRemoveSignal>(RemoveText);
    }
}

