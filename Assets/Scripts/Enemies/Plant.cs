using UnityEngine;

public class Plant : MonoBehaviour
{
	[Header("Attack Parameters")]
	[SerializeField] private float attackCooldown;
	[SerializeField] private float range;
	[SerializeField] private int damage;

	[Header("Ranged Attack")]
	[SerializeField] private Transform bulletpoint; // Where the bullets will spawn
	[SerializeField] private GameObject[] bullets; // Array of bullets to use

	[Header("Collider Parameters")]
	[SerializeField] private float colliderDistance; // Distance from the center of the collider, affects range
	[SerializeField] private BoxCollider2D boxCollider;

	[Header("Player Layer")]
	[SerializeField] private LayerMask playerLayer;

	[Header("Bullet Sound")]
	[SerializeField] private AudioClip bulletSound;

	private float cooldownTimer = Mathf.Infinity;
	private Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// Fires a bullet at the bulletpoint, then resets cd
	private void RangedAttack()
	{
		SoundManager.instance.PlaySound(bulletSound);
		cooldownTimer = 0;

		bullets[FindBullet()].transform.position = bulletpoint.position;
		bullets[FindBullet()].GetComponent<EnemyProjectile>().ActivateProjectile();
	}

	// Finds an inactive bullet to use
	private int FindBullet()
	{
		for (int i = 0; i < bullets.Length; i++)
		{
			if (!bullets[i].activeInHierarchy)
				return i;
		}
		return 0;
	}

	// Shoots if the player is in sight and the cd is up
	private void Update()
	{
		cooldownTimer += Time.deltaTime;

		if (PlayerInSight())
		{
			if (cooldownTimer >= attackCooldown)
			{
				cooldownTimer = 0;
				anim.SetTrigger("attack");
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

		return hit.collider != null;
	}

	// Draws the range of the attack in scene view
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
			new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y / 3, boxCollider.bounds.size.z));
	}
}
