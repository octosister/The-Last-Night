using UnityEngine;
using TMPro;
using System.Collections;

public class InteractableObject : MonoBehaviour
{
    public string objectName;
    public string description;
    public TextMeshProUGUI uiText;
    public AudioClip soundEffect;
    private AudioSource audioSource;

    public float liftDuration = 2f;
    public float liftHeight = 1f;
    public float moveCloserDistance = 0.5f; 

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isLifted = false;

    public GameObject player;
    private PlayerMovement playerMovement;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    public void Interact()
    {
        uiText.text = objectName + ": " + description;

        if (soundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }

        if (!isLifted)
        {
            StartCoroutine(LiftAndRotateObject());
        }

        StartCoroutine(HideTextAfterDelay(5f));
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiText.text = "";
    }

    private IEnumerator LiftAndRotateObject()
    {
        isLifted = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        
        Vector3 playerPosition = player.transform.position;
        Vector3 targetPosition = transform.position + Vector3.up * liftHeight + (playerPosition - transform.position).normalized * moveCloserDistance;

        Quaternion targetRotation = Quaternion.Euler(270f, 0f, -90f); 

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < liftDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / liftDuration);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / liftDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;

        yield return new WaitForSeconds(5f);

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isLifted = false;

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}
