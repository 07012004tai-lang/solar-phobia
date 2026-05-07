using UnityEngine;
using UnityEngine.UI;

namespace SolarPhobia.UI
{
    public class LoadGameMenu : MonoBehaviour
    {
        [SerializeField] private Transform _saveSlotContainer;
        [SerializeField] private GameObject _saveSlotPrefab;

        private void Start() => PopulateSaveSlots();

        private void PopulateSaveSlots() { /* Populate save slots */ }
        public void LoadSave(int slotIndex) { /* Load game from slot */ }
    }
}