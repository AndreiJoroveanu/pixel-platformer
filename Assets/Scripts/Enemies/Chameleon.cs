using UnityEngine;

public class Chameleon : MonoBehaviour
{
	[Header("Attack Parameters")]
	[SerializeField] private float attackCooldown;
	[SerializeField] private float range;
	[SerializeField] private int damage;

	[Header("Collider Parameters")]
	[SerializeField] private float colliderDistance; // Distance from the center of the collider, affects range
	[SerializeField] private BoxCollider2D boxCollider;

	[Header("Player Layer")]
	[SerializeField] private LayerMask playerLayer;
	private float cooldownTimer = Mathf.Infinity;

	[Header("SFX")]
	[SerializeField] private AudioClip attackSound;
	[SerializeField] private AudioClip attackHitSound;

	private Animator anim;
	private Health playerHealth;
	private EnemyPatrol enemyPatrol;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		enemyPatrol = GetComponentInParent<EnemyPatrol>();
	}

	// Attacks if the player is in sight and the cd is up
	private void Update()
	{
		cooldownTimer += Time.deltaTime;

		if (PlayerInSight())
		{
			if (cooldownTimer >= attackCooldown)
			{
				cooldownTimer = 0;
				anim.SetTrigger("attack");
				if (enemyPatrol != null)
					enemyPatrol.enabled = false;
			}
		}
	}

	// Checks if the player is in sight
	private bool PlayerInSight()
	{
		RaycastHit2D hit =
			Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
			new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y / 3, boxCollider.bounds.size.z),
			0, Vector2.left, 0, playerLayer);

		if (hit.collider != null)
			playerHealth = hit.transform.GetComponent<Health>();

		return hit.collider != null;
	}

	// Draws the range of the attack in scene view
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
			new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y / 3, boxCollider.bounds.size.z));
	}

	// Damages the player if they are in sight and not invulnerable, is called by animation event
	private void DamagePlayer()
	{
		SoundManager.instance.PlaySound(attackSound);

		if (PlayerInSight() && playerHealth.isInvulnerable == false)
		{
			playerHealth.TakeDamage(damage);
			SoundManager.instance.PlaySound(attackHitSound);
		}
	}

	// Resumes the patrol after attacking
	private void ResumePatrol()
	{
		if (enemyPatrol != null)
			enemyPatrol.enabled = true;
	}
}
