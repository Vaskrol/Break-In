// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;

public class GameOver : MonoBehaviour
{
	private GameState _currentGameState;

	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;
	
	// Use this for initialization
	void Start ()
	{
		_currentGameState = GameState.Playing;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Building"))
		{
			if (Mathf.Abs(Mathf.Abs(collision.contacts[0].normal.y) - 1) > 0.1) 
			{
				var objects = FindObjectsOfType(typeof(GameObject));
				foreach (GameObject go in objects)
				{
					go.SendMessage(
						"OnGameOver",
						SendMessageOptions.DontRequireReceiver);
				}

				_currentGameState = GameState.GameOver;
			}
		}
	}

	private void OnGUI()
	{
		if (_currentGameState == GameState.GameOver)
			DrawGameoverWindow(
				new Rect(
					Screen.width / 7f,
					Screen.height / 5f ,
					Screen.width / 7f * 5f,
					Screen.height / 5f * 3f), 
				new Color(0, 0, 0, 0.5f));
	}

	// Note that this function is only meant to be called from OnGUI() functions.
	public static void DrawGameoverWindow(Rect position, Color color)
	{
		if (_staticRectTexture == null)
			_staticRectTexture = new Texture2D(1, 1);

		if (_staticRectStyle == null)
			_staticRectStyle = new GUIStyle();

		_staticRectTexture.SetPixel(0, 0, color);
		_staticRectTexture.Apply();

		_staticRectStyle.normal.background = _staticRectTexture;

		GUI.Box(position, GUIContent.none, _staticRectStyle);
	}
}
