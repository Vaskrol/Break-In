// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// Map.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017

using UnityEngine.Tilemaps;

namespace DataModel
{
	using System.Collections.Generic;
    using Assets.Scripts.DataModel;

    public class Map
	{
		public Tile[,] Ground { get; set; }

		public List<MapObjectData> MapObjects { get; set; }

		public int Width { get { return Ground.GetLength(0); } }

		public int Length { get { return Ground.GetLength(1); } }
	}
}