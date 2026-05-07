using UnityEngine;
using UnityEngine.UI;

namespace SolarPhobia.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Toggle _colorblindToggle;
        [SerializeField] private Dropdown _textScaleDropdown;

        private void Start() => LoadSettings();

        public void LoadSettings() { /* Load saved settings */ }
        public void SaveSettings() { /* Save current settings */ }
        public void ResetToDefaults() { /* Reset settings to default */ }
    }
}