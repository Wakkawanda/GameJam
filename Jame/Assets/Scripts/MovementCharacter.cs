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
    public float WalkingSpeed = 8f; // A D
    public float BackSpeedAD = 6f; // A or D + S
    public float BackSpeed = 5f; // D
    public float SpeedCrouch = 3.5f; // CTRL : C
    
    // Ускорение
    [Header("Параметры разгона с нуля")]
    public float FirstOverclocking  = 4f; 
    public float FirstАcceleration = 6f;
    public float SecondАcceleration = 10f;
    public float Deceleration = 100f; // Обнуление скорости
    
    // Гравитация
    public float gravityIncreaseRate = 2f; // Скорость увеличения гравитации
    public float maxGravity = -50f; // Максимальное значение гравитации
    private float _fallTimer = 0f; // Таймер падения
    
    private float _currentGravity; // Текущее значение гравитации
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
    public float RngeTackle = 1f;
    public float Distance;
    public float Timet = 10f;
    
    
    // Пресидание
    public bool isCrouch;
    public float crouchHeight = 0.5f; // Высота в седячем положение
    public float crouchTransitionSpeed = 10f;
    private float originalHeight;
    public float crouchCameraOffset = -0.5f;
    private Vector3 cameraStandPosition;
    private Vector3 cameraCrouchPosition;
    private Vector3 transformStandLocalScale;
    private Vector3 transformCrouchLocalScaleStandart;
    
    // Зажерка при подкате 
    float time = 0f;
    
    
    
    
    
    void Start()
    {
        // Блокировка курсора
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentGravity = _gravity; // Устанавлииваем гравитацию
        
        // Инециализируем переменные для реализации подката и приседания 
        originalHeight = controller.height;
        cameraStandPosition = playerCamera.transform.localPosition;
        cameraCrouchPosition = cameraStandPosition + new Vector3(0, crouchCameraOffset, 0);
        transformStandLocalScale = transform.localScale;
        transformCrouchLocalScaleStandart = new Vector3(transformStandLocalScale.x, crouchHeight, transformStandLocalScale.z);
    }
    
    void Update()
    {
        // Реализация задерки
        if (time > 0f)
        {
            time -= Time.deltaTime;
        }
        
        // Проверки
        _isGrounded = Physics.CheckSphere(groundCheck.position, _groundDistance,groundMask);
        isCrouch = Physics.CheckSphere(ceilingCheck.position, _groundDistance, groundMask) && !isTackle;
        
        // Передвижение
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            MaxSpeed = WalkingSpeed;
            stat = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            MaxSpeed = RunningSpeed;
            stat = true;
        }
        
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            MaxSpeed = SprintSpeed;
            stat = true;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                MaxSpeed = BackSpeedAD;
                   
            }
            else
            {
                MaxSpeed = BackSpeed;
            }
            stat = true;
        }
        
        
        Vector3 move = transform.right * x + transform.forward * y;
        
        // Подкат : Приседание 
        if (Input.GetKey(KeyCode.C) && !isTackle)
        {
            if (_currentSpeed > 5 && _isGrounded && time <= 0) // Подкат сработает если сорость больше 5
            {
                time = 2f;
                isTackle = true;
                
                Distance = Timet * _currentSpeed;
                
                move = transform.right * x + transform.forward * y; 
                _inertiaVelocity = move * _currentSpeed * _horizontalInertia;
            }
            
            if (_isGrounded) // Блокирует изменнеие скорости в воздухе
            {
                MaxSpeed = SpeedCrouch;   
            }
            
            controller.height = crouchHeight;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraCrouchPosition, crouchTransitionSpeed * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, transformCrouchLocalScaleStandart, crouchTransitionSpeed * Time.deltaTime);   
        }
        else
        {
            if (!Physics.CheckSphere(ceilingCheck.position, _groundDistance, groundMask) && !isTackle)
            {
                controller.height = originalHeight;
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraStandPosition, crouchTransitionSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(transform.localScale, transformStandLocalScale, crouchTransitionSpeed * 4 * Time.deltaTime);   
            }
        }

        if (isTackle)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, transformCrouchLocalScaleStandart, crouchTransitionSpeed * Time.deltaTime);
            if (Distance <= 0f)
            {
                isTackle = false;
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
            if (!isTackle)
            {
                _currentSpeed -= Deceleration * Time.deltaTime; // Обнуление скорости
            }
        } 
        
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, MaxSpeed);
        
        // Прыжок
        if (Input.GetKey(KeyCode.Space) && _isGrounded && !Physics.CheckSphere(ceilingCheck.position, _groundDistance, groundMask) && !isTackle)
        {
            _velocity.y = Mathf.Sqrt(_jumpH * -2f * _gravity);

            if (isTackle)
            {
                _inertiaVelocity = Vector3.zero;
                isTackle = false;
            }
            
            move = transform.right * x + transform.forward * y; 
            _inertiaVelocity = move * _currentSpeed * _horizontalInertia;
        }
        
        if (_isGrounded && !isTackle)
        {
            _inertiaVelocity = Vector3.zero;
            
            // Передвижение
            if (stat == false)
            {
                controller.Move(move * (4f * Time.deltaTime)); // Остаточное движение
            }
            else
            {
                controller.Move(move * (_currentSpeed * Time.deltaTime));  // Базовое движение 
            }
            
            _inertiaVelocity = move;
        }
        else
        {
            if (isTackle)
            {
                controller.Move(_inertiaVelocity * (RngeTackle * Time.deltaTime));
                Distance -= RngeTackle;
            }
            else
            {
                controller.Move(_inertiaVelocity * (InertiaSpeed * Time.deltaTime));
            }
            controller.Move(move * (4f * Time.deltaTime));
        }
        
        
        //Гравитация
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
            _currentGravity = _gravity;
            _fallTimer = 0f;
        }
        else
        { 
            _fallTimer += Time.deltaTime;
            
            float gravityIncrease = gravityIncreaseRate * _fallTimer;
            _currentGravity = Mathf.Clamp(_gravity + gravityIncrease, maxGravity, _gravity);
        }
        
        // Применение гравитации
        _velocity.y += _currentGravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);  
        
        // Отменяем все Bool
        stat = false;
    }
}
