using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public float ScoreMultiplier
    {
        get { return _scoreMultiplier; }
        set
        {
            if (value > 0)
                _scoreMultiplier = value;
        }
    }
    [SerializeField] private float _scoreMultiplier = 2f; // ��������� ������ ���������� ����� � ����� ������ �������
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set
        {
            if (_moveSpeed > 0)
                _moveSpeed = value;
        }
    }
    [SerializeField] private float _moveSpeed = 5f; // �������� ������ 

    public float EnemyMoveSpeed
    {
        get { return _enemyMoveSpeed; }
    }
    [SerializeField] private float _enemyMoveSpeed = 10f; // �������� ������� ������

    public bool IsPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }
    private bool _isPaused;
    public float Score
    {
        get { return _score; }
    }
    private float _score; // ������� ���������� ����� 

    public int Gems
    {
        get { return _gems; }
    }
    private int _gems; // ������� ���������� �������

    public bool IsGameOver
    {
        get { return _isGameOver; }
    }
    private bool _isGameOver;

    private RoadBlocksSpawner _roadSpawner;
    private UIMethods _uiMethods;

    void Start()
    {
        Time.timeScale = 1;
        _roadSpawner = FindObjectOfType<RoadBlocksSpawner>();
        _uiMethods = FindObjectOfType<UIMethods>();

    }

    void Update()
    {
        if (!_isGameOver)
        {
            _score += Time.deltaTime * _scoreMultiplier;
        }
    }
    public void TakeGem(int count)
    {
        if (count > 0)
        {
            _gems += count;
        }
        _uiMethods.ChangeGemCount(_gems); // ����������� ���������� �� ����������
    }

    public void GameOver() // ��������� �������� �������� � ������ ���� ���������
    {
        _isGameOver = true;
        _moveSpeed = 0;
        _enemyMoveSpeed = 0;

        StartCoroutine(_uiMethods.ShowGameOverPanel());
    }
}
