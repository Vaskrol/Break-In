// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;
using System.Linq;
using Vehicles.Machines;

public class EnemyToPlayerFiring : IFiringBehaviour {

    public GameObject GameObject { get; set; }

    protected readonly VehicleBase _currentVehicle;

    protected readonly Transform _parentTransform;

    protected readonly Transform _player;

    public EnemyToPlayerFiring(Transform parentTransform, VehicleBase currentVehicle) {
        _currentVehicle = currentVehicle;
        _parentTransform = parentTransform;
        _player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    public void Update() {
        foreach (var wSlot in _currentVehicle.Slots.OfType<WeaponSlot>()) {
            wSlot.Weapon.Fire(_player.position);
        }

        foreach (var wSlot in _currentVehicle.Slots.OfType<WeaponSlot>()) {
            wSlot.Weapon.UpdateRotation();
        }
    }
}
