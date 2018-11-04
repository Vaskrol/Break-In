// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using Vehicles.Handling.Destroyers;

public interface IVehicle
{
	float UserAccelerating { get; set; }

	float Braking { get; set; }

	float Steering { get; set; }

	float Acceleration { get; set; }

	float MaxSpeed { get; set; }

	float Mass { get; set; }

	ISlot[] Slots { get; set; }

	IHandlingBehaviour Handling { get; set; }

	IJumpingBehaviour Jumping { get; set; }

	IFiringBehaviour Firing { get; set; }

	IVehicleDestroyer Destroyer { get; set; }

	void Update();
}
