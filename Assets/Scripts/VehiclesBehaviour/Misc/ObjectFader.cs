// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
	public float TimeToFade { get; set; }

	private Renderer _renderer;

	public void Start()
	{
		_renderer = GetComponent<Renderer>();
	}
	public void Update()
	{
		if (_renderer is LineRenderer)
		{
			_renderer.material.color =
				Color.Lerp(
					_renderer.material.color,
					Color.clear,
					TimeToFade * Time.deltaTime);
		}
	}
}
