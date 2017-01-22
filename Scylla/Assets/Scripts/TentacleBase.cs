using System.Collections; using System.Collections.Generic;
using UnityEngine;

public class TentacleBase : MonoBehaviour {

    public int apathy;
    public GameObject Tentacle;
    public int segments;
    public float speed;
    public KeyCode key;

    private GameObject LastTentacle;

    // Use this for initialization
    void Start()
    {
        var myRidgid = this.GetComponent<Rigidbody2D>();

        //Create the chain :O 
        GameObject prevObj = null;
        HingeJoint2D joint = null;

        for (int i = 0; i < segments; i++)
        {
            var link = Instantiate(Tentacle, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
            link.transform.position = this.transform.position; 
            link.layer = 8;

            link.GetComponent<TentacleJoint>().Parent = this;

            joint = link.GetComponent<HingeJoint2D>();
          
         
              
            if (prevObj == null)
            {
                joint.connectedBody = myRidgid;
            }
            else
            {
                var rigid = prevObj.GetComponent<Rigidbody2D>(); 
                joint.connectedBody = rigid;
            }

            prevObj = link;
            LastTentacle = link;
        }

        var obj = LastTentacle.GetComponent(typeof(TentacleJoint)) as TentacleJoint;
        obj.CanGrab = true;
        var rig = LastTentacle.GetComponent<Rigidbody2D>();
    }

    public void PullToWall( Vector3 location)
    {
        var Wallpoint = this.GetComponent<TargetJoint2D>();
        Wallpoint.enabled = true;

        Wallpoint.target = location; 
    }

    public void ReleaseFromWall()
    {
        var Wallpoint = this.GetComponent<TargetJoint2D>();
        Wallpoint.enabled = false;
    }

    private void MoveTentacle(Vector3 targetPosition, Vector3 RootPosition,  GameObject tentacleSegment, float speed)
    {
        tentacleSegment.GetComponent<TargetJoint2D>().target = Vector3.Lerp(tentacleSegment.GetComponent<TargetJoint2D>().target, targetPosition, speed * Time.deltaTime);
        tentacleSegment.GetComponent<TargetJoint2D>().enabled = true;

        int count = 0;
        DoingMove = () => 
        {
            //Check if the Tentacle has grabbed a wall 
            var obj = LastTentacle.GetComponent(typeof(TentacleJoint)) as TentacleJoint;
            if (obj.HasGrabbedWall)
            {
                DoingMove = null;
                return; 
            }

            count++;
            tentacleSegment.GetComponent<TargetJoint2D>().target = Vector3.Lerp(tentacleSegment.GetComponent<TargetJoint2D>().target, targetPosition, speed * Time.deltaTime);

            if(count >= apathy)
                tentacleSegment.GetComponent<TargetJoint2D>().enabled = false;
        };
    }

    private Vector3 mouseTarget;
    private Vector3 mouseTarget2;

    private System.Action DoingMove; 

    // Update is called once per frame
    void Update()
    {
        if (DoingMove != null) DoingMove();

        if (Input.GetMouseButtonDown(0) && Input.GetKey(this.key))
        {
            var obj = LastTentacle.GetComponent(typeof(TentacleJoint)) as TentacleJoint;
            if (obj.HasGrabbedWall)
            {
                obj.ReleaseArm();                
            }
            else
            {
                var pos = Input.mousePosition;
                pos.z = 47;

                mouseTarget = Camera.main.ScreenToWorldPoint(pos);
                MoveTentacle(mouseTarget, this.transform.position, LastTentacle, speed);
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            var tj = LastTentacle.GetComponent(typeof(TentacleJoint)) as TentacleJoint;
            tj.ReleaseUnit(); 
        }
    }
} 