using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameManager _gameManager;
    private Vector3 _moveDirection;
    private Transform _playerTransform;

    [SerializeField] private float _distanceToStartMove;
    private bool _playerPassed;
    private bool _isMove = false;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
        _moveDirection = Vector3.forward;
    }


    void Update()
    {
        if (!_isMove && Vector3.Distance(_playerTransform.position, transform.position) < _distanceToStartMove)
        {
            _isMove = true;
            transform.parent = null;
        }
        if (_isMove)
        {
            transform.Translate(_moveDirection * Time.deltaTime * _gameManager.EnemyMoveSpeed);
            if (Vector3.Distance(_playerTransform.position, transform.position) > _distanceToStartMove)
            {
                Destroy(transform.gameObject);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distanceToStartMove);
    }

}
