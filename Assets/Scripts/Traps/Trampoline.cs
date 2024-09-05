using UnityEngine;

public class Trampoline : MonoBehaviour
{
	[SerializeField] private float jumpPower;
	[SerializeField] private AudioClip activated;

	private Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// When the player collides with the trampoline, they will jump high
	protected void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			anim.SetTrigger("activated");
			SoundManager.instance.PlaySound(activated);

			// Makes the player jump with increased velocity
			collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, jumpPower);
			collision.GetComponent<Animator>().SetTrigger("jumping");
			collision.GetComponent<PlayerMovement>().jumpCounter = 1;
		}
	}
}
