using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleController : MonoBehaviour {

    [Range(0,1)]
    public float DayRatioStart = 1f;
    public float currentDayRatio;
    public Material[] materials;


    void Start()
    {
        ChangeDayCycle(DayRatioStart);
    }
	
	void Update ()
    {
		if (Input.GetButtonDown("ToggleDay"))
            ChangeDayCycle(currentDayRatio == 0 ? 1 : 0);
    }

    private void ChangeDayCycle(float dayRatio)
    {
        currentDayRatio = dayRatio;
        foreach (Material m in materials)
            m.SetFloat("_DayRatio", dayRatio);
    }
}
