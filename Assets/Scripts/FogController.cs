using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
	void Start ()
    {
        TurnFog(RenderSettings.fog);    // ensure initial volume levels are set
	}
	
	void Update ()
    {
		if (Input.GetButtonDown("ToggleFog"))
            TurnFog(!RenderSettings.fog);
	}

    public void TurnFog(bool on)
    {
        RenderSettings.fog = on;

        if (on)
        {
            MazeGameManager.instance.audioController.SetBgmVolumeModifier(0.3f);
        }
        else
        {
            MazeGameManager.instance.audioController.SetBgmVolumeModifier(1f);
        }
    }
}
