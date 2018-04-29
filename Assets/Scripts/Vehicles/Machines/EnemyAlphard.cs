// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using Assets.Scripts;
using Assets.Scripts.Vehicles.Firing;
using Assets.Scripts.Vehicles.Handling;
using Assets.Scripts.Vehicles.Machines;
using UnityEngine;

public class EnemyAlphard : VehicleBase, IDestroyable
{
	public EnemyAlphard(GameObject player) : base(player)
	{
		HealthPoints     = 100.0f;
		UserAccelerating = 0.1f;
		Braking          = 0.2f;
		Steering         = 3.0f;
		Acceleration     = 4.0f;
		MaxSpeed         = 6f;
		Mass             = 1000f;
		Slots = new ISlot[]
		{
			new WeaponSlot { Position = new Vector2(0, -0.5f) }
		};

		Handling = new StupidEnemyCarHandling(player, this);
		Destroyer = new SlowVehicleDestroyer(player, this, 0.1f);
	}
}
