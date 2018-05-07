// RigidBodyCarHandling.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017

using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Assets.Scripts.Vehicles.Machines;
using Tools;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Handling.PlayerHandling
{
	public class RigidBodyCarHandling : IHandlingBehaviour
	{
		public GameObject HandlingObject { get; set; }
		public HandlingCondition CurrentCondition { get; set; }

		public Vector3 CurrentVelocity
		{
			get { return _vehicleRb.velocity; }
		}

		private readonly VehicleBase _currentVehicle;
		private Rigidbody2D _vehicleRb;
		private float _prevAngle = 0;

		public RigidBodyCarHandling(GameObject player, VehicleBase currentVehicle)
		{
			HandlingObject = player;
			_currentVehicle = currentVehicle;
			_vehicleRb = HandlingObject.GetComponent<Rigidbody2D>();
		}

        public void Update()
		{
            ProcessHandling();

            // Accelerating
            //ProcessAccelerating();

            //// Steering
            //ProcessSteering();

            //         // Self aligning
            //         ProcessSelfAligning();
        }

        private void ProcessHandling() {

            // X axis
            var userHandlingPos =
                Camera.main.ScreenToWorldPoint(InputTool.InputPosition);
            var xDiff = userHandlingPos.x -
                        HandlingObject.transform.position.x;
            var xAxis = Mathf.Clamp(xDiff, -1, 1);
            var steer =
                _currentVehicle.Steering
                * Mathf.Pow(xAxis, 2)
                * Mathf.Sign(xAxis);

            // Accelerating
            _vehicleRb = HandlingObject.GetComponent<Rigidbody2D>();
            var currentSpeed = _vehicleRb.velocity.y;
            var maxSpeed = _currentVehicle.MaxSpeed;
            var vehicleAcceleration = _currentVehicle.Acceleration;
            var acceleration = vehicleAcceleration -
                               vehicleAcceleration * currentSpeed /
                               maxSpeed;

            Vector2 speed = HandlingObject.transform.up * (acceleration);
            _vehicleRb.AddForce(speed);

            float direction = Vector2.Dot(_vehicleRb.velocity, _vehicleRb.GetRelativeVector(Vector2.up));
            if (direction >= 0.0f) {
                _vehicleRb.rotation += steer * (_vehicleRb.velocity.magnitude / 5.0f);
                //_vehicleRb.AddTorque((h * steering) * (_vehicleRb.velocity.magnitude / 10.0f));
            }
            else {
                _vehicleRb.rotation -= steer * (_vehicleRb.velocity.magnitude / 5.0f);
                //_vehicleRb.AddTorque((-h * steering) * (_vehicleRb.velocity.magnitude / 10.0f));
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
    

        private void ProcessSteering()
		{
			// Player turning
			var userHandlingPos =
				Camera.main.ScreenToWorldPoint(InputTool.InputPosition);
			var xDiff = userHandlingPos.x -
			            HandlingObject.transform.position.x;
			var xAxis = Mathf.Clamp(xDiff, -1, 1);
			var steer = 
				_currentVehicle.Steering 
				* Mathf.Pow(xAxis, 2) 
				* Mathf.Sign(xAxis);

			_vehicleRb.AddForce(Vector2.right * steer);

        }

        private void ProcessSelfAligning() {


            var curAngle = HandlingObject.transform.rotation.eulerAngles.z;

            if (Math.Abs(curAngle) < 0.01f || (Math.Abs(_prevAngle) > 0.01f && Mathf.Abs(curAngle - _prevAngle) > 300))
                if (Math.Abs(curAngle) < 1f || (Math.Abs(curAngle) > 359f)) {
                    HandlingObject.transform.rotation =
                        Quaternion.AngleAxis(0, Vector3.back);
                    Debug.Log("Going straight");
                    return;
                }


            var alignTorque = 1.5f;

            Debug.Log("curAngle: " + curAngle);

            // Turned right
            if (curAngle >= 0.01 && curAngle <= 180) {
                _vehicleRb.AddTorque(-alignTorque * Mathf.Pow(curAngle, 2) / 180);
               
            }

            // Turned left
            else {
                _vehicleRb.AddTorque(alignTorque * Mathf.Pow(curAngle - 180, 2) / 180);            
            }
                        
            _prevAngle = curAngle;
        }

        private void ProcessAccelerating()
		{
			_vehicleRb = HandlingObject.GetComponent<Rigidbody2D>();
			var currentSpeed = _vehicleRb.velocity.y;
			var maxSpeed = _currentVehicle.MaxSpeed;
			var vehicleAcceleration = _currentVehicle.Acceleration;
			var acceleration = vehicleAcceleration -
			                   vehicleAcceleration * currentSpeed /
			                   maxSpeed;

			_vehicleRb.AddForce(HandlingObject.transform.up *
			                    acceleration);
		}

		public void InstallEquipment()
		{
			foreach (var eqSlot in _currentVehicle.Slots.OfType<EquipmentSlot>())
			{
				eqSlot.Equipment.Install(_currentVehicle);
			}

			SetSpecifications();
		}

		private void SetSpecifications()
		{
			HandlingObject.GetComponent<Rigidbody2D>().mass
				= _currentVehicle.Mass;
		}
	}
}