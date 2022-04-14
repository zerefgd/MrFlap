using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource _musicSource, _effectSource;

    [SerializeField] private AudioClip _clickSound;

    private bool isMusicMuted;
    private bool IsMusicMuted
    {
        get
        {
            isMusicMuted = PlayerPrefs.HasKey(Constants.Settings.SETTINGS_MUSIC) ?
                (PlayerPrefs.GetInt(Constants.Settings.SETTINGS_MUSIC) == 0) : false;
            return isMusicMuted;
        }
        set
        {
            isMusicMuted = value;
            PlayerPrefs.SetInt(Constants.Settings.SETTINGS_MUSIC, isMusicMuted ? 0 : 1);
        }
    }

    private bool isEffectMuted;
    private bool IsEffectMuted
    {
        get
        {
            isEffectMuted = PlayerPrefs.HasKey(Constants.Settings.SETTINGS_SOUND) ?
                (PlayerPrefs.GetInt(Constants.Settings.SETTINGS_SOUND) == 0) : false;
            return isEffectMuted;
        }
        set
        {
            isEffectMuted = value;
            PlayerPrefs.SetInt(Constants.Settings.SETTINGS_SOUND, IsEffectMuted ? 0 : 1);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayerPrefs.SetInt(Constants.Settings.SETTINGS_MUSIC, IsMusicMuted ? 0 : 1);
        PlayerPrefs.SetInt(Constants.Settings.SETTINGS_SOUND, IsEffectMuted ? 0 : 1);
        _effectSource.mute = IsEffectMuted;
        _musicSource.mute = IsMusicMuted;
        if (!isMusicMuted && !_musicSource.isPlaying)
        {
            _musicSource.Play();
        }
        AddButtonSounds();
    }

    public void AddButtonSounds()
    {
        var buttons = FindObjectsOfType<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() =>
            {
                PlaySound(_clickSound);
            });
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (IsEffectMuted) return;
        _effectSource.PlayOneShot(clip);
    }

    public void ToggleEffect()
    {
        _effectSource.mute = IsEffectMuted;
    }

    public void ToggleMusic()
    {
        _musicSource.mute = IsMusicMuted;
        if (!isMusicMuted && !_musicSource.isPlaying)
            _musicSource.Play();
    }
}