using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
	[SerializeField] private int levelIndex;
	private int lastLevelCompleted;

	private void Awake()
	{
		// Get the last level completed from the player prefs
		lastLevelCompleted = PlayerPrefs.GetInt("LastLevelCompleted", 0);

		// Changes text and color if the level is locked
		if (levelIndex > lastLevelCompleted + 1)
		{
			GetComponent<Text>().color = new Color(0.6f, 0.4f, 0.4f, 1);
			GetComponent<Text>().text = "Locked";
		}
		// Displays the best time if it's not the default value
		else
		if (PlayerPrefs.GetFloat("Level" + levelIndex + "BestTime", 600) < 600)
		{
			GetComponent<Text>().text = "Level " + levelIndex + " - " + TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level" + levelIndex + "BestTime", 600)).ToString(@"m\:ss\.f");
			GetComponent<RectTransform>().sizeDelta = new Vector2(850, 125);
		}
	}

	// Starts the selected level (only if unlocked; resumes game time)
	public void SelectLevel()
	{
		if (levelIndex <= lastLevelCompleted + 1)
		{
			SceneManager.LoadScene(levelIndex);
			Time.timeScale = 1;
		}
	}
}
