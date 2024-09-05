using UnityEngine;

public class ContactDamage : MonoBehaviour
{
	[SerializeField] private int contactDamage;

	// Trigger
	protected void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			// If the player is above the enemy, the enemy will take damage and the player will be pushed upwards
			if (collision.transform.position.y > (transform.position.y + GetComponent<BoxCollider2D>().size.y / 2 + collision.GetComponent<BoxCollider2D>().size.y / 2))
			{
				// Damages the enemy
				GetComponent<Health>().TakeDamage(1);

				// Jumps the player and resets their jump counter
				collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, collision.GetComponent<PlayerMovement>().jumpPower);
				collision.GetComponent<Animator>().SetTrigger("jumping");
				SoundManager.instance.PlaySound(collision.GetComponent<PlayerMovement>().jump);
				collision.GetComponent<PlayerMovement>().jumpCounter = 1;
			}
			// If the player is not above the enemy, the player will take damage and be pushed away
			else
			{
				// Damages the player
				collision.GetComponent<Health>().TakeDamage(contactDamage);

				// Stuns the player
				collision.GetComponent<PlayerMovement>().wallJumpCooldown = 0;

				// Pushes the player away
				if (collision.transform.position.x < transform.position.x)
					collision.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, collision.GetComponent<Rigidbody2D>().velocity.y);
				else
					collision.GetComponent<Rigidbody2D>().velocity = new Vector2(5, collision.GetComponent<Rigidbody2D>().velocity.y);
			}
	}

	// Collision
	protected void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
			// If the player is above the enemy, the enemy will take damage and the player will be pushed upwards
			if (collision.transform.position.y > (transform.position.y + GetComponent<BoxCollider2D>().size.y / 2 + collision.collider.GetComponent<BoxCollider2D>().size.y / 2))
			{
				// Damages the enemy
				GetComponent<Health>().TakeDamage(1);

				// Jumps the player and resets their jump counter
				collision.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.collider.GetComponent<Rigidbody2D>().velocity.x, collision.collider.GetComponent<PlayerMovement>().jumpPower);
				collision.collider.GetComponent<Animator>().SetTrigger("jumping");
				SoundManager.instance.PlaySound(collision.collider.GetComponent<PlayerMovement>().jump);
				collision.collider.GetComponent<PlayerMovement>().jumpCounter = 1;
			}
			// If the player is not above the enemy, the player will take damage and be pushed away
			else
			{
				// Damages the player
				collision.collider.GetComponent<Health>().TakeDamage(contactDamage);

				// Stuns the player
				collision.collider.GetComponent<PlayerMovement>().wallJumpCooldown = 0;

				// Pushes the player away
				if (collision.transform.position.x < transform.position.x)
					collision.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, collision.collider.GetComponent<Rigidbody2D>().velocity.y);
				else
					collision.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(5, collision.collider.GetComponent<Rigidbody2D>().velocity.y);
			}
	}
}
