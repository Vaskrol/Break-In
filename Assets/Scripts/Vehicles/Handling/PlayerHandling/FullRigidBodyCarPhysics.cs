// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Globalization;
using System.Linq;

using Assets.Scripts.Vehicles.Machines;

using Tools;
using UnityEngine;
using UnityEngine.UI;

// TODO: Separate this to "phisics" class
public class FullRigidBodyCarPhysics : IHandlingBehaviour
{    
    public HandlingCondition CurrentCondition { get; set; }
    public GameObject HandlingObject { get; set; }

    public Vector3 CurrentVelocity {
        get { return _vehicleRb == null ? Vector2.zero : _vehicleRb.velocity; }
    }
    private readonly Text _speedText;
    private readonly VehicleBase _currentVehicle;
    private Rigidbody2D _vehicleRb;

    public FullRigidBodyCarPhysics(GameObject player, VehicleBase currentVehicle) {
        HandlingObject = player;
        _currentVehicle = currentVehicle;
        _vehicleRb = HandlingObject.GetComponent<Rigidbody2D>();
        SetSpecifications();

        var uiCanvas = GameObject.Find("UiCanvas").GetComponent<Canvas>();
        _speedText = uiCanvas.transform.Find("SpeedLabel").GetComponent<Text>();
        CurrentCondition = HandlingCondition.OnGround;
    }
    
    private void SetSpecifications() {
        HandlingObject.GetComponent<Rigidbody2D>().mass
            = _currentVehicle.Mass;
    }

    public void Update() {
        float h = -Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        var acceleration = _currentVehicle.Acceleration;

        var steering = _currentVehicle.Steering;

        Vector2 speed = HandlingObject.transform.up * (v * acceleration);
        _vehicleRb.AddForce(speed);

        float direction = Vector2.Dot(_vehicleRb.velocity, _vehicleRb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f) {
            //_vehicleRb.rotation += h * steering * (_vehicleRb.velocity.magnitude / 5.0f);
            _vehicleRb.AddTorque((h * steering) * (_vehicleRb.velocity.magnitude / 10.0f));
        }
        else {
            //_vehicleRb.rotation -= h * steering * (_vehicleRb.velocity.magnitude / 5.0f);
            _vehicleRb.AddTorque((-h * steering) * (_vehicleRb.velocity.magnitude / 10.0f));
        }

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (_vehicleRb.angularVelocity > 0) {
            steeringRightAngle = -90;
        }
        else {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        Debug.DrawLine((Vector3)_vehicleRb.position, (Vector3)_vehicleRb.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(_vehicleRb.velocity, _vehicleRb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        Debug.DrawLine((Vector3)_vehicleRb.position, (Vector3)_vehicleRb.GetRelativePoint(relativeForce), Color.red);

        _vehicleRb.AddForce(_vehicleRb.GetRelativeVector(relativeForce));
    }

    //public Vector3 CurrentVelocity { get; set; }
    //public HandlingCondition CurrentCondition { get; set; }
    //public GameObject HandlingObject { get; set; }

    //private readonly Text _speedText;
    //private readonly VehicleBase _currentVehicle;
    //private Vector3 _prevPosition;

    //public PlayerCarHandling2(GameObject player, VehicleBase currentVehicle)
    //{
    //	HandlingObject = player;
    //	_currentVehicle = currentVehicle;
    //	_prevPosition = HandlingObject.transform.position;
    //	SetSpecifications();

    //	var uiCanvas = GameObject.Find("UiCanvas").GetComponent<Canvas>();
    //	_speedText = uiCanvas.transform.Find("SpeedLabel").GetComponent<Text>();
    //	CurrentCondition = HandlingCondition.OnGround;
    //}

    //public void Update()
    //{
    //	UpdateHandling();
    //	UiUpdate();
    //}

    public void InstallEquipment() {
        foreach (var eqSlot in _currentVehicle.Slots.OfType<EquipmentSlot>()) {
            eqSlot.Equipment.Install(_currentVehicle);
        }

        SetSpecifications();
    }

    //public void Restart()
    //{
    //	HandlingObject.transform.position = Vector3.zero;
    //}

    //private void SetSpecifications()
    //{
    //	HandlingObject.GetComponent<Rigidbody2D>().mass 
    //		= _currentVehicle.Mass;
    //}

    //private void UpdateHandling()
    //{
    //	// ACCELERATING
    //	var movingDelta = HandlingObject.transform.position
    //					  - _prevPosition;
    //	CurrentVelocity = movingDelta / Time.deltaTime;
    //	var currentSpeed = Mathf.Clamp(CurrentVelocity.y, 0, _currentVehicle.MaxSpeed);
    //	var maxSpeed = _currentVehicle.MaxSpeed;
    //	currentSpeed += 1 - currentSpeed / maxSpeed;
    //	_prevPosition = HandlingObject.transform.position;
    //	HandlingObject.transform.Translate(Vector2.up * currentSpeed * Time.deltaTime);

    //	// STEERING
    //	var userHandlingPos = Camera.main.ScreenToWorldPoint(InputTool.InputPosition);
    //	HandlingObject.transform.position =
    //		Vector2.MoveTowards(
    //			HandlingObject.transform.position,
    //			new Vector2(
    //				userHandlingPos.x,
    //				HandlingObject.transform.position.y),
    //			_currentVehicle.Steering * Time.deltaTime);

    //	// ALIGNING
    //	var playerRotation =
    //		HandlingObject.transform.rotation.eulerAngles.z;
    //	if (playerRotation > 0.5f && playerRotation < 359.5f)
    //	{
    //		var rotateAngle = _currentVehicle.Steering * 15
    //		                  * Time.deltaTime;

    //		if (playerRotation < 180) rotateAngle *= -1;

    //		HandlingObject.transform.Rotate(new Vector3(0, 0, rotateAngle));

    //		if (Math.Abs(playerRotation - HandlingObject.transform.rotation.eulerAngles.z) > 180)
    //		{
    //			HandlingObject.transform.rotation = Quaternion.identity;
    //		}
    //	}
    //	else
    //	{
    //		HandlingObject.transform.rotation = Quaternion.identity;
    //	}
    //}

    //// TODO: Extract to separate class
    //private void UiUpdate()
    //{
    //	var showingSpeed = Mathf.Round(CurrentVelocity.y * 10);
    //	_speedText.text = showingSpeed.ToString(CultureInfo.InvariantCulture);
    //}
}
