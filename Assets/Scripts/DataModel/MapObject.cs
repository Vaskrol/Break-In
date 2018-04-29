// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Xml.Serialization;

[Serializable]
public class MapObject
{
	[XmlAttribute("id")]
	public int Id { get; set; }

	[XmlAttribute("gid")]
	public int Gid { get; set; }

	[XmlAttribute("x")]
	public float X { get; set; }

	[XmlAttribute("y")]
	public float Y { get; set; }

	[XmlAttribute("width")]
	public int Width { get; set; }

	[XmlAttribute("height")]
	public int Height { get; set; }

	public string PrefabPath { get; set; }

	public int Rotation { get; set; }
}