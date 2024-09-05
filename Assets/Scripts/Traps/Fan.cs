using UnityEngine;

public class Fan : MonoBehaviour
{
	[SerializeField] private float jumpPower;

	// While the player is in the fan's range, they will be pushed upwards
	protected void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, jumpPower);
			collision.GetComponent<Animator>().SetTrigger("jumping");
			collision.GetComponent<PlayerMovement>().jumpCounter = 1;
		}
	}
}
