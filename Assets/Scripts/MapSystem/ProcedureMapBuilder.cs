// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using UnityEngine.Tilemaps;

namespace MapSystem
{
	using DataModel;
	using UnityEngine;
	using System.Collections.Generic;
	using UnityEngine.Tilemaps;

	public class ProcedureMapBuilder : MapBuilder
	{
		private Map _map;
		private int _mapWidth, _mapLength;
		private float _tileSize;

		private Sprite _mainBackgroundSprite;

		public ProcedureMapBuilder()
		{
			_map = new Map();
		}
		
		/// <summary>
		/// Setups map size in tiles
		/// 1 tile is usually 2.56 in world coordinates
		/// </summary>
		/// <param name="length">Length of the map</param>
		/// <param name="width">Width of the map</param>
		/// <param name="tileSize">Size of the map tile in world coordinates</param>
		public override void SetMapSize(int length, int width, float tileSize)
		{
			_mapWidth = width;
			_mapLength = length;
			_tileSize = tileSize;
		}

		public override void SetMapParams(object[] parameters)
		{
			foreach (var param in parameters)
			{
				if (param is Sprite)
				{
					_mainBackgroundSprite = param as Sprite;
					continue;
				}
			}
		}

		public override void GenerateGround()
		{
			// Add 2 to width for borders
			_map.Ground = new Tile[_mapWidth, _mapLength];
			for (int y = 0; y < _map.Length; y++)
			{
				// Shift to right by one cell to fit a border line at left
				for (int x = 0; x < _map.Width; x++)
				{
					if (x == 0 || x == _map.Width - 1)
					{
						// Borders
						var tile = new Tile();
						tile.sprite = _mainBackgroundSprite;
						_map.Ground[x, y] = tile;
					}
					else
					{
						// Grounds
						var tile = new Tile();
						tile.sprite = _mainBackgroundSprite;
						_map.Ground[x, y] = tile;
					}
				}
			}
		}



		public override void GenerateObjects()
		{
			_map.MapObjects = new List<MapObject>();
			for (int i = 0; i < 300; i++)
			{
				var obj = new MapObject();
				obj.PrefabPath = "Prefabs/Objects/Rocks/Rock_1";
				obj.X = Random.Range(0, _mapWidth + 2) * _tileSize;
				obj.Y = Random.Range(5, _mapLength) * _tileSize;
				obj.Rotation = Random.Range(0, 359);

				_map.MapObjects.Add(obj);
			}
		}

		public override Map GetMap()
		{
			return _map;
		}
	}
}