
using System.Collections.Generic;
using Assets.Scripts.Basics;
using UnityEngine;
using System.Linq;
using Vehicles.Modules.Equipment;

class ModulesController : MbSingleton<ModulesController> {

    [SerializeField]
    private List<GameObject> _weaponPrefabs;
    
    public List<IWeapon> Weapons {
        get {
            return _weaponPrefabs.Select(w => w.GetComponent<IWeapon>()).ToList();
        }
    }

    public List<IEquipment> Equipment = new List<IEquipment>();

    private void Start() {
        Equipment.Add(new EngineA());
    }   
}

