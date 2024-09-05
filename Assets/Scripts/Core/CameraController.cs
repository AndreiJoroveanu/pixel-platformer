using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("General Variables")]
	public bool isRoomCamera;
	[SerializeField] private Transform startingRoom;
	public bool smoothCamera;

	[Header("Room Camera Variables")]
	[SerializeField] private float speed;
	private float currentPosX;
	private float currentPosY;
	private Vector3 velocity = Vector3.zero;

	[Header("Subject Camera Variables")]
	public Transform subject;
	public enum Axis { verticalCamera, horizontalCamera, unlocked }
	public Axis axis;
	public float xOffset;
	public float yOffset;

	// Moves the camera to the starting room
	private void Awake()
	{
		MoveToNewRoom(startingRoom);
	}

	private void LateUpdate()
	{
		// Room camera
		if (isRoomCamera)
		{
			transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, currentPosY, transform.position.z), ref velocity, speed);
		}
		// Subject camera
		// Horizontal camera
		else if (axis == Axis.horizontalCamera)
		{
			if (smoothCamera)
				transform.position = Vector3.SmoothDamp(transform.position, new Vector3(subject.position.x + xOffset, transform.position.y + yOffset, transform.position.z), ref velocity, 0.2f);
			else
				transform.position = new Vector3(subject.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
		}
		// Vertical camera
		else if (axis == Axis.verticalCamera)
		{
			if (smoothCamera)
				transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x + xOffset, subject.position.y + yOffset, transform.position.z), ref velocity, 0.2f);
			else
				transform.position = new Vector3(transform.position.x + xOffset, subject.position.y + yOffset, transform.position.z);
		}
		// Unlocked camera
		else
		{
			if (smoothCamera)
				transform.position = Vector3.SmoothDamp(transform.position, new Vector3(subject.position.x + xOffset, subject.position.y + yOffset, transform.position.z), ref velocity, 0.2f);
			else
				transform.position = new Vector3(subject.position.x + xOffset, subject.position.y + yOffset, transform.position.z);
		}
	}

	// Moves the camera to a new room
	public void MoveToNewRoom(Transform _newRoom)
	{
		currentPosX = _newRoom.position.x;
		currentPosY = _newRoom.position.y;
	}
}
