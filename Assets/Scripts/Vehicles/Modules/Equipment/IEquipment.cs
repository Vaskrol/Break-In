// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// IEquipment.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016

using Assets.Scripts.Vehicles.Machines;

public interface IEquipment
{
	/// <summary>
	/// Installs equipment on the vehicle, affecting it parameters
	/// </summary>
	/// <param name="vehicle"></param>
	void Install(VehicleBase vehicle);

	/// <summary>
	/// Uninstalls equipment from the vehicle, removing this equipment effects
	/// </summary>
	/// <param name="vehicle"></param>
	void Uninstall(VehicleBase vehicle);
}
