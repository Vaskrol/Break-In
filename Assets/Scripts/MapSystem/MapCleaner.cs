// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCleaner : MonoBehaviour
{
	private GameObject _player;
	private GameObject _enemies;

	// Use this for initialization
	void Start () {
		_enemies = GameObject.Find("Enemies");
		_player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

		// Removing hidden enemies, etc
		foreach (Transform child in _enemies.transform)
		{
			if (_player.transform.position.y - child.position.y > 10)
			{
				var destroyer = child.gameObject.AddComponent<ObjectDestroyer>();
				destroyer.LifeTime = 0;
			}
		}
	}
}
