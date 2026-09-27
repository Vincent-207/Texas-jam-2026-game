using System;
using UnityEngine;

public class AudioOneShotManager : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    private AudioClip destroySound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDestroySound()
    {
        audioSource.PlayOneShot(destroySound);
    }
}
