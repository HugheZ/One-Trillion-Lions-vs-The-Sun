using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public bool big; //is big or small
    public int type; //type of enemy, 0 for bigsteroid, 1 or smallsteroid, 2 for star
    public float maxSpeed; //maximum speed
    public float minSpeed; //minimum speed
    public int pointValue; //value in points
    public Vector3 direction; //direction of the asteroid
    protected LevelManager lm; //level manager
    public Rigidbody rb; //rigidbody

	// Use this for initialization
	void Start () {
        lm = LevelManager.Instance;
	}

    //On enable or activate, give direction
    protected void OnEnable()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!lm) lm = LevelManager.Instance;
        direction = new Vector3(Random.Range(minSpeed, maxSpeed), Random.Range(minSpeed, maxSpeed), Random.Range(minSpeed, maxSpeed));
        //apply random sign
        for(int i = 0; i < 3; i++)
        {
            if (Random.Range(0, 2) == 0) direction[i] *= -1;
        }
        ApplyDirection();
    }

    //Applies new direction, here to change if I do orbit or not
    protected void ApplyDirection()
    {
        rb.velocity = direction;
    }

    //Collision logic, die if hit by bullet, else reverse direction
    protected void OnCollisionEnter(Collision collision)
    {
        //hit by player bullet, die, else reverse direction
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            lm.ObjectDied(transform.position, pointValue, type);
            if (big) lm.BigsteroidDied(transform.position);
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Barrier"))
        {
            BarrierWall bw = collision.gameObject.GetComponent<BarrierWall>();
            //not hit by bullet, reverse direction depending on wall
            if (bw.orientation.x != 0) direction = new Vector3(-direction.x, direction.y, direction.z);
            else if (bw.orientation.y != 0) direction = new Vector3(direction.x, -direction.y, direction.z);
            else direction = new Vector3(direction.x, direction.y, -direction.z);
            ApplyDirection();
        }
        else
        {
            direction = rb.velocity;
        }
    }

    //On trigger enter, check if beam and die if so
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            lm.ObjectDied(transform.position, pointValue, type);
            if (big) lm.BigsteroidDied(transform.position);
            gameObject.SetActive(false);
        }
    }
}
