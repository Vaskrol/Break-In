using UnityEngine;

namespace Assets.Scripts.DataModel {
    public class MapObjectData {
        public MapObject Prefab { get; set; }
        public Vector3 InstantiatePosition { get; set; }
        public Quaternion InstantiateRotation { get; set; }
    }
}
