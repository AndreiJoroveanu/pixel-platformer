using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
	[SerializeField] private float damage;

	[Header("FireTrap Timers")]
	[SerializeField] private float activationDelay;
	[SerializeField] private float activeTime;
	private Animator anim;

	[Header("SFX")]
	[SerializeField] private AudioClip hit;
	[SerializeField] private AudioClip active;

	private bool triggered;
	private bool isActive;

	private Health player;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// Triggers the trap when the player collides with it
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (!triggered)
				StartCoroutine(ActivateFiretrap());

			player = collision.GetComponent<Health>();
		}
	}
	// Fixes a bug where the player would take damage even after leaving the trigger
	private void OnTriggerExit2D(Collider2D collision)
	{
		player = null;
	}

	// Damages the player
	private void FixedUpdate()
	{
		if (isActive && player != null)
			player.TakeDamage(damage);
	}

	private IEnumerator ActivateFiretrap()
	{
		// Triggers the trap when hit
		SoundManager.instance.PlaySound(hit);
		triggered = true;
		anim.SetTrigger("hit");

		// Activates the trap after a delay
		yield return new WaitForSeconds(activationDelay);
		SoundManager.instance.PlaySound(active);
		isActive = true;
		anim.SetBool("active", true);

		// Deactivates the trap after a delay
		yield return new WaitForSeconds(activeTime);
		isActive = false;
		triggered = false;
		anim.SetBool("active", false);
	}
}
