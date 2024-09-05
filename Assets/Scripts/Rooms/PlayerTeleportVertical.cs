using UnityEngine;

public class PlayerTeleportVertical : MonoBehaviour
{
	[Header("TP Coordonates")]
	[SerializeField] private float upCoordonate;
	[SerializeField] private float bottomCoordonate;

	private enum Side { upSide, bottomSide }
	[Header("TP Side")]
	[SerializeField] private Side side;

	// Teleports the player to the other side of the room
	protected void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (collision.transform.position.y > transform.position.y && side == Side.bottomSide)
				collision.transform.position = new Vector2(collision.transform.position.x, upCoordonate);
			else if (collision.transform.position.y < transform.position.y && side == Side.upSide)
				collision.transform.position = new Vector2(collision.transform.position.x, bottomCoordonate);
		}
	}
}
