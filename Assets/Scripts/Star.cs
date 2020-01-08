using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour{

    public float shootDelayMax; //maximum shoot delay
    public float shootDelayMin; //minimum shoot delay
    public GameObject rig; //the player
    bool paused; //is the object paused
    LevelManager lm; //manager

	// Use this for initialization
	void Start () {
        //get rig and manager
        lm = LevelManager.Instance;
        rig = lm.playerRig;
        paused = false;
	}

    //Sets the pause variable
    public void SetPause(bool pause)
    {
        paused = pause;
    }
	
	void OnEnable()
    {
        //if lm has yet to be defined, define it
        if (!lm)
        {
            lm = LevelManager.Instance;
            rig = lm.playerRig;
        }
        //begin shoot cycle
        StartCoroutine(ShootCycle());
    }


    //defines the shoot cycle for the star
    private IEnumerator ShootCycle()
    {
        while (!paused)
        {
            //wait for next shot
            yield return new WaitForSeconds(Random.Range(shootDelayMin, shootDelayMax));

            //second check in case waiting since before pause
            if (!paused)
            {
                GameObject flare = lm.GetFlare();
                //if we have a flare, shoot, else do nothing
                if (flare)
                {
                    //set transform to the star and rotate to look at the player
                    flare.transform.position = transform.position;
                    flare.transform.LookAt(rig.transform.position);
                    //and shoot
                    flare.SetActive(true);
                }
            }
        }
    }
}
