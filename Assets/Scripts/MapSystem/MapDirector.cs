// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using UnityEngine;

namespace MapSystem {
	public class MapDirector {
		private readonly MapBuilder _mapBuilder;

		public MapDirector(MapBuilder builder) {
			_mapBuilder = builder;
		}

		public void SetupMap(MapParameters mapParams) {
			_mapBuilder.SetMapParams(mapParams);
		}

		public void BuildMap() {
			_mapBuilder.GenerateGround();
			_mapBuilder.GenerateObjects();
		}
	}
}
