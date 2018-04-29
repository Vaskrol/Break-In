// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// VehicleBase.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace Assets.Scripts.Vehicles.Machines
{
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

		public void RecieveDamage(float damage, string damageType)
		{
			HealthPoints -= damage;

			if (HealthPoints <= 0)
				BlowUpVehicle();
		}

		public void BlowUpVehicle()
		{
			GameObject.Instantiate(
				Resources.Load(
					"Prefabs/ParticleSystems/CarExplosionEffect",
					typeof(GameObject)),
				_player.transform.position + Vector3.back * 10,
				Quaternion.identity);

			var spriteRenderer = _player.GetComponentInChildren<SpriteRenderer>();
			spriteRenderer.color = new Color(0.25f, 0.25f, 0.25f);

			var dust = _player.transform.Find("CarDustTrack").gameObject;
			if (dust != null) GameObject.Destroy(dust);

			OnDestroy();
		}

		private void OnDestroy()
		{
			CurrentGameState = GameState.GameOver;
		}
	}
}