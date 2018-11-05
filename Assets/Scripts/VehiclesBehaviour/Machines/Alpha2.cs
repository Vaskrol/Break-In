// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Linq;
using Assets.Scripts.Vehicles.Handling;
using UnityEngine;
using Vehicles.Machines;
using VehiclesBehaviour.Handling.PlayerHandling;

namespace VehiclesBehaviour.Machines {
	public class Alpha2 : VehicleBase
	{
		public Alpha2(GameObject player) : base(player)
		{
			Performance.HealthPoints     = 200f;
			Performance.MaxHealthPoints  = 200f;
			Performance.UserAccelerating = 0.3f;
			Performance.Braking          = 1.5f;
			Performance.Steering         = 0.1f;
			Performance.Acceleration     = 80.0f;
			Performance.MaxSpeed         = 6f;
			Performance.Mass             = 9;
			Performance.CenterOfMass     = new Vector3(0, 0.5f);
			Slots = new ISlot[]
			{
				new WeaponSlot { Position = new Vector2(0, -0.4f) }
			};
        		
			//Handling = new FullRigidBodyCarPhysics(player, this);
			Handling  = new ArcadeCarPhysics(player, this);
			Firing    = new PlayerFiring(player.transform, this);
			Destroyer = new OverloadVehicleDestroyer(player, this, 7.0f);        

			AddWeapon(ModulesController.Instance.Weapons.First(w => w is PistolA));
		}
	}
}