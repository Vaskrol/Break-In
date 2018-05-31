﻿// RigidBodyCarHandling.cs. 
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
	public class RigidBodyCarHandling : IHandlingBehaviour
	{
		public GameObject HandlingObject { get; set; }
		public HandlingCondition CurrentCondition { get; set; }
        
        private const float _defaultAngularDrag = 1.1f;

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
            // Accelerating
            ProcessAccelerating();

            // Steering
            ProcessSteering();

            // Self aligning
            ProcessSelfAligning();
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


            var alignTorque = 1.1f;

            Debug.Log("curAngle: " + curAngle);

            // Turned right
            if (curAngle >= 5 && curAngle <= 180) {
                _vehicleRb.angularDrag = _defaultAngularDrag;
                _vehicleRb.AddTorque(-alignTorque * curAngle / 180);
               
            }

            // Turned left
            else if (curAngle <= 355) {
                _vehicleRb.angularDrag = _defaultAngularDrag;
                _vehicleRb.AddTorque(alignTorque * (curAngle - 180) / 180);
            }

            // (Almost) straight
            else {
                _vehicleRb.angularDrag = 100;
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