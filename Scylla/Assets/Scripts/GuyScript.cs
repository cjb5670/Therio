using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyScript : MonoBehaviour
{
    public float WaterLevel;
    public float NotInWaterDrag;
    public float InWaterDrag;

    // Use this for initialization
    void Start()
    {

    }

    public void IsEaten()
    {
        Debug.Log("I got eaten");
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var joint = this.GetComponent<SpringJoint2D>();
        joint.connectedAnchor = new Vector2(this.transform.position.x, this.WaterLevel);

        if(this.transform.position.y >= WaterLevel)
        {
            var rig = this.GetComponent<Rigidbody2D>();
            rig.drag = NotInWaterDrag;
            joint.enabled = false;
        }
        else
        {
            var rig = this.GetComponent<Rigidbody2D>();
            rig.drag = InWaterDrag;
            joint.enabled = true;
        }

    }
}
