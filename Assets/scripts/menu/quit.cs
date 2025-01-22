using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quit : MonoBehaviour
{
    Text textComponent = GetComponent<Text>();
        if (textComponent == null)
        {
            Debug.LogError("������ �� �������� ���������� Text. ����������, �������� ���.");
        }
        else
    {
        // ��������� ���������� �����
        Button button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnTextClick);
    }
        }

    private void OnTextClick()
{
    // ��������� ����
    Debug.Log("����� �� ����");
    Application.Quit(); 
}
}
