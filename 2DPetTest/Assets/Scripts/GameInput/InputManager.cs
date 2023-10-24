using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Text _buttonText; // текст кнопки
	[SerializeField] private string _defaultKeyName; // ключ/имя вызова
	[SerializeField] private KeyCode _defaultKeyCode; // клавиша ключа
    public KeyCode keyCode { get; set; }

	private IEnumerator coroutine;
	private string tmpKey;

	public Text buttonText
	{
		get{ return _buttonText; }
	}

	public string defaultKeyName
	{
		get{ return _defaultKeyName; }
	}
	
	public KeyCode defaultKeyCode
	{
		get{ return _defaultKeyCode; }
	}

	public void ButtonSetKey() // событие кнопки, для перехода в режим ожидания
	{
		tmpKey = _buttonText.text;
		_buttonText.text = "???";
		coroutine = Wait();
		StartCoroutine(coroutine);
	}

	// ждем, когда игрок нажмет какую-нибудь клавишу, для привязки
	// если будет нажата клавиша 'Escape', то отмена
	IEnumerator Wait()
	{
		while(true)
		{
			yield return null;

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				_buttonText.text = tmpKey;
				StopCoroutine(coroutine);
			}

			foreach(KeyCode k in KeyCode.GetValues(typeof(KeyCode)))
			{
				if(Input.GetKeyDown(k) && !Input.GetKeyDown(KeyCode.Escape))
				{
					keyCode = k;
					_buttonText.text = k.ToString();
					StopCoroutine(coroutine);
				}
			}
		}
	}
}
