// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Linq;
using Assets.Scripts.DataModel;
using DataModel;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace MapSystem {
    public class ProcedureMapBuilder : MapBuilder {
        private readonly Map           _map;
        private          MapParameters _mapParams;

        public ProcedureMapBuilder() {
            _map = new Map();
        }
        
        /// 1 tile is usually 2.56 in world coordinates
        public override void SetMapParams(MapParameters parameters) {
            _mapParams = parameters;
        }

        public override void GenerateGround() {
            // Add 2 to width for borders
            _map.Ground = new Tile[_mapParams.MapWidth, _mapParams.MapLength];
            for (int y = 0; y < _map.Length; y++) {
                // Shift to right by one cell to fit a border line at left
                for (int x = 0; x < _map.Width; x++) {
                    if (x == 0 || x == _map.Width - 1) {
                        // Borders
                        var tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite       = _mapParams.MainMapSprite;
                        _map.Ground[x, y] = tile;
                    }
                    else {
                        // Grounds
                        var tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite       = _mapParams.MainMapSprite;
                        _map.Ground[x, y] = tile;
                    }
                }
            }
        }


        public override void GenerateObjects() {
            _map.MapObjects = new List<MapObjectData>();
            var obstacles = _mapParams.MapObjectPrefabs
                .Where(o => o.ObjectType == ObjectTypeEnum.UndestructableObstacle)
                .ToArray();
            for (int i = 0; i < 100; i++) {
                var objData = new MapObjectData();

                objData.Prefab = obstacles[Random.Range(0, obstacles.Length)];
                objData.InstantiatePosition = new Vector3(
                    Random.Range(0, _mapParams.MapWidth + 2) * _mapParams.TileSize,
                    Random.Range(5, _mapParams.MapLength) * _mapParams.TileSize,
                    0);
                objData.InstantiateRotation = Quaternion.Euler(0, 0, Random.Range(0, 359));

                _map.MapObjects.Add(objData);
            }
        }

        public override Map GetMap() {
            return _map;
        }
    }
}