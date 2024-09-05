using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	[SerializeField] private float damageValue;

    [SerializeField] private float speed;
    [SerializeField] private float resetTime; // Time before the projectile is reset
    public float lifetime;
	private Animator anim;

	private bool hit;

	[Header ("SFX")]
	[SerializeField] private AudioClip destroyed;

    private Rigidbody2D body;

	private void Awake()
    {
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// When the projectile is fired
    public void ActivateProjectile()
    {
		hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
    }

	// Moves the projectile and resets it after a certain time
    private void Update()
    {
		if (hit) return;

		body.velocity = new Vector2(speed * Mathf.Sign(transform.parent.localScale.x), body.velocity.y);

		lifetime += Time.deltaTime;
		if (lifetime >= resetTime)
			gameObject.SetActive(false);
	}

	// When the projectile hits something (collider)
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Destroys the projectile when it hits a wall
		if (collision.gameObject.CompareTag("Wall"))
		{
			Destroy();
			hit = true;
		}

		// Destroys the projectile when it hits a player and damages them
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<Health>().TakeDamage(damageValue);

			Destroy();
			hit = true;
		}
	}

	// When the projectile hits something (trigger)
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Destroys the projectile when it hits a wall
		if (collision.gameObject.CompareTag("Wall"))
		{
			Destroy();
			hit = true;
		}

		// Destroys the projectile when it hits a player and damages them
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<Health>().TakeDamage(damageValue);

			Destroy();
			hit = true;
		}
	}

	// Plays the destroyed sound and animation, if it exists
	private void Destroy()
	{
		SoundManager.instance.PlaySound(destroyed);

		// If the projectile has an animation, else skip to Deactivate()
		if (anim != null)
		{
			// Stops the projectile momentum and resets the rotation
			body.velocity = Vector2.zero;
			body.angularVelocity = 0;
			transform.rotation = Quaternion.Euler(0, 0, 0);

			// Plays the destroyed animation
			anim.SetTrigger("destroyed");
		}
		else
			Deactivate();
	}

	// Deactivates the projectile
	private void Deactivate()
	{
		gameObject.SetActive(false);
	}
}
