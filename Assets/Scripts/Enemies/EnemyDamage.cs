using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
	[SerializeField] protected float damageValue;

	// Damages the player when they collide with the enemy
	protected void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			collision.GetComponent<Health>().TakeDamage(damageValue);
	}
}
