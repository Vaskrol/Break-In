﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// IPathFinder.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace Assets.Scripts
{
	using UnityEngine;

	public interface IPathFinder
	{
		float FindWay(GameObject gameObject);
	}
}