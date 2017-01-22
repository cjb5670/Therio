using UnityEngine;

public struct TriangleData
{
    #region TriangleData Member Variables
    public Vector3 m_p1;
    public Vector3 m_p2;
    public Vector3 m_p3;
    public Vector3 m_center;

    public Vector3 m_normal;
    public float m_distanceToSurface;
    public float m_area;
    #endregion

    #region TriangleData Methods
    public TriangleData(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        m_p1 = p1;
        m_p2 = p2;
        m_p3 = p3;

        m_center = (p1 + p2 + p3) / 3f;
        m_distanceToSurface = Mathf.Abs(WaterController.current.DistanceToWater(m_center, Time.time));
        m_normal = Vector3.Cross(p2 - p1, p3 - p1).normalized;

        float a = Vector3.Distance(p1, p2);
        float c = Vector3.Distance(p3, p1);
        m_area = (a * c * Mathf.Sin(Vector3.Angle(p2 - p1, p3 - p1) * Mathf.Deg2Rad)) / 2f;
    }
    #endregion
}
