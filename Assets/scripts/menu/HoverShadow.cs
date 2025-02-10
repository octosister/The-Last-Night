using UnityEngine;
using UnityEngine.UI; // Для UI-элементов
using UnityEngine.EventSystems; // Для наведения мыши

public class HoverShadow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Shadow shadow; // Переменная для тени

    void Start()
    {
        shadow = GetComponent<Shadow>(); // Получаем компонент Shadow
        shadow.enabled = false; // Отключаем тень по умолчанию
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shadow.enabled = true; // Включаем тень при наведении
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shadow.enabled = false; // Выключаем тень, когда курсор уходит
    }
}
