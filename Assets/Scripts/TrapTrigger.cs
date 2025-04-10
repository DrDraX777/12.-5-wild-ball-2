using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [Tooltip("Аниматор объекта, который нужно активировать (капсулы)")]
    public Animator targetAnimator; // Сюда перетащим капсулу

    [Tooltip("Имя триггер-параметра в аниматоре для старта анимации")]
    public string animationTriggerName = "StartAnimationTrigger"; // Имя из Шага 1

    [Tooltip("Тег объекта, который должен активировать триггер (обычно игрок)")]
    public string triggeringTag = "Player";

    private bool hasBeenTriggered = false; // Флаг, чтобы триггер сработал только один раз

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, не был ли триггер уже активирован И вошел ли объект с нужным тегом
        if (!hasBeenTriggered && other.CompareTag(triggeringTag))
        {
            // Проверяем, назначен ли аниматор
            if (targetAnimator != null)
            {
                Debug.Log($"Игрок ({other.name}) вошел в триггер {gameObject.name}. Запускаем анимацию капсулы.");

                // Устанавливаем флаг, что триггер сработал
                hasBeenTriggered = true;

                // Запускаем триггер в аниматоре
                targetAnimator.SetTrigger(animationTriggerName);

                // Опционально: можно деактивировать сам триггер после срабатывания,
                // чтобы он точно больше не реагировал.
                // gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Target Animator не назначен в скрипте TrapTrigger на объекте " + gameObject.name);
            }
        }
    }
}
