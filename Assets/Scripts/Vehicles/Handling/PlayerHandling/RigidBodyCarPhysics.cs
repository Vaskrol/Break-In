// RigidBodyCarHandling.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017

using System;
using System.Linq;
using Assets.Scripts.Vehicles.Machines;
using Tools;
using UnityEngine;

namespace Assets.Scripts.Vehicles.Handling.PlayerHandling
{
	public class RigidBodyCarPhysics : IHandlingBehaviour
	{
		public GameObject HandlingObject { get; set; }
		public HandlingCondition CurrentCondition { get; set; }
        
        private const float _defaultAngularDrag = 1.1f;
        private const float _sidewayFriction = 3.0f;

		public Vector3 CurrentVelocity
		{
			get { return _vehicleRb.velocity; }
		}

		private readonly VehicleBase _currentVehicle;
		private Rigidbody2D _vehicleRb;
		private float _prevAngle = 0;

		public RigidBodyCarPhysics(GameObject player, VehicleBase currentVehicle)
		{
			HandlingObject = player;
			_currentVehicle = currentVehicle;
			_vehicleRb = HandlingObject.GetComponent<Rigidbody2D>();
		}

        public void Update()
		{
            ProcessAccelerating();
			
            ProcessSteering();
			
            ProcessSelfAligning();

	        ProcessSideFriction();
		}

        private void ProcessSteering()
		{
			// TODO: Move input to separate class
			var userHandlingPos =
				Camera.main.ScreenToWorldPoint(InputTool.InputPosition);
			var xDiff = userHandlingPos.x -
			            HandlingObject.transform.position.x;

            float leftClamp = -1, rightClamp = 1;

            var curAngle = HandlingObject.transform.rotation.eulerAngles.z;            
            if (curAngle >= 1 && curAngle <= 180) { // Turned left
                rightClamp = Mathf.Cos(Mathf.Deg2Rad * curAngle);
            }            
            else if (curAngle > 180 && curAngle <= 359) { // Turned right
                leftClamp = Mathf.Cos(Mathf.Deg2Rad * (curAngle - 270));
            }

            var msg = string.Format("Angle: {0}, Clamps: {1},{2}", curAngle, leftClamp, rightClamp);
            Debug.Log(msg);

            var xAxis = Mathf.Clamp(xDiff, leftClamp, rightClamp);
			var steer = 
				_currentVehicle.Steering 
				* Mathf.Pow(xAxis, 2) 
				* Mathf.Sign(xAxis);

			_vehicleRb.AddForce(Vector2.right * steer);

        }

        private void ProcessSelfAligning() {
            
            var curAngle = HandlingObject.transform.rotation.eulerAngles.z;

            var alignTorque = 40f;

            float torque = 0, angularDrag = _defaultAngularDrag;

            // Turned left
            if (curAngle >= 1 && curAngle <= 180) {
                //angularDrag = _defaultAngularDrag;
                torque = - alignTorque * curAngle / 180;
			}

            // Turned right
            else if (curAngle > 180 && curAngle <= 359) {
                //angularDrag = _defaultAngularDrag;
                torque = alignTorque * (360 - curAngle) / 180;
            }

            // (Almost) straight
            else {
                //angularDrag = 100;
            }

	        float fullAlighSpeed = 8f;
			var currentSpeed = _vehicleRb.velocity.y ;
			var modifier = Mathf.Clamp(currentSpeed, 0, fullAlighSpeed) / fullAlighSpeed;
	        torque *= modifier;

			_vehicleRb.angularDrag = angularDrag;
            _vehicleRb.AddTorque(torque);

            _prevAngle = curAngle;

            Debug.DrawRay(
                 HandlingObject.transform.position + HandlingObject.transform.up,
                 HandlingObject.transform.right * torque * -2,
                 Color.red,
                 0);
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

			var curAngle = HandlingObject.transform.rotation.eulerAngles.z;
			// Turned right
			if (curAngle < 359 && curAngle > 1)
			{
				acceleration *= 0.5f;
			}

			_vehicleRb.AddForce(HandlingObject.transform.up *
			                    acceleration);

            Debug.DrawRay(
                HandlingObject.transform.position,
                HandlingObject.transform.up * acceleration,
                Color.green,
                0);
        }

		private void ProcessSideFriction()
		{
			var sideFriction = 1.2f;
			_vehicleRb.velocity = new Vector2(
				_vehicleRb.velocity.x / sideFriction,
				_vehicleRb.velocity.y);
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