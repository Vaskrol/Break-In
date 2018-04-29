// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class TiledMapBuilder : MonoBehaviour
{
	private string[] _tileNames;
	private string[] _tiles;
	private GameObject[,] _tilesGameObjects;
	private List<GameObject> _mapObjectsGameObjects;

	// Tile size in world coordinates
	private float _tileSize;
	private int _tilePxSize;
	private int _mapHeight;
	private int _mapWidth;

	private GameObject _player;
	private GameObject _walls;
	private int _chunkSize;
	private int _currentChunk;
	private MapObject[] _mapObjects;

	// Use this for initialization
	void Start()
	{
		_chunkSize = 8;
		_currentChunk = -1;

		_player = GameObject.Find("Player");
		_walls = GameObject.Find("Walls");

		InitializeMap(ApplicationModel.CurrentLevelName);

		_tilesGameObjects = new GameObject[_mapWidth, _mapHeight];
		_mapObjectsGameObjects = new List<GameObject>();

		DrawChunk(0);
		DrawChunk(1);
		DrawObjects(0);
		DrawObjects(1);
	}

	// Update is called once per frame
	void Update()
	{
		int currentTileX = (int)(_player.transform.position.x / _tileSize);

		// Drawing background
		if (_currentChunk != currentTileX / _chunkSize)
		{
			_currentChunk = currentTileX / _chunkSize;
			int maxChunkNumber = _mapWidth / _chunkSize + 1;

			// Draw next chunk
			if (_currentChunk > 0 && _currentChunk < maxChunkNumber)
			{
				DrawChunk(_currentChunk + 1);
				DrawObjects(_currentChunk + 1);
			}

			// Remove hidden chunk
			if (_currentChunk > 1)
			{
				RemoveChunk(_currentChunk - 2);
				RemoveObjects(_currentChunk - 2);
			}
		}

		_walls.transform.position = new Vector3(
			_player.transform.position.x,
			_walls.transform.position.y,
			_walls.transform.position.z);
	}

	private void InitializeMap(string mapName)
	{
		// Default map name for test purposes
		mapName = string.IsNullOrEmpty(mapName) ? "MapA" : mapName;

		var mapXml = (TextAsset)
			Resources.Load("Maps/" + mapName, typeof(TextAsset));

		var xmlStringReader = new StringReader(mapXml.text);

		try
		{
			var serializer = new XmlSerializer(typeof(MapCache));
			var map = (MapCache)serializer.Deserialize(xmlStringReader);

			var groundLayer = map.Layers.First(l => l.Name == "Ground");
			_mapHeight = groundLayer.Height;
			_mapWidth = groundLayer.Width;

			var maxFirstGid = map.TileSets.Select(c => c.FirstGid).Max();
			var lastTileSetTilesCount = map.TileSets.Last().TileCount;
			_tileNames = new string[maxFirstGid + lastTileSetTilesCount];
			foreach (var tileSet in map.TileSets)
			{
				foreach (var tile in tileSet.Tiles)
				{
					_tileNames[tileSet.FirstGid + tile.Id] =
						tile.Properties.First(p => p.Name == "Name").Value;
				}
			}

			_tiles = groundLayer.Data.Split(',');

			_mapObjects = map.ObjectGroups.SelectMany(o => o.MapObjects).ToArray();

			var tileObject =
				(GameObject)
				Resources.Load(
					"Prefabs/Ground/" + _tileNames.FirstOrDefault(c => !string.IsNullOrEmpty(c)),
					typeof(GameObject));
			_tileSize =
				tileObject.GetComponent<SpriteRenderer>()
					.sprite.texture.width
				/ tileObject.GetComponent<SpriteRenderer>()
					  .sprite.pixelsPerUnit;
			_tilePxSize = map.TileSets.FirstOrDefault().TileWidth;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			Debug.LogError(ex.StackTrace);
		}
		finally
		{
			xmlStringReader.Close();
		}
	}

	private void DrawChunk(int chunkX)
	{
		DrawTiles(
			chunkX * _chunkSize,
			(chunkX + 1) * _chunkSize,
			_mapHeight,
			_tileSize);
	}

	private void RemoveChunk(int chunkX)
	{
		for (int i = chunkX * _chunkSize; i < (chunkX + 1) * _chunkSize; i++)
			for (int j = 0; j < _mapHeight; j++)
				Destroy(_tilesGameObjects[i, j]);
	}

	private void DrawObjects(int chunkX)
	{
		var parentObject = GameObject.Find("Objects");

		int minX = (int)(chunkX * _chunkSize * _tileSize);
		int maxX = (int)((chunkX + 1) * _chunkSize * _tileSize);    // 20.48 units / 8 tiles * 256 px for 1 chunk 
		foreach (var mapObject in _mapObjects)
		{
			var objX = mapObject.X / (_tilePxSize / _tileSize);
			var objY = mapObject.Y / (_tilePxSize / _tileSize);

			if (objX > minX && objX < maxX)
			{
				var go = Instantiate(
					Resources.Load(
					"Prefabs/Objects/" + _tileNames[mapObject.Gid],
					typeof(GameObject))) as GameObject;
				go.transform.position = new Vector3(objX, InvertYAxis(objY), 0);
				go.transform.parent = parentObject.transform;
				_mapObjectsGameObjects.Add(go);
			}
		}
	}

	private void RemoveObjects(int chunkX)
	{
		int minX = (int)(chunkX * _chunkSize * _tileSize);
		int maxX = (int)((chunkX + 1) * _chunkSize * _tileSize);

		for (int i = 0; i < _mapObjectsGameObjects.Count; i++)
		{
			var mapObject = _mapObjectsGameObjects[i];
			if (mapObject.transform.position.x > minX
				&& mapObject.transform.position.x < maxX)
			{
				Destroy(mapObject);
				_mapObjectsGameObjects.RemoveAt(i);
				i--;
			}
		}
	}

	private void DrawTiles(int startX, int endX, int mapHeight, float tileSize)
	{
		var parentGround = GameObject.Find("Ground");

		for (int i = startX; i < endX; i++)
		{
			for (int j = 0; j < mapHeight; j++)
			{
				var tileId = int.Parse(_tiles[j * mapHeight + i]);
				var groundTile =
				Instantiate(
					Resources.Load(
						"Prefabs/Ground/" + _tileNames[tileId],
						typeof(GameObject))) as GameObject;
				groundTile.transform.position =
					new Vector3(i * tileSize, InvertYAxis(j * tileSize), 5);
				groundTile.transform.parent = parentGround.transform;
				_tilesGameObjects[i, j] = groundTile;
			}
		}
	}

	private float InvertYAxis(float inGameUnitsY)
	{
		return _tileSize * _mapHeight - inGameUnitsY;
	}

}
