using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLogic : MonoBehaviour {

    public TentacleBase[] AllMyArms;
    public KeyCode KeyJump;
    public float JumpPower; 

	// Use this for initialization
	void Start () {
		
	}

    public void DoJump()
    {

        Vector3 inFront = (transform.localRotation * new Vector3(0, 1, 1)) * JumpPower;
        Debug.Log(string.Format("magdir x:{0}, y:{1}", inFront.x, inFront.y));

        foreach (var arms in AllMyArms)
        {
            arms.ReleaseFromWall(); 
        }

        var rig = this.GetComponent<Rigidbody2D>();
        rig.AddForce(inFront);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyJump))
        {
            DoJump(); 
        }
    }
}
