using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class start : MonoBehaviour
{
    public string sceneName = "Game";

    private void Start()
    {
        
        Text textComponent = GetComponent<Text>();
        if (textComponent == null)
        {
            Debug.LogError("Объект не содержит компонента Text. Пожалуйста, добавьте его.");
        }
        else
        {
            
            Button button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(OnTextClick);
        }
    }

    private void OnTextClick()
    {
        
        SceneManager.LoadScene(sceneName);
    }
}
