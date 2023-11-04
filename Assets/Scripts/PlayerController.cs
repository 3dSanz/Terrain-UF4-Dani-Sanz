using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
 private CharacterController _controller;
    private float _horizontal;
    private float _vertical;
    private Transform _camera;

    //variables para velocidad, salto y gravedad
    [SerializeField] private float _playerSpeed = 5;
    [SerializeField] private float _jumpHeight = 1;
    private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    //variables para rotacion
    private float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = 0.1f;

    //varibles para sensor
    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isGrounded;

    //animaciones
    private Animator _anim;

    private GameManager _gameManager;

    [SerializeField] private int _hp = 10;
    

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _anim = GetComponentInChildren<Animator>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        

        if(Input.GetButton("Fire2") && _gameManager._gameOver == false)
        {
            AimMovement();
        }else if (_gameManager._gameOver == false)
        {
            Movement();
        } 
        
        if (_gameManager._gameOver == false)
        {
            Jump();
        }
        _anim.SetBool("IsJump", !_isGrounded);
        
        
        TakeDamage();
    }

    void TakeDamage()
    {

        if(_hp <= 0 &&  _gameManager._gameOver == false)
        {
                _gameManager.GameOver();
                _anim.SetTrigger("IsDead");

        }

        if(Input.GetKey(KeyCode.J) &&  _gameManager._gameOver == false)
        {
            _gameManager.GameOver();
            _anim.SetTrigger("IsDead");
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.layer == 8 && _gameManager._gameOver == false)
        {
            _gameManager.GameOver();
            _anim.SetTrigger("IsDead");
        }
    }


    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _anim.SetFloat("VelX", 0);
        _anim.SetFloat("VelZ", direction.magnitude);

        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle,0) * Vector3.forward;

            _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);


        }
    }

        void AimMovement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _anim.SetFloat("VelX", _horizontal);
        _anim.SetFloat("VelZ", _vertical);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camera.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        if(direction != Vector3.zero)
        {
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle,0) * Vector3.forward;

            _controller.Move(moveDirection.normalized * _playerSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {

        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = 0;
        }

        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;
        
        _controller.Move(_playerGravity * Time.deltaTime);
    }
}
