using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCrashAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource crashSound;

    private void OnDisable()
    {
        crashSound.Play();
    }
}
