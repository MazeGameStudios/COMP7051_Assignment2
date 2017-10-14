using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeOpening : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered/exited a maze");
        }
    }
}
