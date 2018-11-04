// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// VehicleBase.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017

using System.Linq;
using UnityEngine;
using Vehicles.Firing;
using Vehicles.Handling.Destroyers;

namespace Vehicles.Machines
{
	public abstract class VehicleBase
	{
		public VehicleSpecifications Performance { get; set; }

		public IHandlingBehaviour Handling { get; set; }

		public IJumpingBehaviour Jumping { get; set; }

		public IFiringBehaviour Firing { get; set; }

		public IVehicleDestroyer Destroyer { get; set; }

		public GameState CurrentGameState { get; set; }
	
		public ISlot[] Slots { get; set; }

		private readonly GameObject _player;

		protected VehicleBase(GameObject player)
		{
			CurrentGameState = GameState.Playing;
			_player = player;
			Performance = new VehicleSpecifications();
			Firing = new NoFiring();
		}

		public void Update()
		{
			Handling.Update();
			Firing.Update();

			if (Destroyer.DestroyNeeded()) {
				BlowUpVehicle();
			}
		}

		public void ReceiveDamage(float damage, DamageType damageType)
		{
			Performance.HealthPoints -= damage;

			if (Performance.HealthPoints <= 0)
				BlowUpVehicle();
		}

		private void BlowUpVehicle()
		{
			var explosion = GameObject.Instantiate(
				Resources.Load(
					"Prefabs/ParticleSystems/CarExplosionEffect",
					typeof(GameObject)),
				_player.transform.position + Vector3.back * 10,
				Quaternion.identity) as GameObject;
			
			if (explosion != null) {
				explosion.name = "Vehicle Explosion";
				explosion.AddComponent<ObjectDestroyer>();
			}

			var spriteRenderer = _player.GetComponentInChildren<SpriteRenderer>();
				spriteRenderer.color = new Color(0.25f, 0.25f, 0.25f);
			//var dust = _player.transform.Find("CarDustTrack").gameObject;
			//if (dust != null)
			//	Destroy(dust);

			OnDestroy();
		}

		private void OnDestroy() {
			CurrentGameState = GameState.GameOver;
		}

        // Resources.Load("Prefabs/Weapons/PistolA", typeof(GameObject))
		protected void AddWeapon(IWeapon weapon) {
            WeaponSlot slot = Slots.OfType<WeaponSlot>().First(s => s.Weapon == null);
            if (slot == null) {
                Debug.LogError("There is no free slot in" + GetType());
                return;
            }
            AddWeapon(weapon, slot);
        }

        public void AddWeapon(IWeapon weapon, int slotNumber) {
            WeaponSlot slot;
            if (Slots.OfType<WeaponSlot>().Count() < slotNumber)
                slot = Slots.OfType<WeaponSlot>().ToArray()[slotNumber];
            else {
                Debug.LogError("There are no slot number " + slotNumber + " in " + GetType());
                return;
            }
            AddWeapon(weapon, slot);
        }

		private void AddWeapon(IWeapon weapon, WeaponSlot slot) {
            var weaponGameObject = GameObject.Instantiate(weapon.GameObject);
            slot.Weapon = weaponGameObject.GetComponent<IWeapon>();
            slot.GameObject = weaponGameObject;
            weaponGameObject.transform.SetParent(_player.transform, false);
            weaponGameObject.transform.localPosition = slot.Position;
        }
    }
}