using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceJunkPauser : MonoBehaviour {

    public bool isStar; //is the object a star? Saves calculations on pausing
    Asteroid asteroid; //asteroid reference
    Star star; //star reference
    Rigidbody rb; //rigidbody
    Vector3 velocity; //saves velocity between pauses

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        asteroid = GetComponent<Asteroid>();
        if (isStar) star = GetComponent<Star>();
    }

    //On enable to listen to events
    private void OnEnable()
    {
        Messenger.AddListener(GameEvents.PAUSE_EVENT, Pause);
        Messenger.AddListener(GameEvents.RESUME_EVENT, Resume);
    }

    //On disable to stop events
    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvents.PAUSE_EVENT, Pause);
        Messenger.RemoveListener(GameEvents.RESUME_EVENT, Resume);
    }

    //Handles pausing, pauses all components and saves velocity
    void Pause()
    {
        asteroid.enabled = false;
        if (isStar)
        {
            star.SetPause(true);
            star.enabled = false;
        }
        velocity = rb.velocity;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    //Handles unpausing, unpauses all components and restores velocity
    void Resume()
    {
        asteroid.enabled = true;
        if (isStar)
        {
            star.SetPause(false);
            star.enabled = true;
        }
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = velocity;
    }

}
