using UnityEngine;
using UnityEngine.EventSystems; // Нужно для обработки событий

public class HoverLoopSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource audioSource; // Источник звука

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true; // Зацикливаем звук
            audioSource.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (audioSource != null)
        {
            audioSource.loop = false; // Отключаем зацикливание
            audioSource.Stop();
        }
    }
}
