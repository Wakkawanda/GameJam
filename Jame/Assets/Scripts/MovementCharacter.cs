using UnityEngine;

public class MovementCharacter : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public Transform ceilingCheck;
    public LayerMask groundMask;
    
    public float _speed = 5f;
    public float _speedWalk = 5f;
    public float _speedBack = 2f;
    public float _speedRun = 10f;
    public float _speedCrouch = 2f;
    
    public float _jumpH = 1f;
    public float CharectorHeight { get; set; }
    public float CellingHeight { get; set; }

    public float _gravity = -9.8f;
    public float _groundDistance = 0.4f;
    public Vector3 _velocity;
    public bool _isGrounded;
    public bool _isCeiling;
    
    void Start()
    {
        CharectorHeight = controller.height;
        CellingHeight = ceilingCheck.position.y;
    }
    
    void Update()
    {
        // Передвижение
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        
        if (Input.GetKey(KeyCode.W))
        {
            _speed = _speedWalk;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            _speed = _speedWalk;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _speed = _speedWalk;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _speed = _speedBack;
        }
        
        //Ускорение
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_speed != _speedBack)
                _speed = _speedRun;
        }
        
        //Приcидание
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _speed = _speedCrouch;

            controller.height = 1f;
        }
        else
        {
            controller.height = CharectorHeight;
        }
        
        // Передвижение
        Vector3 move = transform.right * x + transform.forward * y;
        controller.Move(move * (_speed * Time.deltaTime));
        
        
        // Прыжок
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpH * -2f * _gravity);
        }
        
        //Гравитация
        _isGrounded = Physics.CheckSphere(groundCheck.position, _groundDistance,groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
            //transform.position = new Vector3(transform.position.x, PlayerPositionY, transform.position.z);
        }
        
        _velocity.y += _gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);  
    }
}
