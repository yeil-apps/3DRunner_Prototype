using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{    
    [SerializeField] private float _sizeReduce = 3f; // во сколько раз уменьшить персонажа?
    [SerializeField] private float _sizeReductionTime = 0.5f; // время в уменьшенном состоянии
    [SerializeField] private float _speedForPlayerSizeChange = 50f; // скорость уменьшения

    [SerializeField] private float _jumpPower = 10f; // сила прыжка
    [SerializeField] private float _sideSpeed = 5f; // скорость передвижения в сторону
    [SerializeField] private float _laneDistanse = 5f; // расстояние между линиями


    private Rigidbody _playerRigidBody;
    private Animator _playerAnimator;
    private GameManager _gameManager;

    // настройка линий для перемещения персонажа
    private int _currentLaneNumber = 1; // текущая линия 
    private int _laneCount = 2; // количество линий
    private float _firstLanePosition; // позиция самой левой линии 

    private bool _characterSizeIsReduced; // уменьшен ли размер персонажа 
    private bool _isGrounded() // персонаж на земле?
    {
        return Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f);
    }

    [SerializeField] private float _fallingSpeed = 0.3f; // скорость по Y которая добавляется к персонажу в воздухе

    void Start()
    {
        _firstLanePosition = -_laneDistanse; // исправить если линий больше 3х, или центральная линия не в позиции 0 по X

        _playerRigidBody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<Animator>();
        _gameManager = FindObjectOfType<GameManager>();

        SwipeController.SwipeEvent += CheckInput; 
    }

    void Update()
    {
        // плавное перемещение гирока между линиями
        Vector3 newPose = transform.position;
        newPose.x = Mathf.Lerp(newPose.x, _firstLanePosition + (_currentLaneNumber * _laneDistanse), Time.deltaTime * _sideSpeed);
        transform.position = newPose;
    }

    private void FixedUpdate()
    {
        if (!_isGrounded())
        {
            _playerRigidBody.velocity = new Vector3(0, _playerRigidBody.velocity.y - _fallingSpeed, 0);
        } else if (_isGrounded() && !_characterSizeIsReduced) // проигрывание анимации падения и обнуление velocity
        {
            _playerAnimator.SetTrigger("Landed");
            _playerRigidBody.velocity = Vector3.zero;
        }
    }
    private void CheckInput(SwipeController.SwipeType type) //действия при различных свайпах
    {
        if (type == SwipeController.SwipeType.Up && _isGrounded())
        {
            Jump();
        }

        if (type == SwipeController.SwipeType.Left || type == SwipeController.SwipeType.Right)
        {
            if (type == SwipeController.SwipeType.Left)
                _currentLaneNumber--;
            else
                _currentLaneNumber++;

            _currentLaneNumber = Mathf.Clamp(_currentLaneNumber, 0, _laneCount);
        }

        if (type == SwipeController.SwipeType.Down && !_characterSizeIsReduced)
        {
            StartCoroutine(DoReduce());
        }
    }
    private void Jump()
    {
        _playerRigidBody.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        _playerAnimator.SetTrigger("Jump");   
    }


    IEnumerator DoReduce()
    {
        _characterSizeIsReduced = true;
        Vector3 startPlayerScale = transform.localScale; //начальный размер персонажа
        Vector3 reducedPlayerScale = transform.localScale / _sizeReduce; // уменьшенный 

        while (transform.localScale != reducedPlayerScale) // уменьшение персонажа
        {
            float deltaX = Mathf.Lerp(transform.localScale.x, reducedPlayerScale.x, Time.deltaTime * _speedForPlayerSizeChange);
            float deltaY = Mathf.Lerp(transform.localScale.y, reducedPlayerScale.y, Time.deltaTime * _speedForPlayerSizeChange);
            float deltaZ = Mathf.Lerp(transform.localScale.z, reducedPlayerScale.z, Time.deltaTime * _speedForPlayerSizeChange);
            transform.localScale = new Vector3 (deltaX,deltaY,deltaZ);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(_sizeReductionTime); // время в уменьшенном состоянии _sizeReductionTime

        while (transform.localScale != startPlayerScale) // увеличение до исходных размеров персонажа
        {
            float deltaX = Mathf.Lerp(transform.localScale.x, startPlayerScale.x, Time.deltaTime * _speedForPlayerSizeChange);
            float deltaY = Mathf.Lerp(transform.localScale.y, startPlayerScale.y, Time.deltaTime * _speedForPlayerSizeChange);
            float deltaZ = Mathf.Lerp(transform.localScale.z, startPlayerScale.z, Time.deltaTime * _speedForPlayerSizeChange);
            transform.localScale = new Vector3(deltaX, deltaY, deltaZ);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _characterSizeIsReduced = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Gem>())
        {
            _gameManager.TakeGem(other.GetComponent<Gem>().GemsCountInObj);
            if (other.gameObject.GetComponent<EffectsSpawn>())
                other.gameObject.GetComponent<EffectsSpawn>().SpawnEffect();
            Destroy(other.gameObject);
        }
        else if(other.gameObject.GetComponent<Obstacle>())
        {
            PlayerDie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Obstacle>())
        {
            PlayerDie();
        } 
    }

    public void UnsubscribeFromEvent() // отписка от события для повтороной инициализации при перезапуске уровня
    {
        SwipeController.SwipeEvent -= CheckInput;
    }

    private void PlayerDie()
    {
        UnsubscribeFromEvent();
        _playerAnimator.SetTrigger("Die");
        _gameManager.GameOver();
    }

}
