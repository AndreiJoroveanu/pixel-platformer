using UnityEngine;

public class PlayerTeleportHorizontal : MonoBehaviour
{
	[Header("TP Coordonates")]
	[SerializeField] private float leftCoordonate;
	[SerializeField] private float rightCoordonate;

	private enum Side { leftSide, rightSide }
	[Header("TP Side")]
	[SerializeField] private Side side;

	// Teleports the player to the other side of the room
	protected void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (collision.transform.position.x > transform.position.x && side == Side.leftSide)
				collision.transform.position = new Vector2(rightCoordonate, collision.transform.position.y);
			else if (collision.transform.position.x < transform.position.x && side == Side.rightSide)
				collision.transform.position = new Vector2(leftCoordonate, collision.transform.position.y);
		}
	}
}
