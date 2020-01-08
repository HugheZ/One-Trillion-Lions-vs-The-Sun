using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierWall : MonoBehaviour {

    public GameObject playerRig; //the player rig
    public Vector3Int orientation; //which axis corresponds to the plane
    public float minDistance; //minimum distance to activate barrier
    MeshRenderer renderer; //renderer of the wall

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update () {
		//depending on plane, activate when player gets near
        if(orientation.x != 0)
        {
            if (Mathf.Abs(playerRig.transform.position.x - transform.position.x) < minDistance)
            {
                renderer.enabled = true;
            }
            else renderer.enabled = false;
        }
        else if(orientation.y != 0)
        {
            if (Mathf.Abs(playerRig.transform.position.y - transform.position.y) < minDistance)
            {
                renderer.enabled = true;
            }
            else renderer.enabled = false;
        }
        else
        {
            if (Mathf.Abs(playerRig.transform.position.z - transform.position.z) < minDistance)
            {
                renderer.enabled = true;
            }
            else renderer.enabled = false;
        }
	}
}
