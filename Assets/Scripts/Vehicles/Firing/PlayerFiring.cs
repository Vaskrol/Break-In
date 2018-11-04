// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;
using System.Linq;
using Vehicles.Machines;

public class PlayerFiring : IFiringBehaviour {

    public GameObject GameObject { get; set; }

    protected readonly VehicleBase _currentVehicle;

    protected readonly Transform _parentTransform;

    public PlayerFiring(Transform parentTransform, VehicleBase currentVehicle) {
        _currentVehicle = currentVehicle;
        _parentTransform = parentTransform;
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
