using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupFacilitator : MonoBehaviour {

    public float laserDuration; //duration of the laser
    float time; //durrent time to count down from
    public Slider lionBeamAmount; //lion beam indicator
    Launcher gun; //launcher reference

    // Use this for initialization
    void Start () {
        gun = GetComponent<Launcher>();
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {

        //if time is greater than 0, then the powerup must be in effect, starts only after StartPowerup() called
        if (time > 0)
        {
            //decrement time and update slider
            time -= Time.deltaTime;
            lionBeamAmount.value = time / laserDuration; ;
        }
        else
        {
            //time over, set slider to empty and disable powerup
            time = 0;
            lionBeamAmount.value = 0;
            gun.SetBeam(false);
        }
    }

    //Starts powerup cycle
    public void StartPowerup()
    {
        //set values on launcher
        gun.SetBeam(true);
        lionBeamAmount.value = 1;
        time = laserDuration;
    }
}
