using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	[SerializeField] private CameraController cam;

	[Header ("Health")]
	[SerializeField] private float startingHealth;
	public float currentHealth { get; private set; }
	private Animator anim;
	private bool isDead = false;
	public bool isInvulnerable = false;

	[Header ("IFrames")]
	[SerializeField] private float iFramesDuration;
	[SerializeField] private int numberOfFlashes;
	private SpriteRenderer spriteRend;

	[Header ("SFX")]
	[SerializeField] private AudioClip hurt;
	[SerializeField] private AudioClip dead;
	[SerializeField] private AudioClip respawned;

	// When the entity first spawns
	private void Awake()
	{
		currentHealth = startingHealth;
		anim = GetComponent<Animator>();
		spriteRend = GetComponent<SpriteRenderer>();

		// Makes the player not ignore ignore enemy collision
		Physics2D.IgnoreLayerCollision(10, 11, false);
	}

	// When the entity takes damage
	public void TakeDamage(float _damage)
	{
		// Decreases health
		currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

		// If the entity is still alive
		if (currentHealth > 0)
		{
			SoundManager.instance.PlaySound(hurt);
			anim.SetTrigger("hurt");

			StartCoroutine(Invulnerability());
		}
		// If the entity is dead
		else if (!isDead)
		{
			// These apply to the player
			if (GetComponent<PlayerMovement>() != null)
			{
				GetComponent<PlayerMovement>().body.gravityScale = 0;
				GetComponent<PlayerMovement>().body.velocity = Vector2.zero;
				GetComponent<PlayerMovement>().enabled = false;
			}
			
			// These apply to the enemies
			if (GetComponent<Chameleon>() != null)
			{
				GetComponent<Chameleon>().enabled = false;
				GetComponent<BoxCollider2D>().enabled = false;
			}
			if (GetComponentInParent<EnemyPatrol>() != null)
				GetComponentInParent<EnemyPatrol>().enabled = false;

			if (GetComponent<Plant>() != null)
			{
				GetComponent<Plant>().enabled = false;
				GetComponent<BoxCollider2D>().enabled = false;
			}

			// Plays the death sound and animation
			SoundManager.instance.PlaySound(dead);
			anim.SetTrigger("dead");

			// Disables the entity and sets its parent to null
			isDead = true;
			transform.SetParent(null);
		}
	}

	// When the player gains health
	public void AddHealth(float _value)
	{
		currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
	}

	// When the player respawns
	public IEnumerator Respawn()
	{
		// Sets player health to full
		isDead = false;
		AddHealth(startingHealth);

		// Plays the respawn sound and animation
		SoundManager.instance.PlaySound(respawned);
		anim.ResetTrigger("dead");
		anim.Play("respawning");

		StartCoroutine(Invulnerability());
		yield return new WaitForSeconds(0.3f);

		GetComponent<PlayerMovement>().enabled = true;
		GetComponent<PlayerMovement>().transform.localScale = new Vector3(3, 3, 1);

		// Sets the camera to room camera
		cam.isRoomCamera = true;
	}

	// When the player is takes damage or respawns
	private IEnumerator Invulnerability()
	{
		// Makes the player ignore enemy collision
		Physics2D.IgnoreLayerCollision(10, 11, true);
		isInvulnerable = true;

		yield return new WaitForSeconds(0.3f);

		// Flashes the player sprite
		for (int i = 0; i < numberOfFlashes; i++)
		{
			spriteRend.enabled = false;
			yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
			spriteRend.enabled = true;
			yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
		}

		// Makes the player not ignore enemy collision
		Physics2D.IgnoreLayerCollision(10, 11, false);
		isInvulnerable = false;
	}
}
