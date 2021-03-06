﻿using UnityEngine;
using System.Collections;

public class Wolf : MonoBehaviour {

    //To make this script work, simply add some empty gameobjects as locations for the dragon to move towards.
    Animator wolfAnimator;
    public Transform[] destinations;
    public float speed = 10.0f;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private Transform nextTarget;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float scanRadius = 20.0f, scanInterval = 2.0f, stalkDistance = 20.0f;
    [SerializeField]
    private bool isStalking, isSitting;

    Vector3 moveToVector;
    private float wolfSightDistance = 3.0f;

    private Transform playerTransform;

    void Start()
    {
        wolfAnimator = GetComponent<Animator>();
        nextTarget = destinations[0];
        distance = 0f;
        currentSpeed = speed;
        //StartCoroutine(ScanSurroundings(scanRadius, scanInterval));
    }

    public void WolfSit(bool isSitting)
    {
        wolfAnimator.SetBool("isSit", isSitting);
    }

    public void WolfFollow(bool isFollowing)
    {
        wolfAnimator.SetBool("isFollow", isFollowing);
    }

    public void StartStalkingPlayer()
    {
        StartCoroutine(ScanSurroundings(scanRadius, scanInterval));
    }

    private IEnumerator ScanSurroundings(float radius, float timeBetweenScans)
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenScans);

            Vector3 center = transform.position;
            center.y += 1f;

            Collider[] hitColliders = Physics.OverlapSphere(center, radius);

            int i = 0;
            while (i < hitColliders.Length)
            {
                if(hitColliders[i].tag == "Player") //make sure to tag player as Player
                {
                    isStalking = true;
                    playerTransform = hitColliders[i].transform;
                }
                i++;
            }

        }
    }

    void Sit()
    {
        wolfAnimator.SetBool("isFollow", false);
        wolfAnimator.SetBool("isSit", true);
    }

    void StalkPlayer()
    {

        Vector3 targetOffset = new Vector3(0, 1, 0);

        float step = speed * Time.deltaTime;
        
        distance = Vector3.Distance(transform.position, playerTransform.position);
        
        if(distance > 5.0f)
        {
            moveToVector = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) + targetOffset;
            transform.LookAt(moveToVector);
            transform.position = Vector3.MoveTowards(transform.position, moveToVector, step);
        }

        if (distance > stalkDistance)
        {
            isStalking = false;
            wolfAnimator.SetBool("isFollow", false);
        }
        else if (distance <= 5.0f)
        {
            Sit();
        }
        else
        {
            wolfAnimator.SetBool("isSit", false);
            wolfAnimator.SetBool("isFollow", true);
        }
    }

    void Update()
    {

        if (isSitting)
        {
            Sit();
        }
        else if(isStalking)
        {
            StalkPlayer();
        }
        else
        {
            /*
            if (distance < 1)
            {
                float choice = Random.Range(0, 3);
                if (choice == 0)
                {
                    wolfAnimator.SetFloat("speed", 0.3f);
                    currentSpeed = speed;
                }
                else if (choice == 1)
                {
                    wolfAnimator.SetFloat("speed", 0.3f);
                    currentSpeed = speed;
                }
                else if (choice == 2)
                {
                    wolfAnimator.SetFloat("speed", 1.0f);
                    currentSpeed = speed * 2.0f;
                }
                SetTarget();
            }
            else
            {
                MoveTowardsTarget(nextTarget);
            }*/

            wolfAnimator.SetFloat("speed", 0.3f);
            currentSpeed = speed;
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            /*
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, wolfSightDistance))
            {
                print("wolf hit somethang");
            }*/
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            float val = Random.Range(0, 180);
            transform.Rotate(0, val, 0);
        }
    }

    public void SetTarget()
    {
        int ranValue = Random.Range(0, destinations.Length);
        nextTarget = destinations[ranValue];

        distance = Vector3.Distance(transform.position, nextTarget.position);
    }

    public void MoveTowardsTarget(Transform target)
    {
        float step = speed * Time.deltaTime;
        distance = Vector3.Distance(transform.position, target.position);
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

}
