using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BoatAI : Movement
{
    #region BoatAI Member Variables
    private const float MAX_DISTANCE = 5;
    private const float MIN_DISTANCE = 2;
    private const float IN_RANGE = 4.5f;
    private bool m_fleeing = false;
    private bool m_reload = false;
    private Vector3 m_target;

    private bool _SpawningGuys;
    private List<GameObject> _GuysOnBoat;
    private object Locker_GuysOnBoat = new object();
    public int _TotalGuysSpawned { get; private set; }

    public bool m_attacking = false;
    public GameObject m_spear;
    public GameObject m_monster;
    public GameObject m_boat;
    public BoatSpawn m_Spawner;
    public int MaxGuysOnBoat;
    public int Max_Health;
    public int Current_Health;

    public AudioClip[] m_AttackClips;
    public AudioClip[] m_Overboard;
    public AudioClip[] m_GroupYell;

    #endregion

    #region BoatAI Methods
    void Start()
    {
        _GuysOnBoat = new List<GameObject>();
        m_position = transform.position;
    }
    
    void Update()
    {
        if (m_fleeing)
        {
            MoveAwayFromMonster();
            CheckIfFarAway();
        }
        else if (m_attacking && CanFire())
        {
            TargetMonster();
            Fire();
            StartCoroutine(ReloadCounter());
        }
        else
        {
            if (TooClose())
            {
                m_fleeing = true;
                return;
            }
            else if (InRange() && !m_reload)
            {
                m_attacking = true;
                return;
            }
            else
            {
                MoveTowardsMonster();
            }
        }

        //Checks if a guy should be spawned
        bool ShouldSpawn = false; 
        lock (Locker_GuysOnBoat)
        {
            ShouldSpawn = _GuysOnBoat.Count < MaxGuysOnBoat &&
                          _TotalGuysSpawned < Max_Health &&
                          !_SpawningGuys;
        }
        if (ShouldSpawn)
        {
            StartCoroutine(SpawnGuy(Random.Range(0.5f, 3.0f)));
        }

        CalculateForces(_GuysOnBoat);
        ClampAngle(0);
        UpdateFacingDirection(0);
    }


    private IEnumerator SpawnGuy(float waitTime)
    {
        _SpawningGuys = true;
        yield return new WaitForSeconds(waitTime);

        _TotalGuysSpawned++; 
        var guy = this.m_Spawner.SpawnMarine();

        lock (Locker_GuysOnBoat)
        {
            this._GuysOnBoat.Add(guy);
        }

        var g_script = guy.GetComponent<GuyScript>();
        bool hasDrowned = false; 

        //When drowning remove from the list
        g_script.evt_Drowning += (sender, e) =>
        {
            if (hasDrowned) return; 
            
            lock (Locker_GuysOnBoat)
            {
                this._GuysOnBoat.Remove(guy);
            }

            hasDrowned = true; 
        };

        //When guy eaten lower health
        g_script.evt_Eaten += (sender, e) =>
        {
            Current_Health--;
        }; 

        _SpawningGuys = false;
    }

    private IEnumerator ReloadCounter()
    {
        m_reload = true;
        yield return new WaitForSeconds(10);
        m_reload = false;
    }

    private void UpdateFacingDirection(float angle)
    {
        // Right
        if (m_velocity.x > 0)
        {
            m_boat.transform.eulerAngles = new Vector3(angle, 270, transform.eulerAngles.z);
        }
        // Left
        else if (m_velocity.x < 0)
        {
            m_boat.transform.eulerAngles = new Vector3(-angle, 90, transform.eulerAngles.z);
        }
    }

    private void MoveAwayFromMonster()
    {
        FleeNoVert(m_monster.transform.position);
    }

    private void MoveTowardsMonster()
    {
        SeekNoVert(m_monster.transform.position);
    }

    private void CheckIfFarAway()
    {
        if (Vector3.Distance(
            m_monster.GetComponent<Renderer>().bounds.center,
            gameObject.GetComponent<Renderer>().bounds.center) > MAX_DISTANCE)
        {
            m_fleeing = false;
        }
    }

    private bool TooClose()
    {
        return Vector3.Distance(
            m_monster.GetComponent<Renderer>().bounds.center,
            gameObject.GetComponent<Renderer>().bounds.center) < MIN_DISTANCE;
    }

    private bool InRange()
    {
        return Vector3.Distance(
            m_monster.GetComponent<Renderer>().bounds.center,
            gameObject.GetComponent<Renderer>().bounds.center) < IN_RANGE;
    }

    private bool CanFire()
    {
        return !m_reload;
    }

    private void TargetMonster()
    {
        m_target = m_monster.GetComponent<Renderer>().bounds.center;
    }

    private void Fire()
    {
        var spear = Instantiate(m_spear, m_position, Quaternion.identity);
        spear.GetComponent<Spear>().Fire(m_target);
    }
    #endregion
}
