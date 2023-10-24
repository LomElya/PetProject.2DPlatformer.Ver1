using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEventBus;

namespace UI.Dialogs
{
    public class SettingsWindow : DialogCore
    {
        [Header("Основное окно")]
        [SerializeField] private GameObject _settingsWindow;

        [Header("Настройки")]
        [SerializeField] private Dropdown _resolutionDropDown;
        [SerializeField] private Dropdown _qualityDropDown;
        [SerializeField] private Toggle _fullScreenToggle;
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private AudioSource _audioSource;
        
        [Header("Кнопки")]
        [SerializeField] private Button _OKButton;
        [SerializeField] private Button _cancelButton;

        private Resolution[] _resolutions;

        private float _audioVolume;

        private EventBus _eventBus;

        private void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            OverwriteResolution();
            AddListener();
        }

        private void OverwriteResolution()
        {
            _resolutionDropDown.ClearOptions();
            List<string> options = new List<string>();
            _resolutions = Screen.resolutions;
            int currentResolutionIndex = 0;

            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + "x" + _resolutions[i].height + " " + _resolutions[i].refreshRateRatio + "Hz";
                options.Add(option);
                if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }
            _resolutionDropDown.AddOptions(options);
            _resolutionDropDown.RefreshShownValue();

            LoadSettings(currentResolutionIndex);
        }

        private void AddListener()
        {
            _OKButton.onClick.AddListener(OnOKButtonClick);
            _cancelButton.onClick.AddListener(OnCancelButtonClick);

            _resolutionDropDown.onValueChanged.AddListener(SetResolution);
            _qualityDropDown.onValueChanged.AddListener(SetQuality);

            _fullScreenToggle.onValueChanged.AddListener(SetFullScreen);

            _volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        private void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        private void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        private void SetFullScreen(bool isFullScreed)
        {
            Screen.fullScreen = isFullScreed;
        }

        private void SetVolume(float volume)
        {
            AudioUtility.SetMasterVolume(volume);
            Debug.Log("Звук изменился: " +  AudioUtility.GetMasterVolume() + "\nЗвук: "+ volume);
        }

        private void LoadSettings(int currentResolutionIndex)
        {
            if (PlayerPrefs.HasKey("QualitySettingPreference"))
                _qualityDropDown.value = PlayerPrefs.GetInt("QualitySettingPreference");
            else
                _qualityDropDown.value = 6;

            if (PlayerPrefs.HasKey("ResolutionPreference"))
                _resolutionDropDown.value = PlayerPrefs.GetInt("ResolutionPreference");
            else
                _resolutionDropDown.value = currentResolutionIndex;

            if (PlayerPrefs.HasKey("FullScreenPreference"))
                Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreenPreference"));
            else
                Screen.fullScreen = true;

            if (PlayerPrefs.HasKey("VolumePreference"))
            {
                
                AudioUtility.SetMasterVolume(PlayerPrefs.GetFloat("VolumePreference"));
                _volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
            }
            else
            {
                AudioUtility.SetMasterVolume(1f);
                _volumeSlider.value = 1f;
            }
        }

        private void OnOKButtonClick()
        {
            var confirm = DialogsManager.ShowDialog<ConfirmWindow>();
            confirm.Init("Сохранить все изменения?", () =>
            {
                PlayerPrefs.SetInt("QualitySettingPreference", _qualityDropDown.value);
                PlayerPrefs.SetInt("ResolutionPreference", _resolutionDropDown.value);
                PlayerPrefs.SetInt("FullScreenPreference", System.Convert.ToInt32(Screen.fullScreen));
                PlayerPrefs.SetFloat("VolumePreference", _volumeSlider.value);

                Hide();
            });
        }

        private void OnCancelButtonClick()
        {
            Hide();
        }

    }
}

