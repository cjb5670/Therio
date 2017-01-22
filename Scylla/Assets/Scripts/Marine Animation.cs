using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineAnimation : MonoBehaviour
{
    #region MarineAnimation Member Variables
    private Animator m_animator;
    #endregion

    #region MarineAnimation Methods
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        m_animator.SetBool("drowning", GetComponent<GuyScript>().drowning);
    }
    #endregion
}
