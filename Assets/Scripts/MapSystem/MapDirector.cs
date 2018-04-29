// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using MapSystem;

public class MapDirector
{
	private MapBuilder mapBuilder;

	public MapDirector(MapBuilder builder)
	{
		mapBuilder = builder;
	}

	public void BuildMap(int length, int width, float tileSize)
	{
		mapBuilder.SetMapSize(length, width, tileSize);
		mapBuilder.GenerateGround();
		mapBuilder.GenerateObjects();
	}
}
