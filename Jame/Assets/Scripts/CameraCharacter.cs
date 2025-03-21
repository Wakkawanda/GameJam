using System;
using UnityEngine;
using UnityEngine.Serialization;
public class CharacterCamera : MonoBehaviour
{
    [FormerlySerializedAs("_playerBody")] public Transform playerBody;

    private float xRotation = 0f;
    public float mouseSensitivityX = 60f;
    public float mouseSensitivityY = 60f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y")* mouseSensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Math.Clamp(xRotation, -90f, 32f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        
    }
}
