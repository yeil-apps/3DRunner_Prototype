using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Text _scoreText; // объекты для отображения прогресса
    [SerializeField] private Text _gemsText;

    private void Start()
    {
        Time.timeScale = 1;
        _scoreText.text = DataManager.Instance.RecordScore.ToString();
        _gemsText.text = DataManager.Instance.GemsCount.ToString();

    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }


}
