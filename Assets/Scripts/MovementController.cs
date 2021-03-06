﻿using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class MovementController : MonoBehaviour{

    public Transform startPosition;
    public float speed = 10.0f;
    public float sprintModifier = 3.0f;
    public float jumpForce = 400f;

	public GameObject ballPrefab; 
	public Transform throwingPosition;
	public float throwingSpeed = 10f;
	public float ballLifetime = 2f;
    public AudioSource footStepSound;

    private int playerLayer, godLayer;


    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        godLayer = LayerMask.NameToLayer("God");

        //GameObject go = GameObject.Find("Maze(Clone)");

        //Transform[] transforms = go.GetComponentsInChildren<Transform>();
        //foreach (Transform t in transforms)
        //    if (t.gameObject.name == "entrance")
        //        easyMaceEntrance = t.gameObject.transform;

        if(startPosition != null)
        {
            transform.position = startPosition.position;
        }
    }



    // TODO: Change movement to be based on velocity
    void Update()
    {
        float deltaX = Input.GetAxisRaw("Horizontal");
        float deltaZ = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(deltaX * speed, 0, deltaZ * speed);
        movement = Vector3.ClampMagnitude(movement, speed);     // limits diagonal movement to the same speed as movement along an axis
        movement *= Time.deltaTime;

        if (deltaX != 0 || deltaZ != 0)
        {
            if (!footStepSound.isPlaying)
            {
                footStepSound.Play();
            }
        }
        

        if (Input.GetKey(KeyCode.LeftShift)) movement *= sprintModifier;
        
        transform.Translate(movement);

        if (Input.GetKeyDown(KeyCode.Home) || Input.GetButtonDown("PS4Restart") ) transform.position = startPosition.position;
        if (Input.GetKeyDown(KeyCode.Space)) GetComponent<Rigidbody>().AddForce(0, jumpForce, 0);
        if (Input.GetButtonDown("ToggleWall")) transform.gameObject.layer = (transform.gameObject.layer == playerLayer) ? godLayer : playerLayer;
        if (Input.GetButtonDown("ThrowBall")) ThrowBall();
    }

	void ThrowBall() 
	{
		var ball = Instantiate (ballPrefab, throwingPosition.position, throwingPosition.rotation);
		ball.GetComponent<Rigidbody> ().velocity = Camera.main.transform.forward * throwingSpeed;
		Destroy (ball, ballLifetime);
	}

}
