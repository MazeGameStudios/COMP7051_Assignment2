using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	private AudioSource bounceSfx;


	void Awake() 
	{
		bounceSfx = GetComponent<AudioSource> ();
	}


	void OnCollisionEnter(Collision collision) 
	{
        if (!bounceSfx.isPlaying)
            bounceSfx.Play();

        if (collision.transform.CompareTag ("Enemy")) {
			// this also needs to play a sfx 
			Destroy (gameObject);
			Destroy (collision.gameObject);
            MazeGameManager.instance.score += 1;
		} else {
		}
	
	}

}
