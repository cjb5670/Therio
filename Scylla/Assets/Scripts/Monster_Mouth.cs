using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Monster_Mouth : MonoBehaviour
{
    public GameObject blood;
    public event EventHandler evt_HasEatenMarine;
    public readonly GameObject EatingMarine;

    // Use this for initialization
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Marine")
        {
            var obj = coll.gameObject.GetComponent<GuyScript>();
            if (obj == null) return; 

            obj.IsEaten();
            blood.active = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
