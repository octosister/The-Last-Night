using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class start : MonoBehaviour
{
    public string sceneName = "Game"; // Название сцены, которую нужно открыть

    private void Start()
    {
        // Проверяем, есть ли компонент Text или Button на объекте
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
        // Загружаем указанную сцену
        SceneManager.LoadScene(sceneName);
    }
}
