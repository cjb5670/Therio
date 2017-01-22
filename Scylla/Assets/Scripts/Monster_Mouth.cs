using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class Monster_Mouth : MonoBehaviour
{
    public GameObject blood;
    public event EventHandler evt_HasEatenMarine;
    public GameObject EatingMarine { get; private set; }

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

            evt_HasEatenMarine(this,new EventArgs());
            obj.IsEaten();
            EatingMarine = null;

            var BloodSplatter = Instantiate(blood, blood.transform.position, Quaternion.identity);
            BloodSplatter.SetActive(true);
            StartCoroutine(DestroyTimer(BloodSplatter, 2.5f));

        }
    }

    private IEnumerator DestroyTimer(GameObject toDestroy, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(toDestroy);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
