// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// VehicleBase.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace Assets.Scripts.Vehicles.Machines
{
    using System.Linq;
    using Firing;
	using Interfaces;
	using UnityEngine;

	public abstract class VehicleBase
	{
		public float Acceleration { get; set; }

		public float Braking { get; set; }

		public float MaxSpeed { get; set; }

		public float Mass { get; set; }

		public float Steering { get; set; }

		public float UserAccelerating { get; set; }

		public float HealthPoints { get; set; }

		public float MaxHealthPoints { get; set; }

		public Vector2 CenterOfMass { get; set; }

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
			Firing = new NoFiring();
		}

		public void Update()
		{
			Handling.Update();
			Firing.Update();

			if (Destroyer.DestroyNeeded())
			{
				BlowUpVehicle();
			}
		}

		public void RecieveDamage(float damage, DamageType damageType)
		{
			HealthPoints -= damage;

			if (HealthPoints <= 0)
				BlowUpVehicle();
		}

		public void BlowUpVehicle()
		{
			var explosion = GameObject.Instantiate(
				Resources.Load(
					"Prefabs/ParticleSystems/CarExplosionEffect",
					typeof(GameObject)),
				_player.transform.position + Vector3.back * 10,
				Quaternion.identity) as GameObject;
            explosion.name = "Vehicle Explosion";
            explosion.AddComponent<ObjectDestroyer>();

            var spriteRenderer = _player.GetComponentInChildren<SpriteRenderer>();
			spriteRenderer.color = new Color(0.25f, 0.25f, 0.25f);

			//var dust = _player.transform.Find("CarDustTrack").gameObject;
			//if (dust != null)
   //             Destroy(dust);

			OnDestroy();
		}

		private void OnDestroy()
		{
			CurrentGameState = GameState.GameOver;
		}

        // Resources.Load("Prefabs/Weapons/PistolA", typeof(GameObject))
        public void AddWeapon(IWeapon weapon) {
            WeaponSlot slot = Slots.OfType<WeaponSlot>().Where(s => s.Weapon == null).First();
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

        public void AddWeapon(IWeapon weapon, WeaponSlot slot) {
            var weaponGameObject = GameObject.Instantiate(weapon.GameObject);
            slot.Weapon = weaponGameObject.GetComponent<IWeapon>();
            slot.GameObject = weaponGameObject;
            weaponGameObject.transform.parent = _player.transform;
            weaponGameObject.transform.localPosition = slot.Position;
        }
    }
}