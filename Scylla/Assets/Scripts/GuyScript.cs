using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuyScript : MonoBehaviour
{

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

    }
}
