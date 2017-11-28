using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public AudioClip hitSfx;


	void OnCollisionEnter(Collision collision) 
	{
        // play hit sound 
        AudioSource.PlayClipAtPoint(hitSfx, collision.contacts[0].point);

        // hitting an enemy 
        if (collision.transform.CompareTag ("Enemy")) {
			Destroy (gameObject);
			// Destroy (collision.gameObject);
            MazeGameManager.instance.score += 1;
		}
	}

}
