using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 0.5f; // Высота при приседании
    public float standHeight = 1.0f; // Высота в стоячем положении
    public float crouchSpeed = 5.0f; // Скорость изменения высоты

    private bool isCrouching = false;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // Сохраняем оригинальный масштаб
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Нажатие клавиши C для приседания
        {
            isCrouching = !isCrouching; // Переключаем состояние приседания
        }

        // Изменяем масштаб персонажа
        if (isCrouching)
        {
            // Приседаем
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(originalScale.x, crouchHeight, originalScale.z), Time.deltaTime * crouchSpeed);
        }
        else
        {
            // Возвращаемся в стоячее положение
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(originalScale.x, standHeight, originalScale.z), Time.deltaTime * crouchSpeed);
        }
    }
}