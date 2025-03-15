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
        if (collision.gameObject.CompareTag("Bullet")) // ���������, ������ �� ����
        {
            targetRenderer.material.color = Color.green; // ������ ���� �� �������
            Destroy(collision.gameObject); // ������� ����
        }
    }
}