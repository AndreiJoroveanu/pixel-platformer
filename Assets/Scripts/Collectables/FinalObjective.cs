using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalObjective : MonoBehaviour
{
	[SerializeField] private PlayerMovement player;

	[SerializeField] private TimerText timer;

	[Header("SFX")]
	[SerializeField] private AudioClip collected;
	[SerializeField] private UIManager ui;

	private Animator anim;
	private bool canCollect = true;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// When the player collides with the object, they will stop moving and the object will be collected
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && canCollect)
		{			
			// Plays the collection animation and sound
			anim.SetTrigger("collected");
			SoundManager.instance.PlaySound(collected);

			// Makes the object uncollectable
			canCollect = false;

			// Sets the last level completed (only if it's higher than the current last level completed)
			if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("LastLevelCompleted"))
				PlayerPrefs.SetInt("LastLevelCompleted", SceneManager.GetActiveScene().buildIndex);

			// Stops the timer (which also saves the time if it's a new best time)
			timer.StopTimer();
		}
	}

	// Displays the win screen, is called by animation event
	private void Collected()
	{
		ui.winScreen.SetActive(true);
		Cursor.visible = true;

		Time.timeScale = 0;
		player.enabled = false;
	}
}
