using UnityEngine;
using System.Collections;

public class Spear : Movement
{
    #region Spear Member Variables
    private GameObject m_spear;
    private Vector3 m_target;
    #endregion

    #region Spear Methods
    private void Start()
    {
        StartCoroutine(SetActiveTimer());
    }

    void Update()
    {
        Seek(m_target);
        CalculateForces();
    }

    public void Fire(Vector3 target)
    {
        m_target = target;
    }

    private IEnumerator SetActiveTimer()
    {
        yield return new WaitForSeconds(1);
        GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    #endregion
}
