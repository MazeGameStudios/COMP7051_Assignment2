using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour {


    public float doorAnimationSpeed = 0.01f;
    public AudioSource doorOpenSound;
    public Transform playerTransform;
    bool isOpen;
    bool isClose;

	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    }

	void Update () {
        if (playerTransform != null)
        {
            if (Vector3.Distance(playerTransform.position, transform.position) < 4f)
                StartCoroutine(OpenDoor());
        }
	}

    IEnumerator OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;

            doorOpenSound.Play();

            int yAngle = 0;
            while (yAngle > -90)
            {
                transform.Rotate(0, -1, 0);
                yAngle--;
                yield return new WaitForSeconds(doorAnimationSpeed);
            }
            yield return new WaitForSeconds(3f);
            while (yAngle < 0)
            {
                transform.Rotate(0, 1, 0);
                yAngle++;
                yield return new WaitForSeconds(doorAnimationSpeed);
            }
            isOpen = false;
        }
    }
}
