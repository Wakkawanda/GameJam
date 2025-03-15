using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchHeight = 0.5f; // ������ ��� ����������
    public float standHeight = 1.0f; // ������ � ������� ���������
    public float crouchSpeed = 5.0f; // �������� ��������� ������

    private bool isCrouching = false;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // ��������� ������������ �������
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // ������� ������� C ��� ����������
        {
            isCrouching = !isCrouching; // ����������� ��������� ����������
        }

        // �������� ������� ���������
        if (isCrouching)
        {
            // ���������
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(originalScale.x, crouchHeight, originalScale.z), Time.deltaTime * crouchSpeed);
        }
        else
        {
            // ������������ � ������� ���������
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(originalScale.x, standHeight, originalScale.z), Time.deltaTime * crouchSpeed);
        }
    }
}