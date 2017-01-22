using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSpawn : MonoBehaviour
{
    public GameObject Monster;
    public GameObject Marine; 
    public KeyCode SpawnKey;
   
    // Use this for initialization
    void Start()
    {

    }

    public GameObject SpawnMarine()
    {
        var marine = Instantiate(Marine, new Vector3(0, 0, 0), Quaternion.identity);
        marine.transform.position = this.transform.position;
        marine.layer = 9;
        marine.SetActive(true);
        var mar_log = marine.GetComponent<GuyScript>();
        mar_log.Monster = this.Monster; 

        return marine; 
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn a player 
        if (Input.GetKeyDown(SpawnKey))
        {
            SpawnMarine();
        }
    }

}
