using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeSlimeSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip punchSound;

    public void PlayPunchSound()
    {
        audioSource.PlayOneShot(punchSound);
    }
}