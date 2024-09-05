using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header ("Movement")]
	[SerializeField] private float speed;
	[SerializeField] public float jumpPower;
	[SerializeField] private float coyoteTime; // How much time after leaving ground to still jump
	private float coyoteTimer;

	[Header ("Wall Jumping")]
	[SerializeField] private float wallJumpX; // Horizontal wall jump force
	[SerializeField] private float wallJumpY; // Vertical wall jump force

	[Header ("Layers")]
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask wallLayer;

	[Header ("SFX")]
	[SerializeField] public AudioClip jump;
	[SerializeField] private AudioClip footsteps;

	[Header("Other")]
	public Rigidbody2D body;
	public Animator anim;
	private BoxCollider2D boxCollider;

	public float wallJumpCooldown = float.PositiveInfinity;
	private int extraJumps = 1;
	public int jumpCounter;

	private float horizontalInput;

	private void Awake()
	{
		// Get references from object
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();

		// Plays respawn animation when first spawning
		anim.Play("respawning");
	}

	private void Update()
	{
		// Horizontal movement
		horizontalInput = Mathf.Ceil(Input.GetAxisRaw("Horizontal"));

		// Calls jump function when pressing the jump button
		if ((Input.GetKeyDown(KeyCode.Space)
			  || Input.GetKeyDown(KeyCode.W)
			  || Input.GetKeyDown(KeyCode.UpArrow)
			  || Input.GetKeyDown(KeyCode.JoystickButton0)   // A button
			  || Input.GetKeyDown(KeyCode.JoystickButton1))) // B button
			Jump();

		// Adjustable jump height, halves vertical velocity when releasing the jump button
		if (Input.GetKeyUp(KeyCode.Space)
			|| Input.GetKeyUp(KeyCode.W)
			|| Input.GetKeyUp(KeyCode.UpArrow)
			|| Input.GetKeyUp(KeyCode.JoystickButton0)  // A button
			|| Input.GetKeyUp(KeyCode.JoystickButton1)) // B button
		{
			if (body.velocity.y > 0)
				body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
		}

		// Animation parameters
		anim.SetBool("running", horizontalInput != 0);
		anim.SetBool("grounded", isGrounded());

		// Fall animation
		if (body.velocity.y < -1f && !onWall())
			anim.SetTrigger("falling");
	}

	// Physics calculations are done in FixedUpdate in order to be consistent regardless of frame rate
	private void FixedUpdate()
	{
		// Locks controls after wall jump
		if (wallJumpCooldown > 0.2f)
		{
			// Flips player sprite depending on direction
			if (horizontalInput > 0.01f) transform.localScale = new Vector3(3, 3, 1);
			else if (horizontalInput < -0.01f) transform.localScale = new Vector3(-3, 3, 1);

			// Makes player stick to walls
			if (onWall() && !isGrounded() && body.velocity.y < -1f)
			{
				body.gravityScale = 5;
				body.velocity = Vector2.zero;
				anim.SetTrigger("wallJumping");
			}
			else
			{
				body.gravityScale = 3;
				body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

				// Resets coyote timer & jump counter when on ground
				if (isGrounded())
				{
					coyoteTimer = coyoteTime;
					jumpCounter = extraJumps;
				}
				// Decreases coyote timer when not on ground
				else
					coyoteTimer -= Time.fixedDeltaTime;
			}
		}
		else
			wallJumpCooldown += Time.fixedDeltaTime;
	}

	// Jump function
	private void Jump()
	{
		// Cancels when the player should not be able to jump
		if (coyoteTimer < 0 && !onWall() && jumpCounter <= 0) return;		

		// If on a wall, wall jump instead
		if (onWall() && !isGrounded())
			WallJump();
		else
		{
			// The player can jump when
			// - they are on the ground
			if (isGrounded())
				body.velocity = new Vector2(body.velocity.x, jumpPower);
			// - the coyote timer is still active
			else if (coyoteTimer > 0)
				body.velocity = new Vector2(body.velocity.x, jumpPower);
			// - they are in the air and have extra jumps left (also decrements jump counter)
			else if (jumpCounter > 0)
			{
				body.velocity = new Vector2(body.velocity.x, jumpPower);
				jumpCounter--;
			}

			coyoteTimer = 0;
		}

		SoundManager.instance.PlaySound(jump);
		anim.SetTrigger("jumping");

		/* Double jump animation (not used because it's inconsistent)
		if (jumpCounter == 1)
			anim.SetTrigger("jumping");
		else
			anim.SetTrigger("doubleJumping");
		*/
	}

	// Wall jump function
	private void WallJump()
	{
		body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
		transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * 3, 3, 1);
		wallJumpCooldown = 0;

		jumpCounter = 0;
	}

	// Plays footstep sound, called by animation event
	private void PlayFootstepSound()
	{
		SoundManager.instance.PlaySound(footsteps);
	}

	// Checks if the player is on the ground
	private bool isGrounded()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
		return raycastHit.collider != null;
	}

	// Checks if the player is on a wall
	private bool onWall()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.02f, wallLayer);
		return raycastHit.collider != null;
	}
}
