using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpitSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip SpitSound;

    public void PlaySpitSound()
    {
        audioSource.PlayOneShot(SpitSound);
    }
}
