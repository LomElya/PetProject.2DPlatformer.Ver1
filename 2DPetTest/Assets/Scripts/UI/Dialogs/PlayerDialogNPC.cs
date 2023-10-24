using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Platformer.Dialogue;
using System.Collections;

namespace UI.Dialogs
{
     /// <summary>
    /// Диалоговое окно с NPC, когда игрок нажимает на кнопку взаимоействия
    /// </summary>
    public class PlayerDialogNPC : DialogCore
    {
        [SerializeField] private Button _nextDialogButton;
        [SerializeField] private Button _cancelDialogButton;

        [SerializeField] private Text _nameNPC;
        [SerializeField] private Text _buttonNextText;
        [SerializeField] private Text _dialogText;

        [SerializeField] private Dialog _dialog;
        [SerializeField] private TextAsset _TXTAsset;
         [SerializeField] private List<Node> _nodes;

        [SerializeField] private int _currentNode;
        


        private EventBus _eventBus;
        
        void Start()
        {   
            //Когда-гибудь удалить
            PlayerPrefs.SetInt(StringConstants.CURRENT_QUEST, (0));
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _nextDialogButton.onClick.AddListener(NextDialog);
            _cancelDialogButton.onClick.AddListener(EndDialog);
        }

        public void Init(TextAsset TXTAsset)
        {
            _TXTAsset = TXTAsset;
            _dialog = Dialog.Load(_TXTAsset);
            FillNode();
            reNameBox();
            
            StopAllCoroutines();
            StartCoroutine(TypeDialogueText(_nodes[_currentNode].npcText)); 
        }

        public void NextDialog()
        {
            if(_currentNode >= _nodes.Count ||
            _nodes[_currentNode].answers[0].end != "true")
            {
                _currentNode++;
                if (_nodes[_currentNode].answers[0].toLevel != null)
                {
                    //Загрузка уровня
                }
                reNameBox();
            }
            else
            {
               /*  PlayerPrefs.SetInt(StringConstants.CURRENT_QUEST, (0));
                PlayerPrefs.SetInt("Quest1", 2); */
                _eventBus.Invoke(new NextQuestSignal());
                EndDialog();
                return;
            }
            StopAllCoroutines();
            StartCoroutine(TypeDialogueText(_nodes[_currentNode].npcText)); 
        }

        IEnumerator TypeDialogueText(string dial)
        {
            _dialogText.text = "";
            foreach(char letter in dial.ToCharArray())
            {
                _dialogText.text += letter;
                yield return null;
            }
        }

        public void EndDialog()
        { 
            _currentNode = 0;
            _eventBus.Invoke(new PlayerEndDialogSignal());
        } 

        public void FillNode()
        {
            int dialogNodesLength = _dialog.nodes.Length; //Кол-во Node в TextAsset'е
            var questName = _dialog.nodes[_currentNode].quest; //Название квеста
            _nodes.Clear();
            for(int i = 0; i < dialogNodesLength; i++)
            {   
                
                //id Нужного Node для диалога
                int idNeedQuest = _dialog.nodes[i].quest.needQuestValue;
                if(questName == null || idNeedQuest == PlayerPrefs.GetInt(StringConstants.CURRENT_QUEST)) 
                {
                    _nodes.Add(_dialog.nodes[i]); 
                }
                
            }
        }
        public void reNameBox()
        {
            _buttonNextText.text = _nodes[_currentNode].answers[0].text;
            _nameNPC.text = _nodes[_currentNode].npcName; 
        }
    }
}

