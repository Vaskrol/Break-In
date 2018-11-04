// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// SlowVehicleDestroyer.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017

using Vehicles.Handling.Destroyers;
using Vehicles.Machines;

namespace Assets.Scripts.Vehicles.Handling
{
	using UnityEngine;

	public class SlowVehicleDestroyer : IVehicleDestroyer
	{
		private readonly float _minSpeed;
		private readonly GameObject _go;
		private readonly VehicleBase _currentVehicle;

		public SlowVehicleDestroyer(
			GameObject gameObject, 
			VehicleBase currentVehicle, 
			float minimumSpeed)
		{
			_minSpeed = minimumSpeed;
			_go = gameObject;
			_currentVehicle = currentVehicle;
		}

		public bool DestroyNeeded()
		{
			var speed = _currentVehicle.Handling.CurrentVelocity.magnitude;
			return speed < _minSpeed;
		}
	}
}