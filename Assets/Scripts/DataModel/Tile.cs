// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class CustomTile : TileBase
{
	[XmlAttribute("id")]
	public int Id { get; set; }

	[XmlArray("properties")]
	[XmlArrayItem("property", typeof(TileProperty))]
	public TileProperty[] Properties { get; set; }

	[XmlElement("image")]
	public TileImage Image { get; set; }

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
		base.GetTileData(position, tilemap, ref tileData);
	}

}