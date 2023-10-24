using UnityEngine;

[System.Serializable]
public class QuestData 
{
    [SerializeField] private int _idQuest;
    [SerializeField] private string _nameQuest;
    [SerializeField] private string _description;
    [SerializeField] private Collider2D _colliderPlayer;
    [SerializeField] private Collider2D _colliderObjectQuest;
     [SerializeField] private bool _questComplete;

    public int IDQuest => _idQuest;
    public string NameQuest => _nameQuest;
    public string Description => _description;
    public Collider2D ColliderPlayer => _colliderPlayer;
    public Collider2D ColliderObjectQuest => _colliderObjectQuest;
     public bool QuestComplete => _questComplete;

    public QuestData(int idQuest, string nameQuest, string description, Collider2D colliderPlayer, Collider2D colliderObjectQuest, bool questComplete)
    {
        _idQuest = idQuest;
        _nameQuest = nameQuest;
        _description = description;
        _colliderPlayer = colliderPlayer;
        _colliderObjectQuest = colliderObjectQuest;
        _questComplete = questComplete;
    }
}
