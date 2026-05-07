using UnityEngine;
using UnityEngine.UI;

namespace SolarPhobia.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _defaultPanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _loadGamePanel;
        [SerializeField] private GameObject _creditsPanel;

        public void ShowDefaultPanel() => SetActivePanel(_defaultPanel);
        public void ShowSettingsPanel() => SetActivePanel(_settingsPanel);
        public void ShowLoadGamePanel() => SetActivePanel(_loadGamePanel);
        public void ShowCreditsPanel() => SetActivePanel(_creditsPanel);
        public void QuitGame() => Application.Quit();

        private void SetActivePanel(GameObject panel)
        {
            _defaultPanel?.SetActive(false);
            _settingsPanel?.SetActive(false);
            _loadGamePanel?.SetActive(false);
            _creditsPanel?.SetActive(false);
            panel?.SetActive(true);
        }
    }
}