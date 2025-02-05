using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> footstepClips;
    public float stepInterval = 0.5f;

    private float stepTimer = 0f;
    private Rigidbody rb;
    private Vector3 lastPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime; // Calculate movement speed
        lastPosition = transform.position;

        if (speed > 0.1f && IsGrounded())
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Count > 0)
        {
            int index = Random.Range(0, footstepClips.Count);
            audioSource.PlayOneShot(footstepClips[index]);
        }
    }

    bool IsGrounded()
    {
        // Raycast to check if the player is on the ground
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
