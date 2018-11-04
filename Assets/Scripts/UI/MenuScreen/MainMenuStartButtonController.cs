using UI.Base;
using UnityEngine.SceneManagement;

namespace UI.MenuScreen {
    public class MainMenuStartButtonController : AbstractButtonController {
        protected override void OnButtonPressed() {
            SceneManager.LoadScene("MapSelection");
        }
    }
}
