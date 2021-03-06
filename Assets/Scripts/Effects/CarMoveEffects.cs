﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;
using System.Collections;

public class CarMoveEffects : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnGameOver()
	{
		var playerGameObject = GameObject.Find("Player");
		var particleSystems =
			playerGameObject.GetComponentsInChildren<ParticleSystem>();

		// Stop dust from wheels
		foreach (var system in particleSystems)
		{
			system.Stop();
		}
	}
}
