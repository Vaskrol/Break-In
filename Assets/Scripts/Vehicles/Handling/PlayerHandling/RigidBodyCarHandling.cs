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
            // Accelerating
            ProcessAccelerating();

            // Steering
            ProcessSteering();

            // Self aligning
            ProcessSelfAligning();
        }

        private void ProcessSteering()
		{
			// TODO: Move input to separate class
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

            // Resetting angle to zero when it is almost equals zero
            //if ((Math.Abs(curAngle) < 0.01f || (Math.Abs(_prevAngle) > 0.01f) && Mathf.Abs(curAngle - _prevAngle) > 300))
            //    if (Math.Abs(curAngle) < 1f || (Math.Abs(curAngle) > 359f)) {
            //        HandlingObject.transform.rotation =
            //            Quaternion.AngleAxis(0, Vector3.back);
            //        Debug.Log("Going straight");
            //        return;
            //    }


            var alignTorque = 4.5f;

            Debug.Log("curAngle: " + curAngle);

            float torque = 0, angularDrag = _defaultAngularDrag;

            var localVelocity = HandlingObject.transform.InverseTransformDirection(_vehicleRb.velocity);
            var sideVelocity = localVelocity.y;

            // Skids friction
            // TODO: fix here
            var sideForce = sideVelocity * _sidewayFriction * -1;
            _vehicleRb.AddForce(sideForce * HandlingObject.transform.right);
            Debug.DrawRay(
                 HandlingObject.transform.position,
                 sideForce * HandlingObject.transform.right * 2,
                 Color.cyan,
                 0);

            // Turned right
            if (curAngle >= 1 && curAngle <= 180) {
                angularDrag = _defaultAngularDrag;
                torque = - alignTorque * curAngle / 180;
               
            }

            // Turned left
            else if (curAngle <= 359) {
                angularDrag = _defaultAngularDrag;
                torque = alignTorque * (360 - curAngle) / 180;
            }

            // (Almost) straight
            else {
                angularDrag = 100;
            }

            _vehicleRb.angularDrag = angularDrag;
            _vehicleRb.AddTorque(torque);

            _prevAngle = curAngle;

            Debug.DrawRay(
                 HandlingObject.transform.position + HandlingObject.transform.up,
                 HandlingObject.transform.right * torque * -2,
                 Color.red,
                 0);

            // Sideways drag
            Vector2 velocity;            
            velocity.x = _vehicleRb.velocity.x / _sidewayFriction; 
            velocity.y = _vehicleRb.velocity.y;
            _vehicleRb.velocity = velocity;
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

            Debug.DrawRay(
                HandlingObject.transform.position,
                HandlingObject.transform.up * acceleration,
                Color.green,
                0);
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