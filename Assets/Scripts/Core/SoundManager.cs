using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance { get; private set; }
	private AudioSource soundSource;
	private AudioSource musicSource;

	private void Awake()
	{
		soundSource = GetComponent<AudioSource>();
		musicSource = transform.GetChild(0).GetComponent<AudioSource>();

		// Ensures that there is only one instance of this object
		if (instance == null)
			instance = this;
		else if (instance != null && instance != this)
			Destroy(gameObject);

		// Initializes the sound and music volume to the player prefs
		ChangeSoundVolume(0);
		ChangeMusicVolume(0);
	}

	public void PlaySound(AudioClip _sound)
	{
		soundSource.PlayOneShot(_sound);
	}

	// Changes either sound volume or music volume
	private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
	{
		// Get the current volume from player prefs
		float currentVolume = PlayerPrefs.GetFloat(volumeName, 0.4f);

		// Changes the volume
		currentVolume = Mathf.Clamp(currentVolume + change, 0, 1);

		// Multiplies the volume by the base volume
		float finalVolume = currentVolume * baseVolume;
		source.volume = finalVolume;

		// Set the new volume to player prefs
		PlayerPrefs.SetFloat(volumeName, currentVolume);
	}

	// Changes the sound volume
	public void ChangeSoundVolume(float _change)
	{
		float baseVolume = 0.6f;

		ChangeSourceVolume(baseVolume, "soundVolume", _change, soundSource);
	}

	// Changes the music volume
	public void ChangeMusicVolume(float _change)
	{
		float baseVolume = 0.4f;

		ChangeSourceVolume(baseVolume, "musicVolume", _change, musicSource);
	}
}
