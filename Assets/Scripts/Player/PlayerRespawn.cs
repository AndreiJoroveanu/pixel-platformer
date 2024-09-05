using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
	[Header("SFX")]
	[SerializeField] private AudioClip checkpointSound;

	private Transform currentCheckpoint;
	private Transform currentRoom;
	private Health playerHealth;

	private void Awake()
	{
		playerHealth = GetComponent<Health>();
	}

	// Sets the current checkpoint when the player collides with one
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Checkpoint"))
		{
			// Remember checkpoint's position and room
			currentCheckpoint = collision.transform;
			currentRoom = currentCheckpoint.parent;

			// Play checkpoint sound and animate the checkpoint
			SoundManager.instance.PlaySound(checkpointSound);
			collision.GetComponent<Animator>().SetTrigger("triggered");

			// Makes the checkpoint trigger only once
			collision.GetComponent<Collider2D>().enabled = false;
		}
	}

	// Respawns the player at the last checkpoint if they die
	public void Respawn()
	{
		transform.position = currentCheckpoint.position;
		playerHealth.Respawn();

		// Unparents the player in case they were on a moving platform
		GetComponent<PlayerMovement>().transform.SetParent(null);

		// Moves the camera and changes the active status of rooms accordingly
		currentRoom.GetComponent<Room>().ActivateRoom(false);
		currentRoom.GetComponent<Room>().ActivateRoom(true);
		Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
	}
}
