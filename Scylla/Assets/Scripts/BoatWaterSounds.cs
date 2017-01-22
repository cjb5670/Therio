using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatWaterSounds : MonoBehaviour {

    public float vol;
    public GameObject Monster; 
    private AudioSource source;
    public AudioClip[] m_Water;

	// Use this for initialization
	void Start () {
        source = this.GetComponent<AudioSource>(); 
	}

    // Update is called once per frame
    void Update()
    {

        if (!source.isPlaying)
        {
            var loc = Random.Range(0, m_Water.Length -1);
            playClip(m_Water[loc]);
        }
    }

    private void playClip(AudioClip clip)
    {
        var getVol = vol * (float)(1 / Vector3.Distance(this.transform.position, Monster.transform.position));
        source.pitch = Random.Range(0.8f, 1);
        source.PlayOneShot(clip, getVol);
    }
}
