// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using Assets.Scripts.Vehicles.Handling;
using Assets.Scripts.Vehicles.Handling.PlayerHandling;
using Assets.Scripts.Vehicles.Machines;
using UnityEngine;
using System.Linq;

public class Alpha2 : VehicleBase
{
	public Alpha2(GameObject player) : base(player)
	{
		UserAccelerating = 0.3f;
		Braking          = 1.5f;
		Steering         = 10.0f;
		Acceleration     = 8.0f;
		MaxSpeed         = 6f;
		Mass             = 900f;
		Slots = new ISlot[]
		{
			new WeaponSlot { Position = new Vector2(0, -0.4f) }
		};
        		
		Handling = new RigidBodyCarHandling(player, this);
		Firing = new PlayerFiring(player.transform, this);
		Destroyer = new OverloadVehicleDestroyer(player, this, 7.0f);

        AddWeapon(ModulesController.Instance().Weapons.First(w => w is PistolA));
	}
}