using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPauser : MonoBehaviour {

    Spinner timer;

    // Use this for initialization
    void Start()
    {
        timer = GetComponent<Spinner>();
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

    //Handles pausing, stops timer
    void Pause()
    {
        timer.enabled = false;
    }

    //Handles unpausing, resumes timer
    void Resume()
    {
        timer.enabled = true;
    }
}
