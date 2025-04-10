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

        // --- ЭТУ ПРОВЕРКУ НУЖНО УБРАТЬ ИЛИ ЗАКОММЕНТИРОВАТЬ ---
        // Так как теперь коллайдер НЕ должен быть триггером
        /*
        if (!platformCollider.isTrigger)
        {
            Debug.LogWarning($"Коллайдер на {gameObject.name} не является триггером! Скрипт RevealPlatform может работать некорректно.", this);
        }
        */
        // --- КОНЕЦ ИЗМЕНЕНИЯ В AWAKE ---

        // Убедимся, что рендерер выключен, если плита еще не проявлена
        // (Добавочная проверка на случай, если забыли выключить в редакторе)
        if (!hasBeenRevealed)
        {
            meshRenderer.enabled = false;
        }
    }

    // --- ИЗМЕНЯЕМ OnTriggerEnter НА OnCollisionEnter ---
    private void OnCollisionEnter(Collision collisionInfo)
    {
        // Если плита еще не проявлена И столкнулся объект с тегом "Player"
        if (!hasBeenRevealed && collisionInfo.gameObject.CompareTag("Player"))
        {
            // Проявляем плиту
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
                hasBeenRevealed = true; // Помечаем, что уже проявили
                Debug.Log($"Плита {gameObject.name} проявлена (OnCollisionEnter).");

                // Опционально: можно сделать коллайдер триггером ПОСЛЕ проявления,
                // если не нужно, чтобы он оставался твердым препятствием.
                // platformCollider.isTrigger = true;
            }
        }
    }
    // --- КОНЕЦ ЗАМЕНЫ МЕТОДА ---

    // OnTriggerExit больше не нужен в этом контексте
}
