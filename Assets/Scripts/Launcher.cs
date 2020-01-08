using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

    public GameObject lionPrototype; //lion projectile prototype
    ObjectPool ammo; //object pool
    public bool growable; //whether the pool can grow or not
    public int outstandingCount; //number of lions capable of being shot out at once
    public Vector3 relativeSpawnPoint; //spawn point relative to ship for bullets
    public GameObject lionBeam; //the mythical lion beam

    //bullet type bools
    public bool standard; //shooting standard bullet
    public bool beam; //shooting beam

	// Use this for initialization
	void Start () {
        ammo = new ObjectPool(lionPrototype, growable, outstandingCount);
	}
	
	// Update is called once per frame
	void Update () {
        //if standard, sense click
        if (standard)
        {
            //if left click, spawn lion if capable
            if (Input.GetMouseButtonDown(0))
            {
                GameObject lion = ammo.GetObject();
                //if not null, we can shoot
                if (lion)
                {
                    //place in correct spot and rotate
                    lion.transform.position = gameObject.transform.TransformPoint(relativeSpawnPoint);
                    lion.transform.rotation = gameObject.transform.rotation;
                    //activate
                    lion.SetActive(true);
                }
                else
                {
                    //left logic here, TODO see if I have time to put a sound in for out of ammo shots
                }
            }
        }
        else if (beam)
        {
            //if left click, shoot beam. Else if left click up, disable beam
            if (Input.GetMouseButtonDown(0))
            {
                lionBeam.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lionBeam.SetActive(false);
            }
        }
    }

    //sets the firing mechanism
    public void SetBeam(bool enable)
    {
        //if enable, enable beam, else disable
        if (enable)
        {
            beam = true;
            standard = false;
        }
        else
        {
            standard = true;
            beam = false;
            //disable beam if active
            lionBeam.SetActive(false);
        }
    }
}
