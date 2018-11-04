// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// FullRotation.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016
using UnityEngine;

public class P_FullRotation : AbstractRotation, IRotationBehaviour {

	public void PerformRotation(GameObject gameObject)	{
		if (Application.isMobilePlatform) {

			// Touch input
			if (Input.touches.Length > 0) {
				foreach (var touch in Input.touches) {
					var mousePosition = Camera.main.ScreenToWorldPoint(touch.position);
                    RotateToTarget(gameObject, mousePosition);
					break;
				}
			}
		}
		else
		{
			// Mouse input
			var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RotateToTarget(gameObject, mousePosition);
        }
	}
}
