﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PongSwitcher : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("PongMain", LoadSceneMode.Single);
    }

}
