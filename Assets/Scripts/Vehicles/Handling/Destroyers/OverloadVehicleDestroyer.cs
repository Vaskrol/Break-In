// SpeedGradientDestroyer.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace Assets.Scripts.Vehicles.Handling
{
	using Interfaces;
	using Machines;
	using UnityEngine;

	public class OverloadVehicleDestroyer : IVehicleDestroyer
	{

		private readonly float _maxOverload;
		private readonly VehicleBase _currentVehicle;

		private float _previousSpeed;

		public OverloadVehicleDestroyer(
			GameObject gameObject,
			VehicleBase currentVehicle,
			float maxOverload)
		{
			_maxOverload = maxOverload;
			_currentVehicle = currentVehicle;
			_previousSpeed =
				_currentVehicle.Handling.CurrentVelocity.y;
		}

		public bool DestroyNeeded()
		{
			var currentSpeed =
				_currentVehicle.Handling.CurrentVelocity.y;
			var overload =
				Mathf.Abs(currentSpeed - _previousSpeed);
			_previousSpeed = currentSpeed;
			if (overload < _maxOverload)
				return false;
			
			Debug.Log(
				"Vehicle " + _currentVehicle + 
				" has been blowed up due to " + overload + " overload.");
			return true;
		}

	}
}