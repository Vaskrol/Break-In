// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Vehicles.Handling;
using UnityEngine;

namespace Vehicles.Machines {
	
	public class EnemyAlphard : VehicleBase, IDestroyable
	{
		public EnemyAlphard(GameObject player) : base(player)
		{
			Performance.HealthPoints     = 50f;
			Performance.MaxHealthPoints  = 50f;
			Performance.UserAccelerating = 0.1f;
			Performance.Braking          = 0.2f;
			Performance.Steering         = 3.0f;
			Performance.Acceleration     = 4.0f;
			Performance.MaxSpeed         = 6f;
			Performance.Mass             = 1000f;
			Slots = new ISlot[]
			{
				new WeaponSlot { Position = new Vector2(0, -0.5f) }
			};

			Handling  = new StupidEnemyCarHandling(player, this);
			Firing    = new EnemyToPlayerFiring(player.transform, this);
			Destroyer = new SlowVehicleDestroyer(player, this, 0.1f);

			AddWeapon(ModulesController.Instance.Weapons.First(w => w is E_PistolA));
		}
	}
}
