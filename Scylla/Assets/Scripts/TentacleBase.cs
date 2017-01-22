using System.Collections; using System.Collections.Generic;
using UnityEngine;

public class TentacleBase : MonoBehaviour {

    public int apathy;
    public GameObject Tentacle;
    public int segments;
    public float speed;
    public KeyCode key;
    public Monster_Mouth TheMouth;
    public Color GlowColor; 
    
    private GameObject LastTentacle;
    private GameObject WhatIamHolding; 
    private System.Action DoingMove;
    private List<GameObject> tentacleChain; 

    // Use this for initialization
    void Start()
    {
        var myRidgid = this.GetComponent<Rigidbody2D>();
        tentacleChain = new List<GameObject>(); 

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

            //adds it to the chain 
            tentacleChain.Add(link);
        }

        var obj = LastTentacle.GetComponent(typeof(TentacleJoint)) as TentacleJoint;
        obj.CanGrab = true;
        var rig = LastTentacle.GetComponent<Rigidbody2D>();

        TheMouth.evt_HasEatenMarine += TheMouth_evt_HasEatenMarine;
    }

    public bool IsHoldinOntoAWall()
    {
        Debug.Log("afdsfadsfasdfadfds");
        var obj = LastTentacle.GetComponent(typeof(TentacleJoint)) as TentacleJoint;

                Debug.Log("HAsd grabbedf wall dsa fasd f asd fa sdf a dsf? " + obj.HasGrabbedWall);
        return obj.HasGrabbedWall;
    }

    private void TheMouth_evt_HasEatenMarine(object sender, System.EventArgs e)
    {
        var obj = LastTentacle.GetComponent(typeof(TentacleJoint)) as TentacleJoint;
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

    private void ShowGlow()
    {
        foreach (var link in this.tentacleChain)
        {
            Behaviour halo = (Behaviour)link.GetComponent("Halo");
            halo.enabled = true;
        }
    }
   
    private void HideGlow()
    {
        foreach (var link in this.tentacleChain)
        {
            Behaviour halo = (Behaviour)link.GetComponent("Halo");
            halo.enabled = false;
        }
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
                MoveTentacle(Camera.main.ScreenToWorldPoint(pos), this.transform.position, LastTentacle, speed);
            }
        }

        if (Input.GetKey(this.key))
            ShowGlow();
        else
            HideGlow(); 

        if (Input.GetKey(KeyCode.Space))
        {
            var tj = LastTentacle.GetComponent(typeof(TentacleJoint)) as TentacleJoint;
            tj.ReleaseUnit(); 
        }
    }
} 