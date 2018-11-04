// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using Assets.Scripts;
using UnityEngine;
using Vehicles.Machines;

public class StupidEnemyCarHandling : IHandlingBehaviour
{
	public Vector3 CurrentVelocity { get; set; }
	public float MaxSpeed { get; set; }
	public GameObject HandlingObject { get; set; }
	public HandlingCondition CurrentCondition { get; set; }

	private readonly VehicleBase _currentVehicle;
	private readonly IPathFinder _pathFinder;

	private float _hAxis;
	private Vector3 _prevPosition;

	public StupidEnemyCarHandling(GameObject enemy, VehicleBase currentVehicle)
	{
		HandlingObject = enemy;
		_currentVehicle = currentVehicle;
		_pathFinder = new StupidPathFinder();
		_prevPosition = HandlingObject.transform.position;
		SetSpecifications();

		UpdateHandling();
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
	}

	private void UpdateHandling()
	{
		// ACCELERATING
		var movingDelta = HandlingObject.transform.position
		                  - _prevPosition;
		CurrentVelocity = movingDelta / Time.deltaTime;
		var currentSpeed = CurrentVelocity.y;
		var maxSpeed = _currentVehicle.Performance.MaxSpeed;
		currentSpeed += 1 - CurrentVelocity.y / maxSpeed;
		_prevPosition = HandlingObject.transform.position;

		HandlingObject.transform.Translate(Vector2.up * currentSpeed * Time.deltaTime);

		// STEERING
		HandlingObject.transform.position =
			Vector2.MoveTowards(
				HandlingObject.transform.position,
				HandlingObject.transform.position + Vector3.right * _hAxis,
				_currentVehicle.Performance.Steering * Time.deltaTime);

		// ALIGNING
		var playerRotation =
			HandlingObject.transform.rotation.eulerAngles.z;
		if (playerRotation > 0.5f)
		{
			var rotateAngle = _currentVehicle.Performance.Steering * 10
							  * Time.deltaTime;

			if (playerRotation < 180)
				rotateAngle *= -1;

			HandlingObject.transform.Rotate(new Vector3(0, 0, rotateAngle));

			if (Math.Abs(playerRotation - HandlingObject.transform.rotation.eulerAngles.z) > 180)
			{
				HandlingObject.transform.rotation = Quaternion.identity;
			}
		}
		else
		{
			HandlingObject.transform.rotation = Quaternion.identity;
		}

		//Debug.Log(
		//	"Spd: " + CurrentVelocity.magnitude + 
		//	"; Vel: " + CurrentVelocity + 
		//	"; Str: " + _hAxis);
	}

	private void SetSpecifications()
	{
		MaxSpeed = _currentVehicle.Performance.MaxSpeed;
	}
}
