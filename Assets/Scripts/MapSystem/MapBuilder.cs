// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// IMapBuilder.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace MapSystem
{
    using System;
    using DataModel;

    public abstract class MapBuilder
	{
		public abstract void SetMapSize(int length, int width, float tileSize);
		public abstract void SetMapParams(object[] parameters);
        public abstract void SetMapObjects(MapObject[] mapObjects);
        public abstract void GenerateGround();
		public abstract void GenerateObjects();
		public abstract Map GetMap();
	}
}