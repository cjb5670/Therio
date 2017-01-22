using UnityEngine;

public class BoatHealth : MonoBehaviour
{
    #region BoatHealth Member Variables
    private float m_startScale;
    public float m_maxHealth;
    public float m_curHealth;
    public GameObject m_healthBar;
    #endregion

    #region BoatHealth Methods
    void Start()
    {
        m_startScale = m_healthBar.transform.localScale.x;
        m_curHealth = m_maxHealth;
    }
    
    void Update()
    {
        //m_curHealth = GetComponent<BoatAI>().currentHealth;
        SetHealthBar(m_curHealth / m_maxHealth);
    }

    private void SetHealthBar(float health)
    {
        m_healthBar.transform.localScale = new Vector3(Mathf.Clamp(health, 0f, 1f) * m_startScale, 
                                                       m_healthBar.transform.localScale.y, 
                                                       m_healthBar.transform.localScale.z);
    }
    #endregion
}