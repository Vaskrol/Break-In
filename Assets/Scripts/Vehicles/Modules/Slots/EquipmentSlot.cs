// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// EquipmentSlot.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016

using UnityEngine;

public class EquipmentSlot : ISlot
{
	public Vector3 Position { get; set; }

	public IEquipment Equipment { get; set; }
}
