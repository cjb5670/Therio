using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleJoint : MonoBehaviour
{
    public GameObject blood;
    public int UnitLayer;
    public int WallLayer;
    public bool CanGrab;

    public TentacleBase Parent;

    public bool HasGrabbedWall { get; private set; }
    public bool HasGrabbedPerson { get; private set; }

    private System.Action Grabber;
    private System.Action GrabberArm;

    // Use this for initialization
    void Start()
    {
    }

    void OnJointBreak2D(Joint2D breakForce)
    {
          blood.SetActive(true);
    }

    public void ReleaseUnit()
    {
        Grabber = null;
        HasGrabbedPerson = false;
    }

    public void ReleaseArm()
    {
        GrabberArm = null;
        HasGrabbedWall = false;
        Parent.ReleaseFromWall();
        this.GetComponent<TargetJoint2D>().maxForce = 10;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (HasGrabbedPerson || HasGrabbedWall) return;

        var hit = coll.contacts[0];
        var wallLoc = new Vector3(hit.point.x, hit.point.y, this.transform.position.z);

        if (coll.gameObject.layer == UnitLayer && CanGrab)
        {
            var mainObj = coll.gameObject;
            var obj = coll.gameObject.GetComponent<TargetJoint2D>();

            Grabber = () => 
            {
                if (mainObj.gameObject != null) mainObj.transform.position = this.transform.position;
            };

            this.HasGrabbedPerson = true;
        }
        else if (coll.gameObject.layer == WallLayer && CanGrab)
        {
            this.GetComponent<TargetJoint2D>().target = wallLoc;
            this.GetComponent<TargetJoint2D>().enabled = true;
            this.GetComponent<TargetJoint2D>().maxForce = 500;
            this.Parent.PullToWall(wallLoc);
            
            this.HasGrabbedWall = true;
        }
    }

    private Vector3 mouseTarget;
    
    // Update is called once per frame
    void Update()
    {
        if (CanGrab)
            Debug.Log("I can grab");

        if (Grabber != null) Grabber();
    }

} 