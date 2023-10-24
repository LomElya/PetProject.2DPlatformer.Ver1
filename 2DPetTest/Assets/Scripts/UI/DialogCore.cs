using UnityEngine;
using UnityEngine.UI;
using CustomEventBus;
using UnityEngine.EventSystems;

namespace UI
{
    public abstract class DialogCore : MonoBehaviour, IService
    {
        [SerializeField] private Button _outsideClickArea;
        protected Transform _draggingParent;
        private EventBus _eventBus;

        protected virtual void Awake()
        {
            //_eventBus = ServiceLocator.Current.Get<EventBus>();

            _draggingParent = ServiceLocator.Current.Get<GUIHolder>().transform;
            if (_outsideClickArea != null)
            {
                _outsideClickArea.onClick.AddListener(Hide);
            }
        }

        public void Hide()
        {
            Destroy(gameObject);
        }

        protected void OnDestroy()
        {
            if (_outsideClickArea != null)
            {
                _outsideClickArea.onClick.RemoveAllListeners();
            }
        }
    }
}