// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using DataModel;
using MapSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapClient : MonoBehaviour
{
	[SerializeField] private Tilemap _tileMap;
	[SerializeField] private GameObject _player;
	[SerializeField] private GameObject _walls;
	[SerializeField] private GameObject _mapObjects;
	[SerializeField] private Sprite _mainSprite;
	
	private List<GameObject> _mapObjectsGameObjects;

	// Tile size in world coordinates
	private float _tileSize = 1.28f;

	private Map _map;

	private int _mapWidth     = 12;
	private int _mapLength    = 500;
	private int _chunkSize    = 24;
	private int _currentChunk = -1;

	private object[] _mapParams;

	void InitializeMap()
	{
		_mapParams = new object[]
		{
			_mainSprite
		};

		MapBuilder builder = new ProcedureMapBuilder();
		var director = new MapDirector(builder);
		director.SetupMap(_mapParams);
		director.BuildMap(_mapLength, _mapWidth, _tileSize);
		_map = builder.GetMap();
	}

	public void Start()
	{
		InitializeMap();
		_mapObjectsGameObjects = new List<GameObject>();

		DrawChunk(0);
		DrawChunk(1);
		DrawObjects(0);
		DrawObjects(1);
	}

	public void Update()
	{
		int currentTileY = (int)(_player.transform.position.y / _tileSize);

		// Drawing map
		if (_currentChunk != currentTileY / _chunkSize)
		{
			_currentChunk = currentTileY / _chunkSize;
			int maxChunkNumber = _map.Length / _chunkSize + 1;

			// Draw next chunk
			if (_currentChunk > 0 && _currentChunk < maxChunkNumber)
			{
				DrawChunk(_currentChunk + 1);
				DrawObjects(_currentChunk + 1);
			}

			// Remove hidden chunk
			if (_currentChunk > 0)
			{
				// RemoveChunk(_currentChunk - 2);
				RemoveObjects(_currentChunk - 2);
			}
		}

		_walls.transform.position = new Vector3(
			_walls.transform.position.x,
			_player.transform.position.y,
			_walls.transform.position.z);
	}

	private void DrawChunk(int chunkY)
	{
		DrawTiles(
			chunkY * _chunkSize,
			(chunkY + 1) * _chunkSize,
			_map.Width,
			_tileSize);
	}

	private void RemoveChunk(int chunkY)
	{
		for (int i = chunkY * _chunkSize; i < (chunkY + 1) * _chunkSize; i++)
			for (int j = 0; j < _map.Width; j++)
			{
				// Not removing?
			} 
	}

	private void DrawObjects(int chunkY)
	{
		int minY = (int)(chunkY * _chunkSize * _tileSize);
		int maxY = (int)((chunkY + 1) * _chunkSize * _tileSize);    // 20.48 units / 8 tiles * 256 px for 1 chunk 
		foreach (var mapObject in _map.MapObjects)
		{
			if (mapObject.Y > minY && mapObject.Y < maxY)
			{
				var go = Instantiate(
					Resources.Load(mapObject.PrefabPath, typeof(GameObject))) 
					as GameObject;
				go.transform.position = new Vector3(mapObject.X, mapObject.Y, 0);
				go.transform.rotation = Quaternion.Euler(0, 0, mapObject.Rotation);
				go.transform.parent = _mapObjects.transform;
				_mapObjectsGameObjects.Add(go);
			}
		}
	}

	private void RemoveObjects(int chunkY)
	{
		int minY = (int)(chunkY * _chunkSize * _tileSize);
		int maxY = (int)((chunkY + 1) * _chunkSize * _tileSize);

		for (int i = 0; i < _mapObjectsGameObjects.Count; i++)
		{
			var mapObject = _mapObjectsGameObjects[i];
			if (mapObject.transform.position.y > minY
				&& mapObject.transform.position.y < maxY)
			{
				Destroy(mapObject);
				_mapObjectsGameObjects.RemoveAt(i); // Bad
				i--;
			}
		}
	}

	private void DrawTiles(
		int startY, 
		int endY, 
		int mapWidth, 
		float tileSize)
	{
		// For each row
		for (int i = startY; i < endY; i++)
		{
			// For each tile in row
			for (int j = 0; j < mapWidth; j++)
			{
				var tile = _map.Ground[j, i];
				_tileMap.SetTile(new Vector3Int(j, i, 5), tile);
			}
		}
	}
}
