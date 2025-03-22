using System;
using UnityEngine;

public class MovementCharacter : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public Transform ceilingCheck;
    public LayerMask groundMask;
    public Camera playerCamera;
    
    
    // Вспомогутельные переменные 
    public float _currentSpeed; // Храненилище скорости
    public bool stat;
    
    // Предел скорости.
    [Header("Пределы скоростей")]
    public float MaxSpeed = 1f;
    public float SprintSpeed = 12f; // Ускорение
    public float RunningSpeed = 10f; // W
    public float WalkingSpeed = 8f; // A S
    public float BackSpeed = 5f; // D
    public float SpeedCrouch = 2f; // CTRL
    
    // Ускорение
    [Header("Параметры разгона с нуля")]
    public float FirstOverclocking  = 4f; 
    public float FirstАcceleration = 6f;
    public float SecondАcceleration = 10f;
    public float Deceleration = 100f; // Обнуление скорости
    
    public float CharectorHeight { get; set; }
    public float CellingHeight { get; set; }
    
    // Гравитация
    public float _gravity = -9.8f;
    public float _groundDistance = 0.4f;
    public bool _isGrounded;
    public Vector3 _velocity;
    
    // Переменные для прыжка 
    public float _jumpH = 1f;
    
    
    // Переменные для инерции
    public float _horizontalInertia = 0.5f; // Сила инерции
    public float InertiaSpeed = 6f; // Скорость инерции
    public Vector3 _inertiaVelocity;
    
    //Подкат
    public bool isTackle = false;
    
    
    // Пресидание
    public bool isCrouching = false;
    public float crouchHeight = 0.5f;
    public float crouchSpeed = 3.5f;
    public float crouchTransitionSpeed = 10f;
    private float originalHeight;
    public float crouchCameraOffset = -0.5f;
    private Vector3 cameraStandPosition;
    private Vector3 cameraCrouchPosition;
    private Vector3 transformStandLocalScale;
    private Vector3 transformCrouchLocalScaleStandart;
    
    
    
    
    
    
    
    void Start()
    {
        CharectorHeight = controller.height;
        CellingHeight = ceilingCheck.position.y;
        
        
        originalHeight = controller.height;

        // Define camera positions for standing and crouching
        cameraStandPosition = playerCamera.transform.localPosition;
        cameraCrouchPosition = cameraStandPosition + new Vector3(0, crouchCameraOffset, 0);
        transformStandLocalScale = transform.localScale;
        transformCrouchLocalScaleStandart = new Vector3(transformStandLocalScale.x, crouchHeight, transformStandLocalScale.z);
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        // Проверка Гравитации 
        _isGrounded = Physics.CheckSphere(groundCheck.position, _groundDistance,groundMask);
        
        // Передвижение
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.W))
        {
            MaxSpeed = RunningSpeed;
            stat = true;
        }
    
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            MaxSpeed = WalkingSpeed;
            stat = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            MaxSpeed = BackSpeed;
            stat = true;
        }
    
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MaxSpeed = SprintSpeed;
            stat = true;
        }
        
        
        
        Vector3 move = transform.right * x + transform.forward * y;
        
        // Подкат : Приседание 
        if (Input.GetKey(KeyCode.C))
        {
            controller.height = crouchHeight;
            MaxSpeed = crouchSpeed;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraCrouchPosition, crouchTransitionSpeed * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, transformCrouchLocalScaleStandart, crouchTransitionSpeed * Time.deltaTime); 
        }
        else
        {
            if (!Physics.CheckSphere(ceilingCheck.position, _groundDistance, groundMask))
            {
                controller.height = originalHeight;
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraStandPosition, crouchTransitionSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, transformStandLocalScale, crouchTransitionSpeed * 4 * Time.deltaTime);   
            }
        }
        
        // Плавное изменение скорости
        if (_currentSpeed < MaxSpeed && stat == true)
        {
            if (_currentSpeed < FirstOverclocking)
            {
                _currentSpeed += FirstАcceleration * Time.deltaTime;  
            }
            else
            {
                _currentSpeed += SecondАcceleration * Time.deltaTime;
            }
        }
        else
        {
            _currentSpeed -= Deceleration * Time.deltaTime; // Обнуление скорости
        } 
        
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, MaxSpeed);
        
        // Прыжок
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpH * -2f * _gravity);
            
            move = transform.right * x + transform.forward * y; 
            _inertiaVelocity = move * _currentSpeed * _horizontalInertia;
        }
        
        if (_isGrounded)
        {
            _inertiaVelocity = Vector3.zero;
            // Передвижение
            if (stat == false)
            {
                controller.Move(move * (4f * Time.deltaTime));
            }
            else
            {
                controller.Move(move * (_currentSpeed * Time.deltaTime));   
            }        
            
            _inertiaVelocity = move;
        }
        else
        {
            controller.Move(_inertiaVelocity * (InertiaSpeed * Time.deltaTime));
            controller.Move(move * (4f * Time.deltaTime));
        }
        
        
        //Гравитация
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
            //transform.position = new Vector3(transform.position.x, PlayerPositionY, transform.position.z);
        }
        
        // Применение гравитации
        _velocity.y += _gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);  
        
        // Отменяем все Bool
        stat = false;
    }
}
