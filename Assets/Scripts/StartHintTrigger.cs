using UnityEngine;
using TMPro; // или using UnityEngine.UI;

public class StartHintTrigger : MonoBehaviour
{
    [Header("UI и Теги")]
    [Tooltip("UI текст с подсказкой о скрытом поле")]
    public GameObject hintTextUI;
    [Tooltip("Тег объекта, активирующего подсказку (игрок)")]
    public string playerTag = "Player";

    [Header("Отслеживание Конца Пути")]
    [Tooltip("Коллайдер ПОСЛЕДНЕЙ плиты пути")]
    public Collider endPlatformCollider; // СЮДА ПЕРЕТАЩИТЬ ПОСЛЕДНЮЮ ПЛИТУ
    [Tooltip("Ссылка на объект игрока (или его коллайдер)")]
    public Collider playerCollider; // СЮДА ПЕРЕТАЩИТЬ КОЛЛАЙДЕР ИГРОКА

    private bool hintIsActive = false;
    private bool endReached = false;

    void Start()
    {
        // Проверки на наличие ссылок
        if (hintTextUI == null)
            Debug.LogError("Hint Text UI не назначен в StartHintTrigger на " + gameObject.name);
        else
            hintTextUI.SetActive(false); // Убедимся, что скрыто

        if (endPlatformCollider == null)
            Debug.LogError("End Platform Collider не назначен в StartHintTrigger на " + gameObject.name);

        if (playerCollider == null)
            Debug.LogError("Player Collider не назначен в StartHintTrigger на " + gameObject.name);

        // Убедимся, что коллайдер последней плиты - это триггер
        if (endPlatformCollider != null && !endPlatformCollider.isTrigger)
            Debug.LogWarning("Коллайдер End Platform Collider должен быть триггером!", endPlatformCollider);
    }

    // Показываем подсказку при входе в триггер первой плиты
    private void OnCollisionEnter(Collision collisionInfo)
    {
        // Если подсказка еще не активна И не конец пути И объект столкновения имеет нужный тег
        if (!hintIsActive && !endReached && collisionInfo.gameObject.CompareTag(playerTag))
        {
            if (hintTextUI != null)
            {
                hintTextUI.SetActive(true); // Показываем подсказку
                hintIsActive = true;       // Помечаем, что показали
                Debug.Log("Подсказка о скрытом пути показана (OnCollisionEnter).");
            }
        }
    }


    private void OnCollisionExit(Collision collisionInfo)
    {
        // Если подсказка еще не активна И не конец пути И объект столкновения имеет нужный тег
        if (hintIsActive && !endReached && collisionInfo.gameObject.CompareTag(playerTag))
        {
            if (hintTextUI != null)
            {
                hintTextUI.SetActive(false); // Показываем подсказку
                hintIsActive = false;       // Помечаем, что показали
                Debug.Log("Подсказка о скрытом пути скрыта (OnCollisionExit).");
            }
        }
    }



    // Постоянно проверяем, не достиг ли игрок конца
    void Update()
    {
        // Если подсказка активна И конец еще не достигнут
        if (!hintIsActive && !endReached)
        {
            // Проверяем, назначен ли коллайдер игрока и последней плиты
            if (playerCollider != null && endPlatformCollider != null)
            {
                // Проверяем, пересекается ли коллайдер игрока с триггером последней плиты
                // Bounds.Intersects - простой способ проверить пересечение AABB (ограничивающих рамок)
                if (playerCollider.bounds.Intersects(endPlatformCollider.bounds))
                {
                    // Дополнительная проверка расстояния для точности (опционально, но надежнее)
                    // Используем ClosestPoint для нахождения ближайшей точки на коллайдере к центру игрока
                    Vector3 closestPointOnEndTrigger = endPlatformCollider.ClosestPoint(playerCollider.transform.position);
                    if (Vector3.Distance(playerCollider.transform.position, closestPointOnEndTrigger) < 0.1f) // Небольшой порог
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
            hintTextUI.SetActive(false); // Скрываем подсказку

        hintIsActive = false; // Подсказка больше не активна
        endReached = true;    // Конец пути достигнут
        Debug.Log("Игрок достиг конца скрытого пути (обнаружено StartHintTrigger). Подсказка скрыта.");

        // Опционально: можно деактивировать этот скрипт или триггер первой плиты,
        // чтобы не тратить ресурсы на проверку в Update
        // this.enabled = false;
    }

    // --- Закомментируйте или удалите OnTriggerExit, если он был ---
    /*
    private void OnTriggerExit(Collider other)
    {
        // Этот метод больше не нужен, так как подсказка скрывается только в конце
    }
    */
}
