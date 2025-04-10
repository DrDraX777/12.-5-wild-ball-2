using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    // Этот публичный метод будет вызываться через Animation Event
    public void DestroyGameObject()
    {
        Debug.Log($"Анимация объекта {gameObject.name} завершена. Уничтожаем объект.");
        Destroy(gameObject); // Уничтожает игровой объект, на котором висит этот скрипт
    }
}
