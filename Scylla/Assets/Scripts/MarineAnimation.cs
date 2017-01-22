using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineAnimation : MonoBehaviour
{
    #region MarineAnimation Member Variables
    private Animator m_animator;
    public GameObject m_guy;
    #endregion

    #region MarineAnimation Methods
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        m_animator.SetBool("drowning", m_guy.GetComponent<GuyScript>().drowning);
    }
    #endregion
}
