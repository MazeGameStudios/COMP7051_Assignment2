
using UnityEngine;
//using UnityEngine.UI;

public class SkyBoxChange : MonoBehaviour {

    public Camera playerCamera;
    public Material[] skyBoxes;
    private int index = 0;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (index >= skyBoxes.Length) index = 0;
        //RenderSettings.skybox = skyBoxes[index++];
        playerCamera.GetComponent<Skybox>().material = skyBoxes[index++];
    }

}
