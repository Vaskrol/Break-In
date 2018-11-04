// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using UnityEngine;

public interface IFiringBehaviour
{
	GameObject GameObject { get; set; }

	void Update();
}
