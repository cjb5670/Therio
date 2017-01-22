using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyScript : MonoBehaviour
{
    public float WaterLevel;
    public float NotInWaterDrag;
    public float InWaterDrag;
    public float vol;
     
    public AudioClip[] DrowningClips;
    public GameObject Monster; 

    private AudioSource source; 

    // Use this for initialization
    void Start()
    {
        source = this.GetComponent<AudioSource>();
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

            if(!source.isPlaying)
            {
                var getVol = vol * (float)(1/Vector3.Distance(this.transform.position, Monster.transform.position));
                Debug.Log("getvol = "+getVol);
                source.pitch = Random.Range(0.8f, 1);
                var loc = Random.Range(0, DrowningClips.Length - 1);
                source.PlayOneShot(DrowningClips[loc], getVol);
            }
        }

    }
}
