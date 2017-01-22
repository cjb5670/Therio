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
    public AudioClip[] YellingClips;
    public GameObject Monster; 

    private AudioSource source;
    private Rigidbody2D rig;

    // Use this for initialization
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody2D>();
    }

    public void IsEaten()
    {
        Debug.Log("I got eaten");
        Destroy(this.gameObject);
    }

    private void playClip(AudioClip clip)
    {
        var getVol = vol * (float)(1 / Vector3.Distance(this.transform.position, Monster.transform.position));
        source.pitch = Random.Range(0.8f, 1);
        source.PlayOneShot(clip, getVol);
    }

    // Update is called once per frame
    void Update()
    {
        var joint = this.GetComponent<SpringJoint2D>();
        joint.connectedAnchor = new Vector2(this.transform.position.x, this.WaterLevel);

        if(this.transform.position.y >= WaterLevel)
        {
            rig.drag = NotInWaterDrag;
            joint.enabled = false;
        }
        else
        {
            rig.drag = InWaterDrag;
            joint.enabled = true;

            if(!source.isPlaying)
            {
                var loc = Random.Range(0, 4);
                playClip(DrowningClips[loc]);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            rig.AddForce(new Vector2(Random.Range(-10,10), 20));
            var loc = Random.Range(0, 4);
            playClip(YellingClips[loc]);
        }

    }
}
