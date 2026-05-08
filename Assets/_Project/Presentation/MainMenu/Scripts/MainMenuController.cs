using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// using Unity.InputSystem; // TODO: fix assembly reference
using UnityEngine.SceneManagement;

namespace SolarPhobia.Presentation.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public enum ScreenState
        {
            MainMenu,
            Settings,
            Credits,
            QuitConfirm
        }

        public static event Action OnNewGameRequested;
        public static event Action OnContinueRequested;
        public static event Action OnQuitRequested;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private VisualElement _settingsPanel;
        private VisualElement _creditsPanel;
        private VisualElement _dialogOverlay;

        private Button _btnNewGame;
        private Button _btnContinue;
        private Button _btnSettings;
        private Button _btnCredits;
        private Button _btnQuit;
        private Button _btnCloseSettingsBtn;
        private Button _btnBackCredits;
        private Button _btnConfirmQuitBtn;
        private Button _btnCancelQuitBtn;

        private RadioButtonGroup _settingsTabs;
        private VisualElement _tabAudio;
        private VisualElement _tabVideo;
        private VisualElement _tabControls;

        private Slider _sliderMasterVolume;
        private Slider _sliderMusicVolume;
        private Slider _sliderSfxVolume;
        private Slider _sliderAmbientVolume;
        private Slider _sliderUiScale;
        private Toggle _toggleSubtitles;
        private Toggle _toggleVSync;
        private Toggle _toggleCameraShake;
        private Toggle _toggleMotionBlur;
        private Toggle _toggleInvertY;
        private Toggle _toggleGamepadVibration;
        private DropdownField _dropdownResolution;
        private DropdownField _dropdownWindowMode;
        private DropdownField _dropdownQuality;
        private DropdownField _dropdownTextSize;
        private DropdownField _dropdownInputDevice;

        private Label _labelTitle;
        private Label _labelSubtitle;
        private Label _labelVersion;
        private Label _settingsTitle;
        private Button _btnCloseSettings;
        private Label _creditsTitle;
        private Label _dialogTitle;
        private Label _dialogMessage;
        private Button _btnConfirmQuit;
        private Button _btnCancelQuit;

        private ScreenState _currentScreen = ScreenState.MainMenu;
        private List<Button> _menuButtons;
        private int _focusedButtonIndex = 0;

        private void Awake()
        {
            _root = _document.rootVisualElement;
            CacheElements();
            BindEvents();
            LoadSettings();
            ApplyLocalizedText();
            LocalizationKeys.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnDestroy()
        {
            LocalizationKeys.OnLanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged()
        {
            ApplyLocalizedText();
        }

        private void OnEnable()
        {
            CheckSaveFile();
            SetScreen(ScreenState.MainMenu);
        }

        private void Update()
        {
            // TODO: Fix Input System reference - commented out for now
            // Input System polling disabled temporarily
        }

        private void CacheElements()
        {
            _btnNewGame = _root.Q<Button>("new-game");
            _btnContinue = _root.Q<Button>("continue");
            _btnSettings = _root.Q<Button>("settings");
            _btnCredits = _root.Q<Button>("credits");
            _btnQuit = _root.Q<Button>("quit");

            _settingsPanel = _root.Q<VisualElement>("settings-panel");
            _btnCloseSettings = _settingsPanel?.Q<Button>("settings-close");
            _settingsTabs = _settingsPanel?.Q<RadioButtonGroup>("settings-tab-group");
            _tabAudio = _settingsPanel?.Q<VisualElement>("settings-tab-content--audio");
            _tabVideo = _settingsPanel?.Q<VisualElement>("settings-tab-content--video");
            _tabControls = _settingsPanel?.Q<VisualElement>("settings-tab-content--controls");

            _sliderMasterVolume = _settingsPanel?.Q<Slider>("master-volume");
            _sliderMusicVolume = _settingsPanel?.Q<Slider>("music-volume");
            _sliderSfxVolume = _settingsPanel?.Q<Slider>("sfx-volume");
            _sliderAmbientVolume = _settingsPanel?.Q<Slider>("ambient-volume");
            _sliderUiScale = _settingsPanel?.Q<Slider>("ui-scale");
            _toggleSubtitles = _settingsPanel?.Q<Toggle>("subtitles");
            _toggleVSync = _settingsPanel?.Q<Toggle>("vsync");
            _toggleCameraShake = _settingsPanel?.Q<Toggle>("camera-shake");
            _toggleMotionBlur = _settingsPanel?.Q<Toggle>("motion-blur");
            _toggleInvertY = _settingsPanel?.Q<Toggle>("invert-y");
            _toggleGamepadVibration = _settingsPanel?.Q<Toggle>("gamepad-vibration");
            _dropdownResolution = _settingsPanel?.Q<DropdownField>("resolution");
            _dropdownWindowMode = _settingsPanel?.Q<DropdownField>("window-mode");
            _dropdownQuality = _settingsPanel?.Q<DropdownField>("quality");
            _dropdownTextSize = _settingsPanel?.Q<DropdownField>("text-size");
            _dropdownInputDevice = _settingsPanel?.Q<DropdownField>("input-device");

            _creditsPanel = _root.Q<VisualElement>("credits-panel");
            _btnBackCredits = _creditsPanel?.Q<Button>("credits-back");

            _dialogOverlay = _root.Q<VisualElement>("dialog-overlay");
            _btnConfirmQuit = _dialogOverlay?.Q<Button>("confirm-quit");
            _btnCancelQuit = _dialogOverlay?.Q<Button>("cancel-quit");

            _labelTitle = _root.Q<Label>("title");
            _labelSubtitle = _root.Q<Label>("subtitle");
            _labelVersion = _root.Q<Label>("version");

            _settingsTitle = _root.Q<Label>("settings-title");
            _btnCloseSettingsBtn = _settingsPanel?.Q<Button>("settings-close");
            _creditsTitle = _root.Q<Label>("credits-title");
            _dialogTitle = _root.Q<Label>("dialog-title");
            _dialogMessage = _root.Q<Label>("dialog-message");
            _btnConfirmQuitBtn = _dialogOverlay?.Q<Button>("confirm-quit");
            _btnCancelQuitBtn = _dialogOverlay?.Q<Button>("cancel-quit");

            _menuButtons = new List<Button> { _btnNewGame, _btnContinue, _btnSettings, _btnCredits, _btnQuit };
        }

        private void BindEvents()
        {
            _btnNewGame?.RegisterCallback<ClickEvent>(_ => OnNewGame());
            _btnContinue?.RegisterCallback<ClickEvent>(_ => OnContinue());
            _btnSettings?.RegisterCallback<ClickEvent>(_ => OnSettings());
            _btnCredits?.RegisterCallback<ClickEvent>(_ => OnCredits());
            _btnQuit?.RegisterCallback<ClickEvent>(_ => OnQuit());

            _btnCloseSettings?.RegisterCallback<ClickEvent>(_ => CloseSettings());
            _btnBackCredits?.RegisterCallback<ClickEvent>(_ => CloseCredits());
            _btnConfirmQuit?.RegisterCallback<ClickEvent>(_ => ConfirmQuit());
            _btnCancelQuit?.RegisterCallback<ClickEvent>(_ => CancelQuit());

            _settingsTabs?.RegisterValueChangedCallback(evt =>
            {
                int idx = evt.newValue;
                string[] tabs = { "audio", "video", "controls" };
                if (idx >= 0 && idx < tabs.Length)
                    ShowSettingsTab(tabs[idx]);
            });

            _sliderMasterVolume?.RegisterValueChangedCallback(evt =>
            {
                AudioListener.volume = evt.newValue;
                PlayerPrefs.SetFloat(PlayerPrefsKeys.MASTER_VOLUME, evt.newValue);
            });
            _sliderMusicVolume?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetFloat(PlayerPrefsKeys.MUSIC_VOLUME, evt.newValue);
            });
            _sliderSfxVolume?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetFloat(PlayerPrefsKeys.SFX_VOLUME, evt.newValue);
            });
            _sliderAmbientVolume?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetFloat(PlayerPrefsKeys.AMBIENT_VOLUME, evt.newValue);
            });
            _sliderUiScale?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetFloat(PlayerPrefsKeys.UI_SCALE, evt.newValue);
                ApplyUiScale(evt.newValue);
            });

            _toggleSubtitles?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.SUBTITLES_ENABLED, evt.newValue ? 1 : 0);
            });
            _toggleVSync?.RegisterValueChangedCallback(evt =>
            {
                QualitySettings.vSyncCount = evt.newValue ? 1 : 0;
                PlayerPrefs.SetInt(PlayerPrefsKeys.V_SYNC, evt.newValue ? 1 : 0);
            });
            _toggleCameraShake?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.CAMERA_SHAKE, evt.newValue ? 1 : 0);
            });
            _toggleMotionBlur?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.MOTION_BLUR, evt.newValue ? 1 : 0);
            });
            _toggleInvertY?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.INVERT_Y_AXIS, evt.newValue ? 1 : 0);
            });
            _toggleGamepadVibration?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.GAMEPAD_VIBRATION, evt.newValue ? 1 : 0);
            });

            _dropdownResolution?.RegisterValueChangedCallback(evt => OnResolutionChanged(evt.newValue));
            _dropdownWindowMode?.RegisterValueChangedCallback(evt => OnWindowModeChanged(evt.newValue));
            _dropdownQuality?.RegisterValueChangedCallback(evt => OnQualityChanged(evt.newValue));
            _dropdownTextSize?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetString(PlayerPrefsKeys.TEXT_SIZE, evt.newValue);
            });
            _dropdownInputDevice?.RegisterValueChangedCallback(evt =>
            {
                PlayerPrefs.SetString(PlayerPrefsKeys.INPUT_DEVICE, evt.newValue);
            });
        }

        private void ApplyLocalizedText()
        {
            if (_labelTitle != null) _labelTitle.text = LocalizationKeys.Get(LocalizationKeys.TITLE);
            if (_labelSubtitle != null) _labelSubtitle.text = LocalizationKeys.Get(LocalizationKeys.SUBTITLE);
            if (_labelVersion != null) _labelVersion.text = LocalizationKeys.Get(LocalizationKeys.VERSION);

            if (_btnNewGame != null) _btnNewGame.text = LocalizationKeys.Get(LocalizationKeys.BUTTON_NEWGAME);
            if (_btnSettings != null) _btnSettings.text = LocalizationKeys.Get(LocalizationKeys.BUTTON_SETTINGS);
            if (_btnCredits != null) _btnCredits.text = LocalizationKeys.Get(LocalizationKeys.BUTTON_CREDITS);
            if (_btnQuit != null) _btnQuit.text = LocalizationKeys.Get(LocalizationKeys.BUTTON_QUIT);

            if (_settingsTitle != null) _settingsTitle.text = LocalizationKeys.Get(LocalizationKeys.SETTINGS_TITLE);
            if (_creditsTitle != null) _creditsTitle.text = LocalizationKeys.Get(LocalizationKeys.CREDITS_TITLE);
            if (_dialogTitle != null) _dialogTitle.text = LocalizationKeys.Get(LocalizationKeys.QUIT_TITLE);
            if (_dialogMessage != null) _dialogMessage.text = LocalizationKeys.Get(LocalizationKeys.QUIT_MESSAGE);
            if (_btnConfirmQuitBtn != null) _btnConfirmQuitBtn.text = LocalizationKeys.Get(LocalizationKeys.QUIT_CONFIRM);
            if (_btnCancelQuitBtn != null) _btnCancelQuitBtn.text = LocalizationKeys.Get(LocalizationKeys.QUIT_CANCEL);

            if (_sliderMasterVolume != null && _sliderMasterVolume.parent != null)
            {
                var label = _sliderMasterVolume.parent.Q<Label>(null, "settings-label");
                if (label != null) label.text = LocalizationKeys.Get(LocalizationKeys.LABEL_MASTER_VOLUME);
            }
        }

        private void LoadSettings()
        {
            float master = PlayerPrefs.GetFloat(PlayerPrefsKeys.MASTER_VOLUME, PlayerPrefsKeys.Defaults.MASTER_VOLUME);
            if (_sliderMasterVolume != null) _sliderMasterVolume.value = master;
            AudioListener.volume = master;

            if (_sliderMusicVolume != null) _sliderMusicVolume.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.MUSIC_VOLUME, PlayerPrefsKeys.Defaults.MUSIC_VOLUME);
            if (_sliderSfxVolume != null) _sliderSfxVolume.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.SFX_VOLUME, PlayerPrefsKeys.Defaults.SFX_VOLUME);
            if (_sliderAmbientVolume != null) _sliderAmbientVolume.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.AMBIENT_VOLUME, PlayerPrefsKeys.Defaults.AMBIENT_VOLUME);
            if (_sliderUiScale != null) { _sliderUiScale.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.UI_SCALE, PlayerPrefsKeys.Defaults.UI_SCALE); ApplyUiScale(_sliderUiScale.value); }

            if (_toggleSubtitles != null) _toggleSubtitles.value = PlayerPrefs.GetInt(PlayerPrefsKeys.SUBTITLES_ENABLED, 1) == 1;
            if (_toggleVSync != null) { _toggleVSync.value = PlayerPrefs.GetInt(PlayerPrefsKeys.V_SYNC, 1) == 1; QualitySettings.vSyncCount = _toggleVSync.value ? 1 : 0; }
            if (_toggleCameraShake != null) _toggleCameraShake.value = PlayerPrefs.GetInt(PlayerPrefsKeys.CAMERA_SHAKE, 1) == 1;
            if (_toggleMotionBlur != null) _toggleMotionBlur.value = PlayerPrefs.GetInt(PlayerPrefsKeys.MOTION_BLUR, 0) == 1;
            if (_toggleInvertY != null) _toggleInvertY.value = PlayerPrefs.GetInt(PlayerPrefsKeys.INVERT_Y_AXIS, 0) == 1;
            if (_toggleGamepadVibration != null) _toggleGamepadVibration.value = PlayerPrefs.GetInt(PlayerPrefsKeys.GAMEPAD_VIBRATION, 1) == 1;

            if (_dropdownResolution != null)
            {
                _dropdownResolution.choices = new List<string>(SettingsDefaults.RESOLUTIONS);
                _dropdownResolution.value = $"{Screen.width}x{Screen.height}";
            }
            if (_dropdownWindowMode != null)
            {
                _dropdownWindowMode.choices = new List<string>(SettingsDefaults.WINDOW_MODES);
                _dropdownWindowMode.value = GetCurrentWindowMode();
            }
            if (_dropdownQuality != null)
            {
                _dropdownQuality.choices = new List<string>(SettingsDefaults.QUALITY_LEVELS);
                int ql = PlayerPrefs.GetInt(PlayerPrefsKeys.QUALITY_LEVEL, PlayerPrefsKeys.Defaults.QUALITY_LEVEL);
                ql = Mathf.Clamp(ql, 0, SettingsDefaults.QUALITY_LEVELS.Length - 1);
                _dropdownQuality.value = SettingsDefaults.QUALITY_LEVELS[ql];
            }
            if (_dropdownTextSize != null)
            {
                _dropdownTextSize.choices = new List<string>(SettingsDefaults.TEXT_SIZES);
                _dropdownTextSize.value = PlayerPrefs.GetString(PlayerPrefsKeys.TEXT_SIZE, PlayerPrefsKeys.Defaults.TEXT_SIZE);
            }
            if (_dropdownInputDevice != null)
            {
                _dropdownInputDevice.choices = new List<string>(SettingsDefaults.INPUT_DEVICES);
                _dropdownInputDevice.value = PlayerPrefs.GetString(PlayerPrefsKeys.INPUT_DEVICE, PlayerPrefsKeys.Defaults.INPUT_DEVICE);
            }
        }

        private void CheckSaveFile()
        {
            bool hasSave = PlayerPrefs.GetInt(PlayerPrefsKeys.HAS_SAVE, 0) == 1;
            if (_btnContinue != null)
            {
                _btnContinue.SetEnabled(hasSave);
                if (hasSave)
                {
                    int day = PlayerPrefs.GetInt(PlayerPrefsKeys.SAVE_DAY_NUMBER, 1);
                    _btnContinue.text = $"{LocalizationKeys.Get(LocalizationKeys.BUTTON_CONTINUE)} (Day {day})";
                }
            }
        }

        private void SetScreen(ScreenState state)
        {
            _currentScreen = state;

            if (_settingsPanel != null) _settingsPanel.style.display = state == ScreenState.Settings ? DisplayStyle.Flex : DisplayStyle.None;
            if (_creditsPanel != null) _creditsPanel.style.display = state == ScreenState.Credits ? DisplayStyle.Flex : DisplayStyle.None;
            if (_dialogOverlay != null) _dialogOverlay.style.display = state == ScreenState.QuitConfirm ? DisplayStyle.Flex : DisplayStyle.None;

            if (state == ScreenState.MainMenu)
                FocusButton(0);
            else if (state == ScreenState.Credits)
                FocusButton(-1);
            else if (state == ScreenState.QuitConfirm)
                FocusButton(3);
        }

        private void NavigateMenu(int direction)
        {
            if (_currentScreen != ScreenState.MainMenu) return;
            if (_menuButtons == null) return;

            _focusedButtonIndex = (_focusedButtonIndex + direction + _menuButtons.Count) % _menuButtons.Count;
            FocusButton(_focusedButtonIndex);
        }

        private void FocusButton(int index)
        {
            if (_menuButtons == null) return;
            if (index >= 0 && index < _menuButtons.Count && _menuButtons[index] != null)
                _menuButtons[index].Focus();
        }

        private void ActivateFocusedButton()
        {
            if (_menuButtons == null) return;
            if (_focusedButtonIndex >= 0 && _focusedButtonIndex < _menuButtons.Count && _menuButtons[_focusedButtonIndex] != null)
                _menuButtons[_focusedButtonIndex].SendEvent(new ClickEvent());
        }

        private void HandleEscape()
        {
            switch (_currentScreen)
            {
                case ScreenState.MainMenu:
                    SetScreen(ScreenState.QuitConfirm);
                    break;
                case ScreenState.Settings:
                    CloseSettings();
                    break;
                case ScreenState.Credits:
                    CloseCredits();
                    break;
                case ScreenState.QuitConfirm:
                    CancelQuit();
                    break;
            }
        }

        private void ApplyUiScale(float scale)
        {
            if (_root != null)
                _root.style.scale = new StyleScale(new UnityEngine.Vector3(scale, scale, 1f));
        }

        private void ShowSettingsTab(string tabId)
        {
            if (_tabAudio != null) _tabAudio.style.display = tabId == "audio" ? DisplayStyle.Flex : DisplayStyle.None;
            if (_tabVideo != null) _tabVideo.style.display = tabId == "video" ? DisplayStyle.Flex : DisplayStyle.None;
            if (_tabControls != null) _tabControls.style.display = tabId == "controls" ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnNewGame()
        {
            OnNewGameRequested?.Invoke();
            Debug.Log("[MainMenu] New Game requested");
        }

        private void OnContinue()
        {
            if (_btnContinue != null && _btnContinue.enabledInHierarchy)
            {
                OnContinueRequested?.Invoke();
                Debug.Log("[MainMenu] Continue requested");
            }
        }

        private void OnSettings()
        {
            SetScreen(ScreenState.Settings);
            ShowSettingsTab("audio");
        }

        private void OnCredits()
        {
            SetScreen(ScreenState.Credits);
        }

        private void OnQuit()
        {
            SetScreen(ScreenState.QuitConfirm);
        }

        private void CloseSettings()
        {
            SetScreen(ScreenState.MainMenu);
        }

        private void CloseCredits()
        {
            SetScreen(ScreenState.MainMenu);
        }

        private void ConfirmQuit()
        {
            OnQuitRequested?.Invoke();
            Debug.Log("[MainMenu] Quit confirmed");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void CancelQuit()
        {
            SetScreen(ScreenState.MainMenu);
        }

        private void OnResolutionChanged(string value)
        {
            var parts = value.Split('x');
            if (parts.Length == 2 && int.TryParse(parts[0], out int w) && int.TryParse(parts[1], out int h))
            {
                Screen.SetResolution(w, h, Screen.fullScreenMode);
                PlayerPrefs.SetInt(PlayerPrefsKeys.RESOLUTION_WIDTH, w);
                PlayerPrefs.SetInt(PlayerPrefsKeys.RESOLUTION_HEIGHT, h);
            }
        }

        private void OnWindowModeChanged(string mode)
        {
            Screen.fullScreenMode = mode switch
            {
                "Fullscreen" => FullScreenMode.ExclusiveFullScreen,
                "Borderless" => FullScreenMode.FullScreenWindow,
                _ => FullScreenMode.Windowed
            };
            PlayerPrefs.SetString(PlayerPrefsKeys.WINDOW_MODE, mode);
        }

        private void OnQualityChanged(string value)
        {
            int index = Array.IndexOf(SettingsDefaults.QUALITY_LEVELS, value);
            if (index >= 0)
            {
                QualitySettings.SetQualityLevel(index, true);
                PlayerPrefs.SetInt(PlayerPrefsKeys.QUALITY_LEVEL, index);
            }
        }

        private string GetCurrentWindowMode()
        {
            return Screen.fullScreenMode switch
            {
                FullScreenMode.ExclusiveFullScreen => "Fullscreen",
                FullScreenMode.FullScreenWindow => "Borderless",
                _ => "Windowed"
            };
        }
    }
}