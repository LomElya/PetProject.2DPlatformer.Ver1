using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

/// <summary>
/// Система отвечающая за золото:
/// Начисление, трата, изменение золота
/// </summary>

public class СoinController : IService, IDisposable
{
    private int _coin;
    public int Coin => _coin;

    private EventBus _eventBus;
    public void Init()
    {
        _coin = PlayerPrefs.GetInt(StringConstants.COIN, 0);

        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<AddCoinSignal>(OnAddCoin);
        _eventBus.Subscribe<SpendCoinSignal>(SpendCoin);
        _eventBus.Subscribe<ChangedCoinSignal>(ChangedCoin);
    }

    private void OnAddCoin(AddCoinSignal signal)
    {
        OnAddGold(signal.Value);
    }
    private void OnAddGold(int value)
    {
        _coin += value;
        _eventBus.Invoke(new ChangedCoinSignal(_coin));
    }

    private void SpendCoin(SpendCoinSignal signal)
    {
        if (HaveEnoughCoin(signal.Value))
        {
            _coin -= signal.Value;
            _eventBus.Invoke(new ChangedCoinSignal(_coin));
        }
    }

    private void ChangedCoin(ChangedCoinSignal signal)
    {
        PlayerPrefs.SetInt(StringConstants.COIN, signal.Value);
    }

    public bool HaveEnoughCoin(int coin)
    {
        return _coin >= coin;
    }


    public void Dispose()
    {
        _eventBus.Unsubscribe<AddCoinSignal>(OnAddCoin);
        _eventBus.Unsubscribe<SpendCoinSignal>(SpendCoin);
        _eventBus.Unsubscribe<ChangedCoinSignal>(ChangedCoin);
    }
}
