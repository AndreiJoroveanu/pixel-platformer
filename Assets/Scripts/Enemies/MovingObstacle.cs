using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private int startingPoint;
	[SerializeField] private Transform[] points;

	private enum Type { Looping, PingPong, OnlyOnce }
	[SerializeField] private Type type;

	private int currentPoint;
	private bool forward;

	// Sets obstacle to starting point
	private void OnEnable()
	{
		currentPoint = 0;
		transform.position = points[startingPoint].position;
	}

	// Moves the obstacle continuously towards the next point
	private void Update()
	{
		transform.position = Vector2.MoveTowards(transform.position, points[currentPoint].position, speed * Time.deltaTime);

		if (Vector2.Distance(transform.position, points[currentPoint].position) < 0.02f)
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
			// Moves through the points towards the final point and disables the obstacle after
			else if (type == Type.OnlyOnce)
			{
				currentPoint++;
				if (currentPoint >= points.Length)
				{
					enabled = false;
				}
			}
		}
	}
}
