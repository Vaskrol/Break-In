// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// IWeapon.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016

using UnityEngine;

public interface IWeapon
{
	/// <summary>
	/// Cooldown time between shots
	/// </summary>
	float Cooldown { get; set; }

	/// <summary>
	/// Damage dealt by one shot
	/// </summary>
	float Damage { get; set; }

	/// <summary>
	/// Perform shot
	/// </summary>
	/// <param name="aim"></param>
	void Fire(Vector2 aim);

	/// <summary>
	/// Update weapon rotation
	/// </summary>
	void UpdateRotation();

	/// <summary>
	/// Behaviour of weapon rotation
	/// </summary>
	IRotationBehaviour RotationBehaviour { get; set; }
}
