using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _endPanel, _startButton;

    [SerializeField]
    private TMPro.TMP_Text _endMessageText, _highScoreText;

    private void Awake()
    {
        _endPanel.SetActive(false);
        _startButton.SetActive(true);
        AudioManager.instance.AddButtonSounds();
    }

    private void OnEnable()
    {
        EventManager.StartListening(Constants.EventNames.GAME_OVER, GameOver);
        EventManager.StartListening(Constants.EventNames.UPDATE_SCORE, UpdateScore);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Constants.EventNames.GAME_OVER, GameOver);
        EventManager.StopListening(Constants.EventNames.UPDATE_SCORE, UpdateScore);
    }

    private void GameOver(Dictionary<string,object> message)
    {
        _endPanel.SetActive(true);
    }

    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void GameStart()
    {
        _startButton.SetActive(false);
        EventManager.TriggerEvent(Constants.EventNames.GAME_START,null);
    }

    private void UpdateScore(Dictionary<string,object> message)
    {
        int score = (int)message[Constants.EventParams.SCORE];
        int highScore = PlayerPrefs.HasKey(Constants.SaveData.HIGHSCORE) 
            ? PlayerPrefs.GetInt(Constants.SaveData.HIGHSCORE) :  0;

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(Constants.SaveData.HIGHSCORE, highScore);
            _endMessageText.text = "SUCCESS!";
            _highScoreText.text = "HighScore : " + highScore.ToString();
        }
        else
        {
            _endMessageText.text = "FAIL!";
            _highScoreText.text = "HighScore : " + highScore.ToString();
        }

        int games = PlayerPrefs.HasKey(Constants.SaveData.GAMES_PLAYED)
            ? PlayerPrefs.GetInt(Constants.SaveData.GAMES_PLAYED) : 0;
        games++;
        PlayerPrefs.SetInt(Constants.SaveData.GAMES_PLAYED, games);
    }

}
