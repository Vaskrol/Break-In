// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// FullRotation.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016
using UnityEngine;

public class E_FullRotation : AbstractRotation, IRotationBehaviour {

    public void PerformRotation(GameObject gameObject) {

        var targets = GameObject.FindGameObjectsWithTag("Player");

        if (targets != null && targets.Length > 0) {
            RotateToTarget(gameObject, targets[0].transform.position);
        }
    }
}
