using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFacilitator : MonoBehaviour {

    LevelManager lm; //level manager
    PowerupFacilitator pf; //handles powerups
    Launcher launcher; //handles launcher
    Mover mover; //handles movement
    Rigidbody rb; //handles the rigidbody
    public AudioSource powerupReady; //ready source for powerup and life
    Vector3 velocity; //stores velocity across pausing
    Vector3 angularVelocity; //stores angular velocity across pausing
    bool invulnerable; //is the player invulnerable
    public float invulnerableTime; //how long the player remains invulnerable
    public GameObject shield; //player's invulnerability shield

    // Use this for initialization
    void Start () {
        invulnerable = false;
        launcher = GetComponent<Launcher>();
        mover = GetComponent<Mover>();
        rb = GetComponent<Rigidbody>();
        pf = GetComponent<PowerupFacilitator>();
        lm = LevelManager.Instance;
	}

    //On enable add to listen to events
    //OnEnable is called at spawn, so also enable invulnerability
    private void OnEnable()
    {
        StartCoroutine(Respawn());
        Messenger.AddListener(GameEvents.PAUSE_EVENT, Pause);
        Messenger.AddListener(GameEvents.RESUME_EVENT, Resume);
    }

    //On disable remove from listener
    private void OnDisable()
    {
        Messenger.RemoveListener(GameEvents.PAUSE_EVENT, Pause);
        Messenger.RemoveListener(GameEvents.RESUME_EVENT, Resume);
    }

    //Pause logic, pauses all components and saves velocities
    void Pause()
    {
        mover.enabled = false;
        pf.enabled = false;
        launcher.enabled = false;
        velocity = rb.velocity;
        angularVelocity = rb.angularVelocity;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    //Unpause logic, pauses all components and restores velocities
    void Resume()
    {
        mover.enabled = true;
        pf.enabled = true;
        launcher.enabled = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = velocity;
        rb.angularVelocity = angularVelocity;
    }

    //Keeps the player invulnerable upon respawn
    IEnumerator Respawn()
    {
        invulnerable = true;
        shield.SetActive(true);

        yield return new WaitForSeconds(invulnerableTime);

        invulnerable = false;
        shield.SetActive(false);
        
    }

    //Collision logic
    private void OnCollisionEnter(Collision collision)
    {
        //if invulnerable and hit space junk, die
        if (!invulnerable && (collision.gameObject.CompareTag("SpaceJunk") || collision.gameObject.CompareTag("Barrier")))
        {
            lm.PlayerDied();
        }
    }

    //Trigger logic for powerup
    //NOTE: not playing sound and destroying by default in case I implement other trigger-based objects
    private void OnTriggerEnter(Collider other)
    {
        //if laser powerup, enable it. Else if life powerup, give a life
        if (other.gameObject.CompareTag("PowerupLaser"))
        {
            powerupReady.Play();
            pf.StartPowerup();
            //destroy powerup
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("PowerupLife"))
        {
            powerupReady.Play();
            lm.GiveLife();
            //destroy life
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Points"))
        {
            powerupReady.Play();
            lm.GivePoints(lm.extraPointsValue, -1);
            //destroy points
            Destroy(other.gameObject);
        }
    }
}
