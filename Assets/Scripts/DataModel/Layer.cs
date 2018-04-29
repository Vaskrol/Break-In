// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Xml.Serialization;

[Serializable]
public class Layer
{
	[XmlAttribute("name")]
	public string Name { get; set; }

	[XmlAttribute("width")]
	public int Width { get; set; }

	[XmlAttribute("height")]
	public int Height { get; set; }

	[XmlElement("data")]
	public string Data { get; set; }
}
