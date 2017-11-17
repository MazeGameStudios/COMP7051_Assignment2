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
		if (Input.GetKeyDown(KeyCode.U))
        {
            TurnFog(!RenderSettings.fog);
        }
	}

    public void TurnFog(bool on)
    {
        RenderSettings.fog = on;

        // TODO: Should only change BGM volume. Also needs to consider enemy modulation.
        if (on)
        {
            AudioListener.volume = 0.5f;
        }
        else
        {
            AudioListener.volume = 1f;
        }
    }
}
