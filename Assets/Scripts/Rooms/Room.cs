using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private Vector3[] initialPosition;

	// Stores the initial position of all enemies in the room
    private void Awake()
    {
		initialPosition = new Vector3[enemies.Length];
		for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position;
		}
	}

	// Activates or deactivates all enemies in the room depending on if the player is in the room or not
    public void ActivateRoom(bool _status)
    {
		for (int i = 0; i < enemies.Length; i++)
		{
			if (enemies[i] != null)
			{
				enemies[i].SetActive(_status);
				enemies[i].transform.position = initialPosition[i];
			}
		}
	}
}
