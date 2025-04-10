using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    // ���� ��������� ����� ����� ���������� ����� Animation Event
    public void DestroyGameObject()
    {
        Debug.Log($"�������� ������� {gameObject.name} ���������. ���������� ������.");
        Destroy(gameObject); // ���������� ������� ������, �� ������� ����� ���� ������
    }
}
