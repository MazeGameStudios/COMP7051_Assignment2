using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour {

    public bool isOn = true;
    private Light flashlight;


    void Start ()
    {
        flashlight = GetComponent<Light>();
        SwitchFlashlight(isOn);
    }
	
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchFlashlight(!isOn);
        }
	}

    public void SwitchFlashlight(bool on)
    {
        isOn = on;
        flashlight.enabled = on;
    }
}
