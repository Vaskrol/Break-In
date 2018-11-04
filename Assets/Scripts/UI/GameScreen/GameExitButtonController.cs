using UI.Base;
using UnityEngine.SceneManagement;

namespace UI.GameScreen {
	public class GameExitButtonController : AbstractButtonController {
		protected override void OnButtonPressed() {
			GameUIController.Instance.FadeGame(true, OnGameFaded);			
		}

		private void OnGameFaded() {
			SceneManager.LoadScene("MapSelection");
		}
	}
}
