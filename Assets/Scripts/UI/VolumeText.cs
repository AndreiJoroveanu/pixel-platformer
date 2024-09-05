using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
	[SerializeField] private string volumeName;
	[SerializeField] private string textIntro;

	// Updates the volume/music text in menus
	private void Update()
	{
		GetComponent<Text>().text = textIntro + (PlayerPrefs.GetFloat(volumeName) * 100).ToString("0");
	}
}
