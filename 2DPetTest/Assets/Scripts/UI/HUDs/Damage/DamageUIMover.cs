using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using UnityEngine;
using UnityEngine.UI;
using CustomEventBus.Signals;

public class DamageUIMover : MonoBehaviour, IService
{
    [SerializeField] private float _maxTime;
    private readonly List<ActiveText> _textList = new List<ActiveText>();
    private Camera _camera;
    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<DamageActivatedSignal>(AddTextList);
    }

    private void AddTextList(DamageActivatedSignal signal)
    {
        var text = signal.Text;
        var unitPos = signal.UnitPosition;
        _camera = signal.Camera;

        ActiveText activeText = new ActiveText() { maxTime = _maxTime };
        activeText.UIText = text;
        activeText.Timer = activeText.maxTime;
        activeText.unitPosition = unitPos + new Vector2(0.1f, 0.1f);

        activeText.MoveText(_camera);

        if (!_textList.Contains(activeText))
        {
            _textList.Add(activeText);
        }
    }
    private void Update()
    {
        if (_textList.Count == 0)
            return;

        for (int i = 0; i < _textList.Count; i++)
        {
            ActiveText activeText = _textList[i];
            activeText.Timer -= Time.deltaTime;

            if (activeText.Timer < 0f)
            {
                _eventBus.Invoke(new DamageRemoveSignal(activeText.UIText));
                if (_textList.Contains(_textList[i]))
                {
                    _textList.Remove(_textList[i]);
                }
            }
            else
            {
                var color = activeText.UIText.color;
                color.a = activeText.Timer / activeText.maxTime;
                activeText.UIText.color = color;

                activeText.MoveText(_camera);
            }
        }
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<DamageActivatedSignal>(AddTextList);
    }
    public class ActiveText
    {
        public Text UIText;
        public float maxTime;
        public float Timer;
        public Vector2 unitPosition;
        public void MoveText(Camera camera)
        {
            float delta = 1f - (Timer / maxTime);
            Vector2 pos = unitPosition + new Vector2(delta / 10, delta / 10);
            pos = camera.WorldToScreenPoint(pos);

            UIText.transform.position = pos;
        }
    }
}
