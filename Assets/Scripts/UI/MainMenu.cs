using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[Header("Screens")]
	[SerializeField] private GameObject LevelSelectMenu;
	[SerializeField] private GameObject TitleScreenMenu;

	[Header("SFX")]
	[SerializeField] private AudioClip interactSound;

	private int lastLevelCompleted;
	private float deleteCountdown = 2;
	static private bool firstTime = true;

	private void Awake()
	{
		// Get the last level completed from the player prefs
		lastLevelCompleted = PlayerPrefs.GetInt("LastLevelCompleted", 0);

		// If the last level completed is the last level in the game
		if (lastLevelCompleted > 2)
		{
			lastLevelCompleted = 2;
		}
	}

	// Plays the interact sound when the player returns to the main menu
	private void Start()
	{
		if (firstTime)
			firstTime = false;
		else
			SoundManager.instance.PlaySound(interactSound);
	}

	private void Update()
	{
		// If the level select menu is active and the player presses Escape key or B button, closes the menu
		if (LevelSelectMenu.activeSelf && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)))
		{
			LevelSelectMenu.SetActive(false);
			TitleScreenMenu.SetActive(true);
		}

		// If the player holds down Delete key for 2 seconds, deletes all player prefs
		if (!LevelSelectMenu.activeSelf && Input.GetKey(KeyCode.Delete))
		{
			if (deleteCountdown > 0)
			{
				deleteCountdown -= Time.deltaTime;
			}
			else
			{
				PlayerPrefs.DeleteAll();
				SceneManager.LoadScene(0);
			}
		}

		// Resets the delete countdown if the player releases the Delete key
		if (!LevelSelectMenu.activeSelf && Input.GetKeyUp(KeyCode.Delete))
			deleteCountdown = 2;
	}

	// Starts the game from the next level (resumes game time)
	public void StartGame()
	{
		SceneManager.LoadScene(lastLevelCompleted + 1);
		Time.timeScale = 1;
	}

	// Opens the level select menu
	public void LevelSelect(bool status)
	{
		SoundManager.instance.PlaySound(interactSound);

		LevelSelectMenu.SetActive(status);
		TitleScreenMenu.SetActive(!status);
	}

	// Changes the sound volume depending on where the player clicks with their mouse cursor
	public void ChangeSoundVolume()
	{
		if (Input.mousePosition.x > transform.position.x)
		{
			SoundManager.instance.PlaySound(interactSound);

			SoundManager.instance.ChangeSoundVolume(0.1f);
		}
		else
		{
			SoundManager.instance.PlaySound(interactSound);

			SoundManager.instance.ChangeSoundVolume(-0.1f);
		}
	}

	// Changes the music volume depending on where the player clicks with their mouse cursor
	public void ChangeMusicVolume()
	{
		if (Input.mousePosition.x > transform.position.x)
		{
			
			SoundManager.instance.PlaySound(interactSound);

			SoundManager.instance.ChangeMusicVolume(0.1f);
		}
		else
		{
			SoundManager.instance.PlaySound(interactSound);

			SoundManager.instance.ChangeMusicVolume(-0.1f);
		}
	}

	// Quits the game
	public void Quit()
	{
		Application.Quit();

		// Stops the game in the Unity Editor
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
