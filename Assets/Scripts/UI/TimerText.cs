using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
	[SerializeField] private GameObject PBText;
	private float time = 0;
	private bool completed = false;

	// Shows the personal best time if it exists
	public void Awake()
	{
		if (PlayerPrefs.GetFloat("Level" + SceneManager.GetActiveScene().buildIndex + "BestTime", 600) < 600)
		{
			PBText.GetComponent<Text>().text = "PB:" + TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level" + SceneManager.GetActiveScene().buildIndex + "BestTime", 600)).ToString(@"m\:ss\.f") + "\n";
		}
	}

	// Updates the timer text unless the timer has been stopped
	private void Update()
    {
		if (completed) return;

		time += Time.deltaTime;

		if (time < 600)
			GetComponent<Text>().text = TimeSpan.FromSeconds(time).ToString(@"m\:ss\.f");
		else
			GetComponent<Text>().text = "Time invalid";
	}

	// Stops the timer and saves the time if it's a new best time
	public void StopTimer()
	{
		completed = true;

		if (time < PlayerPrefs.GetFloat("Level" + SceneManager.GetActiveScene().buildIndex + "BestTime", 600))
		{
			PlayerPrefs.SetFloat("Level" + SceneManager.GetActiveScene().buildIndex + "BestTime", time);
			PBText.GetComponent<Text>().text = "New Personal Best!\nProgress Saved";
		}
	}
}
