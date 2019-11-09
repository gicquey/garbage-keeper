using UnityEngine;


public class SoundPlayer : AudioHelper
{
    protected float soundReduceRate = 1F;

    new public void Start()
    {
        base.Start();

        loadAudioSource();
    }

    public void Update() {
        loadSettingForSounds();
    }

    public void loadAudioSource()
    {
        if (audioSource == null)
        {

            if (GetComponentInChildren<AudioSource>() == null)
            {
                Debug.LogError("Error : this component doesn't have an audio clip");
                return;
            }

            this.audioSource = GetComponent<AudioSource>();
        }
    }

    public void loadSettingForSounds()
    {
        this.audioSource.volume = Settings.Instance.soundVolume / soundReduceRate;
        this.audioSource.enabled = Settings.Instance.soundEnable;
    }

    public virtual void play(AudioClip sound)
    {
        play(sound, audioSource, 0F, false);
    }

    public virtual void playWithLoop(AudioClip sound)
    {
        play(sound, audioSource, 0F, true);
    }

    protected void play(AudioClip sound, AudioSource source, float delay, bool loop)
    {
        if (!Settings.Instance.soundEnable)
        {
            return;
        }

        MusicSingleton.Instance.SetCutBySound(AudioConfig.Instance.soundsThatCutMusic.Contains(sound));

        source.loop = loop;
        source.clip = sound;

        StartCoroutine(playAfterDelay(delay, source));
    }
}