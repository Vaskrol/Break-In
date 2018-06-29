// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCarPhysics : IHandlingBehaviour
{
	public float CurrentSpeed { get; set; }
	public float MaxSpeed { get; set; }
	public Vector3 CurrentVelocity { get; set; }
	public HandlingCondition CurrentCondition { get; set; }
	public GameObject HandlingObject { get; set; }

	private readonly Text _speedText;
	private float _minSpeed = 3.0F;
	private float _userAccelerating;
	private float _userBraking;
	private float _userSteering;
	private float _acceleration;
	private readonly IVehicle _currentVehicle;
	
	public PlayerCarPhysics(GameObject player)
	{
		HandlingObject = player;

		_currentVehicle = HandlingObject.GetComponent<IVehicle>();
		SetSpecifications();

		CurrentSpeed = 0;
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
			//eqSlot.Equipment.Install(_currentVehicle);
		}

		SetSpecifications();
	}

	public void Restart()
	{
		HandlingObject.transform.position = Vector3.zero;
		CurrentSpeed = 0;
	}

	private void SetSpecifications()
	{
		_userAccelerating = _currentVehicle.UserAccelerating;
		_userBraking = _currentVehicle.Braking;
		_userSteering = _currentVehicle.Steering;
		_acceleration = _currentVehicle.Acceleration;
		MaxSpeed = _currentVehicle.MaxSpeed;
	}

	private void UpdateHandling()
	{
		// TURNING
		if (CurrentCondition == HandlingCondition.OnGround)
		{
			float turning = Input.GetAxis("Vertical")
			                * _userSteering;
			turning *= Time.deltaTime;

			float currentRotation = HandlingObject.transform.rotation.eulerAngles.z;
			float newRotation;

			if (Mathf.Abs(turning) > 0.01f)
			{
				// Turning key pressed 
				if (Math.Round(currentRotation) <= 30.0f
					|| Math.Round(currentRotation) >= 330.0f)
					HandlingObject.transform.Rotate(0, 0, turning);

				newRotation = HandlingObject.transform.rotation.eulerAngles.z;

				if (newRotation > 30.0f
					&& newRotation <= 30.0f + _userSteering)
					HandlingObject.transform.rotation = Quaternion.Euler(0, 0, 30.0f);

				if (newRotation < 330.0f
					&& newRotation >= 330.0f - _userSteering)
					HandlingObject.transform.rotation = Quaternion.Euler(0, 0, 330.0f);
			}
			else
			{
				// No turn key pressed
				if (currentRotation != 0.0f) SelfAligning();

				newRotation = HandlingObject.transform.rotation.eulerAngles.z;

				// If turn side changed
				if (Mathf.Abs(currentRotation - newRotation) > _userSteering)
					HandlingObject.transform.rotation = Quaternion.identity;
			}
		}
		
		// ACCELERATING AND BRAKING --
		var horAxis = Input.GetAxis("Horizontal");
		float throttle = horAxis >= 0
							 ? horAxis * _userAccelerating
							 : horAxis * _userBraking;

		throttle = CurrentSpeed > 0.8f * MaxSpeed && throttle == 0f
			? -_acceleration : throttle;

		if (CurrentSpeed < 0.8f * MaxSpeed)
			CurrentSpeed += _acceleration;
		if (CurrentSpeed <= MaxSpeed && CurrentSpeed > _minSpeed)
			CurrentSpeed += throttle;
		CurrentSpeed = Mathf.Clamp(CurrentSpeed, _minSpeed, MaxSpeed);

		var moving = CurrentSpeed * Time.deltaTime;

		HandlingObject.transform.parent.Translate(
			HandlingObject.transform.right * moving);
	}

	// TODO: Extract to separate class
	private void UiUpdate()
	{
		var showingSpeed = Mathf.Round(CurrentSpeed * 6);
		_speedText.text = showingSpeed.ToString(CultureInfo.InvariantCulture);
	}

	private void SelfAligning()
	{	
		var currentRotation = HandlingObject.transform.rotation.eulerAngles.z;
		if (currentRotation < 90)
		{
			HandlingObject.transform.Rotate(0, 0, -_userSteering * Time.deltaTime);
		}
		else if (currentRotation > 270)
		{
			HandlingObject.transform.Rotate(0, 0, _userSteering * Time.deltaTime);
		}
	}
}
