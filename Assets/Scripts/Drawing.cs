// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drawing : MonoBehaviour
{
	public Canvas UiCanvas;
	private Text _touchesText;

	// Use this for initialization
	void Start()
	{
		_touchesText = 
			UiCanvas
			.transform
			.Find("TouchesLabel")
			.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update()
	{
	//		string debugText;

	//#if UNITY_IPHONE || UNITY_ANDROID

	//		debugText = "Touches: " + Input.touchCount;

	//#else

	//		debugText = 
	//			"Mouse pos (x, y): " + 
	//			Input.mousePosition.x + ", " + 
	//			Input.mousePosition.y;

	//#endif

	//		_touchesText.text = debugText;

	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnPointerUp(PointerEventData eventData)
	{
	}
}
