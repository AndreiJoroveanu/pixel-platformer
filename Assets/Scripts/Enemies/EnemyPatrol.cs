using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
	[Header("Patrol Points")]
	[SerializeField] private Transform leftEdge;
	[SerializeField] private Transform rightEdge;

	[Header("Enemy")]
	[SerializeField] private Transform enemy;

	[Header("Movement parameters")]
	[SerializeField] private float speed;
	private Vector3 initScale;
	private bool movingLeft;

	[Header("Idle Behaviour")]
	[SerializeField] private float idleDuration;
	private float idleTimer;

	[Header("Enemy Animator")]
	[SerializeField] private Animator anim;

	// Stores the initial scale of the enemy
	private void Awake()
	{
		initScale = enemy.localScale;
	}

	// Stops moving when the enemy is disabled
	private void OnDisable()
	{
		anim.SetBool("moving", false);
	}

	// Continously moves the enemy back and forth between the patrol points
	private void Update()
	{
		// Moving heft
		if (movingLeft)
		{
			if (enemy.position.x >= leftEdge.position.x)
				MoveInDirection(-1);
			else
				DirectionChange();
		}
		// Moving right
		else
		{
			if (enemy.position.x <= rightEdge.position.x)
				MoveInDirection(1);
			else
				DirectionChange();
		}
	}

	// Changes the direction of the enemy after the idle duration
	private void DirectionChange()
	{
		anim.SetBool("moving", false);
		idleTimer += Time.deltaTime;

		if (idleTimer > idleDuration)
			movingLeft = !movingLeft;
	}

	// Moves the enemy in a certain direction
	private void MoveInDirection(int _direction)
	{
		idleTimer = 0;
		anim.SetBool("moving", true);

		// Makes enemy face the correct direction
		enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
			initScale.y, initScale.z);

		// Moves in that direction
		enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
			enemy.position.y, enemy.position.z);
	}
}
