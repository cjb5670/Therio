using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MonsterLogic : MonoBehaviour {

    public TentacleBase[] AllMyArms;
    public KeyCode KeyJump;
    public float JumpPower;

    public int MaxHealth;
    public int CurrentHealth; 

	// Use this for initialization
	void Start () {
		
	}

    public void DoJump()
    {
        bool hit = false;
        foreach (var arms in AllMyArms)
        {
            if (arms.IsHoldinOntoAWall())
            {
                hit = true;
            }
        }

        if (!hit) return; 

        Vector3 inFront = (transform.localRotation * new Vector3(0, 1, 1)) * JumpPower;
        foreach (var arms in AllMyArms)
        {
            arms.ReleaseFromWall(); 
        }

        var rig = this.GetComponent<Rigidbody2D>();
        rig.AddForce(inFront);

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Spear")
        {
            CurrentHealth--;
            CheckForDeath(); 
            StartCoroutine(DestroyTimer(coll.gameObject));
        }
    }

    private void CheckForDeath()
    {
        if (CurrentHealth <= 0)
        {
			SceneManager.LoadSceneAsync("Lose");
        }
    }

    private IEnumerator DestroyTimer(GameObject obj)
    {
        yield return new WaitForSeconds(1);
        Destroy(obj);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyJump))
        {
            DoJump(); 
        }
    }
}
