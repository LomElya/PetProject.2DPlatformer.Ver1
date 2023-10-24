using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Dialogs
{
    public class ConfirmWindow : DialogCore
    {
        [SerializeField] private Text _messageText;
        [SerializeField] private Button _OKButton;
        [SerializeField] private Button _cancelButton;

        protected override void Awake()
        {
            base.Awake();

            _OKButton.onClick.AddListener(() => { Hide(); });
            _cancelButton.onClick.AddListener(() => { Hide(); });
        }
        public void Init(string textValue, UnityAction onOKButtonClicked)
        {
            _messageText.text = textValue;
            _OKButton.onClick.AddListener(onOKButtonClicked);
        }
    }
}

