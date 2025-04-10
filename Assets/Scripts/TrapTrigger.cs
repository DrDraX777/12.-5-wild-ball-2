using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [Tooltip("�������� �������, ������� ����� ������������ (�������)")]
    public Animator targetAnimator; // ���� ��������� �������

    [Tooltip("��� �������-��������� � ��������� ��� ������ ��������")]
    public string animationTriggerName = "StartAnimationTrigger"; // ��� �� ���� 1

    [Tooltip("��� �������, ������� ������ ������������ ������� (������ �����)")]
    public string triggeringTag = "Player";

    private bool hasBeenTriggered = false; // ����, ����� ������� �������� ������ ���� ���

    private void OnTriggerEnter(Collider other)
    {
        // ���������, �� ��� �� ������� ��� ����������� � ����� �� ������ � ������ �����
        if (!hasBeenTriggered && other.CompareTag(triggeringTag))
        {
            // ���������, �������� �� ��������
            if (targetAnimator != null)
            {
                Debug.Log($"����� ({other.name}) ����� � ������� {gameObject.name}. ��������� �������� �������.");

                // ������������� ����, ��� ������� ��������
                hasBeenTriggered = true;

                // ��������� ������� � ���������
                targetAnimator.SetTrigger(animationTriggerName);

                // �����������: ����� �������������� ��� ������� ����� ������������,
                // ����� �� ����� ������ �� ����������.
                // gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Target Animator �� �������� � ������� TrapTrigger �� ������� " + gameObject.name);
            }
        }
    }
}
