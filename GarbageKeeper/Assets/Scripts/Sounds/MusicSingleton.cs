using System;
using UnityEngine;

public class MusicSingleton : MonoBehaviour
{
	public AudioSource audioSource;

	private static MusicSingleton instance = null;
	public static MusicSingleton Instance {
		get { return instance; }
	}
		
	private Action _onMusicEnded;

	//Variables used for fading
	private bool _isFading;
	private static float _desiredFadingTime = 1.5f;
	private float _currentFadingTime;
	private float _baseVolume;
	private bool _switchedToNewMusic;
	private AudioClip _newMusic;
	private bool _mustNewMusicLoop;
    private bool _cutBySound;


	void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
        _cutBySound = false;
	}

	public void Start() {

		//string music = "music_"+ RandomGenerator.Instance.Range(1, 5);
		//play (music, true);

		if (audioSource == null) {
			audioSource = GetComponent<AudioSource> ();
		}
	}

	private void LateUpdate()
	{
		audioSource.enabled = Settings.Instance.musicEnable && !_cutBySound;
		if (_isFading) 
		{
			_currentFadingTime += Time.deltaTime;
			if (_currentFadingTime < _desiredFadingTime / 2f)
			{
				//Linearly fading out first music to 0 volume during the first half of fading time
				audioSource.volume = ((-_baseVolume / (_desiredFadingTime / 2f)) * _currentFadingTime) + _baseVolume;
			}
			else
			{
				if (!_switchedToNewMusic) 
				{
					SwitchToMusic (_newMusic, _mustNewMusicLoop);
					audioSource.Play();
					_switchedToNewMusic = true;
				}

				if (_currentFadingTime < _desiredFadingTime) 
				{
					//Linearly fading in new music to base volume during the second half of the time
					audioSource.volume = (_baseVolume / (_desiredFadingTime/2f)) * (_currentFadingTime - (_desiredFadingTime/2f));
				}
				else
				{
					_isFading = false;	
				}
			}
		}
		else
		{
			audioSource.volume = Settings.Instance.musicVolume;
		}
	}

	public void EnableOrDisable() {
		if (audioSource.enabled) {
			audioSource.enabled = false;
		} else {
			audioSource.enabled = true;
		}
	}
		
	public void play(AudioClip music, bool loop, Action onMusicEnded = null)
	{
		if (!Settings.Instance.musicEnable) {
			return;
		}

		_onMusicEnded = onMusicEnded;

		if (audioSource.clip == null) 
		{
			SwitchToMusic (music, loop);
			audioSource.Play ();
		}
		else
		{
			StartFadingToMusic (music, loop);
		}
	}

    public void SetCutBySound(bool cut)
    {
        _cutBySound = cut;
    }

	private void StartFadingToMusic (AudioClip music,  bool loop)
	{
		_isFading = true;
		_currentFadingTime = 0;
		_baseVolume = audioSource.volume;
		_switchedToNewMusic = false;
		_newMusic = music;
		_mustNewMusicLoop = loop;
	}

	private void SwitchToMusic (AudioClip music, bool loop)
	{
		audioSource.loop = loop;

		if (audioSource == null) {
			Debug.Log ("sound audio source not define");
			return;
		}
			
		audioSource.clip = music;

		CancelInvoke ("CallMusicEndedHandler");
		if (!loop)
			Invoke ("CallMusicEndedHandler", music.length - (_desiredFadingTime / 2f));
	}

	private void CallMusicEndedHandler()
	{
		if (_onMusicEnded != null)
			_onMusicEnded.Invoke ();
	}
}