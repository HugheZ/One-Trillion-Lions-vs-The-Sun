using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

    public float spinAmount; //amount to spin each frame
    float timer; //times how long to wait
    public float lifeTime; //how long to stay

    // Use this for initialization
    void Start(){
        timer = 0;
    }

    // Update is called once per frame
    void Update () {
        //if after lifetime, kill
        timer += Time.deltaTime;
        if (timer >= lifeTime) Destroy(gameObject);

        //still has time to live, rotate
        transform.Rotate(new Vector3(0, spinAmount, 0), Space.Self);
	}
}
