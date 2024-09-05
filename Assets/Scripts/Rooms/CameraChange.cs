using UnityEngine;

public class CameraChange : MonoBehaviour
{
	[SerializeField] private Transform currentRoom;
	[SerializeField] private CameraController cam;
	[SerializeField] private Transform subject;
	[SerializeField] private bool smoothCamera;
	private enum Axis { verticalCamera, horizontalCamera, unlocked }
	[SerializeField] private Axis axis;
	private enum Type { toggle, roomCamera, subjectCamera }
	[SerializeField] private Type type;
	[SerializeField] private bool singleUse;
	[SerializeField] private float xOffset;
	[SerializeField] private float yOffset;

	// Changes the camera type between room camera and subject camera
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			// Changes the camera type according to the type setting
			if (type == Type.toggle)
				cam.isRoomCamera = !cam.isRoomCamera;
			else if (type == Type.roomCamera)
				cam.isRoomCamera = true;
			else if (type == Type.subjectCamera)
				cam.isRoomCamera = false;

			// Sets the camera's settings
			cam.smoothCamera = smoothCamera;
			cam.MoveToNewRoom(currentRoom);
			cam.subject = subject;
			cam.axis = (CameraController.Axis)axis;
			cam.xOffset = xOffset;
			cam.yOffset = yOffset;

			// Deactivates the trigger if it's a single use one
			if (singleUse)
				gameObject.SetActive(false);
		}
	}

	// Also sets the camera type when the player leaves the trigger for good measure
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			// Changes the camera type according to the type setting
			if (type == Type.roomCamera)
				cam.isRoomCamera = true;
			else if (type == Type.subjectCamera)
				cam.isRoomCamera = false;
		}
	}
}
