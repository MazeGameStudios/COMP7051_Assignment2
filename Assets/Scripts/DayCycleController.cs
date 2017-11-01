using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleController : MonoBehaviour {

    public bool isDaytime = true;
    public Material daySky, nightSky;
    public Color dayColor = Color.white;
    public Color nightColor = Color.blue;


    void Start()
    {
        ChangeDayCycle(isDaytime);
    }
	
	void Update ()
    {
		if (Input.GetButtonDown("ToggleDay"))
            ChangeDayCycle(!isDaytime);
    }

    private void ChangeDayCycle(bool isDay)
    {
        Debug.Log("Changing day cycle");

        this.isDaytime = isDay;

        if (isDay)
        {
            RenderSettings.skybox = daySky;
            RenderSettings.ambientLight = dayColor;
        } 
        else
        {
            RenderSettings.skybox = nightSky;
            RenderSettings.ambientLight = nightColor;
        }
    }
}
