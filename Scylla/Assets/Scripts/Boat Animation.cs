using UnityEngine;

public class BoatAnimation : MonoBehaviour
{
    #region BoatAnimation Member Variables
    private Animator m_animator;
    #endregion

    #region BoatAnimation Methods
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        m_animator.SetBool("attacking", GetComponent<BoatAI>().m_attacking);
    }
    #endregion
}
