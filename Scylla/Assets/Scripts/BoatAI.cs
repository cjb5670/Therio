using UnityEngine;
using System.Collections;

public class BoatAI : Movement
{
    #region BoatAI Member Variables
    private const float MAX_DISTANCE = 5;
    private const float MIN_DISTANCE = 2;
    private const float IN_RANGE = 4.5f;
    private bool m_fleeing = false;
    private bool m_reload = false;
    private Vector3 m_target;

    public bool m_attacking = false;
    public GameObject m_spear;
    public GameObject m_monster;
    public GameObject m_boat;
    #endregion

    #region BoatAI Methods
    void Start()
    {
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

        CalculateForces();
        ClampAngle(0);
        UpdateFacingDirection(0);
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
