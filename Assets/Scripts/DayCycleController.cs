using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleController : MonoBehaviour {

    public bool isDaytime = true;
    public Material daySky, nightSky;
    public Color dayColor = Color.white;
    public Color nightColor = Color.blue;
    private Camera cam;


    void Start()
    {
        cam = Camera.main;
        ChangeDayCycle(isDaytime);
    }
	
	void Update ()
    {
		if (Input.GetButtonDown("ToggleDay"))
            ChangeDayCycle(!isDaytime);
    }

    public void ChangeDayCycle(bool isDay)
    {
        Debug.Log("Changing day cycle");

        this.isDaytime = isDay;

        if (isDay)
        {

            cam.GetComponent<Skybox>().material = daySky;
            RenderSettings.ambientLight = dayColor;
        }
        else
        {
            cam.GetComponent<Skybox>().material = nightSky;
            RenderSettings.ambientLight = nightColor;
        }

        DynamicGI.UpdateEnvironment();

    }
}
