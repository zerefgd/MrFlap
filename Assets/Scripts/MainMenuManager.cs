using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsPanel, _soundOverlay, _musicOverlay;

    [SerializeField]
    private TMPro.TMP_Text _gamesPlayedText, _highScoreText;

    private void Awake()
    {
        _settingsPanel.SetActive(false);
        _gamesPlayedText.text =
            (PlayerPrefs.HasKey(Constants.SaveData.GAMES_PLAYED) ? PlayerPrefs.GetInt(Constants.SaveData.GAMES_PLAYED) : 0)
            .ToString();
        _highScoreText.text =
            (PlayerPrefs.HasKey(Constants.SaveData.HIGHSCORE) ? PlayerPrefs.GetInt(Constants.SaveData.HIGHSCORE) : 0)
            .ToString();
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
        RefreshSettings();
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void ToggleMusic()
    {
        int temp =
            (PlayerPrefs.HasKey(Constants.Settings.SETTINGS_MUSIC) ?
            PlayerPrefs.GetInt(Constants.Settings.SETTINGS_MUSIC) : 1);
        temp = temp == 1 ? 0 : 1;
        PlayerPrefs.SetInt(Constants.Settings.SETTINGS_MUSIC, temp);
        RefreshSettings();
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSound()
    {
        int temp =
            (PlayerPrefs.HasKey(Constants.Settings.SETTINGS_SOUND) ?
            PlayerPrefs.GetInt(Constants.Settings.SETTINGS_SOUND) : 1);
        temp = temp == 1 ? 0 : 1;
        PlayerPrefs.SetInt(Constants.Settings.SETTINGS_SOUND, temp);
        RefreshSettings();
        AudioManager.instance.ToggleEffect();
    }

    private void RefreshSettings()
    {
        bool tempBool =
            (PlayerPrefs.HasKey(Constants.Settings.SETTINGS_MUSIC) ?
            PlayerPrefs.GetInt(Constants.Settings.SETTINGS_MUSIC) : 1) == 0;
        _musicOverlay.SetActive(tempBool);

        tempBool =
            (PlayerPrefs.HasKey(Constants.Settings.SETTINGS_SOUND) ?
            PlayerPrefs.GetInt(Constants.Settings.SETTINGS_SOUND) : 1) == 0;
        _soundOverlay.SetActive(tempBool);
    }
}
