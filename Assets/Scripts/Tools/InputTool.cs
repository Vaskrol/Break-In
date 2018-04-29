// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
// InputTool.cs. 
// 
// Vpetrov. Петров Василий Александрович. 
// 
// 2017
namespace Tools
{
	using UnityEngine;

	public static class InputTool
	{
		public static Vector2 InputPosition
		{
			get
			{
				if (Application.isEditor)
					return Input.mousePosition;

#if UNITY_IPHONE || UNITY_ANDROID
				return Input.GetTouch(0).position;
#else
				return Input.mousePosition;
#endif
			}
		}
	}
}