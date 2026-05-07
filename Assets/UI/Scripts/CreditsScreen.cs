using UnityEngine;
using UnityEngine.UI;

namespace SolarPhobia.UI
{
    public class CreditsScreen : MonoBehaviour
    {
        [SerializeField] private Text _creditsText;

        public void SetCreditsText(string credits) => _creditsText.text = credits;
    }
}