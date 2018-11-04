// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;
using System.Collections;

public class EnemyDestroy : MonoBehaviour {

	private GameState _currentGameState;

	// Use this for initialization
	void Start()
	{
		_currentGameState = GameState.Playing;
	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Building"))
		{
			gameObject.SendMessage(
				"OnDestroy",
				SendMessageOptions.DontRequireReceiver);

			_currentGameState = GameState.GameOver;
		}
	}
}
