using System.Reflection;
using DataModel;
using UnityEngine;

namespace MapSystem {
    public class MapParameters {
        public int MapLength;
        public int MapWidth;
        public int ObstaclesCount;
        public float TileSize;
        public MapObject[] MapObjectPrefabs;
        public Sprite MainMapSprite;

        public void MixWith(MapParameters newParams) {
            FieldInfo[] fields = typeof(MapParameters).GetFields();
            foreach (var field in fields) {
                var oldValue = field.GetValue(this);
                var newValue = field.GetValue(newParams);

                if (newValue != null && newValue != oldValue) {
                    field.SetValue(this, newValue);
                }
            }
        }
    }
}