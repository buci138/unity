using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public void Init(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }

    void Update()
    {
        if (audioSource.isPlaying ==false)
        {
            PoolManager.Instance.PushObj(GameManager.Instance.GameConf.EFAudio,gameObject);
        }
    }
}
