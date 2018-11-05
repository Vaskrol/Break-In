﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// IRotationBehaviour.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016

using UnityEngine;

public interface IRotationBehaviour
{
	/// <summary>
	/// Rotate gameobject with spicific logic
	/// </summary>
	/// <param name="gameObject"></param>
	void PerformRotation(GameObject gameObject);
}