// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using UnityEngine;
using Vehicles.Handling.Destroyers;

public class Alpha : MonoBehaviour, IVehicle
{
	public float Acceleration { get; set; }

	public float Braking { get; set; }

	public IHandlingBehaviour Handling { get; set; }

	public IJumpingBehaviour Jumping { get; set; }

	public IFiringBehaviour Firing { get; set; }

	public float MaxSpeed { get; set; }

	public float Mass { get; set; }

	public ISlot[] Slots { get; set; }

	public float Steering { get; set; }

	public float UserAccelerating { get; set; }
	
	public GameState CurrentGameState { private get; set; }

	public IVehicleDestroyer Destroyer { get; set; }

	public void Start()
	{
		UserAccelerating = 0.3f;
		Braking = 1.5f;
		Steering = 30.0f;
		Acceleration = 5.0f;
		MaxSpeed = 6f;
		Mass = 900f;

		CurrentGameState = GameState.Playing;

		Slots = new ISlot[]
		{
			new WeaponSlot { Position = new Vector2(0, -0.4f) }
		};

		var player = GameObject.Find("Player");

		//Handling = new PlayerCarHandling2(player, this);
		//Firing = new PlayerFiring(player);
		//Jumping = new CarJumping(gameObject);
	}

	public void Update()
	{
		if (CurrentGameState == GameState.Playing)
		{
			Handling.Update();
			Firing.Update();
			//Jumping.Update();

			if (Destroyer.DestroyNeeded())
			{
				BlowUpVehicle();
			}
		}
	}

	private void BlowUpVehicle()
	{
		Instantiate(
			Resources.Load(
				"Prefabs/ParticleSystems/CarExplosionEffect",
				typeof(GameObject)),
			transform.position,
			Quaternion.identity);

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = new Color(0.25f, 0.25f, 0.25f);

		OnDestroy();
	}

	private void OnDestroy()
	{
		CurrentGameState = GameState.GameOver;
	}
}