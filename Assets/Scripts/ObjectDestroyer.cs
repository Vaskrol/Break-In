// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;
using System.Collections;

public class ObjectDestroyer : MonoBehaviour
{
	public float LifeTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		LifeTime -= Time.deltaTime;
		if (LifeTime <= 0)
			Destroy(gameObject);
	}
}
