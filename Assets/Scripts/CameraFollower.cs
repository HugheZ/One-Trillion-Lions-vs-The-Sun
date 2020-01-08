using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public Vector3 followOffset; //offset distance from player
    public float followSpeed; //speed to follow
    public GameObject player; //player to follow

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //reference velocity
        Vector3 velocity = Vector3.zero;

        //Follow the player
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.TransformPoint(followOffset), ref velocity, followSpeed * Time.deltaTime);
        transform.rotation = player.transform.rotation;
	}
}
