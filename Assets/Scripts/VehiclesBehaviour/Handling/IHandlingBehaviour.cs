// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using UnityEngine;

public interface IHandlingBehaviour
{
	GameObject HandlingObject { get; set; }

	Vector3 CurrentVelocity { get; }

	HandlingCondition CurrentCondition { get; set; }

	void Update();

	void InstallEquipment();
}
