// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Globalization;
using System.Linq;

using Assets.Scripts.Vehicles.Machines;

using Tools;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCarHandling2 : IHandlingBehaviour
{
	public Vector3 CurrentVelocity { get; set; }
	public HandlingCondition CurrentCondition { get; set; }
	public GameObject HandlingObject { get; set; }
	
	private readonly Text _speedText;
	private readonly VehicleBase _currentVehicle;
	private Vector3 _prevPosition;

	public PlayerCarHandling2(GameObject player, VehicleBase currentVehicle)
	{
		HandlingObject = player;
		_currentVehicle = currentVehicle;
		_prevPosition = HandlingObject.transform.position;
		SetSpecifications();

		var uiCanvas = GameObject.Find("UiCanvas").GetComponent<Canvas>();
		_speedText = uiCanvas.transform.Find("SpeedLabel").GetComponent<Text>();
		CurrentCondition = HandlingCondition.OnGround;
	}

	public void Update()
	{
		UpdateHandling();
		UiUpdate();
	}

	public void InstallEquipment()
	{
		foreach (var eqSlot in _currentVehicle.Slots.OfType<EquipmentSlot>())
		{
			eqSlot.Equipment.Install(_currentVehicle);
		}

		SetSpecifications();
	}

	public void Restart()
	{
		HandlingObject.transform.position = Vector3.zero;
	}

	private void SetSpecifications()
	{
		HandlingObject.GetComponent<Rigidbody2D>().mass 
			= _currentVehicle.Mass;
	}

	private void UpdateHandling()
	{
		// ACCELERATING
		var movingDelta = HandlingObject.transform.position
						  - _prevPosition;
		CurrentVelocity = movingDelta / Time.deltaTime;
		var currentSpeed = Mathf.Clamp(CurrentVelocity.y, 0, _currentVehicle.MaxSpeed);
		var maxSpeed = _currentVehicle.MaxSpeed;
		currentSpeed += 1 - currentSpeed / maxSpeed;
		_prevPosition = HandlingObject.transform.position;
		HandlingObject.transform.Translate(Vector2.up * currentSpeed * Time.deltaTime);
		
		// STEERING
		var userHandlingPos = Camera.main.ScreenToWorldPoint(InputTool.InputPosition);
		HandlingObject.transform.position =
			Vector2.MoveTowards(
				HandlingObject.transform.position,
				new Vector2(
					userHandlingPos.x,
					HandlingObject.transform.position.y),
				_currentVehicle.Steering * Time.deltaTime);

		// ALIGNING
		var playerRotation =
			HandlingObject.transform.rotation.eulerAngles.z;
		if (playerRotation > 0.5f)
		{
			var rotateAngle = _currentVehicle.Steering * 10
			                  * Time.deltaTime;

			if (playerRotation < 180) rotateAngle *= -1;

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
	}

	// TODO: Extract to separate class
	private void UiUpdate()
	{
		var showingSpeed = Mathf.Round(CurrentVelocity.y * 10);
		_speedText.text = showingSpeed.ToString(CultureInfo.InvariantCulture);
	}
}
