// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// EngineA.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016

using Vehicles.Machines;

namespace Vehicles.Modules.Equipment {
	public class EngineA : IEquipment
	{
		private readonly float _maxSpeed = 0.5f;

		public void Install(VehicleBase vehicle)
		{
			vehicle.Performance.MaxSpeed += _maxSpeed;
		}

		public void Uninstall(VehicleBase vehicle)
		{
			vehicle.Performance.MaxSpeed -= _maxSpeed;
		}
	}
}
