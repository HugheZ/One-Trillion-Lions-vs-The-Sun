using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float maxDistance; //maximum distance the bullet can travel before despawning
    public float speed; //speed of the bullet
    public float spinRange; //range of random spin
    Vector3 origin; //start position of the bullet
    Rigidbody rb; //rigidbody of the bullet

    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody>();
    }

    //On enable and active, resets origin, gives velocity, and adds random spin
    private void OnEnable()
    {
        //set origin and speed
        origin = transform.position;
        rb.velocity = transform.forward * speed;
        rb.AddRelativeTorque(new Vector3(Random.Range(-spinRange, spinRange),
            Random.Range(-spinRange, spinRange), Random.Range(-spinRange, spinRange)), ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update () {
        //if gone too far, deactivate
        if ((transform.position - origin).magnitude > maxDistance) gameObject.SetActive(false);
	}

    //on entering collision, deactivate
    private void OnCollisionEnter()
    {
        gameObject.SetActive(false);
    }
}
