﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;
using System.Linq;

public class Firing : MonoBehaviour
{
	private IVehicle _currentVehicle;

	// Use this for initialization
	void Start ()
	{
		_currentVehicle = GetComponent<IVehicle>();
		foreach (var wSlot in _currentVehicle.Slots.OfType<WeaponSlot>())
		{
			// TODO: Connect weapon change system here
			var weaponGameObject = Instantiate(
				Resources.Load("Prefabs/Weapons/PistolA", typeof(GameObject))) 
				as GameObject;

			wSlot.Weapon = weaponGameObject.GetComponent<IWeapon>();
			wSlot.GameObject = weaponGameObject;
			weaponGameObject.transform.parent = transform;
			weaponGameObject.transform.localPosition = wSlot.Position;
		}
	}
	
	void Update()
	{
		if (Input.touchCount > 0)
		{
			var touch = Input.touches.FirstOrDefault();

			foreach (var wSlot in _currentVehicle.Slots.OfType<WeaponSlot>())
			{
				wSlot.Weapon.Fire(touch.position);
			}
		}

		foreach (var wSlot in _currentVehicle.Slots.OfType<WeaponSlot>())
		{
			wSlot.Weapon.UpdateRotation();
		}
	}
}
