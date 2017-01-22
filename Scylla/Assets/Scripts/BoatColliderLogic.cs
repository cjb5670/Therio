using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatColliderLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = this.transform.parent.position;
        this.transform.rotation = Quaternion.identity;
	}
}
