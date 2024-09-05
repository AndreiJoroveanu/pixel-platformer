using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
	void Start()
    {
		// Sets the game to run at the refresh rate of the monitor
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = (int) Screen.currentResolution.refreshRateRatio.value;
    }
}
