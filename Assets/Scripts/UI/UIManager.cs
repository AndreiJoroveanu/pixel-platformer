using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	[SerializeField] private PlayerMovement player;

	[Header("Screens")]
	[SerializeField] private GameObject pauseScreen;
	[SerializeField] public GameObject winScreen;

	[Header("SFX")]
	[SerializeField] private AudioClip pauseGame;
	[SerializeField] private AudioClip interactSound;

	private void Awake()
	{
		pauseScreen.SetActive(false);
		winScreen.SetActive(false);

		// Hides the cursor except when the Pause Menu is active
		Cursor.visible = false;
	}

	private void Update()
	{
		// Pause game if Esc key or Menu button is pressed
		if (Input.GetKeyDown(KeyCode.Escape))
			PauseGame(!pauseScreen.activeSelf);
		if (Input.GetKeyDown(KeyCode.JoystickButton7))
			PauseGame(!pauseScreen.activeSelf);

		// Exit pause screen if B is pressed on controller
		if (pauseScreen.activeSelf && Input.GetKeyDown(KeyCode.JoystickButton1))
			PauseGame(false);
	}

	public void PauseGame(bool status)
	{
		// Prevents pausing the game when the win screen is active
		if (winScreen.activeSelf) return;

		// Displays the Pause Menu and cursor
		pauseScreen.SetActive(status);
		Cursor.visible = status;

		// Disables player movement when paused
		player.enabled = !status;

		// Pauses game time
		if (status)
		{
			SoundManager.instance.PlaySound(pauseGame);
			Time.timeScale = 0;
		}
		else
		{
			SoundManager.instance.PlaySound(interactSound);
			Time.timeScale = 1;
		}
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

	// Restarts the current level (reloads the same scene and resumes game time)
	public void Restart()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// Starts the next level (loads the next scene and resumes game time)
	public void NextLevel()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	// Loads the Main Menu (loads the first scene)
	public void MainMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
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
