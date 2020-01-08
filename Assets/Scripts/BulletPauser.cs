using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPauser : MonoBehaviour {

    Vector3 velocity; //holder for velocity
    Rigidbody rb; //rigidbody

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    //Handles pausing, stops bullet movement
    void Pause()
    {
        velocity = rb.velocity;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    //Handles unpausing, resumes bullet movement
    void Resume()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = velocity;
    }
}
