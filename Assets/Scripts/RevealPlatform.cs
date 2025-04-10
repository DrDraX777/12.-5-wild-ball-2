using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class RevealPlatform : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider platformCollider;
    private bool hasBeenRevealed = false;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<Collider>();

        // --- ��� �������� ����� ������ ��� ���������������� ---
        // ��� ��� ������ ��������� �� ������ ���� ���������
        /*
        if (!platformCollider.isTrigger)
        {
            Debug.LogWarning($"��������� �� {gameObject.name} �� �������� ���������! ������ RevealPlatform ����� �������� �����������.", this);
        }
        */
        // --- ����� ��������� � AWAKE ---

        // ��������, ��� �������� ��������, ���� ����� ��� �� ���������
        // (���������� �������� �� ������, ���� ������ ��������� � ���������)
        if (!hasBeenRevealed)
        {
            meshRenderer.enabled = false;
        }
    }

    // --- �������� OnTriggerEnter �� OnCollisionEnter ---
    private void OnCollisionEnter(Collision collisionInfo)
    {
        // ���� ����� ��� �� ��������� � ���������� ������ � ����� "Player"
        if (!hasBeenRevealed && collisionInfo.gameObject.CompareTag("Player"))
        {
            // ��������� �����
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
                hasBeenRevealed = true; // ��������, ��� ��� ��������
                Debug.Log($"����� {gameObject.name} ��������� (OnCollisionEnter).");

                // �����������: ����� ������� ��������� ��������� ����� ����������,
                // ���� �� �����, ����� �� ��������� ������� ������������.
                // platformCollider.isTrigger = true;
            }
        }
    }
    // --- ����� ������ ������ ---

    // OnTriggerExit ������ �� ����� � ���� ���������
}
