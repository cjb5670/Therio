using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour {

    private Vector3 mouseTarget;

    // Use this for initialization
    void Start()
    {
        mouseTarget = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Input.mousePosition;
            pos.z = 47; 
            
            mouseTarget = Camera.main.ScreenToWorldPoint(pos);

        }

            var speed = 10f;
            this.transform.position = Vector3.Lerp(this.transform.position, mouseTarget, speed * Time.deltaTime);


        if (this.transform.position == mouseTarget)
        {
           // mouseTarget = Vector3.zero; 
        }

	}
}
