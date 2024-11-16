using UnityEngine;

namespace Setting
{
    public class Settings : MonoBehaviour
    {
        #region Variable
        [SerializeField] private UnityEngine.UI.Button settingsButton;
        [SerializeField] private UnityEngine.UI.Button vibrateButton;
        [SerializeField] private UnityEngine.UI.Button soundButton;
        [SerializeField] private CanvasGroup vibrateAlpha;
        [SerializeField] private CanvasGroup soundAlpha;
        [SerializeField] private GameObject settingsPanel;
        [HideInInspector] public static int vibrateActive;
        [HideInInspector] public static int soundActive;
        #endregion
        void Start()
        {
            ButtonsControl();
            VibrateControl();
            SoundControl();
        }
        public void OpenSettings() => settingsPanel.SetActive(!settingsPanel.activeSelf);
        private void VibrateButton()
        {
            if (vibrateActive == 1)
                vibrateActive = 0;
            else
                vibrateActive = 1;
            PlayerPrefs.SetInt("vibrateStatus", vibrateActive);
            vibrateAlpha.alpha = ((float)vibrateActive + .25f);
        }
        private void SoundButton()
        {
            if (soundActive == 1)
                soundActive = 0;
            else
                soundActive = 1;
            PlayerPrefs.SetInt("soundStatus", soundActive);
            soundAlpha.alpha = ((float)soundActive + .25f);

            GameObject.FindWithTag("Music").GetComponent<BackgroundMusic>().MusicEnabled();
        }
        private void SoundControl()
        {
            soundActive = PlayerPrefs.GetInt("soundStatus", 1);
            soundAlpha.alpha = ((float)soundActive + .25f);
        }
        private void VibrateControl()
        {
            vibrateActive = PlayerPrefs.GetInt("vibrateStatus", 1);
            vibrateAlpha.alpha = ((float)vibrateActive + .25f);
        }
        private void ButtonsControl()
        {
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(OpenSettings);

            vibrateButton.onClick.RemoveAllListeners();
            vibrateButton.onClick.AddListener(VibrateButton);

            soundButton.onClick.RemoveAllListeners();
            soundButton.onClick.AddListener(SoundButton);
        }
    }
}