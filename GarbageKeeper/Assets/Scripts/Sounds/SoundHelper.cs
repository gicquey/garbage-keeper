// Singleton used for generic sounds
using System.Collections.Generic;
using UnityEngine;

public class SoundHelper : SoundPlayer {

	private static SoundHelper instance = null;

	public static SoundHelper Instance {
		get { return instance; }
	}

	private float rateForSound = 3F;
    protected List<AudioSource> _audioSourcesCopies = new List<AudioSource>();
    private List<AudioSource> _reservedForThisFrame = new List<AudioSource>();

    void Awake()
	{
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

    public override void play(AudioClip sound)
    {
        play(sound, GetAvailableSource(), 0F, false);
    }

    public override void playWithLoop(AudioClip sound)
    {
        play(sound, GetAvailableSource(), 0F, true);
    }

    /*Uses the audio source as protoype and manages a list of copies of it,
      so that any number of sounds can be played at the same time */
    private AudioSource GetAvailableSource()
    {
        AudioSource returnedSource = null;
        foreach(var source in _audioSourcesCopies)
        {
            if (!source.isPlaying && !_reservedForThisFrame.Contains(source))
            {
                returnedSource = source;
                break;
            }
        }

        if(returnedSource == null)
        {
            //If here, no source is available, creating one
            returnedSource = Instantiate(audioSource, audioSource.transform.parent, true);
            _audioSourcesCopies.Add(returnedSource);
        }
        _reservedForThisFrame.Add(returnedSource);
        return returnedSource;
    }

    void LateUpdate()
	{
		audioSource.enabled = Settings.Instance.soundEnable;
		audioSource.volume = Settings.Instance.soundVolume / rateForSound;
        foreach(var source in _audioSourcesCopies)
        {
            source.enabled = Settings.Instance.soundEnable;
            source.volume = Settings.Instance.soundVolume / rateForSound;
        }
        _reservedForThisFrame.Clear();
	}
}
