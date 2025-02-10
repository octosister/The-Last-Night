using UnityEngine;
using UnityEngine.UI; // ��� UI-���������
using UnityEngine.EventSystems; // ��� ��������� ����

public class HoverShadow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Shadow shadow; // ���������� ��� ����

    void Start()
    {
        shadow = GetComponent<Shadow>(); // �������� ��������� Shadow
        shadow.enabled = false; // ��������� ���� �� ���������
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shadow.enabled = true; // �������� ���� ��� ���������
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shadow.enabled = false; // ��������� ����, ����� ������ ������
    }
}
