using UnityEngine;
using UnityEngine.UI;

public class PlayText : MonoBehaviour
{
	[SerializeField] private string textIntro;
	private int lastLevelCompleted;

	// Changes the text to show the next level
	private void Awake()
	{
		// Get the last level completed from the player prefs
		lastLevelCompleted = PlayerPrefs.GetInt("LastLevelCompleted", 0);

		// If the last level completed is the last level in the game
		if (lastLevelCompleted > 2)
		{
			lastLevelCompleted = 2;
		}

		GetComponent<Text>().text = textIntro + (lastLevelCompleted + 1).ToString("0");
	}
}
