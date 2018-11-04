// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// NoRotation.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace Assets.Scripts.Vehicles.Weapons.Rotation
{
	using UnityEngine;

	public class NoRotation : IRotationBehaviour
	{
		public void PerformRotation(GameObject gameObject)
		{
			// Do not rotate
		}
	}
}