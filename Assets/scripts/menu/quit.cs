using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quit : MonoBehaviour
{
    Text textComponent = GetComponent<Text>();
        if (textComponent == null)
        {
            Debug.LogError("Объект не содержит компонента Text. Пожалуйста, добавьте его.");
        }
        else
    {
        // Добавляем обработчик клика
        Button button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnTextClick);
    }
        }

    private void OnTextClick()
{
    // Завершаем игру
    Debug.Log("Выход из игры");
    Application.Quit(); 
}
}
