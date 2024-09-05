using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
	[SerializeField] private float healthValue;

	[Header("SFX")]
	[SerializeField] private AudioClip collected;

	private Animator anim;
	private bool canCollect = true;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// When the player collides with the object, they will gain health
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && canCollect)
		{
			// Adds health to the player
			collision.GetComponent<Health>().AddHealth(healthValue);

			// Plays the collection animation and sound
			anim.SetTrigger("collected");
			SoundManager.instance.PlaySound(collected);

			// Makes the object uncollectable
			canCollect = false;
		}
	}

	// Disables the object, is called by animation event
	private void Collected()
	{
		gameObject.SetActive(false);
	}
}
