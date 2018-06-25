// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

using Assets.Scripts.Vehicles.Machines;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
	public VehicleBase CurrentVehicle { get; set; }

	public string VehicleName;

    private bool _isPlayer = false;

	public void Start()
	{
		var tag = gameObject.tag;
		switch (tag)
		{
			// TODO: Make cars indiffirent to driver (AI / player)
			case "Player":
				var player = GameObject.Find("Player");
				CurrentVehicle = new Alpha2(player);
                _isPlayer = true;
                break;
			case "Enemy":
				CurrentVehicle = new EnemyAlphard(gameObject);
				break;
		}
	}

	public void Update()
	{
		if (CurrentVehicle != null 
			&& CurrentVehicle.CurrentGameState == GameState.Playing)
		{
			CurrentVehicle.Update();
		}

        if (_isPlayer)
            UIController.Instance.HealthBar.SetPercent(CurrentVehicle.HealthPoints / CurrentVehicle.MaxHealthPoints * 100f);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Obstacle")
		{
			CurrentVehicle.RecieveDamage(10f, DamageType.Impact);
        }
	}
}
