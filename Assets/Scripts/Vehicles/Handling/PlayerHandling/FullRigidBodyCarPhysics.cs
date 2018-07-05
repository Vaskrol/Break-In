// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Linq;
using Assets.Scripts.Vehicles.Machines;
using Tools;
using UnityEngine;
using UnityEngine.UI;

// TODO: Separate this to "phisics" class
public class FullRigidBodyCarPhysics : IHandlingBehaviour
{
    private float _h, _v;

    public HandlingCondition CurrentCondition { get; set; }
    public GameObject HandlingObject { get; set; }

    public Vector3 CurrentVelocity {
        get { return _vehicleRb == null ? Vector2.zero : _vehicleRb.velocity; }
    }
    private readonly Text _speedText;
    private readonly VehicleBase _currentVehicle;
    private Rigidbody2D _vehicleRb;
    private Text _debugLabel;

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
        _debugLabel = GameObject.Find("Direction").GetComponent<Text>();
    }

    public void Update() {

        ProcessInput();

        ProcessPhysics();
    }

    private void ProcessPhysics() {
        var steering = _currentVehicle.Steering;

        Vector2 speed = HandlingObject.transform.up * (_v * _currentVehicle.Acceleration);
        _vehicleRb.AddForce(speed);

        // Speed vector * Car forward vector in global space
        // Shows how is speed vector equals car forward direction
        // 0 when moving sidewais, positive when moving forward, negative when moving backward
        float direction = Vector2.Dot(_vehicleRb.velocity, _vehicleRb.GetRelativeVector(Vector2.up));

        var curAngle = HandlingObject.transform.rotation.eulerAngles.z;

        // IF WHAT? if ((curAngle > 0 && curAngle < 1) 
        {
            // Moving forward
            if (direction >= 0.0f) {
                _vehicleRb.AddTorque((_h * steering) * (_vehicleRb.velocity.magnitude / 10.0f));
            }

            // Moving backward
            else {
                _vehicleRb.AddTorque((-_h * steering) * (_vehicleRb.velocity.magnitude / 10.0f));
            }
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
        Debug.Log("Rught angle from forward: " + rightAngleFromForward);

        Debug.DrawLine((Vector3)_vehicleRb.position, (Vector3)_vehicleRb.GetRelativePoint(rightAngleFromForward), Color.green);
        float driftForce = Vector2.Dot(_vehicleRb.velocity, _vehicleRb.GetRelativeVector(rightAngleFromForward.normalized));
        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);
        Debug.DrawLine((Vector3)_vehicleRb.position, (Vector3)_vehicleRb.GetRelativePoint(relativeForce), Color.red);

        _vehicleRb.AddForce(_vehicleRb.GetRelativeVector(relativeForce));
    }

    private void ProcessInput() {
        _v = 0.5f + Input.GetAxis("Vertical") / 1.5f;
        
        var userHandlingPos = Camera.main.ScreenToWorldPoint(InputTool.InputPosition).x - HandlingObject.transform.position.x;
        _h = - Mathf.Clamp(userHandlingPos, -1, 1);

       
        //if (_h == 0) {
        //    var curAngle = HandlingObject.transform.rotation.eulerAngles.z;
        //    if (curAngle >= 1 && curAngle <= 180) { // Turned left
        //        _h = 0.5f;
        //    }
        //    else if (curAngle > 180 && curAngle <= 359) { // Turned right
        //        _h = -0.5f;
        //    }
        //}
    }

    public void InstallEquipment() {
        foreach (var eqSlot in _currentVehicle.Slots.OfType<EquipmentSlot>()) {
            eqSlot.Equipment.Install(_currentVehicle);
        }

        SetSpecifications();
    }

   }
