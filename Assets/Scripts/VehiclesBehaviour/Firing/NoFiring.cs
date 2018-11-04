// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// NoFiring.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017

using UnityEngine;

namespace Vehicles.Firing
{
	public class NoFiring : IFiringBehaviour
	{
		public GameObject GameObject { get; set; }

		public void Update()
		{
			// Do not fire at all
		}
	}

}