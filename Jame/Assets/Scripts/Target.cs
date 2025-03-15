using UnityEngine;

public class Target : MonoBehaviour
{
    private Renderer targetRenderer;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) // Проверяем, попала ли пуля
        {
            targetRenderer.material.color = Color.green; // Меняем цвет на зеленый
            Destroy(collision.gameObject); // Удаляем пулю
        }
    }
}