using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMethods : MonoBehaviour
{
    private GameManager _gameManager;
    private PlayerController _playerController;

    [SerializeField] private Text _scoreText; // отображения очков в процессе игры
    [SerializeField] private Text _gemsCountText; // отображение количества алмазов в процессе игры
    [SerializeField] private GameObject _gameOverPanel; // меню проигрыша
    [SerializeField] private GameObject _pausePanel; // меню паузы
    [SerializeField] private GameObject _newRecordPanel; // панель, появляющаяся при новом рекорде
    [SerializeField] private Text _gameOverScoreText; // отображение очков в меню проигрыша
    [SerializeField] private Text _gameOverRecordText; //отображение рекорда в меню проигрыша

    void Start()
    {
        _gameOverPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _newRecordPanel.SetActive(false);

        _gameManager = FindObjectOfType<GameManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (!_gameManager.IsGameOver)
        {
            _scoreText.text = ((int)_gameManager.Score).ToString();
        }
        if (Input.GetKey(KeyCode.Escape) && !_gameManager.IsPaused)
        {
            if (!_gameManager.IsPaused)
            {
                PauseOn();
            }
        }
    }

 
    public IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(2f);

        if (_gameManager.Score > DataManager.Instance.RecordScore) // показать панель с поздравлением если рекорд побит
        {
            DataManager.Instance.RecordScore = (int)_gameManager.Score;
            _newRecordPanel.SetActive(true);
        }

        _gameOverScoreText.text = ((int)_gameManager.Score).ToString(); 
        _gameOverRecordText.text = DataManager.Instance.RecordScore.ToString();
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void ToMenu()
    {
        _playerController.UnsubscribeFromEvent();
        DataManager.Instance.PlusGems(_gameManager.Gems);
        SceneManager.LoadScene(0); // сцена 0 - меню
    }

    public void RestartGame()
    {
        DataManager.Instance.PlusGems(_gameManager.Gems);
        SceneManager.LoadScene(1); // сцена 1 - игра
    }

    public void PauseOff()
    {
        _gameManager.IsPaused = false;
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
    }

    public void PauseOn()
    {
        _gameManager.IsPaused = true;
        Time.timeScale = 0;
        _pausePanel.SetActive(true);
    }
    public void ChangeGemCount(int currentCount)
    {
        _gemsCountText.text = currentCount.ToString();
    }
}
