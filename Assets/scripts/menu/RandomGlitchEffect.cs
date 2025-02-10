using UnityEngine;
using System.Collections;

public class RandomGlitchEffect : MonoBehaviour
{
    private Material material;
    public float glitchIntensity = 0.5f;
    private float defaultIntensity;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        defaultIntensity = material.GetFloat("_GlitchIntensity");
        StartCoroutine(GlitchRoutine());
    }

    IEnumerator GlitchRoutine()
    {
        while (true)
        {
            float randomTime = Random.Range(5f, 15f); // ��� � 5-15 ������
            yield return new WaitForSeconds(randomTime);
            StartCoroutine(GlitchEffect());
        }
    }

    IEnumerator GlitchEffect()
    {
        material.SetFloat("_GlitchIntensity", glitchIntensity);
        yield return new WaitForSeconds(0.3f); // �������� �����
        material.SetFloat("_GlitchIntensity", defaultIntensity);
    }
}
