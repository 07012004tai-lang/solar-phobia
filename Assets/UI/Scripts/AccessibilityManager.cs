using UnityEngine;
using UnityEngine.UI;

namespace SolarPhobia.UI
{
    public class AccessibilityManager : MonoBehaviour
    {
        [SerializeField] private float _minTextScale = 1f;
        [SerializeField] private float _maxTextScale = 3f;
        [SerializeField] private Toggle _colorblindToggle;

        public void SetTextScale(float scale) { /* Apply text scale */ }
        public void ToggleColorblindMode(bool enabled) { /* Apply colorblind filter */ }
    }
}