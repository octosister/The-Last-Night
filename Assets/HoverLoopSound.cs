using UnityEngine;
using UnityEngine.EventSystems; // ����� ��� ��������� �������

public class HoverLoopSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource audioSource; // �������� �����

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true; // ����������� ����
            audioSource.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (audioSource != null)
        {
            audioSource.loop = false; // ��������� ������������
            audioSource.Stop();
        }
    }
}
