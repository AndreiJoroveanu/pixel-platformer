using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
	[SerializeField] private Health playerHealth;
	[SerializeField] private Image totalHealthBar;
	[SerializeField] private Image currentHealthBar;

	// Divides the current health by 10, since the original image has 10 hearts
	private void Start()
	{
		totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
	}

	// Updates the health bar, still dividing by 10
	private void Update()
	{
		currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
	}
}
