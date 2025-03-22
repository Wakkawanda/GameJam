using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab; // ������ ����
    public Transform shootingPoint; // �����, ������ ����� ��������

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // �� ��������� ��� ����� ������ ����
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // ������� ����
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        // ������������� �������� ����
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = shootingPoint.forward * bullet.GetComponent<Bullet>().speed; // ������� ���� ������
    }
}