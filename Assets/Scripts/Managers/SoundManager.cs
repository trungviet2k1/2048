using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    [Header("Background Music")]
    public AudioSource backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip mergingSound;
    public AudioClip moveSound;
    public AudioClip loserSound;
    public AudioSource mergeAudio;
    public AudioSource moveAudio;
    public AudioSource loserAudio;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayMergeSound()
    {
        mergeAudio.PlayOneShot(mergingSound);
    }

    public void PlayMoveSound()
    {
        moveAudio.PlayOneShot(moveSound);
    }

    public void PlayLoserSound()
    {
        loserAudio.PlayOneShot(loserSound);
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }
}