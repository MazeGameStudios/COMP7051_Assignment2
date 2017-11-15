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
		if (collision.transform.CompareTag ("Enemy")) {
			// this also needs to play a sfx 
			Destroy (gameObject);
			Destroy (collision.gameObject);
			// update score 
		} else {
			if (!bounceSfx.isPlaying)
				bounceSfx.Play ();
		}
	
	}

}
