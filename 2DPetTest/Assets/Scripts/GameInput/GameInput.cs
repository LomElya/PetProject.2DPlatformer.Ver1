using UnityEngine;
using System.Collections;
using System.IO;

public class GameInput : MonoBehaviour {

	[SerializeField] private string fileName = "input.settings"; // имя файла сохранения с разрешением
	[SerializeField] private InputManager[] _input; // массив кнопок

	private static GameInput gameInput;

	public static GameInput Key
	{
		get{ return gameInput; }
	}

	void Awake()
	{
		gameInput = this;
	}

	KeyCode FindKey(string name) // поиск кей-кода по имени
	{
		for(int i = 0; i < _input.Length; i++)
		{
			if(name == _input[i].defaultKeyName) return _input[i].keyCode;
		}

		return KeyCode.None;
	}

	int GetInt(string text)
	{
		int value;
		if(int.TryParse(text, out value)) return value;
		return 0;
	}

	string Path() // путь сохранения
	{
		return Application.dataPath + "/" + fileName;
	}

	void SetKey(string value) // настройка кнопок
	{
		string[] result = value.Split(new char[]{'='});

		for(int i = 0; i < _input.Length; i++)
		{
			if(result[0] == _input[i].defaultKeyName)
			{
				_input[i].keyCode = (KeyCode)GetInt(result[1]);
				_input[i].buttonText.text = _input[i].keyCode.ToString();
			}
		}
	}

	public void DefaultSettings() // возврат настроек по умолчанию
	{
		for(int i = 0; i < _input.Length; i++)
		{
			_input[i].keyCode = _input[i].defaultKeyCode;
			_input[i].buttonText.text = _input[i].defaultKeyCode.ToString();
		}
	}

	public void LoadSettings() // загрузка установок
	{
		if(!File.Exists(Path()))
		{
			DefaultSettings();
			return;
		}

		StreamReader reader = new StreamReader(Path());

		while(!reader.EndOfStream)
		{
			SetKey(reader.ReadLine());
		}

		reader.Close();
	}

	public void SaveSettings()
	{
		StreamWriter writer = new StreamWriter(Path());

		for(int i = 0; i < _input.Length; i++)
		{
			writer.WriteLine(_input[i].defaultKeyName + "=" + (int)_input[i].keyCode);
		}

		writer.Close();
		Debug.Log(this + " сохранения настроек привязки клавиш: " + Path());
	}

	public bool GetKey(string name)
	{
		return Input.GetKey(FindKey(name));
	}

	public bool GetKeyDown(string name)
	{
		return Input.GetKeyDown(FindKey(name));
	}

	public bool GetKeyUp(string name)
	{
		return Input.GetKeyUp(FindKey(name));
	}
}