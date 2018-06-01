
using System.Collections.Generic;
using Assets.Scripts.Basics;
using UnityEngine;
using System.Linq;

class ModulesController : MbSingleton<ModulesController> {

    [SerializeField]
    private List<GameObject> _weaponPrefabs;
    
    public List<IWeapon> Weapons {
        get {
            return _weaponPrefabs.Select(w => w.GetComponent<IWeapon>()).ToList();
        }
    }

    public List<IEquipment> Equipment = null;

    private void Start() {
        Equipment.Add(new EngineA());
    }   
}

