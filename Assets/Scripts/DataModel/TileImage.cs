// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Xml.Serialization;

[Serializable]
public class TileImage
{
	[XmlAttribute("width")]
	public int Width { get; set; }

	[XmlAttribute("height")]
	public int Height { get; set; }
}