using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // ��� ��������� ������� ����

public class HorrorTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalPosition;
    private TextMeshProUGUI text;
    public float shakeAmount = 2f;
    public float flickerSpeed = 8f;
    private bool isHovered = false; // ����, ������� �� ������

    void Start()
    {
        originalPosition = transform.position;
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isHovered) // �������� ������ ��� ���������
        {
            // ��������
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * shakeAmount);

            // ��������
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed));
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
        else
        {
            transform.position = originalPosition; // ���������� ����� �� �����
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1); // ������ ���������
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true; // �������� ������
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false; // ��������� ������
    }
}
