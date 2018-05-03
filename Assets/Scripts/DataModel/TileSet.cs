// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Xml.Serialization;
using UnityEngine.Tilemaps;

[Serializable]
public class TileSet
{
	[XmlAttribute("firstgid")]
	public int FirstGid { get; set; }

	[XmlAttribute("name")]
	public string Name { get; set; }

	[XmlAttribute("tilewidth")]
	public int TileWidth { get; set; }

	[XmlAttribute("tileheight")]
	public int TileHeight { get; set; }

	[XmlAttribute("tilecount")]
	public int TileCount { get; set; }

	[XmlAttribute("columns")]
	public int Columns { get; set; }

	[XmlElement("tile")]
	public Tile[] Tiles { get; set; }
}