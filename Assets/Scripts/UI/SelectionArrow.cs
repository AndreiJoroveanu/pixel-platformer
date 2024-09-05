using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
	// Array of buttons to navigate
	[SerializeField] private RectTransform[] buttons;

	[Header("SFX")]
	[SerializeField] private AudioClip changeSound;
	[SerializeField] private AudioClip interactSound;

	// Volume sliders to enable horizontal input
	[Header("Volume Buttons")]
	[SerializeField] private int soundVolumeButton;
	[SerializeField] private int musicVolumeButton;

	private RectTransform rect;
	public int currentPosition;
	private Animator anim;

	// Inputs for menu navigation
	private float horizontalInput;
	private float verticalInput;
	private bool horizontalAxisInUse;
	private bool verticalAxisInUse;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
		anim = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		horizontalAxisInUse = true;
		verticalAxisInUse = true;
	}

	private void Update()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");
		
		// Vertical input for menu scrolling
		if (verticalInput > 0.75f && !verticalAxisInUse)
		{
			verticalAxisInUse = true;
			ChangePosition(-1);
		}
		else if (verticalInput < -0.75f && !verticalAxisInUse)
		{
			verticalAxisInUse = true;
			ChangePosition(1);
		}
		else if (verticalInput == 0 && verticalAxisInUse)
			verticalAxisInUse = false;

		// Horizontal input for volume sliders
		else if (currentPosition == soundVolumeButton)
		{
			if (horizontalInput < -0.75f && !horizontalAxisInUse)
			{
				horizontalAxisInUse = true;
				InteractSoundVolume(-0.1f);
			}
			else if (horizontalInput > 0.75f && !horizontalAxisInUse)
			{
				horizontalAxisInUse = true;
				InteractSoundVolume(0.1f);
			}
			else if (horizontalInput == 0 && horizontalAxisInUse)
				horizontalAxisInUse = false;
			else return;
		}

		else if (currentPosition == musicVolumeButton)
		{
			if (horizontalInput < -0.75f && !horizontalAxisInUse)
			{
				horizontalAxisInUse = true;
				InteractMusicVolume(-0.1f);
			}
			else if (horizontalInput > 0.75f && !horizontalAxisInUse)
			{
				horizontalAxisInUse = true;
				InteractMusicVolume(0.1f);
			}
			else if (horizontalInput == 0 && horizontalAxisInUse)
				horizontalAxisInUse = false;
			else return;
		}

		// Interacts with button (with either Enter, Space or E keys, or A button)
		else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0))
			Interact();
	}

	// Changes the arrow's position based on input
	public void ChangePosition(int _change)
	{
		currentPosition += _change;

		// Plays sound and animation if moving
		if (_change != 0)
		{
			SoundManager.instance.PlaySound(changeSound);
			anim.SetTrigger("press");
		}

		// Loops around if going out of bounds
		if (currentPosition < 0)
			currentPosition = buttons.Length - 1;
		else if (currentPosition > buttons.Length - 1)
			currentPosition = 0;

		// Moves the arrow sprite to the new position
		rect.position = new Vector3(rect.position.x, buttons[currentPosition].position.y, 0);
	}

	// Interacts with the button
	private void Interact()
	{
		buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
	}

	// Changes the sound volume
	private void InteractSoundVolume(float change)
	{
		SoundManager.instance.PlaySound(interactSound);

		SoundManager.instance.ChangeSoundVolume(change);
	}

	// Changes the music volume
	private void InteractMusicVolume(float change)
	{
		SoundManager.instance.PlaySound(interactSound);

		SoundManager.instance.ChangeMusicVolume(change);
	}
}
