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

        if (playerRigidbody == null) { Debug.LogError("Rigidbody не найден на игроке!"); }
        if (playerRenderer == null) { Debug.LogError("Renderer не найден на игроке!"); }
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
            StartRespawnSequence(); // <--- Вызываем исправленный метод
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
            // Решай, что делать при падении: рестарт или респаун
            Die();
            // StartRespawnSequence();
        }
    }

    void Die()
    {
        if (isRespawning) return;
        Debug.Log("Игрок умер! Перезапуск уровня...");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        if (isRespawning) return;
        // ... (код загрузки уровня) ...
        Debug.Log("Уровень пройден! Загрузка следующего...");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Поздравляем! Все уровни пройдены! Возврат в главное меню.");
            SceneManager.LoadScene(0);
        }
    }


    void StartRespawnSequence()
    {
        if (isRespawning) return;

        Debug.Log("Игрок коснулся опасности! Респаун на старте...");
        isRespawning = true;

        // === ИЗМЕНЕНИЕ ЗДЕСЬ ===
        // Сначала сбрасываем скорость, ПОКА Rigidbody еще не кинематический
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;

            // Теперь делаем кинематическим, чтобы физика не мешала перемещению и вспышкам
            playerRigidbody.isKinematic = true;
        }
        // =======================

        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        // 1. Перемещение (скорость УЖЕ сброшена)
        transform.position = startPosition;

        // 2. Эффект вспышки
        if (playerRenderer != null)
        {
            for (int i = 0; i < flashCount; i++)
            {
                playerRenderer.enabled = false;
                yield return new WaitForSeconds(flashDuration);
                playerRenderer.enabled = true;
                yield return new WaitForSeconds(flashDuration);
            }
            playerRenderer.enabled = true; // Убедимся, что включен в конце
        }
        else
        {
            yield return new WaitForSeconds(flashCount * flashDuration * 2);
        }

        // 3. Восстановление физики и сброс флага
        if (playerRigidbody != null)
        {
            // Возвращаем Rigidbody в нормальное состояние
            playerRigidbody.isKinematic = false;
        }
        isRespawning = false;
        Debug.Log("Респаун завершен.");
    }
}
