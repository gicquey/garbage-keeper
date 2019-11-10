using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SoundTypes
{
    SHOOT,
    DIE_END,
    DIE_DAMAGE,
    LOOT,
    IMPACT_PUDDLE,
    IMPACT_BATTERY,
    IMPACT_POISON,
    BUILD_TURRET,
    DESTROY_TURRET,
}

public enum MusicTypes
{
    MAIN
}

[Serializable]
public class SoundConfigItem
{
    public SoundTypes soundType;
    public List<AudioClip> associatedSounds;
}

[Serializable]
public class MusicConfigItem
{
    public MusicTypes musicType;
    public List<AudioClip> associatedMusics;
}

public class AudioConfig : MonoBehaviour
{
    public List<SoundConfigItem> soundConfig;
    public List<MusicConfigItem> musicConfig;

    public List<AudioClip> soundsThatCutMusic = new List<AudioClip>();

	private static AudioConfig _instance = null;
	public static AudioConfig Instance {
		get { return _instance; }
	}

	void Awake()
	{
		if (_instance != null && _instance != this) 
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
            _instance = this;
		}
        soundsThatCutMusic = new List<AudioClip>();
        DontDestroyOnLoad(this.gameObject);
	}

    public AudioClip GetClipForSoundType (SoundTypes soundType)
    {
        var soundItem = soundConfig.FirstOrDefault(item => item.soundType == soundType);
        if(soundItem != null)
        {
            var soundsList = soundItem.associatedSounds;
            if(soundsList != null && soundsList.Count != 0)
            {
                return soundsList[UnityEngine.Random.Range(0, soundsList.Count)];
            }
            else
            {
                Debug.LogError("Sound type " + soundType.ToString() + "doesn't have any linked sounds");
                return null;
            }
        }
        else
        {
            Debug.LogError("Sound " + soundType.ToString() + " isn't configured");
            return null;
        }
    }

    public AudioClip GetClipForMusicType(MusicTypes musicType)
    {
        var musicItem = musicConfig.FirstOrDefault(item => item.musicType == musicType);
        if (musicItem != null)
        {
            var musicsList = musicItem.associatedMusics;
            if (musicsList != null && musicsList.Count != 0)
            {
                return musicsList[UnityEngine.Random.Range(0, musicsList.Count)];
            }
            else
            {
                Debug.LogError("Music type " + musicType.ToString() + "doesn't have any linked music");
                return null;
            }
        }
        else
        {
            Debug.LogError("Music type " + musicType.ToString() + " isn't configured");
            return null;
        }
    }
}

