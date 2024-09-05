using UnityEngine;

public class ToggleBoundryCollision : MonoBehaviour
{
	[SerializeField] private GameObject[] boundries;
	private bool toggled = false;

	// When the player collides with the trigger, toggles the off-screen boundries (only once)
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !toggled)
		{
			for (int i = 0; i < boundries.Length; i++)
			{
				boundries[i].GetComponent<BoxCollider2D>().enabled = !boundries[i].GetComponent<BoxCollider2D>().enabled;
				toggled = true;
			}
		}
	}
}
