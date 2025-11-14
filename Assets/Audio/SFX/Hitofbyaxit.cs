using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitofbyaxit : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip SpitSound;

    public void PlayHitofbyaxitSound()
    {
        audioSource.PlayOneShot(SpitSound);
    }
}