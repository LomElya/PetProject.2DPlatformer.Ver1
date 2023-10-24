using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEventBus;
using CustomEventBus.Signals;

namespace UI
{
    /// <summary>
    /// Количество монет(всего)
    /// </summary>
    public class CoinHUD : MonoBehaviour
    {   
        [SerializeField] private int _coin;
        [SerializeField] private Text _coinText;
        private int _currentCoin;
        private EventBus _eventBus;

        public void Awake()
        {
            _currentCoin = PlayerPrefs.GetInt(StringConstants.COIN);
            _coinText.text = _currentCoin.ToString();
        }
        public void Start()
        {   
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<ChangedCoinSignal>(ChangedCoin);
        }

        public void ChangedCoin(ChangedCoinSignal signal)
        {
            _currentCoin = PlayerPrefs.GetInt(StringConstants.COIN);
            _coinText.text = _currentCoin.ToString();
        }
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<ChangedCoinSignal>(ChangedCoin);
        }
    }
}

