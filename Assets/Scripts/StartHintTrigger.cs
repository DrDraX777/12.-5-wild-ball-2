using UnityEngine;
using TMPro; // ��� using UnityEngine.UI;

public class StartHintTrigger : MonoBehaviour
{
    [Header("UI � ����")]
    [Tooltip("UI ����� � ���������� � ������� ����")]
    public GameObject hintTextUI;
    [Tooltip("��� �������, ������������� ��������� (�����)")]
    public string playerTag = "Player";

    [Header("������������ ����� ����")]
    [Tooltip("��������� ��������� ����� ����")]
    public Collider endPlatformCollider; // ���� ���������� ��������� �����
    [Tooltip("������ �� ������ ������ (��� ��� ���������)")]
    public Collider playerCollider; // ���� ���������� ��������� ������

    private bool hintIsActive = false;
    private bool endReached = false;

    void Start()
    {
        // �������� �� ������� ������
        if (hintTextUI == null)
            Debug.LogError("Hint Text UI �� �������� � StartHintTrigger �� " + gameObject.name);
        else
            hintTextUI.SetActive(false); // ��������, ��� ������

        if (endPlatformCollider == null)
            Debug.LogError("End Platform Collider �� �������� � StartHintTrigger �� " + gameObject.name);

        if (playerCollider == null)
            Debug.LogError("Player Collider �� �������� � StartHintTrigger �� " + gameObject.name);

        // ��������, ��� ��������� ��������� ����� - ��� �������
        if (endPlatformCollider != null && !endPlatformCollider.isTrigger)
            Debug.LogWarning("��������� End Platform Collider ������ ���� ���������!", endPlatformCollider);
    }

    // ���������� ��������� ��� ����� � ������� ������ �����
    private void OnCollisionEnter(Collision collisionInfo)
    {
        // ���� ��������� ��� �� ������� � �� ����� ���� � ������ ������������ ����� ������ ���
        if (!hintIsActive && !endReached && collisionInfo.gameObject.CompareTag(playerTag))
        {
            if (hintTextUI != null)
            {
                hintTextUI.SetActive(true); // ���������� ���������
                hintIsActive = true;       // ��������, ��� ��������
                Debug.Log("��������� � ������� ���� �������� (OnCollisionEnter).");
            }
        }
    }


    private void OnCollisionExit(Collision collisionInfo)
    {
        // ���� ��������� ��� �� ������� � �� ����� ���� � ������ ������������ ����� ������ ���
        if (hintIsActive && !endReached && collisionInfo.gameObject.CompareTag(playerTag))
        {
            if (hintTextUI != null)
            {
                hintTextUI.SetActive(false); // ���������� ���������
                hintIsActive = false;       // ��������, ��� ��������
                Debug.Log("��������� � ������� ���� ������ (OnCollisionExit).");
            }
        }
    }



    // ��������� ���������, �� ������ �� ����� �����
    void Update()
    {
        // ���� ��������� ������� � ����� ��� �� ���������
        if (!hintIsActive && !endReached)
        {
            // ���������, �������� �� ��������� ������ � ��������� �����
            if (playerCollider != null && endPlatformCollider != null)
            {
                // ���������, ������������ �� ��������� ������ � ��������� ��������� �����
                // Bounds.Intersects - ������� ������ ��������� ����������� AABB (�������������� �����)
                if (playerCollider.bounds.Intersects(endPlatformCollider.bounds))
                {
                    // �������������� �������� ���������� ��� �������� (�����������, �� ��������)
                    // ���������� ClosestPoint ��� ���������� ��������� ����� �� ���������� � ������ ������
                    Vector3 closestPointOnEndTrigger = endPlatformCollider.ClosestPoint(playerCollider.transform.position);
                    if (Vector3.Distance(playerCollider.transform.position, closestPointOnEndTrigger) < 0.1f) // ��������� �����
                    {
                        HideHintAndMarkEndReached();
                    }
                }
            }
        }
    }

    private void HideHintAndMarkEndReached()
    {
        if (hintTextUI != null)
            hintTextUI.SetActive(false); // �������� ���������

        hintIsActive = false; // ��������� ������ �� �������
        endReached = true;    // ����� ���� ���������
        Debug.Log("����� ������ ����� �������� ���� (���������� StartHintTrigger). ��������� ������.");

        // �����������: ����� �������������� ���� ������ ��� ������� ������ �����,
        // ����� �� ������� ������� �� �������� � Update
        // this.enabled = false;
    }

    // --- ��������������� ��� ������� OnTriggerExit, ���� �� ��� ---
    /*
    private void OnTriggerExit(Collider other)
    {
        // ���� ����� ������ �� �����, ��� ��� ��������� ���������� ������ � �����
    }
    */
}
