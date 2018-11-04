namespace UI.Base {
    
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class AbstractButtonController : MonoBehaviour {

        private Button _button;
	
        private void Awake () {
            _button = GetComponent<Button>();
            if (_button == null) {
                Debug.LogError("Cannot get button component.");
                return;
            }
            _button.onClick.AddListener(OnButtonPressed);
        }

        protected virtual void OnButtonPressed() { }

        private void OnDestroy() {
            _button.onClick.RemoveListener(OnButtonPressed);
        }
    }
}