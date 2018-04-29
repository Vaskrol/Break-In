// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// MapCache.cs. 
// 
// Vpetrov. Петров Василий. 
// 
// 2016

using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("map")]
public class MapCache
{
	[XmlElement("tileset")]
	public TileSet[] TileSets { get; set; }

	[XmlElement("layer")]
	public Layer[] Layers { get; set; }

	[XmlElement("objectgroup")]
	public ObjectGroup[] ObjectGroups { get; set; }
}