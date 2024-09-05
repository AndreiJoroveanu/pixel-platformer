using UnityEngine;

public class SpikeballTrap : MonoBehaviour
{
	[SerializeField] private float attackCooldown;
	[SerializeField] private Transform firePoint; // Where the spikeballs will spawn
	[SerializeField] private GameObject[] spikeballs; // Array of spikeballs to use
	private float cooldownTimer;

	[Header("SFX")]
	[SerializeField] private AudioClip attack;

	private Animator anim;
	private bool active = true;

	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		// Physics2D.IgnoreLayerCollision(11, 11, true); <- uncomment if spikeballs don't have velocity
	}

	// Spawns a spikeball at the firepoint, then resets cd
	private void Attack()
	{
		SoundManager.instance.PlaySound(attack);
		cooldownTimer = 0;
		
		spikeballs[FindSpikeball()].transform.position = firePoint.position;
		spikeballs[FindSpikeball()].GetComponent<EnemyProjectile>().ActivateProjectile();
	}

	// Finds an inactive spikeball to use
	private int FindSpikeball()
	{
		for (int i = 0; i < spikeballs.Length; i++)
		{
			if (!spikeballs[i].activeInHierarchy)
				return i;
		}
		return 0;
	}

	// Attacks when the cd is up
	private void Update()
	{
		if (active)
			cooldownTimer += Time.deltaTime;

		if (cooldownTimer >= attackCooldown)
		{
			anim.SetTrigger("attack");
			Attack();
		}
	}

	// Stops the trap from attacking in the player's face
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			cooldownTimer = 0;
			active = false;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			active = true;
		}
	}
}
