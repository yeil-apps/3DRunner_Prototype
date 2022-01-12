using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockMove : MonoBehaviour
{
    //������� ����� �� ��������� ��������� � GameManager
    private GameManager _gameManager;
    private Vector3 _moveDirection;
    
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _moveDirection = -Vector3.forward;
    }

    void Update()
    {
        transform.Translate(_moveDirection * Time.deltaTime * _gameManager.MoveSpeed);
    }
}
