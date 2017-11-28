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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCoroutine(OpenDoor());
        }

        if (playerTransform != null)
        {
            if (Vector3.Distance(playerTransform.position, transform.position) < 4f)
            {
                isClose = true;
            }
            else
            {
                isClose = false;
            }
        }
	}

    void OnGUI()
    {
        if (isClose)
        {
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUI.color = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "F1", centeredStyle);
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
