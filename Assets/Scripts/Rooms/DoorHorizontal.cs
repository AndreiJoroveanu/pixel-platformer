using UnityEngine;

public class DoorHorizontal : MonoBehaviour
{
	[SerializeField] private Transform previousRoom;
	[SerializeField] private Transform nextRoom;
	[SerializeField] private CameraController cam;

	// Moves the camera to the next room when the player collides with the door...
	// (also activates the enemies in the next room and deactivates the previous one's)
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (collision.transform.position.y < transform.position.y)
			{
				cam.MoveToNewRoom(nextRoom);
				nextRoom.GetComponent<Room>().ActivateRoom(true);
				previousRoom.GetComponent<Room>().ActivateRoom(false);
			}
			else
			{
				cam.MoveToNewRoom(previousRoom);
				nextRoom.GetComponent<Room>().ActivateRoom(false);
				previousRoom.GetComponent<Room>().ActivateRoom(true);
			}
		}
	}

	// ...and when they leave the door collider
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (collision.transform.position.y < transform.position.y)
			{
				cam.MoveToNewRoom(nextRoom);
				nextRoom.GetComponent<Room>().ActivateRoom(true);
				previousRoom.GetComponent<Room>().ActivateRoom(false);
			}
			else
			{
				cam.MoveToNewRoom(previousRoom);
				nextRoom.GetComponent<Room>().ActivateRoom(false);
				previousRoom.GetComponent<Room>().ActivateRoom(true);
			}
		}
	}
}
