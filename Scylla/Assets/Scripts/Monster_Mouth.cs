using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Mouth : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Marine")
        {
            coll.gameObject.GetComponent<GuyScript>().IsEaten();
        }
    }   

    // Update is called once per frame
    void Update()
    {

    }
}
