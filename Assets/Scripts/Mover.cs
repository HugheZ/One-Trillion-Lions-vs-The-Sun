using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    //movement values
    public float speed; //speed of the ship
    public float maxSpeed; //max speed of the ship
    public float turnSpeed; //turn speed of the ship
    public float spinFactor; //factor for tweaking spin
    public bool moving; //is the ship currently moving?

    public float xDist; //distance to rotate around X axis
    public float yDist; //distance to rotate around y axis
    public float spin; //spinning about z axis value

    //physics references
    Rigidbody rb;

    //Model and effects values
    public ParticleSystem jet; //jet on the back of the player
    public AudioSource jetSound; //sound of the jet



	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //if player is pushing w, move forward
        if (Input.GetKey(KeyCode.W))
        {
            moving = true;
            if(!jet.isPlaying) jet.Play();
            if (!jetSound.isPlaying) jetSound.Play();
        }
        else
        {
            moving = false;
            jet.Stop();
            jetSound.Stop();
        }

        //if pressing spin buttons
        if (Input.GetKey(KeyCode.Q))
        {
            spin = spinFactor;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            spin = -spinFactor;
        }
        else spin = 0;

        //follow mouse to turn
        xDist = Input.GetAxis("Mouse X");
        yDist = Input.GetAxis("Mouse Y");

	}

    private void FixedUpdate()
    {
        //if moving, apply force
        if (moving)
        {
            //apply acceleration
            rb.AddForce(transform.forward*speed, ForceMode.VelocityChange);
            //if faster than max speed, reset
            if (rb.velocity.magnitude > maxSpeed) rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        //rotate
        rb.AddRelativeTorque(new Vector3(-yDist, xDist, spin) * turnSpeed * Time.deltaTime);

    }
}
