using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatColliderLogic : MonoBehaviour {

    public event System.EventHandler evt_MonsterHitMe;
    public float velThreshold;

    // Use this for initialization
    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "MonsterHead")
        {

            var mag = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;

            Debug.Log("HIT WITH:" + mag);
            if (mag > velThreshold)
            {
                
                if (this.evt_MonsterHitMe != null)
                    this.evt_MonsterHitMe(this, new System.EventArgs());
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        this.transform.position = this.transform.parent.position;
        this.transform.rotation = Quaternion.identity;
	}
}
