using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private bool waitForPlayer; // If the platform will only move after the player first steps on it
	[SerializeField] private int startingPoint;
	[SerializeField] private Transform[] points;
	
	private bool active;
	private enum Type { Looping, PingPong, OnlyOnce }
	[SerializeField] private Type type;

	private int currentPoint;
	private bool forward;
	private bool disabled;

	// Sets platform parameters
	private void OnEnable()
	{
		active = !waitForPlayer;
		disabled = false;
		currentPoint = 0;
		transform.position = points[startingPoint].position;
	}

	// Moves the platform continuously towards the next point when active
	private void Update()
	{
		if (active)
		{
			transform.position = Vector2.MoveTowards(transform.position, points[currentPoint].position, speed * Time.deltaTime);

			if (Vector2.Distance(transform.position, points[currentPoint].position) < 0.002f)
			{
				// Loops through the points
				if (type == Type.Looping)
				{
					currentPoint++;
					if (currentPoint >= points.Length)
						currentPoint = 0;
				}
				// Moves back and forth between the points
				else if (type == Type.PingPong)
				{
					// Moves forward through the points
					if (forward)
					{
						currentPoint++;
						if (currentPoint >= points.Length)
						{
							currentPoint = points.Length - 2;
							forward = false;
						}
					}
					// Moves backward through the points
					else
					{
						currentPoint--;
						if (currentPoint < 0)
						{
							currentPoint = 1;
							forward = true;
						}
					}
				}
				// Moves through the points towards the final point and disables the platform after
				else if (type == Type.OnlyOnce)
				{
					currentPoint++;
					if (currentPoint >= points.Length)
					{
						disabled = true;
						active = false;
					}
				}
			}
		}
	}

	// When the player collides with the platform
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Sets the player as a child of the platform
		collision.transform.SetParent(transform);
		// Disables player interpolation to prevent jittery movement
		collision.transform.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
	}

	// While the player is on the platform
	private void OnCollisionStay2D(Collision2D collision)
	{
		// Activates the platform when the player is above it and close to the center
		if (collision.transform.CompareTag("Player")
			&& !active && !disabled
			&& (collision.transform.position.y > (transform.position.y + 0.67f))
			&& (collision.transform.position.x > transform.position.x - (GetComponent<BoxCollider2D>().size.x / 2 - 0.5))
			&& (collision.transform.position.x < transform.position.x + (GetComponent<BoxCollider2D>().size.x / 2 - 0.5)))
			active = true;
	}

	// When the player leaves the platform
	private void OnCollisionExit2D(Collision2D collision)
	{
		// Sets the player's parent to null
		if (transform.root.gameObject.activeInHierarchy)
			collision.transform.SetParent(null);
		// Re-enables player interpolation
		collision.transform.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
	}
}
