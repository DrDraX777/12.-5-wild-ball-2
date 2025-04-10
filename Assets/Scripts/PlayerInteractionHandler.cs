using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerInteractionHandler : MonoBehaviour
{
    private Vector3 startPosition;
    private Rigidbody playerRigidbody;
    private Renderer playerRenderer;

    public int flashCount = 3;
    public float flashDuration = 0.1f;
    private bool isRespawning = false;

    void Awake()
    {
        startPosition = transform.position;
        playerRigidbody = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

        if (playerRigidbody == null) { Debug.LogError("Rigidbody �� ������ �� ������!"); }
        if (playerRenderer == null) { Debug.LogError("Renderer �� ������ �� ������!"); }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isRespawning && collision.gameObject.CompareTag("Hazard"))
        {
            Die();
            return;
        }

        if (!isRespawning && collision.gameObject.CompareTag("RespawnHazard"))
        {
            StartRespawnSequence(); // <--- �������� ������������ �����
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            LoadNextLevel();
        }
        if (other.gameObject.CompareTag("DeathZone"))
        {
            // �����, ��� ������ ��� �������: ������� ��� �������
            Die();
            // StartRespawnSequence();
        }
    }

    void Die()
    {
        if (isRespawning) return;
        Debug.Log("����� ����! ���������� ������...");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        if (isRespawning) return;
        // ... (��� �������� ������) ...
        Debug.Log("������� �������! �������� ����������...");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("�����������! ��� ������ ��������! ������� � ������� ����.");
            SceneManager.LoadScene(0);
        }
    }


    void StartRespawnSequence()
    {
        if (isRespawning) return;

        Debug.Log("����� �������� ���������! ������� �� ������...");
        isRespawning = true;

        // === ��������� ����� ===
        // ������� ���������� ��������, ���� Rigidbody ��� �� ��������������
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;

            // ������ ������ ��������������, ����� ������ �� ������ ����������� � ��������
            playerRigidbody.isKinematic = true;
        }
        // =======================

        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        // 1. ����������� (�������� ��� ��������)
        transform.position = startPosition;

        // 2. ������ �������
        if (playerRenderer != null)
        {
            for (int i = 0; i < flashCount; i++)
            {
                playerRenderer.enabled = false;
                yield return new WaitForSeconds(flashDuration);
                playerRenderer.enabled = true;
                yield return new WaitForSeconds(flashDuration);
            }
            playerRenderer.enabled = true; // ��������, ��� ������� � �����
        }
        else
        {
            yield return new WaitForSeconds(flashCount * flashDuration * 2);
        }

        // 3. �������������� ������ � ����� �����
        if (playerRigidbody != null)
        {
            // ���������� Rigidbody � ���������� ���������
            playerRigidbody.isKinematic = false;
        }
        isRespawning = false;
        Debug.Log("������� ��������.");
    }
}
