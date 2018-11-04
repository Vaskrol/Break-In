using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CitySelectionButtonController : MonoBehaviour {
	
	[SerializeField] private string _cityName;
	
	private Button _button;
	
	void Start() {
		_button = GetComponent<Button>();

		if (string.IsNullOrEmpty(_cityName)) {
			var subButtonText = GetComponentInChildren<Text>();
			if (subButtonText == null)
				throw new ArgumentNullException("No city name specified!");

			_cityName = subButtonText.text;
		}
		
		if (_button == null)
			throw new NullReferenceException("No button component for city selection.");

		_button.onClick.AddListener(LoadGame);
	}

	private void LoadGame() {
		
		//TODO: Set game parameters here
		Debug.Log("Loading city " + _cityName);
		
		SceneManager.LoadScene("Game");
	}

	private void OnDestroy() {
		if (_button == null)
			return;
		
		_button.onClick.RemoveListener(LoadGame);
	}
}
