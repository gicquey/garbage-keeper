using System.Collections;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
	public AudioSource audioSource;

	public void Start () {

		if(audioSource == null) {
			if(GetComponentInChildren<AudioSource>() != null) {
				this.audioSource = GetComponentInChildren<AudioSource>();
			} else {
				this.gameObject.AddComponent<AudioSource>();
			}
		}
	}

    protected IEnumerator playAfterDelay(float delay, AudioSource source)
    {
        yield return new WaitForSeconds(delay);
        source.Play();
    }

	public void stop()
	{
		if (this == null) {
			return;
		}

		if (audioSource == null) 
		{
			audioSource = GetComponent<AudioSource>();
		}

		audioSource.Stop ();
		audioSource.clip = null;
	}

}

