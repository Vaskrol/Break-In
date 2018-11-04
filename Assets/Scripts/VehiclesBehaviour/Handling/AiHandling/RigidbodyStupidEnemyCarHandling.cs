// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;

using Assets.Scripts;

using UnityEngine;

public class RigidbodyStupidEnemyCarHandling : IHandlingBehaviour
{
	public GameObject HandlingObject { get; set; }

	public Vector3 CurrentVelocity { get; set; }

	public HandlingCondition CurrentCondition { get; set; }
	private float _hAxis = 0, _vAxis = 0;
	private Vector2 _collisionPoint;
	private float _collisionDistance;
	private readonly IVehicle _currentVehicle;
	private readonly Rigidbody2D _vehicleRb;
	private PathFinder _pathFinder = new PathFinder();

	public RigidbodyStupidEnemyCarHandling(GameObject enemy)
	{
		HandlingObject = enemy;

		_currentVehicle = HandlingObject.GetComponent<IVehicle>();
		_vehicleRb = HandlingObject.GetComponent<Rigidbody2D>();
		SetSpecifications();
		CurrentCondition = HandlingCondition.OnGround;
	}

	public void Update()
	{
		UpdateLogic();
		UpdateHandling();
	}

	public void InstallEquipment()
	{
		// No equipment installed
	}

	private void UpdateLogic()
	{
		_hAxis = _pathFinder.FindWay(HandlingObject);
		_vAxis = 1;
	}

	private void UpdateHandling()
	{
		// TURNING
		if (CurrentCondition == HandlingCondition.OnGround)
		{
			float steeringPower = _currentVehicle.Steering;
			float treshhold = 0.1f;
			if (Math.Abs(_hAxis - HandlingObject.transform.position.x) < treshhold)
			{
				steeringPower *= Math.Abs(_hAxis - HandlingObject.transform.position.x) / treshhold;
			}

			_vehicleRb.AddForce(Vector2.right * _hAxis * steeringPower);

		}

		// ACCELERATING AND BRAKING --
		var maxSpeed = _currentVehicle.MaxSpeed;
		var acceleration = _currentVehicle.Acceleration;
		acceleration *= 1 - _vehicleRb.velocity.y / maxSpeed;
		_vehicleRb.AddForce(Vector2.up * acceleration);
	}

	private void SetSpecifications()
	{
		HandlingObject.GetComponent<Rigidbody2D>().mass
			= _currentVehicle.Mass;
	}
}
