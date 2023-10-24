using UnityEngine;
using UI.Dialogs;

namespace UI
{
    /// <summary>
    /// Компонент, который мы навесим как рут для всех UI объектов
    /// </summary>
    public class GUIHolder : MonoBehaviour, IService
    {
        [SerializeField] private HUDPanel _HUDPanel;
        public void Init()
        {
            _HUDPanel.gameObject.SetActive(true);
        }
    }
}