using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // Для обработки событий мыши

public class HorrorTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalPosition;
    private TextMeshProUGUI text;
    public float shakeAmount = 2f;
    public float flickerSpeed = 8f;
    private bool isHovered = false; // Флаг, наведен ли курсор

    void Start()
    {
        originalPosition = transform.position;
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isHovered) // Анимация только при наведении
        {
            // Дрожание
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * shakeAmount);

            // Мерцание
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed));
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }
        else
        {
            transform.position = originalPosition; // Возвращаем текст на место
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1); // Полная видимость
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true; // Включаем эффект
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false; // Выключаем эффект
    }
}
