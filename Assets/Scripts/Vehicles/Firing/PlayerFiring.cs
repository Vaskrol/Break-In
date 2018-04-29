// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;
using System.Linq;

using Assets.Scripts.Vehicles.Machines;

public class PlayerFiring : IFiringBehaviour
{
	public GameObject GameObject { get; set; }

	private readonly VehicleBase _currentVehicle;

	// Use this for initialization
	public PlayerFiring(GameObject player, VehicleBase currentVehicle)
	{
		_currentVehicle = currentVehicle;

		foreach (var wSlot in _currentVehicle.Slots.OfType<WeaponSlot>())
		{
			var weaponGameObject = GameObject.Instantiate(
				Resources.Load("Prefabs/Weapons/PistolA", typeof(GameObject))) 
				as GameObject;

			wSlot.Weapon = weaponGameObject.GetComponent<IWeapon>();
			wSlot.GameObject = weaponGameObject;
			weaponGameObject.transform.parent = player.transform;
			weaponGameObject.transform.localPosition = wSlot.Position;
		}
	}
	
	// Update is called once per frame
	public void Update()
	{
		foreach (var wSlot in _currentVehicle.Slots.OfType<WeaponSlot>())
		{
			wSlot.Weapon.Fire(Vector2.up * 3);
		}

		foreach (var wSlot in _currentVehicle.Slots.OfType<WeaponSlot>())
		{
			wSlot.Weapon.UpdateRotation();
		}
	}
}
